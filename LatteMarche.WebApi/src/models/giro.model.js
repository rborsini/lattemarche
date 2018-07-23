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
var Item = /** @class */ (function () {
    function Item() {
        this.IdGiro = 0;
        this.IdAllevamento = 0;
        this.Allevatore = "";
        this.RagioneSociale = "";
        this.Indirizzo = "";
        this.Selezionato = false;
        this.Priorita = 0;
    }
    return Item;
}());
export { Item };
