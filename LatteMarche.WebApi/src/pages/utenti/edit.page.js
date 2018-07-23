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
import { RuoliService } from "../../services/ruoli.service";
var UtentiEditPage = /** @class */ (function (_super) {
    __extends(UtentiEditPage, _super);
    function UtentiEditPage() {
        var _this = _super.call(this) || this;
        _this.ruoli = [];
        _this.opzioniSesso = [];
        _this.opzioniAbilitato = [];
        _this.opzioniVisibile = [];
        _this.comuni = [];
        _this.opzioniProvince = [];
        _this.isNew = true;
        _this.comune = new Comune;
        _this.id = $('#id').val();
        _this.tipoLatte = new TipoLatte;
        _this.utente = new Utente();
        _this.comuniService = new ComuniService();
        _this.tipiLatteService = new TipiLatteService();
        _this.utentiService = new UtentiService();
        _this.ruoliService = new RuoliService();
        return _this;
    }
    UtentiEditPage.prototype.mounted = function () {
        var _this = this;
        this.$refs.waiter.open();
        if (this.id != '') {
            this.loadUtente(function (utente) {
                _this.isNew = false;
                _this.loadComuni(_this.utente.SiglaProvincia);
                _this.comuniService.getProvince()
                    .then(function (response) {
                    _this.opzioniProvince = response.data;
                });
                _this.opzioniSesso = _this.getOpzioniSessoUtente();
                _this.opzioniAbilitato = _this.getOpzioniAbilitato();
                _this.opzioniVisibile = _this.getOpzioniAbilitato();
                _this.loadTipiLatte();
                _this.loadProfili();
                _this.$refs.waiter.close();
            });
        }
    };
    // carica utente
    UtentiEditPage.prototype.loadUtente = function (done) {
        var _this = this;
        this.utentiService.getDetails(this.id)
            .then(function (response) {
            _this.utente = response.data;
            done(_this.utente);
        });
    };
    // carica dropdown sesso
    UtentiEditPage.prototype.getOpzioniSessoUtente = function () {
        var opzioniSesso = [];
        opzioniSesso.push(new DropdownItem("M", "Maschio"));
        opzioniSesso.push(new DropdownItem("F", "Femmina"));
        return opzioniSesso;
    };
    // carica opzioni abilitato
    UtentiEditPage.prototype.getOpzioniAbilitato = function () {
        var opzioniAbilitato = [];
        opzioniAbilitato.push(new DropdownItem("true", "Si"));
        opzioniAbilitato.push(new DropdownItem("false", "No"));
        return opzioniAbilitato;
    };
    // carica opzioni visibile
    UtentiEditPage.prototype.getOpzioniVisibile = function () {
        var opzioniVisibile = [];
        opzioniVisibile.push(new DropdownItem("true", "Si"));
        opzioniVisibile.push(new DropdownItem("false", "No"));
        return opzioniVisibile;
    };
    // caricamento tipi latte
    UtentiEditPage.prototype.loadTipiLatte = function () {
        var _this = this;
        this.tipiLatteService.getTipiLatte()
            .then(function (response) {
            if (response.data != null) {
                _this.tipoLatte = response.data;
            }
        });
    };
    // carica comuni
    UtentiEditPage.prototype.loadComuni = function (provincia) {
        var _this = this;
        this.comuniService.getComuni(provincia)
            .then(function (response) {
            if (response.data != null) {
                _this.comuni = response.data;
            }
        });
    };
    // carica tipi profilo
    UtentiEditPage.prototype.loadProfili = function () {
        var _this = this;
        this.ruoliService.getRuoli()
            .then(function (response) {
            if (response.data != null) {
                _this.ruoli = response.data;
            }
        });
    };
    // carico provincia se seleziono comune (senza aver precedentemente selezionato la provincia)
    UtentiEditPage.prototype.onComuneSelezionato = function (idComune) {
        var _this = this;
        if (this.utente.SiglaProvincia == '') {
            this.comuniService.getComuneDetails(idComune)
                .then(function (response) {
                _this.utente.SiglaProvincia = response.data.Provincia;
            });
        }
    };
    // salvataggio utente
    UtentiEditPage.prototype.onSave = function () {
        var _this = this;
        this.$refs.waiter.open();
        if (!this.isNew) {
            this.utentiService.update(this.utente)
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
        }
        else {
            this.utentiService.create(this.utente)
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
        }
    };
    UtentiEditPage = __decorate([
        Component({
            el: '#utenti-edit',
            components: {
                Select2: Select2,
                Waiter: Waiter,
                NotificationDialog: NotificationDialog
            }
        }),
        __metadata("design:paramtypes", [])
    ], UtentiEditPage);
    return UtentiEditPage;
}(Vue));
export default UtentiEditPage;
var page = new UtentiEditPage();
Vue.config.devtools = true;
