<template>
    <table class="table table-striped table-bordered table-margin">
        <thead class="thead-dark">
            <tr>
                <th scope="col">Commessa</th>
                <th scope="col">Cliente</th>                        
                <th scope="col">Sede</th>
                <th scope="col">Sottolinea</th>
                <th scope="col">Stato</th>
                <th scope="col">Ricavo</th>
                <th scope="col">Costo</th>
                <th scope="col">Margine</th>
                <th scope="col">Margine percentuale</th>
            </tr>
        </thead>
        <tbody>                
            <tr v-for="(workOrder, index) in rows" :key="index">
                <td><a v-bind:href="'/workOrders/edit?id=' + workOrder.Id" >{{workOrder.Code}}</a></td>
                <td>{{workOrder.Customer_FullBusinessName}}</td>
                <td>{{workOrder.HeadQuarter_Description}}</td>
                <td>{{workOrder.BusinessSubline_Description}}</td>
                <td>{{workOrder.Status}}</td>
                <td class="text-right" >{{workOrder.Revenue | currency}}</td>
                <td class="text-right" >{{workOrder.Cost | currency}}</td>
                <td class="text-right" >{{workOrder.Margin | currency}}</td>
                <td class="text-right" >{{workOrder.MarginPerc | percentage}}</td>
            </tr>
        </tbody>
    </table>    
</template>

<script lang="ts">

import { Prop, Watch, Emit, Vue, Component } from "vue-property-decorator";

import { WidgetsService } from "../../services/widgets.service";

import { TableWidgetModel, RowWidgetModel } from "../../models/widgets/admin/tableWidget.model";
import { WidgetParameters } from '../../models/widget.model';

@Component({
    filters: {
        currency(value: number) {
            return value ?  value.toFixed(2).toLocaleString() + ' €' : '-';
        },
        percentage(value: any) {
            return value ?  value.toFixed(2) + ' %' : '-';
        }
    }
})
export default class TableWidget extends Vue {

    public widgetService: WidgetsService = new WidgetsService();
    public rows: RowWidgetModel[] = [];
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
                this.rows = (response.data as TableWidgetModel).Rows;
            });
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