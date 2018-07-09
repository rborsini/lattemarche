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
import Waiter from "../../components/common/waiter.vue";
import Datepicker from "../../components/common/datepicker.vue";
import EditazionePrelievoModal from "../prelievi-latte/components/editazionePrelievoModal.vue";
import { PrelievoLatte } from "../../models/prelievoLatte.model";
import { Utente } from "../../models/utente.model";
import { PrelieviLatteService } from "../../services/prelieviLatte.service";
import { UtentiService } from "../../services/utenti.service";
var PrelieviLatteEditPage = /** @class */ (function (_super) {
    __extends(PrelieviLatteEditPage, _super);
    function PrelieviLatteEditPage() {
        var _this = _super.call(this) || this;
        _this.dataInzio = "";
        _this.dataFine = "";
        _this.prelievi = [];
        _this.id = $('#id').val();
        _this.utente = new Utente();
        _this.today = new Date();
        _this.prelieviLatteService = new PrelieviLatteService();
        _this.utentiService = new UtentiService();
        _this.prelievoSelezionato = new PrelievoLatte();
        return _this;
    }
    PrelieviLatteEditPage.prototype.mounted = function () {
        var _this = this;
        this.$refs.waiter.open();
        this.dataFine = String(this.today.getDate()) + '/' + String(this.today.getMonth() + 1) + '/' + String(this.today.getFullYear());
        //this.today = this.today.setMonth(this.today.getMonth() - 1);
        this.loadUtente();
        this.dataInzio = '25-04-2018'; //String(this.today.getDate()) + '-' + String(this.today.getMonth() + 1) + '-' + String(this.today.getFullYear());
        this.loadPrelievi(function (prelievi) {
            _this.$refs.waiter.close();
        });
    };
    PrelieviLatteEditPage.prototype.loadPrelievi = function (done) {
        var _this = this;
        console.log('Chiamata servizio');
        this.prelieviLatteService.getPrelievi(this.id, this.dataInzio, this.dataFine)
            .then(function (response) {
            _this.prelievi = response.data;
            done(_this.prelievi);
        });
    };
    PrelieviLatteEditPage.prototype.loadUtente = function () {
        var _this = this;
        this.utentiService.getDetails(this.id)
            .then(function (response) {
            _this.utente = response.data;
        });
    };
    PrelieviLatteEditPage.prototype.onPrelievoSelezionato = function (prelievo) {
        this.prelievoSelezionato = prelievo;
        this.$refs.editazionePrelievoModal.open();
    };
    PrelieviLatteEditPage = __decorate([
        Component({
            el: '#prelievi-latte-edit',
            components: {
                Waiter: Waiter,
                Datepicker: Datepicker,
                EditazionePrelievoModal: EditazionePrelievoModal
            }
        }),
        __metadata("design:paramtypes", [])
    ], PrelieviLatteEditPage);
    return PrelieviLatteEditPage;
}(Vue));
export default PrelieviLatteEditPage;
var page = new PrelieviLatteEditPage();
Vue.config.devtools = true;
