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
                <button :disabled="idProduttore == ''" class="btn btn-success" v-on:click="onCaricaClick" >Carica</button>
            </div>
            
        </div>

                        
    </div>

    <div class="container-fluid pt-3" >

        <ul class="nav nav-tabs" id="tabWrapper">
          <li class="active">
            <a data-toggle="tab" class="nav-link active" href="#analisi-quantitativa">Analisi quantitativa</a>
          </li>
          <li>
            <a data-toggle="tab" class="nav-link" href="#analisi-qualitativa">Analisi qualitativa</a>
          </li>
          <li>
            <a data-toggle="tab" class="nav-link" href="#analisi-comparativa">Analisi comparativa</a>
          </li>          
        </ul>

        <div class="tab-content">

            <!-- Analisi Quantitativa -->        
            <div id="analisi-quantitativa" class="tab-pane fade show active">
                <analisi-quantitativa ref="analisiQuantitativa" ></analisi-quantitativa>
            </div>

            <!-- Analisi Qualitativa -->        
            <div id="analisi-qualitativa" class="tab-pane fade">
                <analisi-qualitativa ref="analisiQualitativa" ></analisi-qualitativa>                
            </div>

            <!-- Analisi Comparativa -->        
            <div id="analisi-comparativa" class="tab-pane fade">
                <analisi-comparativa ref="analisiComparativa" ></analisi-comparativa>            
            </div>            

        </div>
    </div>
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
        this.keepSelectedTabOnRefresh();

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

    // Mantengo la tab selezionata per il refresh della pagina
    public keepSelectedTabOnRefresh() {
        $("ul.nav-tabs > li > a").on("shown.bs.tab", function(e) {
            window.location.hash = String($(e.target).attr("href"));
        });

        $('#tabWrapper a[href="' + window.location.hash + '"]').tab("show");
    }    

}
</script>