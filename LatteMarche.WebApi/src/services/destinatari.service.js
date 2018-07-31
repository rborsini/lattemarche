import axios from 'axios';
var DestinatariService = /** @class */ (function () {
    function DestinatariService() {
    }
    DestinatariService.prototype.getDestinatari = function () {
        return axios.get('/api/destinatari');
    };
    DestinatariService.prototype.getDetails = function (id) {
        return axios.get('/api/destinatari/details?id=' + id);
    };
    DestinatariService.prototype.update = function (destinatario) {
        return axios.put('/api/destinatari/update', destinatario);
    };
    return DestinatariService;
}());
export { DestinatariService };
