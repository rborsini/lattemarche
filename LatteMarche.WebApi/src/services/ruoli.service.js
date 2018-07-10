import axios from 'axios';
var RuoliService = /** @class */ (function () {
    function RuoliService() {
    }
    RuoliService.prototype.getRuoli = function () {
        return axios.get('/api/ruoli');
    };
    RuoliService.prototype.getDetails = function (id) {
        return axios.get('/api/ruoli/details?id=' + id);
    };
    RuoliService.prototype.update = function (ruolo) {
        return axios.put('/api/ruoli/update', ruolo);
    };
    RuoliService.prototype.create = function (ruolo) {
        return axios.post('/api/ruoli/create', ruolo);
    };
    return RuoliService;
}());
export { RuoliService };
