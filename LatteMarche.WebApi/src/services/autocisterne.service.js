import axios from 'axios';
var AutocisterneService = /** @class */ (function () {
    function AutocisterneService() {
    }
    AutocisterneService.prototype.index = function () {
        return axios.get('/api/autocisterne');
    };
    AutocisterneService.prototype.details = function (id) {
        return axios.get('/api/autocisterne/details?id=' + id);
    };
    AutocisterneService.prototype.save = function (autocisterna) {
        return axios.post('/api/autocisterne/save', autocisterna);
    };
    AutocisterneService.prototype.delete = function (idAutocisterna) {
        return axios.delete('/api/autocisterne/delete?id=' + idAutocisterna);
    };
    return AutocisterneService;
}());
export { AutocisterneService };
