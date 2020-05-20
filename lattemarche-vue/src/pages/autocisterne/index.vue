<template>
    
    <div id="autocisterne-page" class="container-fluid p-0">



        <!-- Pannello editazione dettaglio -->
        <editazione-autocisterna-modal ref="editazioneAutocisternaModal"
                                    :autocisterna="autocisterna"
                                    v-on:salvato="$refs.savedDialog.open()"></editazione-autocisterna-modal>

        <!-- Pannello notifica salvatagggio -->
        <notification-dialog ref="savedDialog"
                            :title="'Conferma salvataggio'"
                            :message="'Autocisterna salvata correttamente'"
                            v-on:ok="window.location = '/Autocisterne'"></notification-dialog>

        <!-- Pannello notifica rimozione -->
        <notification-dialog ref="removedDialog"
                            :title="'Conferma rimozione'"
                            :message="'Autocisterna rimossa correttamente'"
                            v-on:ok="window.location = '/Autocisterne'"></notification-dialog>

        <!-- Pannello modale conferma eliminazione -->
        <confirm-dialog ref="confirmDeleteDialog"
                        :title="'Conferma eliminazione'"
                        :message="'Sei sicuro di voler rimuovere l\'autocisterna selezionata?'"
                        v-on:confirmed="onRemove()"></confirm-dialog>

        <!-- Tabella -->
        <data-table :options="tableOptions" :rows="autocisterne" v-on:data-loaded="onDataLoaded">

            <!-- Toolbox -->
            <template slot="toolbox" v-if="canAdd">
                <button class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>
            </template>

            <!-- Colonne -->
            <template slot="thead">
                <th>Marca</th>
                <th>Modello</th>
                <th>Targa</th>
                <th>Portata</th>
                <th v-if="canEdit || canRemove"></th>
            </template>

        </data-table>
    </div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";
import EditazioneAutocisternaModal from "../autocisterne/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { Autocisterna } from "../../models/autocisterna.model";
import { AutocisterneService } from "../../services/autocisterne.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    components: {
        ConfirmDialog,
        NotificationDialog,
        EditazioneAutocisternaModal,
        DataTable
    }
})


export default class AutocisterneIndexPage extends Vue {


    $refs: any = {
        savedDialog: Vue,
        editazioneAutocisternaModal: Vue,
        confirmDeleteDialog: Vue,
        removedDialog: Vue
    }
    
    private autocisterneService: AutocisterneService;
    private autocisterna: Autocisterna;
    private idAutocisterna!: number;

    public tableOptions: any = {};
    public autocisterne: Autocisterna[] = [];
    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.autocisterneService = new AutocisterneService();
        this.autocisterna = new Autocisterna();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {
        this.initTable();

        this.autocisterneService.index()
            .then(response => {
                this.autocisterne = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.autocisterneService.details(rowId)
                .then(response => {
                    this.autocisterna = response.data;
                    this.$refs.editazioneAutocisternaModal.open();
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idAutocisterna = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }


    // nuova autocisterna
    public onAdd() {

        this.autocisterna = new Autocisterna();
        this.$refs.editazioneAutocisternaModal.open();

    }

    // rimozione autocisterna
    public onRemove() {

        this.autocisterneService.delete(this.idAutocisterna)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }


    // inizializzazione tabella
    private initTable(): void {
        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Marca" });
        options.columns.push({ data: "Modello" });
        options.columns.push({ data: "Targa" });
        options.columns.push({ data: "Portata" });

        var ce = this.canEdit;
        var cr = this.canRemove;

        if (ce || cr) {

            options.columns.push({
                render: function (data: any, type: any, row: any) {

                    var html = '<div class="text-center">';

                    if (ce)
                        html += '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-edit"></i></a>';

                    if (cr)
                        html += '<a class="pl-3 delete" title="elimina" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-trash-alt"></i></a>';

                    html += '</div>';

                    return html;
                },
                className: "edit-column",
                orderable: false
            });

        }
        this.tableOptions = options;


    }

}

</script>