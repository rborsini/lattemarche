<template>
  <div class="container-fluid">
    <!-- <waiter ref="waiter"></waiter> -->
    <!-- Summary -->
    <div class="row pt-4">
      <div class="col-12">
        <!-- <h3>Progressivo</h3> -->
      </div>
    </div>
    <div class="row">
      <div class="col-xs-12 col-md-4 mt-4">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <h3 class="card-title col-7 pt-2">Settimanale</h3>
              <h1 class="card-text col-5 text-center">
                <span v-if="!(sommarioWidgetModel.Qta_Settimanale >= 0)">
                  <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Loading...</span>
                  </div>
                </span>
                <i18n-n
                  v-if="sommarioWidgetModel.Qta_Settimanale"
                  :value="sommarioWidgetModel.Qta_Settimanale"
                />
              </h1>
            </div>
          </div>
        </div>
      </div>

      <div class="col-xs-12 col-md-4 mt-4">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <h3 class="card-title col-7 pt-2">Mensile</h3>
              <h1 class="card-text col-5 text-center">
                <span v-if="!(sommarioWidgetModel.Qta_Mensile >= 0)">
                  <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Loading...</span>
                  </div>
                </span>
                <i18n-n
                  v-if="sommarioWidgetModel.Qta_Mensile >= 0"
                  :value="sommarioWidgetModel.Qta_Mensile"
                />
              </h1>
            </div>
          </div>
        </div>
      </div>

      <div class="col-xs-12 col-md-4 mt-4">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <h3 class="card-title col-4 pt-2">Annuale</h3>
              <h1 class="card-text col-8 text-center">
                <span v-if="!(sommarioWidgetModel.Qta_Annuale >= 0)">
                  <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Loading...</span>
                  </div>
                </span>
                <i18n-n
                  v-if="sommarioWidgetModel.Qta_Annuale"
                  :value="sommarioWidgetModel.Qta_Annuale"
                />
              </h1>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row p-3">
      <div class="col-md-6 p-5">
        <div class="row">
          <div class="col-12">
            <h3>Tipologia di latte</h3>
          </div>
        </div>
        <highcharts v-if="tipoLatteOptions" :options="tipoLatteOptions"></highcharts>
      </div>
      <div class="col-md-6 p-5">
        <div class="row">
          <div class="col-12">
            <h3>Acquirenti</h3>
          </div>
        </div>
        <highcharts v-if="acquirentiOptions" :options="acquirentiOptions"></highcharts>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

// import Waiter from "@/components/waiter.vue";

import DashboardService from "@/services/dashboard.service";
import SommarioWidgetModel from "@/models/sommarioWidget.model";
import GraficoWidgetModel, { SerieModel } from "@/models/graficoWidget.model";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}

@Component({
  components: {
    // Waiter
  }
})
export default class DashboardPage extends Vue {
  // $refs: any = {
  //   waiter: Vue
  // };
  private dashboardService: DashboardService;

  public sommarioWidgetModel: SommarioWidgetModel;
  public tipiLatteWidgetModel: GraficoWidgetModel;
  public acquirentiWidgetModel: GraficoWidgetModel;

  public tipoLatteOptions: any;
  public acquirentiOptions: any;

  constructor() {
    super();

    this.dashboardService = new DashboardService();

    this.sommarioWidgetModel = new SommarioWidgetModel();
    this.tipiLatteWidgetModel = new GraficoWidgetModel();
    this.acquirentiWidgetModel = new GraficoWidgetModel();

    this.tipoLatteOptions = this.createChartWidget(this.tipiLatteWidgetModel);
    this.acquirentiOptions = this.createChartWidget(this.acquirentiWidgetModel);
  }

  async mounted() {
    this.loadSommario();
    this.loadTipiLatte();
    this.loadAcquirenti();
  }

  private async loadSommario() {
    const sommarioRequest = await this.dashboardService.sommario();
    // Popolo le card
    this.sommarioWidgetModel = sommarioRequest.data;
  }

  private async loadTipiLatte() {
    const tipiLatteRequest = await this.dashboardService.tipiLatte();
    // Grafico Tipi di latte
    this.tipiLatteWidgetModel = new GraficoWidgetModel(
      tipiLatteRequest.data.ValoriAsseX,
      tipiLatteRequest.data.Serie
    );

    this.tipoLatteOptions = this.createChartWidget(this.tipiLatteWidgetModel);
  }

  private async loadAcquirenti() {
    const acquirentiRequest = await this.dashboardService.acquirenti();
    // Grafico Acquirenti
    this.acquirentiWidgetModel = new GraficoWidgetModel(
      acquirentiRequest.data.ValoriAsseX,
      acquirentiRequest.data.Serie
    );

    this.acquirentiOptions = this.createChartWidget(this.acquirentiWidgetModel);
  }

  private createChartWidget(model: GraficoWidgetModel) {
    return {
      chart: {
        backgroundColor: "rgba(0,0,0,0)",
        plotBorderWidth: null,
        plotShadow: false,
        type: "column"
      },
      title: {
        text: "",
        style: {
          display: "none"
        }
      },
      exporting: { enabled: false },
      xAxis: {
        categories: model.toHighchartsLabels()
      },
      yAxis: { title: { text: "Kg" }, min: 0 },
      lang: {
        noData: "Dati in caricamento..."
      },
      plotOptions: {
        column: {
          stacking: "normal",
          dataLabels: {
            enabled: false
          }
        }
      },
      series: model.toHighchartsSerie()
    };
  }
}
</script>

<style scoped>
.card-body {
  background-color: rgba(0, 123, 255, 0.25);
  border: none;
}

.spinner-border {
  border: 0.1em solid currentColor;
  border-right-color: transparent;
}
</style>