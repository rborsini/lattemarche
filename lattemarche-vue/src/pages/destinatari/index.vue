<template>
    
    <div id="destinatari-page" class="container-fluid p-0">



        <!-- Pannello editazione dettaglio -->
        <editazione-destinatario-modal ref="editazioneDestinatarioModal"
                                    :destinatario="destinatario"
                                    v-on:salvato="$refs.savedDialog.open()"
                                    v-on:ok="window.location = '/Destinatari'"></editazione-destinatario-modal>

        <!-- Pannello notifica salvatagggio -->
        <notification-dialog ref="savedDialog"
                            :title="'Conferma salvataggio'"
                            :message="'Il destinatario è stato salvato correttamente'"></notification-dialog>

        <!-- Pannello notifica rimozione -->
        <notification-dialog ref="removedDialog"
                            :title="'Conferma rimozione'"
                            :message="'Destinatario rimosso correttamente'"
                            v-on:ok="window.location = '/Destinatari'"></notification-dialog>

        <!-- Pannello modale conferma eliminazione -->
        <confirm-dialog ref="confirmDeleteDialog"
                        :title="'Conferma eliminazione'"
                        :message="'Sei sicuro di voler rimuovere il destinatario selezionato?'"
                        v-on:confirmed="onRemove()"></confirm-dialog>

        <!-- Tabella -->
        <data-table :options="tableOptions" :rows="destinatari" v-on:data-loaded="onDataLoaded">

            <!-- Toolbox -->
            <template slot="toolbox" v-if="canAdd">
                <button class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>
            </template>

            <!-- Colonne -->
            <template slot="thead">
                <th>P. IVA</th>
                <th>Ragione sociale</th>
                <th v-if="canEdit || canRemove"></th>
            </template>

        </data-table>
    </div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";
import Select2 from "../../components/select2.vue";
import EditazioneDestinatarioModal from "../destinatari/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { Destinatario } from "../../models/destinatario.model";
import { DestinatariService } from "../../services/destinatari.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        openDestinatario(destinatario: Destinatario): void
        close(): void
    }
}

@Component({
    components: {
        Select2,
        ConfirmDialog,
        NotificationDialog,
        EditazioneDestinatarioModal,
        DataTable
    }
})


export default class DestinatariIndexPage extends Vue {


    $refs: any = {
        savedDialog: Vue,
        removedDialog: Vue,
        editazioneDestinatarioModal: Vue,
        confirmDeleteDialog: Vue
    }

    private destinatariService: DestinatariService;
    private destinatario: Destinatario;
    private idDestinatarioDaRimuovere!: number;

    public tableOptions: any = {};
    public destinatari: Destinatario[] = [];
    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.destinatariService = new DestinatariService();
        this.destinatario = new Destinatario();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {
        this.initTable();

        this.destinatariService.index()
            .then(response => {
                this.destinatari = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.destinatariService.details(rowId)
                .then(response => {
                    this.destinatario = response.data;
                    this.$refs.editazioneDestinatarioModal.openDestinatario(this.destinatario);
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idDestinatarioDaRimuovere = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }

    // nuovo destinatario
    public onAdd() {

        this.destinatario = new Destinatario();
        this.$refs.editazioneDestinatarioModal.open();

    }

    // rimozione destinatario
    public onRemove() {

        this.destinatariService.delete(this.idDestinatarioDaRimuovere)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }

    // inizializzazione tabella
    private initTable(): void {
        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "P_IVA" });
        options.columns.push({ data: "RagioneSociale" });

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