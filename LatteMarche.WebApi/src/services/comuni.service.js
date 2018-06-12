import axios from "axios";
var ComuniService = /** @class */ (function () {
    function ComuniService() {
    }
    ComuniService.prototype.getProvince = function () {
        return axios.get('/api/Comuni/Province');
    };
    return ComuniService;
}());
export { ComuniService };
