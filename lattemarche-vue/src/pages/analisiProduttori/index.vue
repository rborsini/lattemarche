<template>
  <div class="container-fluid">

    <!-- Filtri -->
    <div class="row pt-3">

        <div class="col-8 row">

            <label class="col-1">Produttore:</label>
            <div class="col-3">
                <select2 class="form-control" :placeholder="'-'" :options="produttori.Items" :value.sync="idProduttore" :value-field="'Value'" :text-field="'Text'" /> 
            </div>

            <label class="col-1">Periodo:</label>
            <div class="col-3">
                <daterangepicker v-on:apply="onPeriodSelected" ></daterangepicker>
            </div>

            <div class="col-1">
                <button class="btn btn-success" v-on:click="onCaricaClick" >Carica</button>
            </div>
            
        </div>

                        
    </div>

    <!-- Analisi Quantitativa -->
    <analisi-quantitativa ref="analisiQuantitativa" ></analisi-quantitativa>

    <!-- Analisi Qualitativa -->
    <analisi-qualitativa ref="analisiQualitativa" ></analisi-qualitativa>    

    <!-- Analisi Comparativa -->
    <analisi-comparativa ref="analisiComparativa" ></analisi-comparativa>

  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import Select2 from "../../components/select2.vue";
import Daterangepicker from "../../components/daterangepicker.vue";
import AnalisiQuantitativa from "./analisiQuantitativa.vue";
import AnalisiQualitativa from "./analisiQualitativa.vue";
import AnalisiComparativa from "./analisiComparativa.vue";

import { DropdownItem, Dropdown } from "../../models/dropdown.model";
import { DropdownService } from "../../services/dropdown.service";
import WidgetsService from "../../services/widgets.service";
import { DateService } from "../../services/date.service";
import { Period } from "../../models/period.model";

@Component({
    components: {
        Select2,
        Daterangepicker,
        AnalisiQuantitativa,
        AnalisiQualitativa,
        AnalisiComparativa
    }
})
export default class AnalisiProduttoriIndexPage extends Vue {
    $refs: any = {
        analisiQuantitativa: Vue,
        analisiQualitativa: Vue,
        analisiComparativa: Vue
    };

    public dropdownService: DropdownService = new DropdownService();
    public widgetsService: WidgetsService = new WidgetsService();

    public produttori: Dropdown = new Dropdown();
    public idProduttore: number = 0;
    public period: Period = new Period();

    constructor() {
        super();
    }

    public mounted() {
        this.loadDropdown();
        this.idProduttore = 100;
        this.onCaricaClick();
    }


    // Click bottone cerca
    public onCaricaClick() {
        this.$refs.analisiQuantitativa.load(this.idProduttore, this.period.From, this.period.To);
        this.$refs.analisiQualitativa.load(this.idProduttore, this.period.From, this.period.To);
        this.$refs.analisiComparativa.load(this.idProduttore, this.period.From, this.period.To);
    }

    // selezione periodo 
    private onPeriodSelected(period: Period) {
        this.period = period;
    }

    // caricamento allevamenti
    private loadDropdown() {
        this.dropdownService.getAllevamenti().then((response) => {
            this.produttori = response.data;
        });        
    }

}
</script>