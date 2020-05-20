<template>
  <div >

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- breadcrumb -->
    <div class="jumbotron" >
      <div class="row">
        <div class="col-6">
          <ol class="breadcrumb mb-0"  >
            <li class="breadcrumb-item"><a class="root" href="/" >Home</a></li>
            <li class="breadcrumb-item active">Richieste offerta</li>
          </ol>
        </div>
        <div v-if="btnNewVisible" class="col-6 text-right">
          <a href="/offerRequests/edit" class="btn btn-primary mt-1" >Nuova richiesta offerta</a>
        </div>
      </div>
    </div>

    <!-- Box ricerca -->
    <div class="jumbotron">

      <!-- Sede / Cliente / Data  -->
      <div class="row pt-1">

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
          <datepicker class="form-control" :value.sync="parameters.Date_To_Str" />
        </div>

      </div>

      <!-- Sottolinea / Referente / Autori -->
      <div class="row pt-3">

        <label class="col-1">Sottolinea:</label>
        <div class="col-2">
          <select2 class="form-control" :placeholder="'-'" :options="businessSublines.Items" :value.sync="parameters.BusinessSublineId" :value-field="'Value'" :text-field="'Text'" /> 
        </div>

        <label class="col-1">Referente:</label>
        <div class="col-2">
          <select2 class="form-control" :placeholder="'-'" :options="referents.Items" :value.sync="parameters.Referent"  :value-field="'Value'" :text-field="'Text'" />
        </div>

        <label class="col-1">Autore:</label>
        <div class="col-2">
          <select2 class="form-control" :placeholder="'-'" :options="referents.Items" :value.sync="parameters.Author" :value-field="'Value'" :text-field="'Text'" />
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
    <data-table ref="table" :options="tableOptions" v-on:data-loaded="onDataLoaded">
      
      <!-- Toolbox -->
      <template slot="toolbox">
          <div class="toolbox">
              <button v-if="btnExcelVisible" class="btn btn-primary float-right mr-3" v-on:click="downloadExcel()">Esporta excel</button>
          </div>
      </template>

      <template slot="thead">
        <th>Sede</th>
        <th>Cliente</th>
        <th>Sottolinea</th>
        <th>Autore</th>
        <th>Referente</th>
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
import { OfferRequestsService } from "@/services/offerRequests.service";
import { DropdownService } from "@/services/dropdown.service";
import { PermissionsService } from "@/services/permissions.service";

// modelli
import { OfferRequestRow, OfferRequestSearchModel } from "@/models/offerRequestRow.model";
import { Dropdown } from "@/models/dropdown.model";
import { DocumentStatus } from '@/models/documentStatus.model';
import { UrlService } from '@/services/url.service';

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}

@Component({
  components: {
    Select2,
    ConfirmDialog,
    DataTable,
    Datepicker,
    Waiter
  }
})
export default class App extends Vue {

  $refs: any = {
    waiter: Vue,
    customerSelect: Vue,
    table: Vue
  };

  public offerRequestsService: OfferRequestsService = new OfferRequestsService();
  public dropdownService: DropdownService = new DropdownService();

  public headQuarters:Dropdown = new Dropdown();
  public businessSublines:Dropdown = new Dropdown();
  public referents: Dropdown = new Dropdown();
  public statuses: DocumentStatus[] = [];

  public tableOptions: any = {};
  public parameters: OfferRequestSearchModel = new OfferRequestSearchModel();

  public btnNewVisible: boolean = false;
  public btnExcelVisible: boolean = false;

  constructor() {
    super();
  }

  public mounted() {
    this.initTable();
    this.loadDropdown();

    this.btnNewVisible = PermissionsService.isViewItemAuthorized("OfferRequests", "Index", "New");    
    this.btnExcelVisible = PermissionsService.isViewItemAuthorized("OfferRequests", "Excel", "Excel", "API");    

    document.addEventListener("keyup", this.onKeyPressed);

  }

  // Gestione eventi tastiera
  public onKeyPressed(e: any) {
    switch(e.keyCode) {
      case 13:  // invio
        this.onSearchClick();
    }
  }

  // Pulizia selezione
  public onClearSelection() {
    this.parameters = new OfferRequestSearchModel();
    this.$refs.customerSelect.setItem('');
  }

  // Cerca
  public onSearchClick() {
    console.log("url", this.parameters.ToUrlQueryString());
    this.$refs.table.load(this.parameters.ToUrlQueryString());
  }

  private initTable(): void {
    var options: any = {};

    options.serverSide = true;
    options.ajax = {
      url: '/api/offerRequests/search',
      type: 'POST'
    };

    options.columns = [];

    options.columns.push({ data: "HeadQuarter.Description", defaultContent: "-" });
    options.columns.push({ data: "Customer.BusinessName", defaultContent: "-" });
    options.columns.push({ data: "BusinessSubline.Description", defaultContent: "-" });
    options.columns.push({ data: "Author" });
    options.columns.push({ data: "Referent" });
    options.columns.push({ 
      data: "Date", 
      render: function(data: any, type: any, row: any) {
        return row.Date_Str;
      }
    });
    options.columns.push({ data: "Status.Code", defaultContent: "-" });

    options.columns.push({
      render: function(data: any, type: any, row: any) {
        return (
          '<div class="text-center"><a class="edit"  href="/offerRequests/edit?id=' + row.Id + '" title="modifica" style="cursor: pointer;" ><i class="far fa-edit"></i></a></div>'
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

  // caricamento dropdown
  private loadDropdown(){

    this.dropdownService.getHeadQuarters()
    .then(response => {
      this.headQuarters=response.data
    });

    this.dropdownService.getBusinessSublines()
    .then(response => {
      this.businessSublines=response.data
    });

    this.dropdownService.getUsers().then(response => {
      this.referents = response.data;
    });

    this.offerRequestsService.statuses().then(response => {
      this.statuses = response.data;
    });

  }

  //Esportazione excel
  public downloadExcel() {
    UrlService.redirect('/api/offerRequests/excel?' + this.parameters.ToUrlQueryString());
  }

}
</script>