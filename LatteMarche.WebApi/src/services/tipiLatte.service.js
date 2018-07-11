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
    TipiLatteService.prototype.save = function (tipolatte, isNew) {
        if (isNew)
            return axios.post('/api/tipilatte/create', tipolatte);
        else
            return axios.put('/api/tipilatte/update', tipolatte);
    };
    return TipiLatteService;
}());
export { TipiLatteService };
