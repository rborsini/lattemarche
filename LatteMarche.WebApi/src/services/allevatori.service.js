import axios from 'axios';
var AllevatoriService = /** @class */ (function () {
    function AllevatoriService() {
    }
    AllevatoriService.prototype.getUtenti = function () {
        return axios.get('/api/utenti');
    };
    AllevatoriService.prototype.update = function (allevatore) {
        return axios.post('/api/utenti/save', allevatore);
    };
    AllevatoriService.prototype.getDetails = function (id) {
        return axios.get('/api/utenti/details?id=' + id);
    };
    return AllevatoriService;
}());
export { AllevatoriService };
