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
import NotificationDialog from "../../components/common/notificationDialog.vue";
import { Ruolo } from "../../models/ruolo.model";
import { RuoliService } from "../../services/ruoli.service";
var UtentiDetailsPage = /** @class */ (function (_super) {
    __extends(UtentiDetailsPage, _super);
    function UtentiDetailsPage() {
        var _this = _super.call(this) || this;
        _this.id = $('#id').val();
        _this.ruolo = new Ruolo();
        _this.ruoliService = new RuoliService();
        return _this;
    }
    UtentiDetailsPage.prototype.mounted = function () {
        this.loadRuolo(function (ruolo) {
            // boh!
        });
    };
    // carica ruolo
    UtentiDetailsPage.prototype.loadRuolo = function (done) {
        var _this = this;
        this.ruoliService.getDetails(this.id)
            .then(function (response) {
            console.log("response.data", response.data);
            _this.ruolo = response.data;
            done(_this.ruolo);
        });
    };
    // salvataggio utente
    UtentiDetailsPage.prototype.onSave = function () {
        var _this = this;
        this.$refs.waiter.open();
        this.ruoliService.update(this.ruolo)
            .then(function (response) {
            if (response.data != undefined) {
                // TODO: msg di validazione
                _this.$refs.waiter.close();
                _this.$refs.savedDialog.open();
            }
            else {
                // save OK !!
                _this.ruolo = response.data;
                //this.$refs.waiter.close();
                _this.$refs.savedDialog.open();
            }
        });
    };
    UtentiDetailsPage = __decorate([
        Component({
            el: '#ruoli-edit',
            components: {
                Waiter: Waiter,
                NotificationDialog: NotificationDialog
            }
        }),
        __metadata("design:paramtypes", [])
    ], UtentiDetailsPage);
    return UtentiDetailsPage;
}(Vue));
export default UtentiDetailsPage;
var page = new UtentiDetailsPage();
Vue.config.devtools = true;
