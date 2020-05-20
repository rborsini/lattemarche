<template>
  <div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- breadcrumb -->
    <div class="jumbotron" >
      <div class="row">
        <div class="col-6">
          <ol class="breadcrumb mb-0"  >
            <li class="breadcrumb-item"><a class="root" href="/" >Home</a></li>
            <li class="breadcrumb-item active">Ordini</li>
          </ol>
        </div>
      </div>
    </div>

    <!-- Box ricerca -->
    <div class="jumbotron">

      <!-- Sede / Cliente / Data -->
      <div class="row pt-3">

        <label class="col-1">Sede:</label>
        <div class="col-2">
          <select2 class="form-control" :placeholder="'-'" :options="headQuarters.Items" :value.sync="parameters.HeadQuarterId" :value-field="'Value'" :text-field="'Text'" /> 
        </div>

        <label class="col-1">Cliente: </label>
        <div class="col-2">
          <select2 class="form-control" ajax="true" ref="customerSelect" :placeholder="''" :url="'/api/customers/search'" :value.sync="parameters.CustomerId" value-field="Id" text-field="FullBusinessName" />       
        </div>

        <label class="col-1">Dal:</label>
        <div class="col-2">
          <datepicker class="form-control" :value.sync="parameters.Date_From_Str" />
        </div>

        <label class="col-1">Al:</label>
        <div class="col-2">
          <datepicker class="form-control"  :value.sync="parameters.Date_To_Str" />
        </div>

      </div>

      <!-- Codice / Sottolinea / Stato Documento -->
      <div class="row pt-3">

        <label class="col-1">Codice:</label>
        <div class="col-2">
          <input class="form-control" v-model="parameters.Code">
        </div>

        <label class="col-1">Sottolinea:</label>
        <div class="col-2">
          <select2 class="form-control" :placeholder="'-'" :options="businessSublines.Items" :value.sync="parameters.BusinessSublineId" :value-field="'Value'" :text-field="'Text'" /> 
        </div>

        <label class="col-1">Stato:</label>
        <div class="col-2">
          <select multiple v-model="parameters.Statuses" class="form-control" >
            <option v-for="status in statuses" :key="status.Id" :value="status.Id" >{{status.Code}}</option>
          </select>
        </div>

      </div>

      <!-- Annulla / Cerca -->
      <div class="row pt-3">
        <div class="col-12 text-right">
          <button class="btn btn-secondary mr-2" v-on:click="onClearSelection()" role="button" >Annulla</button>
          <button class="btn btn-primary" v-on:click="onSearchClick()" role="button">Cerca</button>
        </div>
      </div>

    </div>


    <!-- Tabella -->
    <data-table ref="table" :options="tableOptions"  v-on:data-loaded="onDataLoaded">
      <!-- Toolbox -->
      <template slot="toolbox">
          <div class="toolbox">
              <button v-if="btnExcelVisible" class="btn btn-primary float-right mr-3" v-on:click="downloadExcel()">Esporta excel</button>
          </div>
      </template>

      <template slot="thead">
        <th>Codice</th>
        <th>Sede</th>
        <th>Cliente</th>
        <th>Sottolinea</th>
        <th>Data</th>
        <th>Stato</th>
        <th></th>
      </template>
    </data-table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import $ from "jquery";

// componenti
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import DataTable from "../../components/dataTable.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";
import Waiter from "../../components/waiter.vue";

// servizi
import { OrdersService } from "@/services/orders.service";
import { DropdownService } from "@/services/dropdown.service";

// modelli
import { OrderRow, OrderSearchModel } from "@/models/orderRow.model";
import { Dropdown } from "@/models/dropdown.model";
import { DocumentStatus } from '@/models/documentStatus.model';
import { UrlService } from '@/services/url.service';
import { PermissionsService } from '@/services/permissions.service';

@Component({
  components: {
    Select2,
    DataTable,
    Datepicker,
    Waiter
  }
})
export default class App extends Vue {

  $refs: any = {
    waiter: Vue,
    customerSelect:Vue,
    table: Vue    
  };

  private ordersService: OrdersService = new OrdersService();
  public dropdownService: DropdownService = new DropdownService();
  
  public headQuarters:Dropdown=new Dropdown();
  public businessSublines:Dropdown=new Dropdown();

  public statuses: DocumentStatus[] = [];


  public tableOptions: any = {};
  public parameters: OrderSearchModel = new OrderSearchModel();

  public btnExcelVisible: boolean = false;

  constructor() {
    super();
  }

  public mounted() {

    this.btnExcelVisible = PermissionsService.isViewItemAuthorized("Orders", "Excel", "Excel", "API");    

    this.initTable();
    this.loadDropdown();

    document.addEventListener("keyup", this.onKeyPressed);
  }

  // Gestione eventi tastiera
  public onKeyPressed(e: any) {
    switch(e.keyCode) {
      case 13:  // invio
        this.onSearchClick();
    }
  }

  // caricamento dropdown
  private loadDropdown(){

    this.dropdownService.getHeadQuarters().then(response => {
      this.headQuarters = response.data
    });

    this.dropdownService.getBusinessSublines().then(response => {
      this.businessSublines = response.data
    });

    this.ordersService.statuses().then(response => {
      this.statuses = response.data;
    });
  }

  // Pulizia selezione
  public onClearSelection() {
    this.parameters = new OrderSearchModel();    
    this.$refs.customerSelect.setItem('');
  }

  // Cerca
  public onSearchClick() {
    this.$refs.table.load(this.parameters.ToUrlQueryString());
  }

  private initTable(): void {
    var options: any = {};

    options.serverSide = true;
    options.ajax = {
      url: '/api/orders/search',
      type: 'POST'
    };

    options.columns = [];

    options.columns.push({ data: "Code" });
    options.columns.push({ data: "HeadQuarter.Description", defaultContent: "-" });
    options.columns.push({ data: "Customer.BusinessName", defaultContent: "-" });
    options.columns.push({ data: "BusinessSubline.Description", defaultContent: "-" });
    options.columns.push({ 
      data: "Date", 
      render: function(data: any, type: any, row: any) {
        return row.Date_Str;
      }
    });
    options.columns.push({ data: "Status.Code", defaultContent: "-"});

    options.columns.push({
      render: function(data: any, type: any, row: any) {
        return (
          '<div class="text-center"><a class="edit" href="/orders/edit?id=' + row.Id + '" title="modifica" style="cursor: pointer;" ><i class="far fa-edit"></i></a></div>'
        );
      },
      className: "edit-column",
      orderable: false
    });

    this.tableOptions = options;
  }

  // evento fine generazione tabella
  public onDataLoaded() {
    $(".edit").click(event => {
      var element = $(event.currentTarget);
      var rowId = $(element).data("row-id");
    });
  }

  //Esportazione excel
  public downloadExcel() {
    UrlService.redirect('/api/orders/excel?' + this.parameters.ToUrlQueryString());
  }
}
</script>