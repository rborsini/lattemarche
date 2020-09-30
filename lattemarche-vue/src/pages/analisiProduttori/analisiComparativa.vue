<template>
    <div>
        <div class="row pl-4 pt-4">
            <h2 class="pl-2" >Analisi comparativa</h2>
        </div>

        <div v-show="loading" class="loader row justify-content-center" >
            <div class="col-1">
                <span></span>
                <span></span>
                <span></span>
            </div>
        </div>

        <div v-show="!loading" class="row pt-3">            
            <!-- Bubble -->
            <div class="col-8">
                <highcharts :options="bubbleOptions"></highcharts>
            </div>
            <!-- Spider -->
            <div class="col-4">
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
export default class AnalisiComparativa extends Vue {

    public widgetsService: WidgetsService = new WidgetsService();

    public model: AnalisiComparativaWidget = new AnalisiComparativaWidget();

    public bubbleOptions: any = { title: { text: '' } };
    public spiderOptions: any = { title: { text: '' } };
    public loading: boolean = false;

    constructor() {
        super();        
    }

    // caricamento dati
    load(idAllevamento: number, da: string, a: string) {
        
        this.loading = true;
        this.widgetsService.analisiComparativa(idAllevamento, da, a)
            .then((response) => {
                this.loading = false;                
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
                color: bolla.Colore,
                x: bolla.X,
                y: bolla.Y,
                z: bolla.Z
            });
        }

        return {
            chart: {
                type: 'bubble',
                plotBorderWidth: 1,
                height: "600px",
                zoomType: 'xy'                
            },
            title: { text: 'Bubble chart' },
            exporting: { enabled: false },
            legend: { enabled: false },
            xAxis: {
                categories: model.ValoriAsseX,
                title: { text: "Grasso (% p/V)" }
            },
            yAxis: { 
                startOnTick: false,
                endOnTick: false,                
                title: { text: "Proteine (% p/V)" } 
            },
            tooltip: {
                formatter: function(e: any) {
                    var point: any = (this as any).point;

                    console.log("point", point);

                    var html = '';
                    html += '<b>' + point.name + '</b><br/><br/>';
                    html += 'Grasso:    <b>' + point.x.toFixed(2) + ' % p/V</b><br/>';
                    html += 'Proteine:  <b>' + point.y.toFixed(2) + ' % p/V</b><br/>';
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
                events: {
                    load: function() {
                        console.log("load");
                    }
                }           
            },
            title: { text: 'Spider chart' },            
            exporting: { enabled: false },
            xAxis: {
                categories: model.ValoriAsseX,
            },
            series: series
        };
    }    

}
</script>