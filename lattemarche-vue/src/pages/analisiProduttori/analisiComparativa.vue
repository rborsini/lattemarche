<template>
    <div>
        <div class="row pl-3 pt-3">
            <h2>Analisi comparativa</h2>
        </div>

        <!-- grafici -->
        <div class="row">
            
            <!-- Bubble -->
            <div class="col-6">
                <highcharts :options="bubbleOptions"></highcharts>
            </div>

            <!-- Spider -->
            <div class="col-6">
                <highcharts :options="spiderOptions"></highcharts>
            </div>
        </div>

    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import HighchartsVue from 'highcharts-vue';
import Highcharts from 'highcharts';

import WidgetsService from "@/services/widgets.service";
import { AnalisiComparativaWidget } from "@/models/analisiComparativaWidget.model";
import GraficoWidgetModel, { BollaModel } from "@/models/graficoWidget.model";

@Component({})
export default class AnalisiQualitativa extends Vue {

    public widgetsService: WidgetsService = new WidgetsService();

    public model: AnalisiComparativaWidget = new AnalisiComparativaWidget();

    public bubbleOptions: any = {};
    public spiderOptions: any = {};

    constructor() {
        super();        
    }

    // caricamento dati
    load(idAllevamento: number, da: string, a: string) {
        
        this.widgetsService.analisiComparativa(idAllevamento, da, a)
            .then((response) => {
                this.model = response.data;
                this.bubbleOptions = this.initBubbleChart(this.model.BubbleChart);
                this.spiderOptions = this.initSpiderChart(this.model.SpiderChart);
            });

    }

    // inizializzazione grafico a bolle
    private initBubbleChart(model: GraficoWidgetModel): any {

        var serie = { data: [] as any };

        for(var i = 0; i < model.Serie[0].Bolle.length; i++) {
            var bolla = model.Serie[0].Bolle[i];
            serie.data.push({
                name: bolla.Nome,
                x: bolla.X,
                y: bolla.Y,
                z: bolla.Z
            });
        }

        return {
            chart: {
                type: 'bubble',
                plotBorderWidth: 1,
                zoomType: 'xy'                
            },
            title: {
                text: "",
                style: { display: "none" },
            },
            exporting: { enabled: false },
            legend: { enabled: false },
            xAxis: {
                categories: model.ValoriAsseX,
                title: { text: "Grasso" }
            },
            yAxis: { 
                startOnTick: false,
                endOnTick: false,                
                title: { text: "Proteine" } 
            },
            tooltip: {
                formatter: function(e: any) {
                    var point: any = (this as any).point;

                    var html = '';
                    html += 'Grasso:    <b>' + point.x + '</b><br/>';
                    html += 'Proteine:  <b>' + point.y + '</b><br/>';
                    html += 'Quantità:  <b>' + point.z + ' kg</b><br/>';

                    return html;
                }
            },            
            plotOptions: {
                series: {
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    }
                }
            },
            series: [ serie ]
        };
    }

    // inizializzazione grafico spider
    private initSpiderChart(model: GraficoWidgetModel): any {

        var series = [] as any;

        for(var i = 0; i < model.Serie.length; i++) {
            series.push({
                name: model.Serie[i].Nome,
                data: model.Serie[i].Valori,
                pointPlacement: 'on'
            });
        }

        return {
            chart: {
                polar: true,
                type: 'line',
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
            yAxis: { min: 0 },
            lang: {
                noData: "Dati in caricamento...",
            },
            series: series
            // [
            //     {
            //         name: 'Giulio',
            //         data: [ 1, 2, 3, 4 ],
            //         pointPlacement: 'on'
            //     },
            //     {
            //         name: 'altri',
            //         data: [ 4, 3, 2, 1 ],
            //         pointPlacement: 'on'
            //     },
            // ]
        };
    }    
}
</script>