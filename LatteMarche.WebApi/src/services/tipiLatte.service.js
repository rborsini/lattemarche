import axios from 'axios';
var TipiLatteService = /** @class */ (function () {
    function TipiLatteService() {
    }
    TipiLatteService.prototype.getTipiLatte = function () {
        return axios.get('/api/TipiLatte');
    };
    return TipiLatteService;
}());
export { TipiLatteService };
