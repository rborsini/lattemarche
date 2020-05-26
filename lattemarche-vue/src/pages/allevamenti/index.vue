<template>
    

    <div id="index-allevamenti-page" class="container-fluid p-0">


        <!-- Pannello editazione dettaglio -->
        <editazione-allevamento-modal ref="editazioneAllevamentoModal"
                                    :allevamento="allevamento"
                                    v-on:salvato="$refs.savedDialog.open()"></editazione-allevamento-modal>

        <!-- Pannello notifica salvatagggio -->
        <notification-dialog ref="savedDialog"
                            :title="'Conferma salvataggio'"
                            :message="'Allevamento salvato correttamente'"
                            v-on:ok="window.location = '/Allevamenti'"></notification-dialog>

        <!-- Pannello notifica rimozione -->
        <notification-dialog ref="removedDialog"
                            :title="'Conferma rimozione'"
                            :message="'Allevamento rimosso correttamente'"
                            v-on:ok="window.location = '/Allevamenti'"></notification-dialog>

        <!-- Pannello modale conferma invio Apice richiesta -->
        <confirm-dialog ref="confirmDeleteDialog"
                        :title="'Conferma eliminazione'"
                        :message="'Sei sicuro di voler rimuovere l\'allevamento selezionato?'"
                        v-on:confirmed="onRemove()"></confirm-dialog>

        <data-table :options="tableOptions" :rows="allevamenti" v-on:data-loaded="onDataLoaded">

            <!-- Toolbox -->
            <template slot="toolbox" >
                <button class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>
            </template>

            <template slot="thead">
                <th>#</th>
                <th>Ragione sociale</th>
                <th>CUAA</th>
                <th>Codice ASL</th>
                <th></th>
            </template>
        </data-table>

    </div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";
import EditazioneAllevamentoModal from "../allevamenti/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { Allevamento } from "../../models/allevamento.model";
import { AllevamentiService } from "../../services/allevamenti.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        openAllevamento(allevamento: Allevamento): void
        close(): void
    }
}

@Component({
    components: {
        ConfirmDialog,
        NotificationDialog,
        EditazioneAllevamentoModal,
        DataTable
    }
})


export default class AllevamentiIndexPage extends Vue {

    $refs: any = {
        savedDialog: Vue,
        removedDialog: Vue,
        editazioneAllevamentoModal: Vue,
        confirmDeleteDialog: Vue
    }

    private allevamentiService: AllevamentiService;
    private allevamento: Allevamento;
    private idAllevamentoDaRimuovere!: number;

    public tableOptions: any = {};
    public allevamenti: Allevamento[] = [];


    constructor() {
        super();

        this.allevamentiService = new AllevamentiService();
        this.allevamento = new Allevamento();

    }

    public mounted() {

        this.initTable();

        this.allevamentiService.index()
            .then(response => {
                this.allevamenti = response.data;
            });

    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.allevamentiService.details(rowId)
                .then(response => {
                    this.allevamento = response.data;
                    this.$refs.editazioneAllevamentoModal.openAllevamento(this.allevamento);
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idAllevamentoDaRimuovere = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }

    // nuovo allevamento
    public onAdd() {

        this.allevamento = new Allevamento();
        this.$refs.editazioneAllevamentoModal.open();

    }


    // rimozione acquirente
    public onRemove() {

        this.allevamentiService.delete(this.idAllevamentoDaRimuovere)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }

    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};

        options.columns = [];

        options.columns.push({ data: "Id" });
        options.columns.push({ data: "RagioneSociale" });
        options.columns.push({ data: "CUAA" });
        options.columns.push({ data: "CodiceAsl" });



  

            options.columns.push({
                render: function (data: any, type: any, row: any) {

                    var html = '<div class="text-center">';


                        html += '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-edit"></i></a>';

    
                        html += '<a class="pl-3 delete" title="elimina" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-trash-alt"></i></a>';

                    html += '</div>';

                    return html;
                },
                className: "edit-column",
                orderable: false
            });



        this.tableOptions = options;
    }

}

</script>