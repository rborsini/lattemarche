<template>
  
<div>

    <!-- Pannello editazione dettaglio -->
    <attachmentCategory-editor ref="attachmentCategoryEditor"
                             :attachmentCategory="attachmentCategory"
                             :isNew="!editing"
                             v-on:saved="$refs.savedDialog.open()" ></attachmentCategory-editor>

    <!-- Pannello notifica salvatagggio -->
    <notification-dialog ref="savedDialog"
                         :title="'Conferma salvataggio'"
                         :message="'Categoria salvata correttamente'"
                         v-on:ok="redirect()"></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog ref="removedDialog"
                         :title="'Conferma rimozione'"
                         :message="'Categoria rimossa correttamente'"
                         v-on:ok="redirect()"></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog ref="confirmDeleteDialog"
                    :title="'Conferma eliminazione'"
                    :message="'Sei sicuro di voler rimuovere la categoria selezionata?'"
                    v-on:confirmed="onRemove()"></confirm-dialog>

    <button v-if="btnNewVisible" class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="attachmentCategories" v-on:data-loaded="onDataLoaded">

        <!-- Colonne -->
        <template slot="thead">
            <th>Pagina</th>
            <th>Descrizione</th>
            <th></th>
            <th></th>
        </template>

    </data-table>
</div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";
import AttachmentCategoryEditor from "./edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { UrlService } from '@/services/url.service';
import { AttachmentCategoriesService } from '@/services/attachmentCategories.service';
import { AttachmentCategory } from '../../models/attachmentCategory.model';
import { PermissionsService } from '@/services/permissions.service';

@Component({
  components: {
    ConfirmDialog,
    NotificationDialog,
    AttachmentCategoryEditor,
    DataTable
  }
})
export default class App extends Vue {

    $refs: any = {
        savedDialog: Vue,
        attachmentCategoryEditor: Vue,
        confirmDeleteDialog: Vue,
        removedDialog: Vue
    }

    public btnNewVisible: boolean = false;
    public btnEditVisible: boolean = false;
    public btnDeleteVisible: boolean = false;

    private attachmentCategoriesService: AttachmentCategoriesService;
    public attachmentCategory: AttachmentCategory;
    private attachmentCategoryId: number = 0;
    public editing:boolean=true;

    public tableOptions: any = {};
    public attachmentCategories: AttachmentCategory[] = [];

    constructor() {
        super();

        this.attachmentCategoriesService = new AttachmentCategoriesService();
        this.attachmentCategory = new AttachmentCategory();
    }

    public mounted() {
        
        this.btnNewVisible = PermissionsService.isViewItemAuthorized("AttachmentCategories", "Index", "New");    
        this.btnEditVisible = PermissionsService.isViewItemAuthorized("AttachmentCategories", "Index", "Edit");    
        this.btnDeleteVisible = PermissionsService.isViewItemAuthorized("AttachmentCategories", "Index", "Delete");    

        this.initTable();

        this.attachmentCategoriesService.index()
            .then(response => {
                this.attachmentCategories = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {
            this.editing=true;
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.attachmentCategoriesService.details(rowId)
                .then(response => {
                    this.attachmentCategory = response.data;
                    this.$refs.attachmentCategoryEditor.open();
                });

        });

        $('.delete').click((event) => {
            
            var element = $(event.currentTarget);
            this.attachmentCategoryId = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }


    // nuova categoria
    public onAdd() {
        this.editing=false;
        this.attachmentCategory = new AttachmentCategory();
        this.$refs.attachmentCategoryEditor.open();

    }

    // rimozione categoria
    public onRemove() {

        this.attachmentCategoriesService.delete(this.attachmentCategoryId)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }


    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Page" });
        options.columns.push({ data: "Description" });
        
        if(this.btnEditVisible) {
            options.columns.push({
                render: function (data: any, type: any, row: any) {

                    var html = '<div class="text-center">';
                    html += '<a class="edit text-primary" title="modifica" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-edit"></i></a>';
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
        UrlService.redirect('/attachmentcategories');
    }



}
</script>