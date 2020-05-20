<template>
  
<div>

    <!-- Pannello notifica salvatagggio -->
    <notification-dialog ref="savedDialog"
                         :title="'Conferma salvataggio'"
                         :message="'Fornitore salvato correttamente'"
                         v-on:ok="redirect()"></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog ref="removedDialog"
                         :title="'Conferma rimozione'"
                         :message="'Fornitore rimosso correttamente'"
                         v-on:ok="redirect()"></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog ref="confirmDeleteDialog"
                    :title="'Conferma eliminazione'"
                    :message="'Sei sicuro di voler rimuovere il fornitore selezionato?'"
                    v-on:confirmed="onRemove()"></confirm-dialog>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="suppliers" v-on:data-loaded="onDataLoaded">
        <!-- Toolbox -->
        <template slot="toolbox">
            <div class="toolbox">
                <button v-if="btnExcelVisible"  class="btn btn-primary float-right ml-3"    v-on:click="downloadExcel()">Esporta excel</button>
                <button v-if="btnNewVisible"    class="btn btn-primary float-right"         v-on:click="newSupplier()" >Aggiungi</button>
            </div>
        </template>

        <!-- Colonne -->
        <template slot="thead">
            <th>Ragione sociale</th>
            <th>Indirizzo</th>
            <th>Comune</th>
            <th>Provincia</th>
            <th>Partita IVA</th>
            <th></th>
            <th></th>
        </template>

    </data-table>
</div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { Supplier } from "../../models/supplier.model";
import { SuppliersService } from "../../services/suppliers.service";
import { UrlService } from '@/services/url.service';
import { PermissionsService } from '@/services/permissions.service';

@Component({
  components: {
    ConfirmDialog,
    NotificationDialog,
    DataTable
  }
})
export default class App extends Vue {

    $refs: any = {
        savedDialog: Vue,
        confirmDeleteDialog: Vue,
        removedDialog: Vue
    }

    public btnNewVisible: boolean = false;
    public btnEditVisible: boolean = false;
    public btnDeleteVisible: boolean = false;
    public btnExcelVisible: boolean = false;

    private suppliersService: SuppliersService;
    public supplier: Supplier;
    private supplierId: number = 0;
    public editing:boolean=true;

    public tableOptions: any = {};
    public suppliers: Supplier[] = [];

    constructor() {
        super();

        this.suppliersService = new SuppliersService();
        this.supplier = new Supplier();
    }

    public mounted() {

        this.btnNewVisible = PermissionsService.isViewItemAuthorized("Suppliers", "Index", "New");    
        this.btnEditVisible = PermissionsService.isViewItemAuthorized("Suppliers", "Index", "Edit");    
        this.btnDeleteVisible = PermissionsService.isViewItemAuthorized("Suppliers", "Index", "Delete");    
        this.btnExcelVisible = PermissionsService.isViewItemAuthorized("Suppliers", "Excel", "Excel", "API");    

        this.initTable();

        this.suppliersService.index()
            .then(response => {
                this.suppliers = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {


        $('.delete').click((event) => {
            
            var element = $(event.currentTarget);
            this.supplierId = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }


    // nuovo fornitore
    public onAdd() {
        this.editing=false;
        this.supplier = new Supplier();
    }

    // rimozione fornitore
    public onRemove() {

        this.suppliersService.delete(this.supplierId)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }


    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "BusinessName" });
        options.columns.push({ data: "Address" });
        options.columns.push({ data: "City" });
        options.columns.push({ data: "Province" });
        options.columns.push({ data: "VAT_Number" });

        if(this.btnEditVisible) {
            options.columns.push({
                render: function (data: any, type: any, row: any) {

                    var html = '<div class="text-center">';
                    html += '<a class="edit text-primary" title="modifica" style="cursor: pointer;" href="/suppliers/edit?id=' + row.Id + '" ><i class="far fa-edit"></i></a>';
                    html += '</div>';

                    return html;
                },
                className: "edit-column",
                orderable: false
            });
        } else {
            options.columns.push({ render: function() { return ''; }, orderable: false });
        }

        if(this.btnDeleteVisible) {
            options.columns.push({
                render: function (data: any, type: any, row: any) {

                    var html = '<div class="text-center">';
                    html += '<a class="delete text-primary" title="rimuovi" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-trash-alt"></i></a>';
                    html += '</div>';

                    return html;
                },
                className: "delete-column",
                orderable: false
            });   
        } else {
            options.columns.push({ render: function() { return ''; }, orderable: false });
        }     

        this.tableOptions = options;
    }

    public redirect() {
        UrlService.redirect('/suppliers');
    }

    // link alla pagina di creazione fornitore
    public newSupplier() {
        UrlService.redirect('/suppliers/edit');
    } 

    //Esportazione excel
    public downloadExcel() {
        UrlService.redirect('/api/suppliers/excel');
    }


}
</script>