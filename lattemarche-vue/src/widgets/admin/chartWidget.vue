<template>

    <div class="row p-3">
        <div class="col-sm-6 p-5" >
            <highcharts :options="costChartOptions"></highcharts>
        </div>         
        <div class="col-sm-6 p-5" >
            <highcharts :options="marginChartOptions"></highcharts>
        </div>                 
    </div>

</template>

<script lang="ts">

import { Prop, Watch, Emit, Vue, Component } from "vue-property-decorator";

import { WidgetsService } from "../../services/widgets.service";

import { ChartWidgetModel } from "../../models/widgets/admin/chartWidget.model";
import { WidgetParameters } from '../../models/widget.model';

@Component({
  components: {}
})
export default class ChartWidget extends Vue {

    public model: ChartWidgetModel = new ChartWidgetModel();

    public widgetService: WidgetsService = new WidgetsService();

    public params: WidgetParameters = new WidgetParameters();

    public costSerie: any = { name: 'Costi', data: [] };
    public revenueSerie: any = { name: 'Ricavi', data: [] };
    public marginSerie: any = { name: 'Margine', data: [] };

    public costChartOptions: any = {
        chart: {
            backgroundColor: 'rgba(0,0,0,0)',                    
            plotBorderWidth: null,
            plotShadow: false,
            type: 'line'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },      
        exporting: { enabled: false },          
        xAxis: { categories: [] },
        yAxis: { title: { text: '€' }, min: 0 },
        series: [ this.costSerie, this.revenueSerie ]
    };

    public marginChartOptions: any = {
        chart: {
            backgroundColor: 'rgba(0,0,0,0)',                    
            plotBorderWidth: null,
            plotShadow: false,
            type: 'line'
        },
        title: {
            text: '',
            style: {
                display: 'none'
            }
        },      
        exporting: { enabled: false },          
        xAxis: { categories: [] },
        yAxis: { title: { text: '€' }, min: 0 },
        series: [ this.marginSerie ]
    };

    @Prop() public widgetId!: number;
    @Prop() public year!: number;
    @Prop() public headQuarter!: number;
    @Prop() public businessLine!: string;
    @Prop() public status!: string;
    @Prop() public customer!: number;

    @Watch('widgetId')
    onWidgetIdChanged() {        
        this.params.WidgetId = this.widgetId;
        this.loadWidget();
    }

    @Watch('year')
    onYearChanged() {
        this.params.Year = this.year;
        this.loadWidget();
    }

    @Watch('headQuarter')
    onHeadQuarterChanged() {
        this.params.HeadQuarterId = this.headQuarter;
        this.loadWidget();
    }

    @Watch('businessLine')
    onBusinessLineChanged() {
        this.params.BusinessLineId = this.businessLine;
        this.loadWidget();
    }

    @Watch('status')
    onStatusChanged() {
        this.params.Status = this.status;
        this.loadWidget();
    }

    @Watch('customer')
    onCustomerChanged() {
        this.params.CustomerId = this.customer;
        this.loadWidget();
    }

    constructor() {
        super();
    }

    public mounted() {
        this.params.WidgetId = this.widgetId;
        this.params.Year = this.year;
        this.params.HeadQuarterId = this.headQuarter;
        this.params.BusinessLineId = this.businessLine;
        this.params.Status = this.status;
        this.params.CustomerId = this.customer;        
        this.loadWidget();
    }

    private loadWidget() {

        if(this.widgetId != 0) {
            this.widgetService.details(this.params)
            .then(response => {
                this.model = response.data as ChartWidgetModel;
                                
                this.bindCostChart(this.model);
                this.bindMarginChart(this.model);

            });
        }

    }

    private bindCostChart(model: ChartWidgetModel) : void {

        this.costChartOptions.xAxis.categories = [];
        this.costChartOptions.series[0].data = [];                
        this.costChartOptions.series[1].data = [];

        for(var i = 0; i < model.Months.length; i++) {
            var month = model.Months[i];
            
            this.costChartOptions.xAxis.categories.push(month.Year + ' - ' + month.Month);
            this.costChartOptions.series[0].data.push(month.Cost);
            this.costChartOptions.series[1].data.push(month.Revenue);

        }

    }

    private bindMarginChart(model: ChartWidgetModel) : void {

        this.marginChartOptions.xAxis.categories = [];
        this.marginChartOptions.series[0].data = [];                

        for(var i = 0; i < model.Months.length; i++) {
            var month = model.Months[i];
            
            this.marginChartOptions.xAxis.categories.push(month.Year + ' - ' + month.Month);
            this.marginChartOptions.series[0].data.push(month.Margin_Euro);

        }

    }

}

</script>

<style lang="scss">

    .table-margin {
        margin-top: 15px;
        margin-right: 0;
    }

</style>