<template>
    <div class="container-fluid">

        <div v-if="isAuthenticated && isAuthorized" >

            <!-- Filtri -->
            <div class="row">
                <label class="col-1">Anno:</label>
                <div class="col-1">
                    <select2 class="form-control" :allowClear="false" :placeholder="'-'" :options="years.Items" :value.sync="year" :value-field="'Value'" :text-field="'Text'" />
                </div>
                
                <label class="col-1">Sede:</label>
                <div class="col-1">
                    <select2 class="form-control" :placeholder="'-'" :options="headQuarters.Items" :value.sync="headQuarterId" :value-field="'Value'" :text-field="'Text'" /> 
                </div>

                <label class="col-1">Sottolinea:</label>
                <div class="col-1">
                    <select2 class="form-control" :placeholder="'-'" :options="businessSublines.Items" :value.sync="businessLineId" :value-field="'Value'" :text-field="'Text'" /> 
                </div>
                <label class="col-1">Stato:</label>
                <div class="col-1">
                    <select2 class="form-control" :placeholder="'-'" :options="statuses.Items" :value.sync="statusId" :value-field="'Value'" :text-field="'Text'" /> 
                </div>
                <label class="col-1">Cliente:</label>
                <div class="col-2">
                    <select2 class="form-control" ajax="true" ref="customerSelect" :placeholder="''" :url="'/api/customers/search'" :value.sync="customerId" value-field="Id" text-field="FullBusinessName" />
                </div>                                
            </div>
                
            <!-- Summary -->
            <div class="row">
                <div class="col-12">
                    <summary-widget :widgetId="idWidgetSummary" :year="year" :headQuarter="headQuarterId" :businessLine="businessLineId" :status="statusId" :customer="customerId" ></summary-widget>
                </div>            
            </div>

            <!-- Chart -->
            <div class="row">
                <div class="col-12">
                    <chart-widget :widgetId="idWidgetYearly" :year="year" :headQuarter="headQuarterId" :businessLine="businessLineId" :status="statusId" :customer="customerId" ></chart-widget>
                </div>            
            </div>

            <!-- Tabella -->
            <div class="row">
                <div class="col-12">
                    <table-widget :widgetId="idWidgetWorkOrderSummary" :year="year" :headQuarter="headQuarterId" :businessLine="businessLineId" :status="statusId" :customer="customerId" ></table-widget>
                </div>            
            </div>

        </div>

        <div v-else >

            <div class="container">
                <div class="row">
                    <div class="col-sm-12">
                        <div class="card ">
                            <img class="card-img" src="Content/img/tuv-austria-800x400-neutral.jpg" alt="TUV Austria Italia">
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>

</template>

<script lang="ts">

    import { Component, Vue } from "vue-property-decorator";

    import { Chart } from 'highcharts-vue'
    import $ from 'jquery';

    // Componenti
    import Select2 from "../../components/select2.vue";    
    
    import SummaryWidget from "../../widgets/admin/summaryWidget.vue";
    import ChartWidget from "../../widgets/admin/chartWidget.vue";
    import TableWidget from "../../widgets/admin/tableWidget.vue";

    // Servizi
    import { DashboardsService } from "../../services/dashboards.service";
    import { PermissionsService } from '../../services/permissions.service';
    import { DropdownService } from "@/services/dropdown.service";

    // Modelli
    import { Dropdown, DropdownItem } from "@/models/dropdown.model";
    import { Dashboard } from '../../models/dashboard.model';
    import { Widget, WidgetParameters } from '../../models/widget.model';

    declare module "vue/types/vue" {
    interface Vue {
        open(): void;
        close(): void;
    }
    }

    @Component({
    components: {
        Select2,
        SummaryWidget,
        ChartWidget,
        TableWidget
    }
    })
    export default class App extends Vue {
        $refs: any = {    
            customerSelect: Vue    
        }

        public dashboardsService: DashboardsService = new DashboardsService();
        public dropdownService: DropdownService = new DropdownService();

        public isAuthenticated: boolean = false;
        public isAuthorized: boolean = false;

        public dashboard: Dashboard = new Dashboard();

        public years: Dropdown = new Dropdown();
        public headQuarters: Dropdown = new Dropdown();
        public businessSublines: Dropdown = new Dropdown();
        public statuses: Dropdown = new Dropdown();

        public idWidgetSummary: number = 0;        
        public idWidgetWorkOrderSummary: number = 0;        
        public idWidgetYearly: number = 0;

        public year: number = 0;
        public headQuarterId: number = -1;
        public businessLineId: string = "";
        public statusId: string = "";
        public customerId?: number = -1;

        constructor() {
            super();

        }

        public mounted() {

            this.isAuthenticated = PermissionsService.isAuthenticated();

            if(this.isAuthenticated) {
                // inizializzazione dropdowns            
                this.initDropdowns();            
            }

            // caricamento dashboard in base all'utente
            this.dashboardsService.index()
            .then(response => {                
                
                // selezione della dashboard di default
                this.dashboard = response.data.find(d => d.IsDefault) as  Dashboard;
                if(this.dashboard != null) {
                    
                    this.isAuthorized = true;
                                        
                    // Summary
                    var widgetSummary = this.dashboard.Widgets.find(w => w.WidgetTypeId == "AdminSummaryWidget") as Widget;                    
                    this.idWidgetSummary = widgetSummary != null ? widgetSummary.Id : 0;

                    // Yearly
                    var widgetYearly = this.dashboard.Widgets.find(w => w.WidgetTypeId == "AdminTimeWidget") as Widget;
                    this.idWidgetYearly = widgetYearly != null ? widgetYearly.Id : 0;

                    // WorkOrders 
                    var widgetWorkOrderSummary = this.dashboard.Widgets.find(w => w.WidgetTypeId == "AdminDetailsWidget") as Widget;
                    this.idWidgetWorkOrderSummary = widgetWorkOrderSummary != null ? widgetWorkOrderSummary.Id : 0;
 
                }

            })

        }

        // inizializzazione dropdown
        private initDropdowns() {

            for(var i = 2018; i <= (new Date()).getFullYear(); i++){                
                var item: DropdownItem = new DropdownItem(i.toString(), i.toString());
                this.years.Items.push(item);
            }   

            this.year = (new Date()).getFullYear();

            this.dropdownService.getHeadQuarters().then(response => {
                this.headQuarters = response.data
            });

            this.dropdownService.getBusinessSublines().then(response => {
                this.businessSublines = response.data
            });

            this.dropdownService.getWorkOrderStatuses().then(response => {
                this.statuses = response.data;
            });

        }

    }

</script>

<style lang="scss">

    .card {
        border: 0px;
    }

</style>