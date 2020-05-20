<template>
  
    <div class="container-fluid">

        <!-- Tabella -->
        <data-table :options="columnOptions" :rows="customers" >
            <!-- Toolbox -->
            <template slot="toolbox">
                <div class="toolbox">
                    <button v-if="btnExcelVisible" class="btn btn-primary float-right" v-on:click="downloadExcel()">Esporta excel</button>
                </div>
            </template>

            <!-- Colonne -->
            <template slot="thead">
                <th>Id</th>
                <th>Ragione Sociale</th>
                <th>Codice Fiscale</th>
                <th>Partita Iva</th>
                <th>Indirizzo</th>
                <th>CAP</th>
                <th>Città</th>
                <th>Prov.</th>
                <th>Telefono</th>
                <th>Attivo</th>
                <th>Reg. Valido</th>
            </template>

        </data-table>
    </div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";

import { Customer } from "../../models/customer.model";
import { CustomersService } from "../../services/customers.service";
import { UrlService } from '@/services/url.service';
import { PermissionsService } from '@/services/permissions.service';

@Component({
  components: {
    DataTable
  }
})
export default class App extends Vue {

    public customers: Customer[] = [];
    private customersService: CustomersService;

    public columnOptions: any[] = [];
    
    public btnExcelVisible: boolean = false;

    constructor() {
        super();

        this.customersService = new CustomersService();
    }

    public mounted() {
        this.initTable();

        this.btnExcelVisible = PermissionsService.isViewItemAuthorized("Customers", "Excel", "Excel");    

        this.customersService.getCustomers()
            .then(response => {
                this.customers = response.data;
            });
    }

    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Id" });
        options.columns.push({ data: "BusinessName" });
        options.columns.push({ data: "FiscalCode" });
        options.columns.push({ data: "VAT_Number" });
        options.columns.push({ data: "Address" });
        options.columns.push({ data: "PostalCode" });
        options.columns.push({ data: "City" });
        options.columns.push({ data: "Province" });
        options.columns.push({ data: "Phone_1" });
        options.columns.push({ data: "Active" });
        options.columns.push({ data: "IsRegistryValid" });

        this.columnOptions = options;

    }

    //Esportazione excell
    public downloadExcel() {
        UrlService.redirect('/customers/excel');
    }

}
</script>