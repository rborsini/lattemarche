import axios from "axios";
var PrelieviLatteService = /** @class */ (function () {
    function PrelieviLatteService() {
    }
    PrelieviLatteService.prototype.getPrelievi = function (id, dataInizio, dataFine) {
        var url = '/api/PrelieviLatte/Search?idAllevamento=' + id;
        url += '&dal=' + dataInizio;
        url += '&al=' + dataFine;
        return axios.get(url);
    };
    PrelieviLatteService.prototype.getPrelievoDetails = function (id) {
        return axios.get('/api/prelievilatte/details?id=' + id);
    };
    PrelieviLatteService.prototype.getLaboratoriAnalisi = function () {
        return axios.get('/api/laboratorianalisi');
    };
    PrelieviLatteService.prototype.update = function (prelievoLatte) {
        return axios.put('/api/utenti/save', prelievoLatte);
    };
    PrelieviLatteService.prototype.create = function (prelievoLatte) {
        return axios.post('/api/utenti/create', prelievoLatte);
    };
    return PrelieviLatteService;
}());
export { PrelieviLatteService };
