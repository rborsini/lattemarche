var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import Vue from "vue";
import Component from "vue-class-component";
import Select2 from "../../components/common/select2.vue";
import Waiter from "../../components/common/waiter.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import { DropdownItem } from "../../models/dropdown.model";
import { Utente } from "../../models/utente.model";
import { TipoLatte } from "../../models/tipoLatte.model";
import { Comune } from "../../models/comune.model";
import { UtentiService } from "../../services/utenti.service";
import { TipiLatteService } from "../../services/tipiLatte.service";
import { ComuniService } from "../../services/comuni.service";
var UtentiNewPage = /** @class */ (function (_super) {
    __extends(UtentiNewPage, _super);
    function UtentiNewPage() {
        var _this = _super.call(this) || this;
        _this.opzioniSesso = [];
        _this.opzioniAbilitato = [];
        _this.opzioniVisibile = [];
        _this.comuni = [];
        _this.opzioniProvince = [];
        _this.comune = new Comune;
        _this.id = $('#id').val();
        _this.tipiLatte = new TipoLatte;
        _this.utente = new Utente();
        _this.comuniService = new ComuniService();
        _this.tipiLatteService = new TipiLatteService();
        _this.utentiServices = new UtentiService();
        return _this;
    }
    UtentiNewPage.prototype.mounted = function () {
        var _this = this;
        this.opzioniSesso = this.getOpzioniSessoUtente();
        this.opzioniAbilitato = this.getOpzioniAbilitato();
        this.opzioniVisibile = this.getOpzioniAbilitato();
        this.loadTipiLatte();
        this.comuniService.getProvince()
            .then(function (response) {
            _this.opzioniProvince = response.data;
        });
    };
    // carica dropdown sesso
    UtentiNewPage.prototype.getOpzioniSessoUtente = function () {
        var opzioniSesso = [];
        opzioniSesso.push(new DropdownItem("M", "Maschio"));
        opzioniSesso.push(new DropdownItem("F", "Femmina"));
        return opzioniSesso;
    };
    // carica opzioni abilitato
    UtentiNewPage.prototype.getOpzioniAbilitato = function () {
        var opzioniAbilitato = [];
        opzioniAbilitato.push(new DropdownItem("true", "Si"));
        opzioniAbilitato.push(new DropdownItem("false", "No"));
        return opzioniAbilitato;
    };
    // carica opzioni visibile
    UtentiNewPage.prototype.getOpzioniVisibile = function () {
        var opzioniVisibile = [];
        opzioniVisibile.push(new DropdownItem("true", "Si"));
        opzioniVisibile.push(new DropdownItem("false", "No"));
        return opzioniVisibile;
    };
    // carica tipi latte
    UtentiNewPage.prototype.loadTipiLatte = function () {
        var _this = this;
        this.tipiLatteService.getTipiLatte()
            .then(function (response) {
            if (response.data != null) {
                _this.tipiLatte = response.data;
            }
        });
    };
    // carica comuni
    UtentiNewPage.prototype.loadComuni = function (SiglaProvincia) {
        var _this = this;
        this.comuniService.getComuni(SiglaProvincia)
            .then(function (response) {
            if (response.data != null) {
                _this.comuni = response.data;
            }
        });
    };
    // salvataggio utente
    UtentiNewPage.prototype.onSave = function () {
        var _this = this;
        this.$refs.waiter.open();
        this.utentiServices.update(this.utente)
            .then(function (response) {
            if (response.data != undefined) {
                // TODO: msg di validazione
                _this.$refs.waiter.close();
                _this.$refs.savedDialog.open();
            }
            else {
                // save OK !!
                _this.utente = response.data;
                //this.$refs.waiter.close();
                _this.$refs.savedDialog.open();
            }
        });
    };
    UtentiNewPage = __decorate([
        Component({
            el: '#nuovo-utente-page',
            components: {
                Select2: Select2,
                Waiter: Waiter,
                NotificationDialog: NotificationDialog
            }
        }),
        __metadata("design:paramtypes", [])
    ], UtentiNewPage);
    return UtentiNewPage;
}(Vue));
export default UtentiNewPage;
var page = new UtentiNewPage();
Vue.config.devtools = true;
