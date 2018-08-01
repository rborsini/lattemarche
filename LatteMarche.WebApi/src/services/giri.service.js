import axios from 'axios';
var GiriService = /** @class */ (function () {
    function GiriService() {
    }
    GiriService.prototype.getGiroDetails = function (id) {
        return axios.get('/api/giri/details?id=' + id);
    };
    //public save(giro: Giro): AxiosPromise<Giro> {
    //    return axios.put('/api/giri/update', giro);
    //}
    GiriService.prototype.save = function (giro) {
        return axios.put('/api/giri/save', giro);
    };
    return GiriService;
}());
export { GiriService };
