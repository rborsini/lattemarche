import axios from "axios";
var ComuniService = /** @class */ (function () {
    function ComuniService() {
    }
    ComuniService.prototype.getComuni = function (idProvincia) {
        var url = '/api/comuni';
        if (idProvincia != '') {
            url += '/search?provincia=';
            url += idProvincia;
        }
        return axios.get(url);
    };
    ComuniService.prototype.getComuneDetails = function (idComune) {
        var url = '/api/comuni/details?id=';
        url += idComune;
        return axios.get(url);
    };
    ComuniService.prototype.getProvince = function () {
        return axios.get('/api/comuni/province');
    };
    return ComuniService;
}());
export { ComuniService };
