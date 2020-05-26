<template>

    <div class="container-fluid p-0">

        <!-- waiter -->
        <waiter ref="waiter"></waiter>

        <!-- Pannello notifica rimozione -->
        <notification-dialog ref="removedDialog"
                            :title="'Conferma rimozione'"
                            :message="'Utente rimosso correttamente'"
                            v-on:ok="window.location = '/Utenti'"></notification-dialog>

        <!-- Pannello modale conferma eliminazione -->
        <confirm-dialog ref="confirmDeleteDialog"
                        :title="'Conferma eliminazione'"
                        :message="'Sei sicuro di voler rimuovere l\'utente selezionato?'"
                        v-on:confirmed="onRemove()"></confirm-dialog>

        <!-- Box ricerca -->
        <div class="jumbotron">

            <div class="row pt-1">

                <label class="col-1">Tipo profilo:</label>
                <div class="col-3">
                    <select2
                        class="form-control"
                        :placeholder="'-'"
                        :options="profilo.Items"
                        :value-field="'Value'"
                        :text-field="'Text'"
                        v-on:value-changed="onProfiloValueChanged"
                    />
                </div>

            </div>

        </div>


        <!-- Tabella -->
        <data-table :options="tableOptions" :rows="utenti" v-on:data-loaded="onDataLoaded">

            <!-- Toolbox -->
            <template slot="toolbox" v-if="canAdd">
                <a class="toolbox btn btn-primary float-right" href="/utenti/edit">Aggiungi</a>
            </template>

            <!-- Colonne -->
            <template slot="thead">
                <th>Ragione sociale</th>
                <th>Nome</th>
                <th>Cognome</th>
                <th>Username</th>
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
    import ConfirmDialog from "../../components/confirmDialog.vue";
    import NotificationDialog from "../../components/notificationDialog.vue";
    import Waiter from "../../components/waiter.vue";

    import { Utente } from "../../models/utente.model";
    import { UtentiService } from "../../services/utenti.service";
import { Dropdown, DropdownItem } from '../../models/dropdown.model';
import { DropdownService } from '../../services/dropdown.service';


    // declare module 'vue/types/vue' {
    //     interface Vue {
    //         open(): void
    //         close(): void
    //     }
    // }

    @Component({
        components: {
            NotificationDialog,
            ConfirmDialog,
            Waiter,
            DataTable
        }
    })


    export default class UtentiIndexPage extends Vue {


        $refs: any = {
            savedDialog: Vue,
            confirmDeleteDialog: Vue,
            waiter: Vue,   
            removedDialog: Vue
        }


        private dropdownService: DropdownService;
        private utentiService: UtentiService;
        public utente: Utente;
        private idUtente!: number;

        public profilo: Dropdown = new Dropdown();

        public tableOptions: any = {};
        public utenti: Utente[] = [];
        public canAdd: boolean = false;
        public canEdit: boolean = false;
        public canRemove: boolean = false;

        constructor() {
            super();

            this.dropdownService = new DropdownService();
            this.utentiService = new UtentiService();
            this.utente = new Utente();

            this.canAdd = $('#canAdd').val() == "true";
            this.canEdit = $('#canEdit').val() == "true";
            this.canRemove = $('#canRemove').val() == "true";
        }

        public mounted() {
            this.initTable();

            this.$refs.waiter.open();
            this.utentiService.index()
                .then(response => {
                    this.utenti = response.data;
                    this.$refs.waiter.close();
                });

            this.dropdownService.getProfili().then(response => {
                this.profilo = response.data;
            });

        }

        // Evento fine generazione tabella
        public onDataLoaded() {

            $('.delete').click((event) => {

                var element = $(event.currentTarget);
                this.idUtente = $(element).data("row-id");

                this.$refs.confirmDeleteDialog.open();

            });

        }

        // selezione profilo di filtro
        public onProfiloValueChanged(value: string){

            console.log("profilo selezionato", value);
        }

        // rimozione utente
        public onRemove() {

            this.utentiService.delete(this.idUtente)
                .then(response => {
                    this.$refs.removedDialog.open();
                });
        }


        // inizializzazione tabella
        private initTable(): void {
            var options: any = {};
            options.columns = [];

            options.columns.push({ data: "RagioneSociale" });
            options.columns.push({ data: "Nome" });
            options.columns.push({ data: "Cognome" });
            options.columns.push({ data: "Username" });

            var ce = this.canEdit;
            var cr = this.canRemove;

            if (ce || cr) {

                options.columns.push({
                    render: function (data: any, type: any, row: any) {

                        var html = '<div class="text-center">';

                        if (ce)
                            html += '<a class="edit" title="modifica" href="/utenti/edit?id=' + row.Id + '" ><i class="far fa-edit"></i></a>';

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