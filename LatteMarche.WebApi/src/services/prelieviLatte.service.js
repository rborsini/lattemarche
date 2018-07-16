import axios from "axios";
var PrelieviLatteService = /** @class */ (function () {
    function PrelieviLatteService() {
    }
    PrelieviLatteService.prototype.getPrelievi = function (id, dataInizio, dataFine) {
        var url = '/api/prelievilatte/Search?idAllevamento=' + id;
        url += '&dal=' + dataInizio;
        url += '&al=' + dataFine;
        return axios.get(url);
    };
    PrelieviLatteService.prototype.getLaboratoriAnalisi = function () {
        return axios.get('/api/laboratorianalisi');
    };
    PrelieviLatteService.prototype.save = function (prelievo) {
        return axios.post('/api/PrelieviLatte/save', prelievo);
    };
    return PrelieviLatteService;
}());
export { PrelieviLatteService };
