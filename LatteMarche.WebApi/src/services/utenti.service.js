import axios from 'axios';
var UtentiService = /** @class */ (function () {
    function UtentiService() {
    }
    UtentiService.prototype.getUtenti = function () {
        return axios.get('/api/utenti');
    };
    UtentiService.prototype.update = function (utente) {
        return axios.put('/api/utenti/update', utente);
    };
    UtentiService.prototype.getDetails = function (id) {
        return axios.get('/api/utenti/details?id=' + id);
    };
    return UtentiService;
}());
export { UtentiService };
