<template>
    
    <div id="index-dispositivi-page">

        <!-- Pannello editazione dettaglio -->
        <editazione-dispositivo-modal ref="editazioneDispositivoModal" :dispositivo="dispositivo" v-on:saved="onPopupSave" ></editazione-dispositivo-modal>


        <!-- Tabella -->
        <data-table :options="tableOptions" :rows="dispositivi" v-on:data-loaded="onDataLoaded">

            <!-- Colonne -->
            <template slot="thead">
                <th>IMEI</th>
                <th>Trasportatore</th>
                <th>Attivo</th>
                <th>Data registrazione</th>
                <th>Data ultimo download</th>
                <th>Data ultimo upload</th>
                <th>Versione</th>
                <th></th>
            </template>

        </data-table>


    </div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

    import DataTable from "../../components/dataTable.vue";
    import EditazioneDispositivoModal from "../dispositivi/edit.vue";

    import Waiter from "../../components/waiter.vue";

    import { Dispositivo } from "../../models/dispositivo.model";
    import { DispositiviService } from "../../services/dispositivi.service";


    declare module 'vue/types/vue' {
        interface Vue {
            open(): void
            close(): void
        }
    }

    @Component({
        components: {
            Waiter,
            DataTable,
            EditazioneDispositivoModal
        }
    })


    export default class DispositiviIndexPage extends Vue {


        $refs: any = {
            waiter: Vue,
            editazioneDispositivoModal: Vue
        };

        private dispositiviService: DispositiviService;

        public tableOptions: any = {};
        public dispositivi: Dispositivo[] = [];
        public dispositivo: Dispositivo = new Dispositivo();

        constructor() {
            super();

            this.dispositiviService = new DispositiviService();
        }

        public mounted() {
            this.initTable();
            this.dispositiviService.index()
                .then(response => {
                    this.dispositivi = response.data;
                });
        }


        // inizializzazione tabella
        private initTable(): void {
            var options: any = {};
            options.columns = [];

            options.columns.push({ data: "Id" });
            options.columns.push({ data: "Trasportatore_RagioneSociale" });
            options.columns.push({ data: "Attivo" });
            options.columns.push({ data: "DataRegistrazione" });
            options.columns.push({ data: "DataUltimoDownload" });
            options.columns.push({ data: "DataUltimoUpload" });
            options.columns.push({ data: "VersioneApp" });
            
            options.columns.push({
                render: function(data: any, type: any, row: any) {

                    var html = '<div class="text-center">';
                    html += '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-edit"></i></a>';
                    html += '</div>';

                    return html;
                },
                className: "edit-column",
                orderable: false            
            });

            this.tableOptions = options;

        }

        // Evento fine generazione tabella
        public onDataLoaded() {

            $('.edit').click((event) => {

                var element = $(event.currentTarget);
                var rowId = $(element).data("row-id");

                this.dispositiviService.details(rowId)
                    .then(response => {
                        this.dispositivo = response.data;
                        this.$refs.editazioneDispositivoModal.open();
                    });
            });
        }

        // evento chiusura popup
        public onPopupSave() {
            window.location = window.location;
        }

    }

</script>