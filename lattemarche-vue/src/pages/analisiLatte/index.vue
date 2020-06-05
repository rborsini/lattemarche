<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Box ricerca -->
    <div class="jumbotron">
      <!-- Campi ricerca -->
      <!-- Codice produttore -->
      <div class="row pt-1">

        <label class="col-1">Allevamento:</label>
        <div class="col-2">
          <select2
            class="form-control"
            :disabled="idProfilo == 3"
            :placeholder="'-'"
            :options="allevamenti.Items"
            :value.sync="params.IdAllevamento"
            :value-field="'Value'"
            :text-field="'Text'"
          />
        </div>

        <!-- dal / al -->        
        <label class="col-1">Dal:</label>
        <div class="col-2">
          <datepicker class="form-control" :value.sync="params.DataPeriodoInizio_Str" />
        </div>

        <label class="col-1">Al:</label>
        <div class="col-2">
          <datepicker class="form-control" :value.sync="params.DataPeriodoFine_Str" />
        </div>

        <!-- Campione -->
        <label class="col-1">Campione:</label>
        <div class="col-2">
          <input type="text" class="form-control" v-model="params.Campione" />
        </div>

      </div>

      <!-- Bottoni di ricerca -->
      <div class="row pt-3">
        <div class="col-12">
          <button v-on:click="onCercaClick" class="float-right btn btn-success" role="button">Cerca</button>
          <!-- <button v-on:click="onSynchClick" class="float-right btn btn-success mr-2" role="button" >Synch</button> -->
          <button v-on:click="onAnnullaClick" class="float-right btn btn-secondary mr-2" href="#" role="button" >Annulla</button>
        </div>
      </div>
    </div>

    <div class="container-fluid">
      <div class="row">
        <div class="col-12">
          <!-- Tabella -->
          <table class="table table-striped table-bordered dataTable mt-3">
            <thead>
              <tr>
                <th v-for="(col, colIndex) in analisiTable.Cols" :key="colIndex">{{col.Nome}}</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(row, rowIndex) in analisiTable.Rows" :key="rowIndex">
                <td v-for="(cell, cellIndex) in row.Cells" :key="cellIndex">
                  <span v-bind:class="{ 'text-danger' : cell.FuoriSoglia }">{{cell.Valore}}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import Waiter from "../../components/waiter.vue";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import { Analisi, AnalisiSearchModel, AnalisiTable, AnalisiCol, AnalisiRow, AnalisiCell } from "../../models/analisi.model";
import { AnalisiService } from "../../services/analisi.service";
import { DropdownItem, Dropdown } from '../../models/dropdown.model';
import { DropdownService } from '../../services/dropdown.service';

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}

@Component({
  components: {
    Waiter,
    Select2,
    Datepicker
  }
})
export default class AnalisiLatteIndexPage extends Vue {
  $refs: any = {
    waiter: Vue
  };

  private analisiService: AnalisiService = new AnalisiService();
  public dropdownService: DropdownService = new DropdownService();

  public params: AnalisiSearchModel = new AnalisiSearchModel();
  public idProfilo: any = '';

  public allevamenti: Dropdown = new Dropdown();

  public analisi: Analisi[] = [];
  public analisiTable: AnalisiTable = new AnalisiTable();

  constructor() {
    super();

    this.idProfilo = $("#idProfilo").val();
  }

  public mounted() {
    this.loadAllevamenti();
    this.initSearchBox();
  }

  private initSearchBox() {
    this.params.IdAllevamento = 0;
    this.params.DataPeriodoFine_Str = this.formatDate(new Date());
    this.params.DataPeriodoInizio_Str = this.formatDate(
      this.subtractMonth(new Date())
    );
    this.params.Campione = ''; // "20201138-001";
  }

  // Click bottone cerca
  public onCercaClick() {
    this.$refs.waiter.open();
    this.analisiService.search(this.params).then(response => {
      this.$refs.waiter.close();
      this.analisi = response.data;
      this.analisiTable = this.bindTable(response.data);
    });
  }

  // Click bottone annulla
  public onAnnullaClick() {
    this.initSearchBox();
  }

  // Click bottone synch
  public onSynchClick() {
    this.$refs.waiter.open();
    this.analisiService
      .synch()
      .then(respose => {
        this.$refs.waiter.close();
        this.onCercaClick();
      })
      .catch(error => {
        this.$refs.waiter.close();
        alert("errore");
      });
  }

  // caricamento allevamenti
  private loadAllevamenti() {
    this.dropdownService.getAllevamenti().then(response => {
      this.allevamenti = response.data;

      if(this.idProfilo == 3)
        this.params.IdAllevamento = response.data.Items[0].Value;      
    });
  }

  // Popolamento tabella
  public bindTable(result: Analisi[]): AnalisiTable {
    var table = new AnalisiTable();

    table.Cols = this.bindCols(result);

    for (var i = 0; i < result.length; i++) {
      var resultRow = result[i];
      var row = new AnalisiRow();

      row.Cells.push(new AnalisiCell(resultRow.CodiceProduttore));
      row.Cells.push(new AnalisiCell(resultRow.NomeProduttore));
      row.Cells.push(new AnalisiCell(resultRow.Id));
      row.Cells.push(new AnalisiCell(resultRow.CodiceASL));

      row.Cells.push(new AnalisiCell(resultRow.DataRapportoDiProva_Str));
      row.Cells.push(new AnalisiCell(resultRow.DataAccettazione_Str));
      row.Cells.push(new AnalisiCell(resultRow.DataPrelievo_Str));

      for (var j = row.Cells.length; j < table.Cols.length; j++) {
        var valoreIndex = resultRow.Valori.map(function(e) {
          return e.Nome;
        }).indexOf(table.Cols[j].Nome);
        var valoreObj = resultRow.Valori[valoreIndex];

        var valore = valoreObj ? valoreObj.Valore : '';
        var fuoriSoglia = valoreObj ? valoreObj.FuoriSoglia : false;

        row.Cells.push(
          new AnalisiCell(valore, fuoriSoglia)
        );
      }

      table.Rows.push(row);
    }

    return table;
  }

  // estrapolazione colonne tabella
  public bindCols(result: Analisi[]): AnalisiCol[] {
    var cols: AnalisiCol[] = [];

    cols.push(new AnalisiCol("Codice Produttore", ""));
    cols.push(new AnalisiCol("Nome Produttore", ""));
    cols.push(new AnalisiCol("Campione", ""));
    cols.push(new AnalisiCol("Codice Asl", ""));
    cols.push(new AnalisiCol("Data rapporto di prova", ""));
    cols.push(new AnalisiCol("Data accettazione", ""));
    cols.push(new AnalisiCol("Data prelievo", ""));

    for (var i = 0; i < result.length; i++) {
      var resultRow = result[i];
      for (var j = 0; j < resultRow.Valori.length; j++) {
        var valoreObj = resultRow.Valori[j];

        var colIndex = cols
          .map(function(e) {
            return e.Nome;
          })
          .indexOf(valoreObj.Nome);
        if (colIndex == -1) {
          cols.push(new AnalisiCol(valoreObj.Nome, valoreObj.Uom));
          colIndex = cols.length - 1;
        }
      }
    }

    return cols;
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