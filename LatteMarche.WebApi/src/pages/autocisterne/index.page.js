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
import DataTable from "../../components/common/dataTable.vue";
import EditazioneAutocisternaModal from "../autocisterne/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import { Autocisterna } from "../../models/autocisterna.model";
import { AutocisterneService } from "../../services/autocisterne.service";
var AutocisterneIndexPage = /** @class */ (function (_super) {
    __extends(AutocisterneIndexPage, _super);
    function AutocisterneIndexPage() {
        var _this = _super.call(this) || this;
        _this.columnOptions = [];
        _this.autocisterne = [];
        _this.autocisterneService = new AutocisterneService();
        _this.autocisterna = new Autocisterna();
        return _this;
    }
    AutocisterneIndexPage.prototype.mounted = function () {
        var _this = this;
        this.initTable();
        this.autocisterneService.getAutocisterne()
            .then(function (response) {
            _this.autocisterne = response.data;
        });
    };
    // Evento fine generazione tabella
    AutocisterneIndexPage.prototype.onDataLoaded = function () {
        var _this = this;
        $('.edit').click(function (event) {
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");
            _this.autocisterneService.getDetails(rowId)
                .then(function (response) {
                _this.autocisterna = response.data;
                _this.$refs.editazioneAutocisternaModal.open();
            });
        });
    };
    // inizializzazione tabella
    AutocisterneIndexPage.prototype.initTable = function () {
        this.columnOptions.push({ data: "Marca" });
        this.columnOptions.push({ data: "Modello" });
        this.columnOptions.push({ data: "Targa" });
        this.columnOptions.push({ data: "Portata" });
        this.columnOptions.push({ data: "NumScomparti" });
        this.columnOptions.push({
            render: function (data, type, row) {
                return '<a class="edit" style="cursor: pointer;" data-row-id="' + row.Id + '" >Dettagli</a>';
            }
        });
    };
    AutocisterneIndexPage = __decorate([
        Component({
            el: '#autocisterne-page',
            components: {
                NotificationDialog: NotificationDialog,
                EditazioneAutocisternaModal: EditazioneAutocisternaModal,
                DataTable: DataTable
            }
        }),
        __metadata("design:paramtypes", [])
    ], AutocisterneIndexPage);
    return AutocisterneIndexPage;
}(Vue));
export default AutocisterneIndexPage;
var page = new AutocisterneIndexPage();
Vue.config.devtools = true;
