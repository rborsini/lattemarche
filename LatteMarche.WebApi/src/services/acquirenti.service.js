import axios from 'axios';
var AcquirentiService = /** @class */ (function () {
    function AcquirentiService() {
    }
    AcquirentiService.prototype.getAcquirenti = function () {
        return axios.get('/api/acquirenti');
    };
    return AcquirentiService;
}());
export { AcquirentiService };
