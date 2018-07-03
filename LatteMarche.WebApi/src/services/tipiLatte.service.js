import axios from 'axios';
var TipiLatteService = /** @class */ (function () {
    function TipiLatteService() {
    }
    TipiLatteService.prototype.getTipiLatte = function () {
        return axios.get('/api/tipilatte');
    };
    TipiLatteService.prototype.getTipoLatte = function (id) {
        return axios.get('/api/tipilatte/details?id=' + id);
    };
    //TODO
    TipiLatteService.prototype.create = function (tipolatte) {
        return axios.post('/api/tipilatte/create', tipolatte);
    };
    TipiLatteService.prototype.update = function (tipolatte) {
        return axios.put('/api/utenti/update', tipolatte);
    };
    return TipiLatteService;
}());
export { TipiLatteService };
