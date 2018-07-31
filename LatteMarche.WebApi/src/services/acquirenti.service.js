import axios from 'axios';
var AcquirentiService = /** @class */ (function () {
    function AcquirentiService() {
    }
    AcquirentiService.prototype.getAcquirenti = function () {
        return axios.get('/api/acquirenti');
    };
    AcquirentiService.prototype.getDetails = function (id) {
        return axios.get('/api/acquirenti/details?id=' + id);
    };
    AcquirentiService.prototype.update = function (acquirente) {
        return axios.put('/api/Acquirenti/update', acquirente);
    };
    return AcquirentiService;
}());
export { AcquirentiService };
