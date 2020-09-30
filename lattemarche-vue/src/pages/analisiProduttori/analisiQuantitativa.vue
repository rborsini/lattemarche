<template>
    <div>
        <div class="row pl-4 pt-4">
            <h2 class="pl-2" >Analisi quantitativa</h2>
        </div>

        <div v-show="loading" class="loader row justify-content-center" >
            <div class="col-1">
                <span></span>
                <span></span>
                <span></span>
            </div>
        </div>
        <div v-show="!loading" >
            <div class="row pt-4">
                <!-- Andamento mensile -->
                <div class="col-6">
                    <highcharts :options="andamentoMensileOptions"></highcharts>
                </div>

                <!-- Andamento giornaliero -->
                <div class="col-6">
                    <highcharts :options="andamentoGiornalieroOptions"></highcharts>
                </div>
            </div>

            <!-- Tabella -->
            <data-table :options="tableOptions" :rows="model.Records" >
                <template slot="thead">
                    <th>Data</th>
                    <th>Qta [kg]</th>
                    <th>Qta [lt]</th>
                    <th>Trasportatore</th>
                    <th>Acquirente</th>
                    <th>Destinatario</th>
                    <th>Tipo latte</th>
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
import { AnalisiQuantitativaWidget, Record } from "@/models/analisiQuantitativaWidget.model";
import GraficoWidgetModel from "@/models/graficoWidget.model";

@Component({
    components: {
        DataTable
    }
})
export default class AnalisiQuantitativa extends Vue {

    public widgetsService: WidgetsService = new WidgetsService();

    public model: AnalisiQuantitativaWidget = new AnalisiQuantitativaWidget();
    
    public andamentoMensileOptions: any = { title: { text: '' } };
    public andamentoGiornalieroOptions: any = { title: { text: '' } };
    public tableOptions: any = {};
    public loading: boolean = false;

    constructor() {
        super();        
    }

    mounted() {
        this.tableOptions = this.initTable();
    }

    // caricamento dati
    load(idAllevamento: number, da: string, a: string) {
        
        this.loading = true;
        this.widgetsService.analisiQuantitativa(idAllevamento, da, a)
            .then((response) => {
                this.loading = false;
                this.model = response.data;

                this.andamentoMensileOptions = this.initChart('column', 'Andamento mensile', this.model.AndamentoMensile);
                this.andamentoGiornalieroOptions = this.initChart('line', 'Andamento giornaliero', this.model.AndamentoGiornaliero);
            });

    }

    // inizializzazione tabella
    private initTable() : any {

        var options: any = {};
        options.responsive = true;
        options.columns = [];
        options.pageLength = 10;
        options.lengthMenu = [[10, 25, 50, -1], [10, 25, 50, "Tutto"]]

        options.columns.push({ 
            data: {
                _: "Data_Str",
                sort: "Data"
            }
        });

        options.columns.push({ data: "Qta_Kg" });
        options.columns.push({ data: "Qta_Lt" });
        options.columns.push({ data: "Trasportatore" });
        options.columns.push({ data: "Acquirente" });
        options.columns.push({ data: "Destinatario" });
        options.columns.push({ data: "TipoLatte" });

        return options;
    }


    // inizializzazione grafico
    private initChart(chartType: string, title: string, model: GraficoWidgetModel): any {

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
            legend: { enabled: false },
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