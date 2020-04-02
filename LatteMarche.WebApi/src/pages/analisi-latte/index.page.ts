import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import Waiter from "../../components/common/waiter.vue";

import { Analisi, AnalisiSearchModel, AnalisiTable, AnalisiCol, AnalisiRow, AnalisiCell } from "../../models/analisi.model";

import { AnalisiService } from "../../services/analisi.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#index-analisi-latte-page',
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

        this.params.CodiceProduttore = "A0530010";

    }

    public onCercaClick() {

        this.analisiService.search(this.params)
            .then(response => {
                this.analisi = response.data;
                this.analisiTable = this.bindTable(response.data);
            })

    }

    public onAnnullaClick() {
        this.params.CodiceProduttore = "";
        this.params.CodiceAsl = "";
    }

    public onSynchClick() {
        this.$refs.waiter.open();
        this.analisiService.synch()
            .then(respose => {
                this.$refs.waiter.close();
                this.onCercaClick();
            })
            .catch(error => {
                alert("errore");
            });
    }

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

                var valoreIndex = resultRow.Valori.map(function (e) { return e.Nome }).indexOf(table.Cols[j].Nome);
                var valoreObj = resultRow.Valori[valoreIndex];

                row.Cells.push(new AnalisiCell(valoreObj.Valore, valoreObj.FuoriSoglia));
            }

            table.Rows.push(row);
        }

        return table;
    }

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

                var colIndex = cols.map(function (e) { return e.Nome }).indexOf(valoreObj.Nome);
                if (colIndex == -1) {
                    cols.push(new AnalisiCol(valoreObj.Nome, valoreObj.Uom));
                    colIndex = cols.length - 1;
                }
            }
        }

        return cols;
    }


}

let page = new AnalisiLatteIndexPage();
Vue.config.devtools = true;