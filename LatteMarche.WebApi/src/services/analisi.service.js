import axios from 'axios';
var AnalisiService = /** @class */ (function () {
    function AnalisiService() {
    }
    AnalisiService.prototype.search = function (parameters) {
        return axios.get('/api/analisi/search', { params: parameters });
    };
    AnalisiService.prototype.synch = function () {
        return axios.post('/api/analisi/synch');
    };
    return AnalisiService;
}());
export { AnalisiService };
