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
import EditazioneAcquirenteModal from "../acquirenti/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import { Acquirente } from "../../models/acquirente.model";
import { AcquirentiService } from "../../services/acquirenti.service";
var AcquirentiIndexPage = /** @class */ (function (_super) {
    __extends(AcquirentiIndexPage, _super);
    function AcquirentiIndexPage() {
        var _this = _super.call(this) || this;
        _this.columnOptions = [];
        _this.acquirenti = [];
        _this.canAdd = false;
        _this.canEdit = false;
        _this.canRemove = false;
        _this.acquirentiService = new AcquirentiService();
        _this.acquirente = new Acquirente();
        _this.canAdd = $('#canAdd').val() == "true";
        _this.canEdit = $('#canEdit').val() == "true";
        _this.canRemove = $('#canRemove').val() == "true";
        return _this;
    }
    AcquirentiIndexPage.prototype.mounted = function () {
        var _this = this;
        this.initTable();
        this.acquirentiService.index()
            .then(function (response) {
            _this.acquirenti = response.data;
        });
    };
    // Evento fine generazione tabella
    AcquirentiIndexPage.prototype.onDataLoaded = function () {
        var _this = this;
        $('.edit').click(function (event) {
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");
            _this.acquirentiService.details(rowId)
                .then(function (response) {
                _this.acquirente = response.data;
                _this.$refs.editazioneAcquirenteModal.openAcquirente(_this.acquirente);
            });
        });
        $('.delete').click(function (event) {
            var element = $(event.currentTarget);
            _this.idAcquirenteDaRimuovere = $(element).data("row-id");
            _this.$refs.confirmDeleteDialog.open();
        });
    };
    // nuovo acquirente
    AcquirentiIndexPage.prototype.onAdd = function () {
        this.acquirente = new Acquirente();
        this.$refs.editazioneAcquirenteModal.open();
    };
    // rimozione acquirente
    AcquirentiIndexPage.prototype.onRemove = function () {
        var _this = this;
        this.acquirentiService.delete(this.idAcquirenteDaRimuovere)
            .then(function (response) {
            _this.$refs.removedDialog.open();
        });
    };
    // inizializzazione tabella
    AcquirentiIndexPage.prototype.initTable = function () {
        this.columnOptions.push({ "data": "Piva" });
        this.columnOptions.push({ "data": "RagioneSociale" });
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
                orderable: false
            });
        }
    };
    AcquirentiIndexPage = __decorate([
        Component({
            el: '#acquirenti-page',
            components: {
                Select2: Select2,
                ConfirmDialog: ConfirmDialog,
                NotificationDialog: NotificationDialog,
                EditazioneAcquirenteModal: EditazioneAcquirenteModal,
                DataTable: DataTable
            }
        }),
        __metadata("design:paramtypes", [])
    ], AcquirentiIndexPage);
    return AcquirentiIndexPage;
}(Vue));
export default AcquirentiIndexPage;
var page = new AcquirentiIndexPage();
Vue.config.devtools = true;
