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
import { TipoLatte } from "../../models/tipoLatte.model";
import { TipiLatteService } from "../../services/tipiLatte.service";
var TipiLatteEditPage = /** @class */ (function (_super) {
    __extends(TipiLatteEditPage, _super);
    function TipiLatteEditPage() {
        var _this = _super.call(this) || this;
        _this.isNew = true;
        _this.id = $('#id').val();
        _this.tipiLatte = new TipoLatte;
        _this.tipiLatteService = new TipiLatteService();
        return _this;
    }
    TipiLatteEditPage.prototype.mounted = function () {
        this.$refs.waiter.open();
        if (this.id != '') {
            this.loadTipiLatte();
        }
        this.$refs.waiter.close();
    };
    // caricamento tipi latte
    TipiLatteEditPage.prototype.loadTipiLatte = function () {
        var _this = this;
        this.tipiLatteService.getTipoLatte(this.id)
            .then(function (response) {
            if (response.data != null) {
                _this.tipiLatte = response.data;
                _this.isNew = false;
            }
        });
    };
    // salvataggio tipi latte
    TipiLatteEditPage.prototype.onSave = function () {
        var _this = this;
        this.$refs.waiter.open();
        if (!this.isNew) {
            this.tipiLatteService.getTipiLatte()
                .then(function (response) {
                if (response.data != undefined) {
                    // TODO: msg di validazione
                    _this.$refs.waiter.close();
                    _this.$refs.savedDialog.open();
                }
                else {
                    // save OK !!
                    _this.tipiLatte = response.data;
                    //this.$refs.waiter.close();
                    _this.$refs.savedDialog.open();
                }
            });
        }
        else {
            this.tipiLatteService.create(this.tipiLatte)
                .then(function (response) {
                if (response.data != undefined) {
                    // TODO: msg di validazione
                    _this.$refs.waiter.close();
                    _this.$refs.savedDialog.open();
                }
                else {
                    // save OK !!
                    _this.tipiLatte = response.data;
                    //this.$refs.waiter.close();
                    _this.$refs.savedDialog.open();
                }
            });
        }
    };
    TipiLatteEditPage = __decorate([
        Component({
            el: '#tipi-latte-edit',
            components: {
                Waiter: Waiter,
                NotificationDialog: NotificationDialog
            }
        }),
        __metadata("design:paramtypes", [])
    ], TipiLatteEditPage);
    return TipiLatteEditPage;
}(Vue));
export default TipiLatteEditPage;
var page = new TipiLatteEditPage();
Vue.config.devtools = true;
