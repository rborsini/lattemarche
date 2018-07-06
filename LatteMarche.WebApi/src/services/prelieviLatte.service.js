import axios from 'axios';
var PrelieviLatteService = /** @class */ (function () {
    function PrelieviLatteService() {
    }
    PrelieviLatteService.prototype.getPrelievi = function (id, dataInizio, dataFine) {
        var url = '/api/PrelieviLatte/Search?idAllevamento=' + id;
        url += '&dal=' + dataInizio;
        url += '&al=' + dataFine;
        return axios.get(url);
    };
    return PrelieviLatteService;
}());
export { PrelieviLatteService };
