import axios from 'axios';
var AcquirentiService = /** @class */ (function () {
    function AcquirentiService() {
    }
    AcquirentiService.prototype.index = function () {
        return axios.get('/api/acquirenti');
    };
    AcquirentiService.prototype.details = function (id) {
        return axios.get('/api/acquirenti/details?id=' + id);
    };
    AcquirentiService.prototype.save = function (acquirente) {
        return axios.post('/api/Acquirenti/save', acquirente);
    };
    AcquirentiService.prototype.delete = function (idAcquirente) {
        return axios.delete('/api/Acquirenti/delete?id=' + idAcquirente);
    };
    return AcquirentiService;
}());
export { AcquirentiService };
