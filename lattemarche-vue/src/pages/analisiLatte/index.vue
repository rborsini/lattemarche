<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Box ricerca -->
    <div class="jumbotron">
      <!-- Campi ricerca -->
      <!-- Codice produttore -->
      <div class="row pt-1">
        <label class="col-2">Codice produttore:</label>

        <div class="col-2">
          <input type="text" class="form-control" v-model="params.CodiceProduttore" />
        </div>

        <label class="col-2">Codice ASL:</label>

        <div class="col-2">
          <input type="text" class="form-control" v-model="params.CodiceAsl" />
        </div>

        <label class="col-2">Campione:</label>

        <div class="col-2">
          <input type="text" class="form-control" v-model="params.Campione" />
        </div>
      </div>

      <!-- Bottoni di ricerca -->
      <div class="row pt-3">
        <div class="col-12">
          <button v-on:click="onCercaClick" class="float-right btn btn-primary" role="button">Cerca</button>
          <button
            v-on:click="onSynchClick"
            class="float-right btn btn-primary mr-2"
            role="button"
          >Synch</button>
          <button
            v-on:click="onAnnullaClick"
            class="float-right btn btn-primary mr-2"
            href="#"
            role="button"
          >Annulla</button>
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
import { Analisi, AnalisiSearchModel, AnalisiTable, AnalisiCol, AnalisiRow, AnalisiCell } from "../../models/analisi.model";
import { AnalisiService } from "../../services/analisi.service";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}

@Component({
  components: {
    Waiter
  }
})
export default class AnalisiLatteIndexPage extends Vue {
  $refs: any = {
    waiter: Vue
  };

  private analisiService: AnalisiService = new AnalisiService();

  public params: AnalisiSearchModel = new AnalisiSearchModel();

  public analisi: Analisi[] = [];
  public analisiTable: AnalisiTable = new AnalisiTable();

  constructor() {
    super();
  }

  public mounted() {
    this.params.CodiceProduttore = "";
    this.params.Campione = "20201138-001";
  }

  // Click bottone cerca
  public onCercaClick() {
    this.analisiService.search(this.params).then(response => {
      this.analisi = response.data;
      this.analisiTable = this.bindTable(response.data);
    });
  }

  // Click bottone annulla
  public onAnnullaClick() {
    this.params.CodiceProduttore = "";
    this.params.CodiceAsl = "";
    this.params.Campione = "";
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

        row.Cells.push(
          new AnalisiCell(valoreObj.Valore, valoreObj.FuoriSoglia)
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
}
</script>