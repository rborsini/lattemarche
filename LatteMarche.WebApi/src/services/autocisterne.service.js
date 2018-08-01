import axios from 'axios';
var AutocisterneService = /** @class */ (function () {
    function AutocisterneService() {
    }
    AutocisterneService.prototype.getAutocisterne = function () {
        return axios.get('/api/autocisterne');
    };
    AutocisterneService.prototype.getDetails = function (id) {
        return axios.get('/api/autocisterne/details?id=' + id);
    };
    AutocisterneService.prototype.update = function (autocisterna) {
        return axios.put('/api/autocisterne/update', autocisterna);
    };
    return AutocisterneService;
}());
export { AutocisterneService };
