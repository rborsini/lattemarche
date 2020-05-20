<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- breadcrumb -->
    <div class="jumbotron">
      <div class="row">
        <div class="col-6">
          <ol class="breadcrumb mb-0">
            <li class="breadcrumb-item">
              <a class="root" href="/">Home</a>
            </li>
            <li class="breadcrumb-item active">Fatture</li>
          </ol>
        </div>
      </div>
    </div>

    <!-- Box ricerca -->
    <div class="jumbotron">


      <!-- Numero e data -->
      <div class="row pt-2">
        <label class="col-1">Num fattura:</label>        
        <div class="col-2">          
          <input type="number" v-model="parameters.Number_From" class="form-control" />
        </div>
        <div class="col-2">          
          <input type="number" v-model="parameters.Number_To" class="form-control" />
        </div>

        <div class="col-2">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="years.Items"
            :value.sync="parameters.Year"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>

        <label class="col-1">Data dal</label>
        <div class="col-1">
          <datepicker class="form-control" :value.sync="parameters.Date_From_Str" />
        </div>
        <div class="col-1">
          <datepicker class="form-control" :value.sync="parameters.Date_To_Str" />
        </div>        

      </div>

      <!-- Sede / Cliente  -->
      <div class="row pt-2">
        <label class="col-1">Sede:</label>
        <div class="col-2">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="headQuarters.Items"
            :value.sync="parameters.HeadQuarter_Id"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>

        <label class="col-1">Cliente:</label>
        <div class="col-2">
          <select2
            class="form-control"
            ajax="true"
            ref="customerSelect"
            :placeholder="''"
            :url="'/api/customers/search'"
            :value.sync="parameters.Customer_Id"
            value-field="Id"
            text-field="FullBusinessName"
          />
        </div>

        <label class="col-1">Data scadenza dal</label>
        <div class="col-1">
          <datepicker class="form-control" :value.sync="parameters.DueDate_From_Str" />
        </div>
        <div class="col-1">
          <datepicker class="form-control" :value.sync="parameters.DueDate_To_Str" />
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
        <th>Cliente</th>
        <th>Numero</th>
        <th>Data</th>
        <th>Data registrazione</th>
        <th>Data scadenza</th>
        <th>Chiusa</th>
        <th>Imponibile [€]</th>
        <th>IVA [€]</th>
        <th>Totale [€]</th>
        <!-- <th></th> -->
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
import Waiter from "../../components/waiter.vue";

// servizi
import { DropdownService } from "@/services/dropdown.service";
import { InvoicesService } from "@/services/ir/invoices.service";

// modelli
import { Invoice, InvoiceSearchModel } from "@/models/ir/invoice.model";
import { Dropdown, DropdownItem } from "@/models/dropdown.model";
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
    customerSelect: Vue,
    table: Vue
  };

  private invoicesService: InvoicesService = new InvoicesService();
  public dropdownService: DropdownService = new DropdownService();

  public headQuarters: Dropdown = new Dropdown();
  public years: Dropdown = new Dropdown();

  public tableOptions: any = {};
  public parameters: InvoiceSearchModel = new InvoiceSearchModel();

  public btnExcelVisible: boolean = false;

  constructor() {
    super();
  }

  public mounted() {
    this.initTable();
    this.loadDropdown();

    this.btnExcelVisible = PermissionsService.isViewItemAuthorized("Invoices", "Excel", "Excel", "API");        

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
    this.parameters = new InvoiceSearchModel();    
    this.$refs.customerSelect.setItem('');
  }

  // Cerca
  public onSearchClick() {    
    this.$refs.table.load(this.parameters.ToUrlQueryString());
  }

  // caricamento dropdown
  private loadDropdown() {
    
    this.dropdownService.getHeadQuarters().then(response => {
      this.headQuarters = response.data;
    });

    for(var year = 2015; year <= new Date().getFullYear(); year++) {
      this.years.Items.push(new DropdownItem(year.toString(), year.toString()));
    }    

    // this.dropdownService.getBusinessSublines().then(response => {
    //   this.businessSublines = response.data;
    // });

    // this.invoicesService.statuses().then(response => {
    //   this.statuses = response.data;
    // });
  }

  // inizializzazione datatable
  private initTable(): void {
    var options: any = {};

    options.serverSide = true;
    options.ajax = {
      url: '/api/invoices/search',
      type: 'POST'
    };

    options.columns = [];

    options.columns.push({ data: "Customer_BusinessName" });
    options.columns.push({ data: "Number" });

    options.columns.push({ 
      data: "Date", 
      render: function(data: any, type: any, row: any) {
        return row.Date_Str;
      }
    });

    options.columns.push({ 
      data: "RegistrationDate", 
      render: function(data: any, type: any, row: any) {
        return row.RegistrationDate_Str;
      }
    });

    options.columns.push({ 
      data: "DueDate", 
      render: function(data: any, type: any, row: any) {
        return row.RegistrationDate_Str;
      }
    });

    options.columns.push({ 
      data: "Status", 
      defaultContent: "-",
      render: function(data: any, type: any, row: any) {
        return row.IsClosed ? "Si" : "No";
      }
    });

    options.columns.push({ data: "Amount", defaultContent: "-" });
    options.columns.push({ data: "VAT", defaultContent: "-" });
    options.columns.push({ data: "Total_Amount", defaultContent: "-" });    

    this.tableOptions = options;
  }

  // evento fine generazione tabella
  public onDataLoaded() {
    // $(".edit").click(event => {
    //   var element = $(event.currentTarget);
    //   var rowId = $(element).data("row-id");
    // });
  }

  //Esportazione excel
  public downloadExcel() {
    UrlService.redirect('/api/offers/excel?' + this.parameters.ToUrlQueryString());
  }
}
</script>