var Giro = /** @class */ (function () {
    function Giro() {
        this.Id = 0;
        this.Denominazione = "";
        this.CodiceGiro = "";
        this.IdTrasportatore = 0;
        this.Items = [];
    }
    return Giro;
}());
export { Giro };
var Items = /** @class */ (function () {
    function Items() {
        this.IdGiro = 0;
        this.IdAllevamento = 0;
        this.Allevatore = "";
        this.RagioneSociale = "";
        this.Indirizzo = "";
        this.BoolPriorita = false;
        this.Priorita = 0;
    }
    return Items;
}());
export { Items };
