<template>
    
    <div id="index-ruoli-page">

        <div class="container-fluid">
            <div class="row">
                <div class="col-12 text-right pb-3">
                    <a class="btn btn-success" href="/ruoli/new">Aggiungi</a>
                </div>
            </div>
        </div>

        <!-- Tabella -->
        <data-table :options="tableOptions" :rows="ruoli" >

            <!-- Colonne -->
            <template slot="thead">
                <th>Nome</th>
                <th>Descrizione</th>
                <th></th>
            </template>

        </data-table>

    </div>


</template>

<script lang="ts">

    import { Component, Vue } from "vue-property-decorator";

    import DataTable from "../../components/dataTable.vue";

    import { Ruolo } from "../../models/ruolo.model";
    import { RuoliService } from "../../services/ruoli.service";

    @Component({
        components: {
            DataTable
        }
    })


    export default class RuoliIndexPage extends Vue {


        private ruoliService: RuoliService;
        private idAcquirenteDaRimuovere!: number;

        public tableOptions: any = {};
        public ruoli: Ruolo[] = [];
        public canAdd: boolean = false;
        public canEdit: boolean = false;
        public canRemove: boolean = false;

        constructor() {
            super();

            this.ruoliService = new RuoliService();
        }

        public mounted() {
            this.initTable();

            this.ruoliService.getRuoli()
                .then((response: { data: Ruolo[]; }) => {
                    this.ruoli = response.data;
                });
        }

        // inizializzazione tabella
        private initTable(): void {

            var options: any = {};

            options.columns = [];

            options.columns.push({ data: "Codice" });
            options.columns.push({ data: "Descrizione" });


            options.columns.push({
                render: function (data: any, type: any, row: any) {

                    var html = '<div class="text-center">';

                    html += '<a class="edit" title="modifica" style="cursor: pointer;" ><i class="far fa-edit"></i></a>';                    

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