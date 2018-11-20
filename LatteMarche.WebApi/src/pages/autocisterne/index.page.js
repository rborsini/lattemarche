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
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import { Autocisterna } from "../../models/autocisterna.model";
import { AutocisterneService } from "../../services/autocisterne.service";
var AutocisterneIndexPage = /** @class */ (function (_super) {
    __extends(AutocisterneIndexPage, _super);
    function AutocisterneIndexPage() {
        var _this = _super.call(this) || this;
        _this.tableOptions = {};
        _this.autocisterne = [];
        _this.canAdd = false;
        _this.canEdit = false;
        _this.canRemove = false;
        _this.autocisterneService = new AutocisterneService();
        _this.autocisterna = new Autocisterna();
        _this.canAdd = $('#canAdd').val() == "true";
        _this.canEdit = $('#canEdit').val() == "true";
        _this.canRemove = $('#canRemove').val() == "true";
        return _this;
    }
    AutocisterneIndexPage.prototype.mounted = function () {
        var _this = this;
        this.initTable();
        this.autocisterneService.index()
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
            _this.autocisterneService.details(rowId)
                .then(function (response) {
                _this.autocisterna = response.data;
                _this.$refs.editazioneAutocisternaModal.open();
            });
        });
        $('.delete').click(function (event) {
            var element = $(event.currentTarget);
            _this.idAutocisterna = $(element).data("row-id");
            _this.$refs.confirmDeleteDialog.open();
        });
    };
    // nuova autocisterna
    AutocisterneIndexPage.prototype.onAdd = function () {
        this.autocisterna = new Autocisterna();
        this.$refs.editazioneAutocisternaModal.open();
    };
    // rimozione autocisterna
    AutocisterneIndexPage.prototype.onRemove = function () {
        var _this = this;
        this.autocisterneService.delete(this.idAutocisterna)
            .then(function (response) {
            _this.$refs.removedDialog.open();
        });
    };
    // inizializzazione tabella
    AutocisterneIndexPage.prototype.initTable = function () {
        var options = {};
        options.columns = [];
        options.columns.push({ data: "Marca" });
        options.columns.push({ data: "Modello" });
        options.columns.push({ data: "Targa" });
        options.columns.push({ data: "Portata" });
        var ce = this.canEdit;
        var cr = this.canRemove;
        if (ce || cr) {
            options.columns.push({
                render: function (data, type, row) {
                    var html = '<div class="text-center">';
                    if (ce)
                        html += '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-edit"></i></a>';
                    if (cr)
                        html += '<a class="pl-3 delete" title="elimina" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-trash-alt"></i></a>';
                    html += '</div>';
                    return html;
                },
                className: "edit-column",
                orderable: false
            });
        }
        this.tableOptions = options;
    };
    AutocisterneIndexPage = __decorate([
        Component({
            el: '#autocisterne-page',
            components: {
                ConfirmDialog: ConfirmDialog,
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
