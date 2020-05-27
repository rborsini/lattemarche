<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- modale modifica/inserisci prelievo -->
    <editazione-prelievo-modal
      ref="editazionePrelievoModal"
      :prelievo-latte="prelievoSelezionato"
      v-on:salvato="$refs.savedDialog.open()"
    ></editazione-prelievo-modal>

    <!-- Pannello Salvataggio prelievo-->
    <notification-dialog
      ref="savedDialog"
      :title="'Conferma salvataggio'"
      :message="'Il prelievo è stato salvato correttamente'"
    ></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog
      ref="removedDialog"
      :title="'Conferma rimozione'"
      :message="'Prelievo rimosso correttamente'"
    ></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog
      ref="confirmDeleteDialog"
      :title="'Conferma eliminazione'"
      :message="'Sei sicuro di voler rimuovere il prelievo selezionato?'"
      v-on:confirmed="onRemove()"
    ></confirm-dialog>

    <!-- Box ricerca -->
    <div class="jumbotron">
      <!-- Campi ricerca -->

      <!-- dal / al -->
      <div class="row pt-1">
        <label class="col-1">Dal:</label>
        <div class="col-3">
          <datepicker class="form-control" :value.sync="parameters.DataPeriodoInizio_Str" />
        </div>

        <label class="col-1">Al:</label>
        <div class="col-3">
          <datepicker class="form-control" :value.sync="parameters.DataPeriodoFine_Str" />
        </div>
      </div>

      <!-- Allevatore / Trasportatore / Tipo latte -->
      <div class="row pt-1" v-if="canSearchAllevatore || canSearchTrasportatore">
        <label class="col-1" v-if="canSearchAllevatore">Allevatore:</label>
        <div class="col-3" v-if="canSearchAllevatore">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="allevatori.Items"
            :value.sync="parameters.IdAllevamento"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>

        <label class="col-1" v-if="canSearchTrasportatore">Trasportatore:</label>
        <div class="col-3" v-if="canSearchTrasportatore">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="trasportatore.Items"
            :value.sync="parameters.IdTrasportatore"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>

        <label class="col-1">Tipo latte:</label>
        <div class="col-3">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="tipiLatte.Items"
            :value.sync="parameters.IdTipoLatte"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>
      </div>

      <!-- Acquirente / Cessionario / Destinatario -->
      <div
        class="row pt-1"
        v-if="canSearchAcquirente || canSearchCessionario || canSearchDestinatario"
      >
        <label class="col-1" v-if="canSearchAcquirente">Acquirente:</label>
        <div class="col-3" v-if="canSearchAcquirente">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="acquirente.Items"
            :value.sync="parameters.IdAcquirente"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>

        <label class="col-1" v-if="canSearchCessionario">Cessionario:</label>
        <div class="col-3" v-if="canSearchCessionario">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="cessionario.Items"
            :value.sync="parameters.IdCessionario"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>

        <label class="col-1" v-if="canSearchDestinatario">Destinatario:</label>
        <div class="col-3" v-if="canSearchDestinatario">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="destinatario.Items"
            :value.sync="parameters.IdDestinatario"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>
      </div>

      <!-- Bottoni di ricerca -->
      <div class="row pt-3">
        <div class="col-12">
          <button v-on:click="onCercaClick" class="float-right btn btn-primary" role="button">Cerca</button>
          <button
            v-on:click="onAnnullaClick"
            class="float-right btn btn-primary mr-2"
            href="#"
            role="button"
          >Annulla</button>
        </div>
      </div>
    </div>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="prelievi" v-on:data-loaded="onDataLoaded">
      <!-- Toolbox -->
      <template slot="toolbox">
        <div class="toolbox text-right">
          <div class="btn-group">
            <button class="btn btn-primary float-right mr-3" v-on:click="onAdd()">Aggiungi</button>
            <button
              type="button"
              class="btn btn-primary dropdown-toggle"
              data-toggle="dropdown"
              aria-haspopup="true"
              aria-expanded="false"
            >Esporta in excel</button>
            <div class="dropdown-menu">
              <a
                class="dropdown-item"
                v-on:click="downloadExcel('allevatori')"
                style="cursor: pointer"
              >Allevatori</a>
              <a
                class="dropdown-item"
                v-on:click="downloadExcel('giornalieri')"
                style="cursor: pointer"
              >Giornalieri</a>
            </div>
          </div>
        </div>
      </template>

      <!-- Colonne -->
      <template slot="thead">
        <th>Allevamento</th>
        <th>Data prelievo</th>
        <th>Data consegna</th>
        <th>Ult mung.</th>
        <th>Kg</th>
        <th>Lt</th>
        <th>Temp.</th>
        <th>Trasportatore</th>
        <th>Acquirente</th>
        <th>Destinatario</th>
        <th>Tipo Latte</th>
        <th v-if="canEdit || canRemove"></th>
      </template>

      <!-- foot -->
      <template slot="tfoot">
        <th colspan="4" style="text-align:right">Totale:</th>
        <th>{{totale_prelievi_kg}} kg</th>
        <th>{{totale_prelievi_lt}} lt</th>
        <th></th>
        <th></th>
        <th></th>
        <th></th>
        <th></th>
        <th v-if="canEdit || canRemove"></th>
      </template>
    </data-table>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import Waiter from "../../components/waiter.vue";
import DataTable from "../../components/dataTable.vue";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import EditazionePrelievoModal from "../prelieviLatte/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { Trasportatore } from "../../models/trasportatore.model";
import { Acquirente } from "../../models/acquirente.model";
import { Destinatario } from "../../models/destinatario.model";
import {
  PrelievoLatte,
  PrelieviLatteSearchModel
} from "../../models/prelievoLatte.model";
import { TipoLatte } from "../../models/tipoLatte.model";

import { PrelieviLatteService } from "../../services/prelieviLatte.service";
import { DropdownService } from "../../services/dropdown.service";
import { UrlService } from "@/services/url.service";

import { Dropdown, DropdownItem } from "../../models/dropdown.model";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}

@Component({
  components: {
    Select2,
    ConfirmDialog,
    Datepicker,
    EditazionePrelievoModal,
    NotificationDialog,
    DataTable,
    Waiter
  }
})
export default class PrelieviLatteIndexPage extends Vue {
  $refs: any = {
    savedDialog: Vue,
    editazionePrelievoModal: Vue,
    confirmDeleteDialog: Vue,
    removedDialog: Vue,
    waiter: Vue
  };

  private prelieviLatteService: PrelieviLatteService;
  public dropdownService: DropdownService;

  public tableOptions: any = {};
  public allevatori: Dropdown = new Dropdown();
  public tipiLatte: Dropdown = new Dropdown();
  public trasportatore: Dropdown = new Dropdown();
  public destinatario: Dropdown = new Dropdown();
  public acquirente: Dropdown = new Dropdown();
  public cessionario: Dropdown = new Dropdown();
  public prelievi: PrelievoLatte[] = [];
  private idPrelievoDaEliminare!: number;

  public parameters: PrelieviLatteSearchModel = new PrelieviLatteSearchModel();

  public prelievoSelezionato: PrelievoLatte;

  public canAdd: boolean = false;
  public canEdit: boolean = false;
  public canRemove: boolean = false;
  public canSearchAllevatore: boolean = false;
  public canSearchTrasportatore: boolean = false;
  public canSearchAcquirente: boolean = false;
  public canSearchDestinatario: boolean = false;
  public canSearchCessionario: boolean = false;

  public totale_prelievi_kg: number = 0;
  public totale_prelievi_lt: number = 0;

  constructor() {
    super();

    this.prelieviLatteService = new PrelieviLatteService();
    this.prelievoSelezionato = new PrelievoLatte();
    this.dropdownService = new DropdownService();

    this.canAdd = $("#canAdd").val() == "true";
    this.canEdit = $("#canEdit").val() == "true";
    this.canRemove = $("#canRemove").val() == "true";
    this.canSearchAllevatore = $("#canSearchAllevatore").val() == "true";
    this.canSearchTrasportatore = $("#canSearchTrasportatore").val() == "true";
    this.canSearchAcquirente = $("#canSearchAcquirente").val() == "true";
    this.canSearchDestinatario = $("#canSearchDestinatario").val() == "true";
    this.canSearchCessionario = $("#canSearchCessionario").val() == "true";
  }

  public mounted() {
    this.initTable();
    this.loadAllevatori();
    this.loadTipiLatte();
    this.loadTrasportatori();
    this.loadDestinatari();
    this.loadAcquirenti();
    this.loadCessionari();
    this.initSearchBox();
  }

  // Pulizia selezione
  public onAnnullaClick() {
    this.initSearchBox();
  }

  // Ricerca
  public onCercaClick() {
    this.$refs.waiter.open();
    this.totale_prelievi_kg = 0;
    this.totale_prelievi_lt = 0;

    this.prelieviLatteService.getPrelievi(this.parameters).then(response => {
      this.prelievi = response.data;

      for (let prelievo of this.prelievi) {
        this.totale_prelievi_kg += prelievo.Quantita;
        this.totale_prelievi_lt += prelievo.QuantitaLitri;
      }

      this.$refs.waiter.close();
    });
  }

  // Evento fine generazione tabella
  public onDataLoaded() {
    $(".edit").click(event => {
      var element = $(event.currentTarget);
      var rowId = $(element).data("row-id");

      this.prelieviLatteService.getPrelievo(rowId).then(response => {
        this.prelievoSelezionato = response.data;
        this.$refs.editazionePrelievoModal.open();
      });
    });

    $(".delete").click(event => {
      var element = $(event.currentTarget);
      this.idPrelievoDaEliminare = $(element).data("row-id");

      this.$refs.confirmDeleteDialog.open();
    });
  }

  // nuova autocisterna
  public onAdd() {
    this.prelievoSelezionato = new PrelievoLatte();
    this.$refs.editazionePrelievoModal.open();
  }

  // rimozione autocisterna
  public onRemove() {
    this.prelieviLatteService
      .delete(this.idPrelievoDaEliminare)
      .then(response => {
        this.$refs.removedDialog.open();
      });
  }

  // inizializzazione tabella
  private initTable(): void {
    var options: any = {};
    options.responsive = true;
    options.columns = [];
    (options.columnDefs = [
      {
        targets: [0, 7, 8, 9, 10],
        createdCell: function(
          td: any,
          cellData: any,
          rowData: any,
          row: any,
          col: any
        ) {
          $(td).attr("title", cellData);
        }
      }
    ]),
      options.columns.push({ className: "truncate", data: "Allevamento" });
    options.columns.push({
      className: "truncate",
      width: "85px",
      data: "DataPrelievoStr"
    });
    options.columns.push({
      className: "truncate",
      width: "85px",
      data: "DataConsegnaStr"
    });
    options.columns.push({
      className: "truncate",
      width: "85px",
      data: "DataUltimaMungituraStr"
    });
    options.columns.push({
      className: "truncate",
      width: "30px",
      data: "Quantita"
    });
    options.columns.push({
      className: "truncate",
      width: "30px",
      data: "QuantitaLitri"
    });
    options.columns.push({
      className: "truncate",
      width: "30px",
      data: "Temperatura"
    });
    options.columns.push({ className: "truncate", data: "Trasportatore" });
    options.columns.push({ className: "truncate", data: "Acquirente" });
    options.columns.push({ className: "truncate", data: "Destinatario" });
    options.columns.push({ className: "truncate", data: "DescrizioneLatte" });

    var ce = this.canEdit;
    var cr = this.canRemove;

    if (ce || cr) {
      options.columns.push({
        render: function(data: any, type: any, row: any) {
          var html = '<div class="text-center">';

          if (ce)
            html +=
              '<a class="edit" title="modifica" style="width: 30px;cursor: pointer;" data-row-id="' +
              row.Id +
              '" ><i class="far fa-edit"></i></a>';

          if (cr)
            html +=
              '<a class="pl-3 delete" title="elimina" style="width: 30px;cursor: pointer;" data-row-id="' +
              row.Id +
              '" ><i class="far fa-trash-alt"></i></a>';

          html += "</div>";

          return html;
        },
        width: "60px",
        className: "edit-column",
        orderable: false
      });
    }

    options.orderFixed = [0, "asc"];

    options.rowGroup = {
      startRender: null,
      endRender: function(rows: any, group: any) {
        var prelieviSumKg = rows
          .data()
          .pluck("Quantita")
          .reduce(function(a: number, b: number) {
            return a + b * 1;
          }, 0);

        var prelieviSumLitri = rows
          .data()
          .pluck("QuantitaLitri")
          .reduce(function(a: number, b: number) {
            return a + b * 1;
          }, 0);

        return $("<tr/>")
          .append(
            '<td colspan="4">' + group + " (" + rows.count() + " prelievi)</td>"
          )
          .append("<td>" + prelieviSumKg.toFixed(0) + " Kg </td>")
          .append("<td>" + prelieviSumLitri.toFixed(0) + " l </td>")
          .append('<td colspan="6" />');
      },
      dataSrc: "Allevamento"
    };

    this.tableOptions = options;
  }

  // inizializzazione parametri di ricerca
  private initSearchBox() {
    this.parameters = new PrelieviLatteSearchModel();
    this.parameters.DataPeriodoFine_Str = this.formatDate(new Date());
    this.parameters.DataPeriodoInizio_Str = this.formatDate(
      this.subtractMonth(new Date())
    );
  }

  private formatDate(date: Date): string {
    var returnDate = "";

    var dd = date.getDate();
    var mm = date.getMonth() + 1; //because January is 0!
    var yyyy = date.getFullYear();

    if (dd < 10) {
      returnDate += `0${dd}-`;
    } else {
      returnDate += `${dd}-`;
    }

    if (mm < 10) {
      returnDate += `0${mm}-`;
    } else {
      returnDate += `${mm}-`;
    }
    returnDate += yyyy;
    return returnDate;
  }

  // caricamento allevatori
  private async loadAllevatori(): Promise<void> {
    const dd = await this.dropdownService.getAllevamenti();
    if (dd.data != null) {
      this.allevatori = dd.data;
    }
  }

  // caricamento tipi latte
  private async loadTipiLatte(): Promise<void> {
    const dd = await this.dropdownService.getTipiLatte();
    if (dd.data != null) {
      this.tipiLatte = dd.data;
    }
  }

  // caricamento trasportatori
  private async loadTrasportatori() {
    const dd = await this.dropdownService.getTrasportatori();
    if (dd.data != null) {
      this.trasportatore = dd.data;
    }
  }

  // caricamento destinatari
  private async loadDestinatari() {
    const dd = await this.dropdownService.getDestinatari();

    if (dd.data != null) {
      this.destinatario = dd.data;
    }
  }

  // caricamento acquirenti
  private async loadAcquirenti() {
    const dd = await this.dropdownService.getAcquirenti();

    if (dd.data != null) {
      this.acquirente = dd.data;
    }
  }

  // caricamento cessionari
  private async loadCessionari() {
    const dd = await this.dropdownService.getCessionari();

    if (dd.data != null) {
      this.cessionario = dd.data;
    }
  }

  //Esportazione excel
  public downloadExcel(tipo: string) {
    UrlService.redirect(
      "/api/prelieviLatte/excel" +
        tipo +
        "?" +
        this.parameters.ToUrlQueryString()
    );
  }

  private subtractMonth(date: Date): Date {
    var days = 0;
    //get month ritorna un numero da 0 a 11. Dovendo considerare il mese precedente
    //da sottrarre, per comodità aggiungo un numero al case, considerando quindi lo 0 come dicembre
    switch (date.getMonth()) {
      case 4: //Aprile
      case 6: //Giugno
      case 9: //Settembre
      case 11: {
        //Novembre
        days = 30;
        break;
      }
      case 2: {
        //febbraio
        if (date.getFullYear() % 4 != 0) {
          //anno non bisestile
          days = 28;
        } else {
          days = 29;
        }
        break;
      }
      default: //altri mesi
      {
        days = 31;
        break;
      }
    }
    date.setDate(date.getDate() - days);
    return date;
  }
}
</script>

<style>
table.dataTable {
  width: 100% !important;
}
td.truncate {
  max-width: 50px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.toolbox .dropdown-menu {
  left: -10px !important;
}
</style>