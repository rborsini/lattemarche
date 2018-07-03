import axios from 'axios';
var TrasportatoreService = /** @class */ (function () {
    function TrasportatoreService() {
    }
    TrasportatoreService.prototype.getTrasportatori = function () {
        return axios.get('/api/trasportatori');
    };
    return TrasportatoreService;
}());
export { TrasportatoreService };
