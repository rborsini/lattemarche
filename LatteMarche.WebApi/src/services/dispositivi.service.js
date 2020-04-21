import axios from 'axios';
var DispositiviService = /** @class */ (function () {
    function DispositiviService() {
    }
    DispositiviService.prototype.index = function () {
        return axios.get('/api/dispositivi');
    };
    DispositiviService.prototype.details = function (id) {
        return axios.get('/api/dispositivi/details?id=' + id);
    };
    DispositiviService.prototype.update = function (dispositivo) {
        return axios.put('/api/dispositivi/update', dispositivo);
    };
    return DispositiviService;
}());
export { DispositiviService };
