import axios from 'axios';
var GiriService = /** @class */ (function () {
    function GiriService() {
    }
    GiriService.prototype.getGiroDetails = function (id) {
        return axios.get('/api/giri/details?id=' + id);
    };
    GiriService.prototype.save = function (giro) {
        return axios.put('/api/giri/update', giro);
    };
    return GiriService;
}());
export { GiriService };
