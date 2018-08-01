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
import GiroTrasportatoriModal from "./components/giroTrasportatoriModal.vue";
import { Trasportatore } from "../../models/trasportatore.model";
import { Giro } from "../../models/giro.model";
import { GiriService } from "../../services/giri.service";
import { TrasportatoriService } from "../../services/trasportatori.service";
var TrasportatoriEditPage = /** @class */ (function (_super) {
    __extends(TrasportatoriEditPage, _super);
    function TrasportatoriEditPage() {
        var _this = _super.call(this) || this;
        _this.trasportatori = [];
        _this.trasportatoreSelezionato = true;
        _this.selectedGiro = 0;
        _this.selectedTrasportatore = 0;
        _this.trasportatore = new Trasportatore();
        _this.trasportatore.Giri[0] = new Giro();
        _this.trasportatoriService = new TrasportatoriService();
        _this.giro = new Giro();
        _this.giriService = new GiriService();
        return _this;
    }
    TrasportatoriEditPage.prototype.mounted = function () {
        this.loadTrasportatori();
    };
    // caricamento trasportatori
    TrasportatoriEditPage.prototype.loadTrasportatori = function () {
        var _this = this;
        this.trasportatoriService.getTrasportatori()
            .then(function (response) {
            if (response.data != null) {
                _this.trasportatori = response.data;
            }
        });
    };
    // carico allevamenti se seleziono trasportatore
    TrasportatoriEditPage.prototype.onTrasportatoreSelezionato = function (idTrasportatore) {
        var _this = this;
        this.trasportatoreSelezionato = false;
        this.giro = new Giro();
        this.selectedGiro = 0;
        this.trasportatoriService.getTrasportatoreDetails(idTrasportatore)
            .then(function (response) {
            _this.trasportatore = response.data;
        });
    };
    // Selezione / Deselezione item del giro
    TrasportatoriEditPage.prototype.onItemSelectedChanged = function (event, item) {
        if (!item.Selezionato) {
            item.Priorita = undefined;
        }
    };
    // modifica giro
    TrasportatoriEditPage.prototype.modificaGiro = function (id) {
        var _this = this;
        this.giriService.getGiroDetails(id)
            .then(function (response) {
            if (response.data != null) {
                _this.$refs.giroTrasportatoriModal.openGiro(response.data);
            }
            else {
                return null;
            }
        });
    };
    // aggiungo giro
    TrasportatoriEditPage.prototype.aggiungiGiro = function () {
        this.giro = new Giro();
        this.$refs.giroTrasportatoriModal.open();
        this.giro.IdTrasportatore = this.trasportatore.Id;
        this.giro.CodiceGiro = "";
        this.giro.Denominazione = "";
    };
    // carico allevatori
    TrasportatoriEditPage.prototype.loadGiro = function (id) {
        var _this = this;
        this.giriService.getGiroDetails(id)
            .then(function (response) {
            if (response.data != null) {
                _this.giro = response.data;
                for (var i = 0; i < _this.giro.Items.length; i++) {
                    if (_this.giro.Items[i].Priorita != null) {
                        _this.giro.Items[i].Selezionato = true;
                    }
                    else {
                        _this.giro.Items[i].Selezionato = false;
                    }
                }
            }
        });
    };
    // salva giro trasportatori
    TrasportatoriEditPage.prototype.salvaGiro = function () {
        var _this = this;
        this.$refs.waiter.open();
        this.giriService.save(this.giro)
            .then(function (response) {
            if (response.data != undefined) {
                _this.$refs.waiter.close();
                _this.$refs.salvataggioGiroModal.open();
            }
            else {
                _this.giro = response.data;
                //this.$refs.waiter.close();
                _this.$refs.salvataggioGiroModal.open();
            }
        });
    };
    TrasportatoriEditPage = __decorate([
        Component({
            el: '#trasportatori-page',
            components: {
                Select2: Select2,
                Waiter: Waiter,
                NotificationDialog: NotificationDialog,
                GiroTrasportatoriModal: GiroTrasportatoriModal
            }
        }),
        __metadata("design:paramtypes", [])
    ], TrasportatoriEditPage);
    return TrasportatoriEditPage;
}(Vue));
export default TrasportatoriEditPage;
var page = new TrasportatoriEditPage();
Vue.config.devtools = true;
