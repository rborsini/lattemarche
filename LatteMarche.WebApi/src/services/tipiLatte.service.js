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
    //public save(tipolatte: TipoLatte, isNew: boolean) {
    //    if (isNew)
    //        return axios.post('/api/tipilatte/create', tipolatte);
    //    else
    //        return axios.put('/api/tipilatte/update', tipolatte);
    //}
    TipiLatteService.prototype.save = function (tipolatte) {
        return axios.post('/api/autocisterne/save', tipolatte);
    };
    TipiLatteService.prototype.delete = function (idTipoLatte) {
        return axios.delete('/api/autocisterne/delete?id=' + idTipoLatte);
    };
    return TipiLatteService;
}());
export { TipiLatteService };
