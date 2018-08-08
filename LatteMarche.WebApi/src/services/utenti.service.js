import axios from 'axios';
var UtentiService = /** @class */ (function () {
    function UtentiService() {
    }
    UtentiService.prototype.index = function () {
        return axios.get('/api/utenti');
    };
    UtentiService.prototype.details = function (id) {
        return axios.get('/api/utenti/details?id=' + id);
    };
    UtentiService.prototype.save = function (utente) {
        return axios.post('/api/utenti/save', utente);
    };
    UtentiService.prototype.delete = function (id) {
        return axios.delete('/api/utenti/delete?id=' + id);
    };
    return UtentiService;
}());
export { UtentiService };
