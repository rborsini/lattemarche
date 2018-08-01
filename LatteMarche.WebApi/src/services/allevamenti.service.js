import axios from 'axios';
var AllevamentiService = /** @class */ (function () {
    function AllevamentiService() {
    }
    AllevamentiService.prototype.index = function () {
        return axios.get('/api/allevamenti');
    };
    AllevamentiService.prototype.details = function (id) {
        return axios.get('/api/allevamenti/details?id=' + id);
    };
    AllevamentiService.prototype.save = function (allevatore) {
        return axios.post('/api/allevamenti/save', allevatore);
    };
    AllevamentiService.prototype.delete = function (idAllevamento) {
        return axios.delete('/api/allevamenti/delete?id=' + idAllevamento);
    };
    return AllevamentiService;
}());
export { AllevamentiService };
