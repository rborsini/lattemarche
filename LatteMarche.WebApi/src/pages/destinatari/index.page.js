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
import Select2 from "../../components/common/select2.vue";
import EditazioneDestinatarioModal from "../destinatari/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import { Destinatario } from "../../models/destinatario.model";
import { DestinatariService } from "../../services/destinatari.service";
var DestinatariIndexPage = /** @class */ (function (_super) {
    __extends(DestinatariIndexPage, _super);
    function DestinatariIndexPage() {
        var _this = _super.call(this) || this;
        _this.columnOptions = [];
        _this.destinatari = [];
        _this.canAdd = false;
        _this.canEdit = false;
        _this.canRemove = false;
        _this.destinatariService = new DestinatariService();
        _this.destinatario = new Destinatario();
        _this.canAdd = $('#canAdd').val() == "true";
        _this.canEdit = $('#canEdit').val() == "true";
        _this.canRemove = $('#canRemove').val() == "true";
        return _this;
    }
    DestinatariIndexPage.prototype.mounted = function () {
        var _this = this;
        this.initTable();
        this.destinatariService.index()
            .then(function (response) {
            _this.destinatari = response.data;
        });
    };
    // Evento fine generazione tabella
    DestinatariIndexPage.prototype.onDataLoaded = function () {
        var _this = this;
        $('.edit').click(function (event) {
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");
            _this.destinatariService.details(rowId)
                .then(function (response) {
                _this.destinatario = response.data;
                _this.$refs.editazioneDestinatarioModal.openDestinatario(_this.destinatario);
            });
        });
        $('.delete').click(function (event) {
            var element = $(event.currentTarget);
            _this.idDestinatarioDaRimuovere = $(element).data("row-id");
            _this.$refs.confirmDeleteDialog.open();
        });
    };
    // nuovo destinatario
    DestinatariIndexPage.prototype.onAdd = function () {
        this.destinatario = new Destinatario();
        this.$refs.editazioneDestinatarioModal.open();
    };
    // rimozione destinatario
    DestinatariIndexPage.prototype.onRemove = function () {
        var _this = this;
        this.destinatariService.delete(this.idDestinatarioDaRimuovere)
            .then(function (response) {
            _this.$refs.removedDialog.open();
        });
    };
    // inizializzazione tabella
    DestinatariIndexPage.prototype.initTable = function () {
        this.columnOptions.push({ data: "P_IVA" });
        this.columnOptions.push({ data: "RagioneSociale" });
        var ce = this.canEdit;
        var cr = this.canRemove;
        if (ce || cr) {
            this.columnOptions.push({
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
    };
    DestinatariIndexPage = __decorate([
        Component({
            el: '#destinatari-page',
            components: {
                Select2: Select2,
                ConfirmDialog: ConfirmDialog,
                NotificationDialog: NotificationDialog,
                EditazioneDestinatarioModal: EditazioneDestinatarioModal,
                DataTable: DataTable
            }
        }),
        __metadata("design:paramtypes", [])
    ], DestinatariIndexPage);
    return DestinatariIndexPage;
}(Vue));
export default DestinatariIndexPage;
var page = new DestinatariIndexPage();
Vue.config.devtools = true;
