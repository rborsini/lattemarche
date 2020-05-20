<template>
    

    <div id="tipilatte-page" class="container-fluid p-0">

        <!-- Pannello editazione dettaglio -->
        <editazione-tipo-latte-modal ref="editazioneTipoLatteModal"
                                    :tipo-latte="tipoLatte"
                                    v-on:salvato="$refs.savedDialog.open()"></editazione-tipo-latte-modal>

        <!-- Pannello notifica salvatagggio -->
        <notification-dialog ref="savedDialog"
                            :title="'Conferma salvataggio'"
                            :message="'Tipo latte salvato correttamente'"
                            v-on:ok="window.location = '/TipiLatte'"></notification-dialog>

        <!-- Pannello notifica rimozione -->
        <notification-dialog ref="removedDialog"
                            :title="'Conferma rimozione'"
                            :message="'Tipo latte rimosso correttamente'"
                            v-on:ok="window.location = '/TipiLatte'"></notification-dialog>

        <!-- Pannello modale conferma eliminazione -->
        <confirm-dialog ref="confirmDeleteDialog"
                        :title="'Conferma eliminazione'"
                        :message="'Sei sicuro di voler rimuovere il tipo latte selezionato?'"
                        v-on:confirmed="onRemove()"></confirm-dialog>

        <!-- Tabella -->
        <data-table :options="tableOptions" :rows="tipiLatte" v-on:data-loaded="onDataLoaded">

            <!-- Toolbox -->
            <template slot="toolbox" v-if="canAdd">
                <button class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>
            </template>

            <!-- Colonne -->
            <template slot="thead">
                <th>#</th>
                <th>Descrizione</th>
                <th>Sigla</th>
                <th>Fattore conversione LT -> KG</th>
                <th v-if="canEdit || canRemove"></th>
            </template>

        </data-table>
    </div>

</template>
<script lang="ts">

    import Vue from "vue";
    import Component from "vue-class-component";
    import { Prop, Watch, Emit } from "vue-property-decorator";

    import DataTable from "../../components/dataTable.vue";
    import EditazioneTipoLatteModal from "../tipiLatte/edit.vue";
    import NotificationDialog from "../../components/notificationDialog.vue";
    import ConfirmDialog from "../../components/confirmDialog.vue";

    import { TipoLatte } from "../../models/tipoLatte.model";
    import { TipiLatteService } from "../../services/tipiLatte.service";


    declare module 'vue/types/vue' {
        interface Vue {
            open(): void
            close(): void
        }
    }

    @Component({
        el: '#tipilatte-page',
        components: {
            ConfirmDialog,
            NotificationDialog,
            EditazioneTipoLatteModal,
            DataTable
        }
    })


    export default class TipiLatteIndexPage extends Vue {


        $refs: any = {
            savedDialog: Vue,
            editazioneTipoLatteModal: Vue,
            confirmDeleteDialog: Vue,
            removedDialog: Vue
        }

        private tipiLatteService: TipiLatteService;
        public tipoLatte: TipoLatte;
        private idTipoLatte!: number;

        public tableOptions: any = {};
        public tipiLatte: TipoLatte[] = [];
        public canAdd: boolean = false;
        public canEdit: boolean = false;
        public canRemove: boolean = false;

        constructor() {
            super();

            this.tipiLatteService = new TipiLatteService();
            this.tipoLatte = new TipoLatte();

            this.canAdd = $('#canAdd').val() == "true";
            this.canEdit = $('#canEdit').val() == "true";
            this.canRemove = $('#canRemove').val() == "true";
        }

        public mounted() {
            this.initTable();

            this.tipiLatteService.index()
                .then(response => {
                    this.tipiLatte = response.data;
                });
        }

        // Evento fine generazione tabella
        public onDataLoaded() {

            $('.edit').click((event) => {

                var element = $(event.currentTarget);
                var rowId = $(element).data("row-id");

                this.tipiLatteService.details(rowId)
                    .then(response => {
                        this.tipoLatte = response.data;
                        this.$refs.editazioneTipoLatteModal.open();
                    });

            });

            $('.delete').click((event) => {

                var element = $(event.currentTarget);
                this.idTipoLatte = $(element).data("row-id");

                this.$refs.confirmDeleteDialog.open();

            });

        }


        // nuova tipoLatte
        public onAdd() {

            this.tipoLatte = new TipoLatte();
            this.$refs.editazioneTipoLatteModal.open();

        }

        // rimozione tipoLatte
        public onRemove() {

            this.tipiLatteService.delete(this.idTipoLatte)
                .then(response => {
                    this.$refs.removedDialog.open();
                });
        }


        // inizializzazione tabella
        private initTable(): void {
            var options: any = {};
            options.columns = [];

            options.columns.push({ data: "Id" });
            options.columns.push({ data: "Descrizione" });
            options.columns.push({ data: "DescrizioneBreve" });
            options.columns.push({ data: "FattoreConversione" });

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