import axios from 'axios';
var DestinatariService = /** @class */ (function () {
    function DestinatariService() {
    }
    DestinatariService.prototype.index = function () {
        return axios.get('/api/destinatari');
    };
    DestinatariService.prototype.details = function (id) {
        return axios.get('/api/destinatari/details?id=' + id);
    };
    DestinatariService.prototype.save = function (destinatario) {
        return axios.post('/api/destinatari/save', destinatario);
    };
    DestinatariService.prototype.delete = function (idDestinatario) {
        return axios.delete('/api/destinatari/delete?id=' + idDestinatario);
    };
    return DestinatariService;
}());
export { DestinatariService };
