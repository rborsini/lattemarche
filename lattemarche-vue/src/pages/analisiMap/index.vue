<template>
    <div class="container-fluid">

        <!-- waiter -->
        <waiter ref="waiter"></waiter>

        <div class="row">
            <div class="col-4 p-4">

                <!-- Periodo -->
                <div class="row p-2">
                    <label class="col-2">Periodo:</label>
                    <div class="col-5">
                        <datepicker class="form-control" :value.sync="parameters.DataInizio_Str" />
                    </div>
                    <div class="col-5">
                        <datepicker class="form-control" :value.sync="parameters.DataFine_Str" />
                    </div>                    
                </div>

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

                <!-- Giri -->
                <div class="row p-2">
                    <label class="col-2">Giro:</label>
                    <div class="col-10">
                        <select2 class="form-control" :placeholder="'-'" :options="giri.Items"
                            :value.sync="parameters.CodiceGiro" :value-field="'Value'" :text-field="'Text'" />
                    </div>
                </div>
                
                <!-- Trasportatori -->
                <div class="row p-2">
                    <label class="col-2">Trasportatore:</label>
                    <div class="col-10">
                        <select2 class="form-control" :placeholder="'-'" :options="trasportatori.Items"
                            :value.sync="parameters.IdTrasportatore" :value-field="'Value'" :text-field="'Text'" />
                    </div>
                </div>  
                
                <!-- Categorie colore -->
                <div class="row p-2">
                    <label class="col-2">Raggruppa per:</label>
                    <div class="col-10">
                        <select2 class="form-control" :placeholder="'-'" :options="categorie.Items"
                            :value.sync="parameters.AggregazioneColore" :value-field="'Value'" :text-field="'Text'" />
                    </div>
                </div>                  

                <!-- Bottone cerca -->
                <div class="row p-2">
                    <div class="col-12">
                        <button v-on:click="onCercaClick" class="float-right btn btn-success"
                            role="button">Cerca</button>
                    </div>
                </div>

                <!-- Legenda -->
                <div class="mt-4 pl-3" >
                    <div v-for="(row, rowIndex) in widgetModel.Legend" :key="rowIndex" class="row mt-1" >
                        <div class="offset-2 col-1" v-bind:style="{ backgroundColor: row.Color }" ></div>
                        <div class="col-9">{{ row.Label }}</div>
                    </div>
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
import { Dropdown, DropdownItem } from "@/models/dropdown.model";
import WidgetsService from "@/services/widgets.service";
import { DateService } from "@/services/date.service";

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
    public dateService: DateService;

    public parameters!: WidgetMapSearchModel;
    public widgetModel: WidgetMap = new WidgetMap();

    public acquirente: Dropdown = new Dropdown();
    public tipiLatte: Dropdown = new Dropdown();
    public giri: Dropdown = new Dropdown();
    public trasportatori: Dropdown = new Dropdown();
    public categorie: Dropdown = new Dropdown();

    public center: Position = new Position(43.4, 13.5);
    public zoom: number = 7;

    constructor() {
        super();

        this.parameters = new WidgetMapSearchModel();
        this.dropdownService = new DropdownService();
        this.widgetsService = new WidgetsService();
        this.dateService = new DateService();
    }

    public mounted() {

        // this.parameters.DataFine_Str = DateService.formatDate(new Date());
        // this.parameters.DataInizio_Str = DateService.formatDate(DateService.subtractMonth(new Date()));

        this.parameters.DataInizio_Str = '01-01-2022';
        this.parameters.DataFine_Str = '01-01-2023';

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
        this.dropdownService.getDropdowns("acquirenti|tipiLatte|giri|trasportatori")
            .then(response => {
                this.acquirente = response.data["acquirenti"] as Dropdown;
                this.tipiLatte = response.data["tipiLatte"] as Dropdown;
                this.giri = response.data["giri"] as Dropdown;
                this.trasportatori = response.data["trasportatori"] as Dropdown;
                this.$refs.waiter.close();
            });

        this.categorie.Items.push(new DropdownItem('giro', 'Giro'));
        this.categorie.Items.push(new DropdownItem('acquirente', 'Acquirente'));
        this.categorie.Items.push(new DropdownItem('tipoLatte', 'Tipo latte'));
    }

    // inizializzazione mappa
    private initMap() {
        this.$refs.mapViewer.initMap(this.center, this.zoom, []);
    }



}
</script>