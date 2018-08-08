import axios from 'axios';
var TipiLatteService = /** @class */ (function () {
    function TipiLatteService() {
    }
    TipiLatteService.prototype.index = function () {
        return axios.get('/api/tipilatte');
    };
    TipiLatteService.prototype.details = function (id) {
        return axios.get('/api/tipilatte/details?id=' + id);
    };
    TipiLatteService.prototype.save = function (tipolatte) {
        return axios.post('/api/tipilatte/save', tipolatte);
    };
    TipiLatteService.prototype.delete = function (idTipoLatte) {
        return axios.delete('/api/tipilatte/delete?id=' + idTipoLatte);
    };
    return TipiLatteService;
}());
export { TipiLatteService };
