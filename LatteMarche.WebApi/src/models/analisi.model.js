var Analisi = /** @class */ (function () {
    function Analisi() {
        this.Id = "";
        this.CodiceProduttore = "";
        this.NomeProduttore = "";
        this.CodiceASL = "";
        this.TipoLatte_Descr = "";
        this.DataRapportoDiProva_Str = "";
        this.DataAccettazione_Str = "";
        this.DataPrelievo_Str = "";
        this.Valori = [];
    }
    return Analisi;
}());
export { Analisi };
var ValoreAnalisi = /** @class */ (function () {
    function ValoreAnalisi() {
        this.Id = 0;
        this.Nome = "";
        this.Uom = "";
        this.Valore = "";
        this.FuoriSoglia = false;
    }
    return ValoreAnalisi;
}());
export { ValoreAnalisi };
var AnalisiSearchModel = /** @class */ (function () {
    function AnalisiSearchModel() {
        this.CodiceProduttore = "";
        this.CodiceAsl = "";
    }
    return AnalisiSearchModel;
}());
export { AnalisiSearchModel };
var AnalisiTable = /** @class */ (function () {
    function AnalisiTable() {
        this.Cols = [];
        this.Rows = [];
    }
    return AnalisiTable;
}());
export { AnalisiTable };
var AnalisiCol = /** @class */ (function () {
    function AnalisiCol(nome, uom) {
        this.Nome = "";
        this.Uom = "";
        this.Nome = nome;
        this.Uom = uom;
    }
    return AnalisiCol;
}());
export { AnalisiCol };
var AnalisiRow = /** @class */ (function () {
    function AnalisiRow() {
        this.Cells = [];
    }
    return AnalisiRow;
}());
export { AnalisiRow };
var AnalisiCell = /** @class */ (function () {
    function AnalisiCell(valore, fuoriSoglia) {
        if (fuoriSoglia === void 0) { fuoriSoglia = false; }
        this.Valore = "";
        this.FuoriSoglia = false;
        this.Valore = valore;
        this.FuoriSoglia = fuoriSoglia;
    }
    return AnalisiCell;
}());
export { AnalisiCell };
