import axios from 'axios';
var ProfiliService = /** @class */ (function () {
    function ProfiliService() {
    }
    ProfiliService.prototype.getProfili = function () {
        return axios.get('/api/tipiprofilo');
    };
    ProfiliService.prototype.getDetails = function (id) {
        return axios.get('/api/tipiprofilo/details?id=' + id);
    };
    return ProfiliService;
}());
export { ProfiliService };
