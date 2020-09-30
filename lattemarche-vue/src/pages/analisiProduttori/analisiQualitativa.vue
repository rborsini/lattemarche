<template>
    <div>
        <div class="row pl-4 pt-4">
            <h2 class="pl-2" >Analisi quantitativa</h2>
        </div>

        <div v-if="loading" class="loader row justify-content-center" >
            <div class="col-1">
                <span></span>
                <span></span>
                <span></span>
            </div>
        </div>
        <div v-show="!loading" >            
            <div class="row pt-4">
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

    public grassoProteineOptions: any =  { title: { text: '' } };
    public caricaBattericaCelluleSomaticheOptions: any =  { title: { text: '' } };
    public tableOptions: any = {};
    public loading: boolean = false;

    public yAxis_GrassoProteine = [
        { title: { text: "% p/V" }, min: 0 },
        { title: { text: "% p/V" }, min: 0, opposite: true }
    ]; 

    public yAxis_CaricaBatterica = [
        { title: { text: 'UFCx1000/mL' }, min: 0 },
        { title: { text: 'cell x 1000/mL' }, min: 0, opposite: true },
    ]

    constructor() {
        super();        
    }

    mounted() {
        this.tableOptions = this.initTable();
    }    

    // caricamento dati
    load(idAllevamento: number, da: string, a: string) {
        
        this.loading = true;
        this.widgetsService.analisiQualitativa(idAllevamento, da, a)
            .then((response) => {
                this.loading = false;
                this.model = response.data;
                this.grassoProteineOptions = this.initChart('line', 'Grasso / Proteine', this.yAxis_GrassoProteine, this.model.Grasso_Proteine);
                this.caricaBattericaCelluleSomaticheOptions = this.initChart('line', 'Carica batterica / Cellule somatiche', this.yAxis_CaricaBatterica, this.model.CaricaBatterica_CelluleSomatiche);
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
    private initChart(chartType: string, title: string, yAxis: any, model: GraficoWidgetModel): any {
        return {
            chart: {
                backgroundColor: "rgba(0,0,0,0)",
                plotBorderWidth: null,
                plotShadow: false,
                zoomType: 'x',
                type: chartType,
            },
            title: { text: title },
            exporting: { enabled: false },
            xAxis: { categories: model.ValoriAsseX },
            yAxis: yAxis,
            plotOptions: {
                column: {
                    stacking: "normal",
                    dataLabels: { enabled: false },
                },
            },
            series: model.Serie.map(
                        elem => ({
                            name: elem.Nome,
                            data: elem.Valori,
                            yAxis: elem.Y_Axis
                        })
            )
        };
    }
}
</script>