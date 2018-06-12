import axios from 'axios';
var AllevatoriServices = /** @class */ (function () {
    function AllevatoriServices() {
    }
    AllevatoriServices.prototype.getUtenti = function () {
        return axios.get('/api/utenti');
    };
    AllevatoriServices.prototype.update = function (allevatore) {
        return axios.post('/api/utenti/save', allevatore);
    };
    AllevatoriServices.prototype.getDetails = function (id) {
        return axios.get('/api/utenti/details?id=' + id);
    };
    return AllevatoriServices;
}());
export { AllevatoriServices };
