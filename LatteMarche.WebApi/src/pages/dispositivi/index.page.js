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
import EditazioneDispositivoModal from "../dispositivi/edit.vue";
import Waiter from "../../components/common/waiter.vue";
import { Dispositivo } from "../../models/dispositivo.model";
import { DispositiviService } from "../../services/dispositivi.service";
var DispositiviIndexPage = /** @class */ (function (_super) {
    __extends(DispositiviIndexPage, _super);
    function DispositiviIndexPage() {
        var _this = _super.call(this) || this;
        _this.tableOptions = {};
        _this.dispositivi = [];
        _this.dispositivo = new Dispositivo();
        _this.dispositiviService = new DispositiviService();
        return _this;
    }
    DispositiviIndexPage.prototype.mounted = function () {
        var _this = this;
        this.initTable();
        this.dispositiviService.index()
            .then(function (response) {
            _this.dispositivi = response.data;
        });
    };
    // inizializzazione tabella
    DispositiviIndexPage.prototype.initTable = function () {
        var options = {};
        options.columns = [];
        options.columns.push({ data: "Id" });
        options.columns.push({ data: "Trasportatore_RagioneSociale" });
        options.columns.push({ data: "Attivo" });
        options.columns.push({ data: "DataRegistrazione" });
        options.columns.push({ data: "DataUltimoDownload" });
        options.columns.push({ data: "DataUltimoUpload" });
        options.columns.push({ data: "VersioneApp" });
        options.columns.push({
            render: function (data, type, row) {
                var html = '<div class="text-center">';
                html += '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-edit"></i></a>';
                html += '</div>';
                return html;
            },
            className: "edit-column",
            orderable: false
        });
        this.tableOptions = options;
    };
    // Evento fine generazione tabella
    DispositiviIndexPage.prototype.onDataLoaded = function () {
        var _this = this;
        $('.edit').click(function (event) {
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");
            _this.dispositiviService.details(rowId)
                .then(function (response) {
                _this.dispositivo = response.data;
                _this.$refs.editazioneDispositivoModal.open();
            });
        });
    };
    // evento chiusura popup
    DispositiviIndexPage.prototype.onPopupSave = function () {
        window.location = window.location;
    };
    DispositiviIndexPage = __decorate([
        Component({
            el: '#index-dispositivi-page',
            components: {
                Waiter: Waiter,
                DataTable: DataTable,
                EditazioneDispositivoModal: EditazioneDispositivoModal
            }
        }),
        __metadata("design:paramtypes", [])
    ], DispositiviIndexPage);
    return DispositiviIndexPage;
}(Vue));
export default DispositiviIndexPage;
var page = new DispositiviIndexPage();
Vue.config.devtools = true;
