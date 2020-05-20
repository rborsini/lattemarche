<template>
 
    <div class="row pt-4">

        <div class="col-4">
            <div class="card">
            <div class="card-body">
                <div class="row">
                <h3 class="card-title col-7 pt-2">Ricavi</h3>
                <h1 class="card-text col-5 text-center text-success">{{model.Revenue | currency}}</h1>
                </div>
            </div>
            </div>
        </div>

        <div class="col-4">
            <div class="card">
            <div class="card-body">
                <div class="row">
                <h3 class="card-title col-7 pt-2">Costi</h3>
                <h1 class="card-text col-5 text-center text-danger">{{model.Cost | currency}}</h1>
                </div>
            </div>
            </div>
        </div>

        <div class="col-4">
            <div class="card">
            <div class="card-body">
                <div class="row">
                <h3 class="card-title col-4 pt-2">Margini</h3>
                <h1 class="card-text col-8 text-center" v-bind:class="{'text-success' : model.Margin_Euro > 0, 'text-danger' : model.Margin_Euro < 0}" >{{model.Margin_Euro | currency}} ({{model.Margin_Perc | percentage}})</h1>
                </div>
            </div>
            </div>
        </div>       
    </div>

 
</template>

<script lang="ts">

import { Prop, Watch, Emit, Vue, Component } from "vue-property-decorator";

import { WidgetsService } from "../../services/widgets.service";

import { SummaryWidgetModel } from "../../models/widgets/admin/summaryWidget.model";
import { WidgetParameters } from '../../models/widget.model';

@Component({
    filters: {
        currency(value: number) {
            return value ?  value.toFixed(0).toLocaleString() + ' €' : '-';
        },
        percentage(value: any) {
            return value ?  value.toFixed(1) + ' %' : '-';
        }
    }
})
export default class SummaryWidget extends Vue {

    public widgetService: WidgetsService = new WidgetsService();
    public model: SummaryWidgetModel = new SummaryWidgetModel();
    public params: WidgetParameters = new WidgetParameters();

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
                this.model = response.data as SummaryWidgetModel;
            });
        }

    }

}

</script>

<style lang="scss">

    .card-body {
        background-color: #F9F8FC;
    }

</style>