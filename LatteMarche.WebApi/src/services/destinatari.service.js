import axios from 'axios';
var DestinatariService = /** @class */ (function () {
    function DestinatariService() {
    }
    DestinatariService.prototype.getDestinatari = function () {
        return axios.get('/api/destinatari');
    };
    return DestinatariService;
}());
export { DestinatariService };
