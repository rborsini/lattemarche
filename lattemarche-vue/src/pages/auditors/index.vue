<template>
  
<div>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog ref="confirmDeleteDialog"
                    :title="'Conferma eliminazione'"
                    :message="'Sei sicuro di voler rimuovere il tecnico selezionato?'"
                    v-on:confirmed="onRemove()"></confirm-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog ref="removedDialog"
                         :title="'Conferma rimozione'"
                         :message="'Tecnico rimosso correttamente'"
                         v-on:ok="redirect()"></notification-dialog>                    

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="auditors" v-on:data-loaded="onDataLoaded">
        <!-- Toolbox -->
        <template slot="toolbox">
            <div class="toolbox">
                <button v-if="btnExcelVisible"  class="btn btn-primary float-right ml-3" v-on:click="downloadExcel()">Esporta excel</button>
                <button v-if="btnNewVisible"    class="btn btn-primary float-right" v-on:click="newAuditor()" >Aggiungi</button>
            </div>
        </template>

        <!-- Colonne -->
        <template slot="thead">
            <th>Id</th>
            <th>Nome</th>
            <th>Cognome</th>
            <th>Fornitore</th>
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
import { AuditorsService } from "../../services/auditors.service";
import { UrlService } from "@/services/url.service";
import { PermissionsService } from "@/services/permissions.service";
import { Auditor } from '../../models/auditor.model';

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
    };

    public btnNewVisible: boolean = false;
    public btnEditVisible: boolean = false;
    public btnDeleteVisible: boolean = false;
    public btnExcelVisible: boolean = false;

    private auditorsService: AuditorsService;

    public tableOptions: any = {};
    public auditors: Auditor[] = [];
    public auditorId: number = 0;

    constructor() {
        super();

        this.auditorsService = new AuditorsService();
    }

    public mounted() {
        this.btnNewVisible = PermissionsService.isViewItemAuthorized("Auditors", "Index", "New" );
        this.btnEditVisible = PermissionsService.isViewItemAuthorized("Auditors", "Index", "Edit" );
        this.btnDeleteVisible = PermissionsService.isViewItemAuthorized("Auditors", "Index", "Delete" );
        this.btnExcelVisible = PermissionsService.isViewItemAuthorized("Auditors", "Excel", "Excel", "API" );

        this.initTable();

        this.auditorsService.index().then(response => {
            this.auditors = response.data;
        });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {
        $(".delete").click(event => {
            var element = $(event.currentTarget);
            this.auditorId = $(element).data("row-id");
            this.$refs.confirmDeleteDialog.open();
        });
    }

    // rimozione fornitore
    public onRemove() {
        this.auditorsService.delete(this.auditorId).then(response => {
            this.$refs.removedDialog.open();
        });
    }

    // inizializzazione tabella
    private initTable(): void {
        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Id" });
        options.columns.push({ data: "FirstName" });
        options.columns.push({ data: "LastName" });
        options.columns.push({ data: "Parent_Name" });

        if (this.btnEditVisible) {
            options.columns.push({
                render: function(data: any, type: any, row: any) {
                    var html = '<div class="text-center">';
                    html +=  '<a class="edit text-primary" title="modifica" style="cursor: pointer;" href="/auditors/edit?id=' + row.Id + '" ><i class="far fa-edit"></i></a>';
                    html += "</div>";

                    return html;
                },
                className: "edit-column",
                orderable: false
            });
        } else {
            options.columns.push({
                render: function() {
                    return "";
                },
                orderable: false
            });
        }

        if (this.btnDeleteVisible) {
            options.columns.push({
                render: function(data: any, type: any, row: any) {
                    var html = '<div class="text-center">';
                    html +=
                        '<a class="delete text-primary" title="rimuovi" style="cursor: pointer;" data-row-id="' +
                        row.Id +
                        '" ><i class="far fa-trash-alt"></i></a>';
                    html += "</div>";

                    return html;
                },
                className: "delete-column",
                orderable: false
            });
        } else {
            options.columns.push({
                render: function() {
                    return "";
                },
                orderable: false
            });
        }

        this.tableOptions = options;
    }

    public redirect() {
        UrlService.redirect("/auditors");
    }

    // link alla pagina di creazione auditor
    public newAuditor() {
        UrlService.redirect('/auditors/edit');
    } 

    //Esportazione excel
    public downloadExcel() {
        UrlService.redirect('/api/auditors/excel');
    }
}
</script>