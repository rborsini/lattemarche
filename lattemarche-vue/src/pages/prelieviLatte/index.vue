<template>
  <div id="index-prelievi-latte-page">
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
      v-on:ok="window.location = '/Prelievi'"
    ></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog
      ref="removedDialog"
      :title="'Conferma rimozione'"
      :message="'Prelievo rimosso correttamente'"
      v-on:ok="window.location = '/Prelievi'"
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
      <!-- Allevatore / dal / al -->

      <div class="row pt-1">
        <!-- <label class="col-1" v-if="canSearchAllevatore">Allevatore:</label> -->

        <!-- <div class="col-3" v-if="canSearchAllevatore">
                <select2 class="form-control"
                         :placeholder="'-'"
                         :options="allevatori"
                         :value.sync="idAllevatoreSelezionato"
                         :value-field="'Id'"
                         :text-field="'RagioneSociale'" />
        </div>-->

        <label class="col-1">Dal:</label>

        <div class="col-3">
          <datepicker class="form-control" :value.sync="dal" />
        </div>

        <label class="col-1">Al:</label>

        <div class="col-3">
          <datepicker class="form-control" :value.sync="al" />
        </div>
      </div>

      <!-- Trasportatore / Acquirente / Destinatario -->

      <div
        class="row pt-1"
        v-if="canSearchTrasportatore || canSearchAcquirente || canSearchDestinatario"
      >
        <label class="col-1" v-if="canSearchTrasportatore">Trasportatore:</label>

        <div class="col-3" v-if="canSearchTrasportatore">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="trasportatore"
            :value.sync="idTrasportatoreSelezionato"
            :value-field="'Id'"
            :text-field="'Cognome'"
          />
        </div>

        <label class="col-1" v-if="canSearchAcquirente">Acquirente:</label>

        <div class="col-3" v-if="canSearchAcquirente">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="acquirente"
            :value.sync="idAcquirenteSelezionato"
            :value-field="'Id'"
            :text-field="'RagioneSociale'"
          />
        </div>

        <label class="col-1" v-if="canSearchDestinatario">Destinatario:</label>

        <div class="col-3" v-if="canSearchDestinatario">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="destinatario"
            :value.sync="idDestinatarioSelezionato"
            :value-field="'Id'"
            :text-field="'RagioneSociale'"
          />
        </div>
      </div>

      <div class="row pt-1">
        <label class="col-1">Tipo latte:</label>

        <div class="col-3">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="tipiLatte"
            :value.sync="idTipoLatteSelezionato"
            :value-field="'Id'"
            :text-field="'Descrizione'"
          />
        </div>
      </div>

      <!-- Bottoni di ricerca -->

      <div class="row pt-1">
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
        <div class="toolbox">
          <a
            download
            v-bind:href="'/prelievi/excel?idAllevamento=' + idAllevatoreSelezionato + '&idTrasportatore=' + idTrasportatoreSelezionato + '&idAcquirente=' + idAcquirenteSelezionato + '&idDestinatario=' + idDestinatarioSelezionato + '&dal=' + dal + '&al=' + al"
            v-on:click="onExportClick"
            class="float-right btn btn-primary"
          >Esporta excel</a>
          <button
            v-if="canAdd"
            class="btn btn-primary float-right mr-3"
            v-on:click="onAdd()"
          >Aggiungi</button>
        </div>
      </template>

      <!-- Colonne -->
      <template slot="thead">
        <th>Allevamento</th>
        <th>Data prelievo</th>
        <th>Data consegna</th>
        <th>Ultima mungitura</th>
        <th>Quant. Kg</th>
        <th>Quant. lt</th>
        <th>Temp. C°</th>
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

import DataTable from "../../components/dataTable.vue";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import EditazionePrelievoModal from "../prelieviLatte/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

// import { Allevatore } from "../../models/allevatore.model";
import { Trasportatore } from "../../models/trasportatore.model";
import { Acquirente } from "../../models/acquirente.model";
import { Destinatario } from "../../models/destinatario.model";
import { PrelievoLatte } from "../../models/prelievoLatte.model";
import { TipoLatte } from "../../models/tipoLatte.model";

// import { AllevatoriService } from "../../services/allevatori.service";
import { TrasportatoriService } from "../../services/trasportatori.service";
import { AcquirentiService } from "../../services/acquirenti.service";
import { DestinatariService } from "../../services/destinatari.service";
import { PrelieviLatteService } from "../../services/prelieviLatte.service";
import { TipiLatteService } from "../../services/tipiLatte.service";

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
    DataTable
  }
})
export default class PrelieviLatteIndexPage extends Vue {
  $refs: any = {
    savedDialog: Vue,
    editazionePrelievoModal: Vue,
    confirmDeleteDialog: Vue,
    removedDialog: Vue
  };

  private prelieviLatteService: PrelieviLatteService;
  // private allevatoriService: AllevatoriService;
  private tipiLatteService: TipiLatteService;
  public trasporatoriService: TrasportatoriService;
  public destinatariService: DestinatariService;
  public acquirentiService: AcquirentiService;

  public tableOptions: any = {};
  // public allevatori: Allevatore[] = [];
  public tipiLatte: TipoLatte[] = [];
  public trasportatore: Trasportatore[] = [];
  public destinatario: Destinatario[] = [];
  public acquirente: Acquirente[] = [];
  public prelievi: PrelievoLatte[] = [];
  private idPrelievoDaEliminare!: number;

  public idAllevatoreSelezionato: number = 0;
  public idTrasportatoreSelezionato: number = 0;
  public idDestinatarioSelezionato: number = 0;
  public idAcquirenteSelezionato: number = 0;
  public idTipoLatteSelezionato: number = 0;

  public prelievoSelezionato: PrelievoLatte;
  public dal: string = "";
  public al: string = "";

  public canAdd: boolean = false;
  public canEdit: boolean = false;
  public canRemove: boolean = false;
  public canSearchAllevatore: boolean = false;
  public canSearchTrasportatore: boolean = false;
  public canSearchAcquirente: boolean = false;
  public canSearchDestinatario: boolean = false;

  public totale_prelievi_kg: number = 0;
  public totale_prelievi_lt: number = 0;

  constructor() {
    super();

    this.prelieviLatteService = new PrelieviLatteService();
    // this.allevatoriService = new AllevatoriService();
    this.tipiLatteService = new TipiLatteService();
    this.prelievoSelezionato = new PrelievoLatte();
    this.trasporatoriService = new TrasportatoriService();
    this.destinatariService = new DestinatariService();
    this.acquirentiService = new AcquirentiService();

    this.canAdd = $("#canAdd").val() == "true";
    this.canEdit = $("#canEdit").val() == "true";
    this.canRemove = $("#canRemove").val() == "true";
    this.canSearchAllevatore = $("#canSearchAllevatore").val() == "true";
    this.canSearchTrasportatore = $("#canSearchTrasportatore").val() == "true";
    this.canSearchAcquirente = $("#canSearchAcquirente").val() == "true";
    this.canSearchDestinatario = $("#canSearchDestinatario").val() == "true";
  }

  public mounted() {
    this.initTable();
    this.loadAllevatori();
    this.loadTipiLatte();
    this.loadTrasportatori();
    this.loadDestinatari();
    this.loadAcquirenti();
    this.initSearchBox();
  }

  // Pulizia selezione
  public onAnnullaClick() {
    this.initSearchBox();
  }

  // Ricerca
  public onCercaClick() {
    this.totale_prelievi_kg = 0;
    this.totale_prelievi_lt = 0;
    var idAllevatoreStr =
      this.idAllevatoreSelezionato == 0
        ? ""
        : this.idAllevatoreSelezionato.toString();
    var idTipoLatteStr =
      this.idTipoLatteSelezionato == 0
        ? ""
        : this.idTipoLatteSelezionato.toString();
    var idTrasportatoreStr =
      this.idTrasportatoreSelezionato == 0
        ? ""
        : this.idTrasportatoreSelezionato.toString();
    var idAcquirenteStr =
      this.idAcquirenteSelezionato == 0
        ? ""
        : this.idAcquirenteSelezionato.toString();
    var idDestinatarioStr =
      this.idDestinatarioSelezionato == 0
        ? ""
        : this.idDestinatarioSelezionato.toString();
    this.loadPrelievi(
      idAllevatoreStr,
      idTipoLatteStr,
      idTrasportatoreStr,
      idAcquirenteStr,
      idDestinatarioStr,
      (prelievi: PrelievoLatte[]) => {
        for (let prelievo of this.prelievi) {
          this.totale_prelievi_kg += prelievo.Quantita;
          this.totale_prelievi_lt += prelievo.QuantitaLitri;
        }
      }
    );
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

  // Evento richiesta esportazione excel
  public onExportClick() {
    console.log("on export click");
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
        createdCell: function(td:any, cellData:any, rowData:any, row:any, col:any) {
          $(td).attr('title', cellData);
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
    this.idAllevatoreSelezionato = 0;
    this.idTrasportatoreSelezionato = 0;
    this.idAcquirenteSelezionato = 0;
    this.idDestinatarioSelezionato = 0;

    var today = new Date();
    this.al =
      today.getDate() +
      "/" +
      (today.getMonth() + 1) +
      "/" +
      today.getFullYear();

    var lastMonth = this.subtractMonth(today);

    this.dal =
      lastMonth.getDate() +
      "/" +
      (lastMonth.getMonth() + 1) +
      "/" +
      lastMonth.getFullYear();
  }

  // caricamento allevatori
  private loadAllevatori(): void {
    // this.allevatoriService.getAllevatori()
    //     .then(response => {
    //         if (response.data != null) {
    //             this.allevatori = response.data;
    //         }
    //     });
  }

  // caricamento tipi latte
  private loadTipiLatte(): void {
    this.tipiLatteService
      .index()
      .then((response: { data: TipoLatte[] | null }) => {
        if (response.data != null) {
          this.tipiLatte = response.data;
        }
      });
  }

  // caricamento trasportatori
  private loadTrasportatori() {
    this.trasporatoriService.getTrasportatori().then(response => {
      if (response.data != null) {
        this.trasportatore = response.data;
      }
    });
  }

  // caricamento destinatari
  private loadDestinatari() {
    this.destinatariService.index().then(response => {
      if (response.data != null) {
        this.destinatario = response.data;
      }
    });
  }

  // caricamento acquirenti
  private loadAcquirenti() {
    this.acquirentiService.index().then(response => {
      if (response.data != null) {
        this.acquirente = response.data;
      }
    });
  }

  private loadPrelievi(
    idAllevatoreStr: string,
    idTipoLatteStr: string,
    idTrasportatoreStr: string,
    idAcquirenteStr: string,
    idDestinatarioStr: string,
    done: (prelievi: PrelievoLatte[]) => void
  ) {
    this.prelieviLatteService
      .getPrelievi(
        idAllevatoreStr,
        idTrasportatoreStr,
        idAcquirenteStr,
        idDestinatarioStr,
        idTipoLatteStr,
        this.dal,
        this.al
      )
      .then(response => {
        this.prelievi = response.data;

        done(this.prelievi);
      });
  }

  private subtractMonth(date: Date): Date {
    var days = 0;
    //get month ritorna un numero da 0 a 11. Dovendo considerare il mese precedente
    //da sottrarre, per comodità aggiungo un numero al case, considerando quindi lo 0 come dicembre
    switch (date.getMonth()) {
      case 4: //Aprile
      case 6: //Giugno
      case 9: //Settembre
      case 11: { //Novembre
        days = 30;
        break;
      }
      case 2: { //febbraio
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
td.truncate {
  max-width: 50px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>