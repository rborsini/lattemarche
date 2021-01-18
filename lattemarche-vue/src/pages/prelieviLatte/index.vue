<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Pannello Salvataggio prelievo-->
    <notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Il prelievo è stato salvato correttamente'" ></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog ref="removedDialog" :title="'Conferma rimozione'" :message="'Prelievo rimosso correttamente'" ></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog ref="confirmDeleteDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler rimuovere il prelievo selezionato?'" v-on:confirmed="onRemove()" ></confirm-dialog>

    <!-- Box ricerca -->
    <div class="jumbotron">
      <!-- Campi ricerca -->

      <!-- dal / al -->
      <div class="row pt-1">
        <label class="col-1">Dal:</label>
        <div class="col-1">
          <datepicker id="cy-data-inizio" v-on:value-changed="onParametersChanged" class="form-control" :value.sync="parameters.DataPeriodoInizio_Str" />
        </div>

        <label class="col-1">Al:</label>
        <div class="col-1">
          <datepicker id="cy-data-fine" v-on:value-changed="onParametersChanged" class="form-control" :value.sync="parameters.DataPeriodoFine_Str" />
        </div>

        <label class="col-1">Lotto:</label>
        <div class="col-3">
          <input id="cy-lotto" v-on:input="onParametersChanged" class="form-control" type="text" v-model="parameters.LottoConsegna" />
        </div>

        <label class="col-1">Giro:</label>
        <div class="col-3">
          <select2 id="cy-giro" v-on:value-changed="onParametersChanged" class="form-control" :placeholder="'-'" :options="giro.Items" :value.sync="parameters.CodiceGiro" :value-field="'Value'" :text-field="'Text'" />          
        </div>        

      </div>

      <!-- Allevatore / Trasportatore / Tipo latte -->
      <div class="row pt-1" v-if="canSearchAllevatore || canSearchTrasportatore">
        <label class="col-1" v-if="canSearchAllevatore">Allevatore:</label>
        <div class="col-3" v-if="canSearchAllevatore">
          <select2 id="cy-allevatore" v-on:value-changed="onParametersChanged" class="form-control" :disabled="idProfilo == 3" :placeholder="'-'" :options="allevatori.Items" :value.sync="parameters.IdAllevamento" :value-field="'Value'" :text-field="'Text'" />
        </div>

        <label class="col-1" v-if="canSearchTrasportatore">Trasportatore:</label>
        <div class="col-3" v-if="canSearchTrasportatore">
          <select2 id="cy-trasportatore" v-on:value-changed="onParametersChanged" class="form-control" :placeholder="'-'" :disabled="idProfilo == 5" :options="trasportatore.Items" :value.sync="parameters.IdTrasportatore" :value-field="'Value'" :text-field="'Text'" />
        </div>

        <label class="col-1">Tipo latte:</label>
        <div class="col-3">
          <select2 id="cy-tipo-latte" v-on:value-changed="onParametersChanged" class="form-control" :placeholder="'-'" :options="tipiLatte.Items" :value.sync="parameters.IdTipoLatte" :value-field="'Value'" :text-field="'Text'" />
        </div>
      </div>

      <!-- Acquirente / Cessionario / Destinatario -->
      <div class="row pt-1" v-if="canSearchAcquirente || canSearchCessionario || canSearchDestinatario" >

        <label class="col-1" v-if="canSearchAcquirente">Acquirente:</label>
        <div class="col-3" v-if="canSearchAcquirente">
          <select2 id="cy-acquirente" v-on:value-changed="onParametersChanged" class="form-control" :disabled="idProfilo == 7" :placeholder="'-'" :options="acquirente.Items" :value.sync="parameters.IdAcquirente" :value-field="'Value'" :text-field="'Text'" />
        </div>

        <label class="col-1" v-if="canSearchCessionario">Cessionario:</label>
        <div class="col-3" v-if="canSearchCessionario">
          <select2 id="cy-cessionario" v-on:value-changed="onParametersChanged" class="form-control" :disabled="idProfilo == 8" :placeholder="'-'" :options="cessionario.Items" :value.sync="parameters.IdCessionario" :value-field="'Value'" :text-field="'Text'" />
        </div>

        <label class="col-1" v-if="canSearchDestinatario">Destinatario:</label>
        <div class="col-3" v-if="canSearchDestinatario">
          <select2 id="cy-destinatario" v-on:value-changed="onParametersChanged" class="form-control" :disabled="idProfilo == 6" :placeholder="'-'" :options="destinatario.Items" :value.sync="parameters.IdDestinatario" :value-field="'Value'" :text-field="'Text'" />
        </div>
      </div>

      <!-- dal / al -->
      <div class="row pt-1">
        <label class="col-1">Data consegna dal:</label>
        <div class="col-1">
          <datepicker id="cy-data-consegna-inizio" v-on:value-changed="onParametersChanged" class="form-control" :value.sync="parameters.DataConsegnaInizio_Str" />
        </div>

        <label class="col-1">Al:</label>
        <div class="col-1">
          <datepicker id="cy-data-consegna-fine" v-on:value-changed="onParametersChanged" class="form-control" :value.sync="parameters.DataConsegnaFine_Str" />
        </div>     

      </div>

      <!-- Bottoni di ricerca -->
      <div class="row pt-3">
        <div class="col-12">
          <button id="cy-btn-search" :disabled="!isLoaded" v-on:click="onCercaClick" class="cy-btn-search float-right btn btn-success" role="button">Cerca</button>
          <button id="cy-btn-clear" :disabled="!isLoaded" v-on:click="onAnnullaClick" class="float-right btn btn-secondary mr-2" href="#" role="button" >Annulla</button>
        </div>
      </div>
    </div>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="prelievi" v-on:data-loaded="onDataLoaded">
      <!-- Toolbox -->
      <template slot="toolbox">
        <div class="toolbox text-right">
          <div class="btn-group">
            <a v-if="idProfilo == 1" class="btn btn-success float-right mr-3" href="/prelievi/edit" >Aggiungi</a>
            <button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >Esporta in excel</button>
            <div class="dropdown-menu">
              <a class="dropdown-item" v-on:click="downloadExcel('allevatori')" style="cursor: pointer" >Allevatori</a>
              <a class="dropdown-item" v-on:click="downloadExcel('trasportatori')" style="cursor: pointer" >Trasportatori</a>
              <a class="dropdown-item" v-on:click="downloadExcel('giornalieri')" style="cursor: pointer" >Giornalieri</a>
            </div>
          </div>
        </div>
      </template>

      <!-- Colonne -->
      <template slot="thead">
        <th>Allevamento</th>
        <th>Data prelievo</th>
        <th>Data consegna</th>
        <th>Lotto consegna</th>
        <th>Kg</th>
        <th>Lt</th>
        <th>Temp.</th>
        <th>Trasportatore</th>
        <th>Acquirente</th>
        <th>Destinatario</th>
        <th>Tipo Latte</th>
        <th v-if="idProfilo == 1" ></th>
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
        <th v-if="idProfilo == 1"></th>
      </template>
    </data-table>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import axios, { AxiosPromise } from 'axios';

import Waiter from "../../components/waiter.vue";
import DataTable from "../../components/dataTable.vue";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { Trasportatore } from "../../models/trasportatore.model";
import { Acquirente } from "../../models/acquirente.model";
import { Destinatario } from "../../models/destinatario.model";
import { PrelievoLatte, PrelieviLatteSearchModel } from "../../models/prelievoLatte.model";
import { TipoLatte } from "../../models/tipoLatte.model";

import { PrelieviLatteService } from "../../services/prelieviLatte.service";
import { DropdownService } from "../../services/dropdown.service";
import { UrlService } from "@/services/url.service";

import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { parseJSON } from 'jquery';
import { AuthorizationsService } from '@/services/authorizations.service';
import { BaseSearchModel } from '@/models/baseSearch.model';

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
    NotificationDialog,
    DataTable,
    Waiter
  }
})
export default class PrelieviLatteIndexPage extends Vue {
  $refs: any = {
    savedDialog: Vue,
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
  public giro: Dropdown = new Dropdown();

  public prelievi: PrelievoLatte[] = [];
  private idPrelievoDaEliminare!: number;
  private isLoaded: boolean = false;

  public parameters!: PrelieviLatteSearchModel;

  public prelievoSelezionato: PrelievoLatte;
  
  public canSearchAllevatore: boolean = true;
  public canSearchTrasportatore: boolean = true;
  public canSearchAcquirente: boolean = true;
  public canSearchDestinatario: boolean = true;
  public canSearchCessionario: boolean = true;
  public canHighligthRow: boolean = false;
  public idProfilo: any = '';

  public totale_prelievi_kg: number = 0;
  public totale_prelievi_lt: number = 0;

  constructor() {
    super();

    this.parameters = new PrelieviLatteSearchModel();
    this.prelieviLatteService = new PrelieviLatteService();
    this.prelievoSelezionato = new PrelievoLatte();
    this.dropdownService = new DropdownService();

    this.idProfilo = $("#idProfilo").val();
  }

  public mounted() {
    this.readPermissions();
    this.initTable();
    this.loadDropdown();
    this.initSearchBox();  

    if(window.location.hash.length > 0)
    {
      this.parameters.decodeUrl(window.location.hash);
      this.onCercaClick();
    }

  }

  // evento modifica parametri ricerca
  private onParametersChanged() {
    window.location.hash = this.parameters.toUrlQueryString(); 
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

    this.prelieviLatteService.search(this.parameters).then(response => {
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
    $(".delete").click(event => {
      var element = $(event.currentTarget);
      this.idPrelievoDaEliminare = $(element).data("row-id");

      this.$refs.confirmDeleteDialog.open();
    });
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

    options.columnDefs = [
      {
        targets: [0, 6, 7, 8, 9],
        createdCell: function(td: any, cellData: any, rowData: any, row: any, col: any) {
          $(td).attr("title", cellData);
        }
      }
    ];
    options.columns.push({ className: "truncate", data: "Allevamento" });
    options.columns.push({
      className: "truncate",
      width: "55px",
      data: { 
        _: "DataPrelievoStr",
        sort: "DataPrelievo"
      }
    });
    options.columns.push({
      className: "truncate",
      width: "55px",
      data: { 
        _: "DataConsegnaStr",
        sort: "DataConsegna"
      }      
    });
    options.columns.push({
      className: "truncate",
      width: "70px",
      data: "LottoConsegna"
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
    options.columns.push({ className: "truncate",  width: "20px", data: "SiglaLatte" });

    if(this.idProfilo == 1) {
      options.columns.push({
        render: function(data: any, type: any, row: any) {
          var html = '<div class="text-center">';

          html += '<a class="edit" title="modifica" style="width: 30px;cursor: pointer;" href="/prelievi/edit?id=' + row.Id + '" ><i class="far fa-edit"></i></a>';            
          html += '<a class="pl-3 delete" title="elimina" style="width: 30px;cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-trash-alt"></i></a>';

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

    if(this.canHighligthRow) {
      options.createdRow = function(row: any, data: any, index: any) {
        var prelievo = data as PrelievoLatte;
        if(prelievo.DistanzaAllevamento) {
          var className = prelievo.DistanzaAllevamento < 500 ? 'coord_ok' : 'coord_ko';
          $(row).addClass(className);
        }
      }
    }

    this.tableOptions = options;
  }

  // inizializzazione parametri di ricerca
  private initSearchBox() {
    this.parameters.clear();
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

  // load dropdown
  private loadDropdown() {

    this.$refs.waiter.open();
    this.dropdownService.getDropdowns("allevatori|acquirenti|cessionari|destinatari|giri|tipiLatte|trasportatori")
      .then(response => {

        this.allevatori = response.data["allevatori"] as Dropdown;
        this.acquirente = response.data["acquirenti"] as Dropdown;
        this.cessionario = response.data["cessionari"] as Dropdown;
        this.destinatario = response.data["destinatari"] as Dropdown;
        this.giro = response.data["giri"] as Dropdown;
        this.tipiLatte = response.data["tipiLatte"] as Dropdown;
        this.trasportatore = response.data["trasportatori"] as Dropdown;

        switch(this.idProfilo) {
          case "3":   // allevatore
            this.parameters.IdAllevamento = this.allevatori.Items[0].Value; 
            break;
          case "5":   // trasportatore
            this.parameters.IdTrasportatore = this.trasportatore.Items[0].Value; 
            break;
          case "6":   // destinatario
            this.parameters.IdDestinatario = this.destinatario.Items[0].Value; 
            break;
          case "7":   // acquirente
            this.parameters.IdAcquirente = this.acquirente.Items[0].Value; 
            break;
          case "8":   // cessionario
            this.parameters.IdCessionario = this.cessionario.Items[0].Value; 
            break;                                        
        }

        this.isLoaded = true;
        this.$refs.waiter.close();

      });

  }

  //Esportazione excel
  public downloadExcel(tipo: string) {
    UrlService.redirect(
      "/api/prelieviLatte/excel" +
        tipo +
        "?" +
        this.parameters.toUrlQueryString()
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

  // lettura permessi da jwt
  private readPermissions() {
    this.canHighligthRow = AuthorizationsService.isViewItemAuthorized("Prelievi","Index","FlagCoordinate");
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

.coord_ok {
  background-color: #8fd19e !important;
}

.coord_ko {
  background-color: #ed969e !important;
}

</style>