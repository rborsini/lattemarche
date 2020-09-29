<template>
    <div>
        <div class="row pl-3 pt-3">
            <h2>Analisi quantitativa</h2>
        </div>

        <!-- grafici -->
        <div class="row">
            <!-- Grasso / Proteine -->
            <div class="col-6">
                <highcharts :options="grassoProteineOptions"></highcharts>
            </div>

            <!-- Carica batterica / Cellule somatiche -->
            <div class="col-6">
                <highcharts :options="caricaBattericaCelluleSomaticheOptions"></highcharts>
            </div>
        </div>

        <!-- Tabella -->
        <data-table :options="tableOptions" :rows="model.Records" >
            <template slot="thead">
                <th>Campione</th>
                <th>Data rapporto</th>
                <th>Data accettazione</th>
                <th>Data prelievo</th>
                <th>Grasso</th>
                <th>Proteine</th>
                <th>Carica batterica</th>
                <th>Cellule somatiche</th>
            </template>
        </data-table>

    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import HighchartsVue from 'highcharts-vue';
import Highcharts from 'highcharts';

import DataTable from "../../components/dataTable.vue";

import WidgetsService from "@/services/widgets.service";
import { AnalisiQualitativaWidget, Record } from "@/models/analisiQualitativaWidget.model";
import GraficoWidgetModel from "@/models/graficoWidget.model";

@Component({
    components: {
        DataTable
    }
})
export default class AnalisiQualitativa extends Vue {

    public widgetsService: WidgetsService = new WidgetsService();

    public model: AnalisiQualitativaWidget = new AnalisiQualitativaWidget();

    public grassoProteineOptions: any = {};
    public caricaBattericaCelluleSomaticheOptions: any = {};
    public tableOptions: any = {};

    constructor() {
        super();        
    }

    mounted() {
        this.tableOptions = this.initTable();
    }    

    // caricamento dati
    load(idAllevamento: number, da: string, a: string) {
        
        this.widgetsService.analisiQualitativa(idAllevamento, da, a)
            .then((response) => {
                this.model = response.data;
                this.grassoProteineOptions = this.initChart('line', this.model.Grasso_Proteine);
                this.caricaBattericaCelluleSomaticheOptions = this.initChart('line', this.model.CaricaBatterica_CelluleSomatiche);
            });

    }

    // inizializzazione tabella
    private initTable() : any {

        var options: any = {};
        options.responsive = true;
        options.columns = [];
        options.pageLength = 10;
        options.lengthMenu = [[10, 25, 50, -1], [10, 25, 50, "Tutto"]]

        options.columns.push({ data: "Campione" });

        options.columns.push({ 
            data: {
                _: "DataRapporto_Str",
                sort: "DataRapporto"
            }
        });

        options.columns.push({ 
            data: {
                _: "DataAccettazione_Str",
                sort: "DataAccettazione"
            }
        });
        
        options.columns.push({ 
            data: {
                _: "DataPrelievo_Str",
                sort: "DataPrelievo"
            }
        });        

        options.columns.push({ data: "Grasso" });
        options.columns.push({ data: "Proteine" });
        options.columns.push({ data: "CaricaBatterica" });
        options.columns.push({ data: "CelluleSomatiche" });

        return options;
    }

    // inizializzazione grafico
    private initChart(chartType: string, model: GraficoWidgetModel): any {
        return {
            chart: {
                backgroundColor: "rgba(0,0,0,0)",
                plotBorderWidth: null,
                plotShadow: false,
                type: chartType,
            },
            title: {
                text: "",
                style: {
                    display: "none",
                },
            },
            exporting: { enabled: false },
            xAxis: {
                categories: model.ValoriAsseX,
            },
            yAxis: { title: { text: "Kg" }, min: 0 },
            lang: {
                noData: "Dati in caricamento...",
            },
            plotOptions: {
                column: {
                    stacking: "normal",
                    dataLabels: {
                        enabled: false,
                    },
                },
            },
            series: model.Serie.map(
                        elem => ({
                            name: elem.Nome,
                            data: elem.Valori
                        })
            )
        };
    }
}
</script>