import axios from 'axios';
var TrasportatoriService = /** @class */ (function () {
    function TrasportatoriService() {
    }
    TrasportatoriService.prototype.getTrasportatori = function () {
        return axios.get('/api/trasportatori');
    };
    TrasportatoriService.prototype.getTrasportatoreDetails = function (idTrasportatore) {
        var url = '/api/trasportatori/details?id=';
        url += idTrasportatore;
        return axios.get(url);
    };
    return TrasportatoriService;
}());
export { TrasportatoriService };
