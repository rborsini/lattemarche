<template>
    <div class="container-fluid">

        <!-- waiter -->
        <waiter ref="waiter"></waiter>

        <div class="row">
            <div class="col-4 p-4">

                <!-- Acquirenti -->
                <div class="row p-2">
                    <label class="col-2">Acquirente:</label>
                    <div class="col-10">
                        <select2 class="form-control" :placeholder="'-'" :options="acquirente.Items"
                            :value.sync="parameters.IdAcquirente" :value-field="'Value'" :text-field="'Text'" />
                    </div>
                </div>

                <!-- Tipi latte -->
                <div class="row p-2">
                    <label class="col-2">Tipo latte:</label>
                    <div class="col-10">
                        <select2 class="form-control" :placeholder="'-'" :options="tipiLatte.Items"
                            :value.sync="parameters.IdTipoLatte" :value-field="'Value'" :text-field="'Text'" />
                    </div>
                </div>

                <!-- Bottone cerca -->
                <div class="row p-2">
                    <div class="col-12">
                        <button v-on:click="onCercaClick" class="float-right btn btn-success"
                            role="button">Cerca</button>
                    </div>
                </div>

                <!-- Legenda acquirenti -->
                <div class="mt-4 pl-3" >
                    <div v-for="(row, rowIndex) in widgetModel.AcquirentiLegend" :key="rowIndex" class="row mt-1" >
                        <div class="offset-2 col-1" v-bind:style="{ backgroundColor: row.Color }" ></div>
                        <div class="col-9">{{ row.Label }}</div>
                    </div>
                </div>


                <!-- Legenda tipi latte -->
                <div class="mt-4 pl-3" >
                    <div v-for="(row, rowIndex) in widgetModel.TipiLatteLegend" :key="rowIndex" class="row mt-1" >
                        <div class="offset-2 col-1" :style="{ 'border': '2px solid ' + row.Color }" ></div>
                        <div class="col-9">{{ row.Label }}</div>
                    </div>
<!-- 
                    <div v-for="(row, rowIndex) in widgetModel.TipiLatteLegend" :key="rowIndex" class="row" >
                        <div class="offset-3 col-6">{{ row.Label }}</div>
                        <div class="col-3">{{ row.Color }}</div>
                    </div> -->
                </div>

            </div>
            <div class="col-8 p-4">
                <mappa-allevamenti ref="mapViewer" style="height: 600px" />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Waiter from "../../components/waiter.vue";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import MappaAllevamenti from "../../components/mappaAllevamenti.vue";

import { DropdownService } from "../../services/dropdown.service";
import { Position } from "@/models/map.model";
import { WidgetMap, WidgetMapSearchModel } from "@/models/mapAllevamenti.model";
import { Dropdown } from "@/models/dropdown.model";
import WidgetsService from "@/services/widgets.service";

@Component({
    components: {
        Select2,
        Waiter,
        Datepicker,
        MappaAllevamenti
    }
})
export default class AnalisiMapIndexPage extends Vue {
    $refs: any = {
        waiter: Vue,
        mapViewer: Vue
    };

    public dropdownService: DropdownService;
    public widgetsService: WidgetsService;

    public parameters!: WidgetMapSearchModel;
    public widgetModel: WidgetMap = new WidgetMap();

    public acquirente: Dropdown = new Dropdown();
    public tipiLatte: Dropdown = new Dropdown();

    public center: Position = new Position(43.4, 13.5);
    public zoom: number = 8.5;

    constructor() {
        super();

        this.parameters = new WidgetMapSearchModel();
        this.dropdownService = new DropdownService();
        this.widgetsService = new WidgetsService();
    }

    public mounted() {
        this.loadDropdown();
        this.initMap();
    }

    // Ricerca
    public onCercaClick() {
        this.$refs.waiter.open();
        this.widgetsService.analisiMappa(this.parameters).then(response => {
            this.widgetModel = response.data;

            this.$refs.mapViewer.initMap(this.center, this.zoom, response.data.Markers);
            this.$refs.waiter.close();
        });
    }

    // load dropdown
    private loadDropdown() {
        this.$refs.waiter.open();
        this.dropdownService.getDropdowns("acquirenti|tipiLatte")
            .then(response => {
                this.acquirente = response.data["acquirenti"] as Dropdown;
                this.tipiLatte = response.data["tipiLatte"] as Dropdown;
                this.$refs.waiter.close();
            });
    }

    // inizializzazione mappa
    private initMap() {
        this.$refs.mapViewer.initMap(this.center, this.zoom, []);
    }

}
</script>