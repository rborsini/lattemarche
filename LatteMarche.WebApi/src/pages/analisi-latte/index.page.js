var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import Vue from "vue";
import Component from "vue-class-component";
import Waiter from "../../components/common/waiter.vue";
import { AnalisiSearchModel, AnalisiTable, AnalisiCol, AnalisiRow, AnalisiCell } from "../../models/analisi.model";
import { AnalisiService } from "../../services/analisi.service";
var AnalisiLatteIndexPage = /** @class */ (function (_super) {
    __extends(AnalisiLatteIndexPage, _super);
    function AnalisiLatteIndexPage() {
        var _this = _super.call(this) || this;
        _this.$refs = {
            waiter: Vue
        };
        _this.analisiService = new AnalisiService();
        _this.params = new AnalisiSearchModel();
        _this.analisi = [];
        _this.analisiTable = new AnalisiTable();
        return _this;
    }
    AnalisiLatteIndexPage.prototype.mounted = function () {
        this.params.CodiceProduttore = "A0530010";
    };
    AnalisiLatteIndexPage.prototype.onCercaClick = function () {
        var _this = this;
        this.analisiService.search(this.params)
            .then(function (response) {
            _this.analisi = response.data;
            _this.analisiTable = _this.bindTable(response.data);
        });
    };
    AnalisiLatteIndexPage.prototype.onAnnullaClick = function () {
        this.params.CodiceProduttore = "";
        this.params.CodiceAsl = "";
    };
    AnalisiLatteIndexPage.prototype.onSynchClick = function () {
        var _this = this;
        this.$refs.waiter.open();
        this.analisiService.synch()
            .then(function (respose) {
            _this.$refs.waiter.close();
            _this.onCercaClick();
        })
            .catch(function (error) {
            alert("errore");
        });
    };
    AnalisiLatteIndexPage.prototype.bindTable = function (result) {
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
                var valoreIndex = resultRow.Valori.map(function (e) { return e.Nome; }).indexOf(table.Cols[j].Nome);
                var valoreObj = resultRow.Valori[valoreIndex];
                row.Cells.push(new AnalisiCell(valoreObj.Valore, valoreObj.FuoriSoglia));
            }
            table.Rows.push(row);
        }
        return table;
    };
    AnalisiLatteIndexPage.prototype.bindCols = function (result) {
        var cols = [];
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
                var colIndex = cols.map(function (e) { return e.Nome; }).indexOf(valoreObj.Nome);
                if (colIndex == -1) {
                    cols.push(new AnalisiCol(valoreObj.Nome, valoreObj.Uom));
                    colIndex = cols.length - 1;
                }
            }
        }
        return cols;
    };
    AnalisiLatteIndexPage = __decorate([
        Component({
            el: '#index-analisi-latte-page',
            components: {
                Waiter: Waiter
            }
        }),
        __metadata("design:paramtypes", [])
    ], AnalisiLatteIndexPage);
    return AnalisiLatteIndexPage;
}(Vue));
export default AnalisiLatteIndexPage;
var page = new AnalisiLatteIndexPage();
Vue.config.devtools = true;
