import axios from "axios";
var PrelieviLatteService = /** @class */ (function () {
    function PrelieviLatteService() {
    }
    PrelieviLatteService.prototype.getPrelievi = function (idAllevamento, idTrasportatore, idAcquirente, idDestinatario, dataInizio, dataFine) {
        var url = '/api/prelievilatte/Search?idAllevamento=' + idAllevamento;
        url += '&idTrasportatore=' + idTrasportatore;
        url += '&idAcquirente=' + idAcquirente;
        url += '&idDestinatario=' + idDestinatario;
        url += '&dal=' + dataInizio;
        url += '&al=' + dataFine;
        return axios.get(url);
    };
    PrelieviLatteService.prototype.getPrelievo = function (id) {
        return axios.get('/api/prelieviLatte/Details?id=' + id);
    };
    PrelieviLatteService.prototype.getLaboratoriAnalisi = function () {
        return axios.get('/api/laboratorianalisi');
    };
    PrelieviLatteService.prototype.save = function (prelievo) {
        return axios.post('/api/PrelieviLatte/save', prelievo);
    };
    PrelieviLatteService.prototype.delete = function (idPrelievo) {
        return axios.delete('/api/PrelieviLatte/delete?id=' + idPrelievo);
    };
    return PrelieviLatteService;
}());
export { PrelieviLatteService };
