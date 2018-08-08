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
import EditazioneAllevamentoModal from "../allevamenti/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import { Allevamento } from "../../models/allevamento.model";
import { AllevamentiService } from "../../services/allevamenti.service";
var AllevamentiIndexPage = /** @class */ (function (_super) {
    __extends(AllevamentiIndexPage, _super);
    function AllevamentiIndexPage() {
        var _this = _super.call(this) || this;
        _this.columnOptions = [];
        _this.allevamenti = [];
        _this.canAdd = false;
        _this.canEdit = false;
        _this.canRemove = false;
        _this.allevamentiService = new AllevamentiService();
        _this.allevamento = new Allevamento();
        _this.canAdd = $('#canAdd').val() == "true";
        _this.canEdit = $('#canEdit').val() == "true";
        _this.canRemove = $('#canRemove').val() == "true";
        return _this;
    }
    AllevamentiIndexPage.prototype.mounted = function () {
        var _this = this;
        this.initTable();
        this.allevamentiService.index()
            .then(function (response) {
            _this.allevamenti = response.data;
        });
    };
    // Evento fine generazione tabella
    AllevamentiIndexPage.prototype.onDataLoaded = function () {
        var _this = this;
        $('.edit').click(function (event) {
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");
            _this.allevamentiService.details(rowId)
                .then(function (response) {
                _this.allevamento = response.data;
                _this.$refs.editazioneAllevamentoModal.openAllevamento(_this.allevamento);
            });
        });
        $('.delete').click(function (event) {
            var element = $(event.currentTarget);
            _this.idAllevamentoDaRimuovere = $(element).data("row-id");
            _this.$refs.confirmDeleteDialog.open();
        });
    };
    // nuovo allevamento
    AllevamentiIndexPage.prototype.onAdd = function () {
        this.allevamento = new Allevamento();
        this.$refs.editazioneAllevamentoModal.open();
    };
    // rimozione acquirente
    AllevamentiIndexPage.prototype.onRemove = function () {
        var _this = this;
        this.allevamentiService.delete(this.idAllevamentoDaRimuovere)
            .then(function (response) {
            _this.$refs.removedDialog.open();
        });
    };
    // inizializzazione tabella
    AllevamentiIndexPage.prototype.initTable = function () {
        this.columnOptions.push({ data: "Id" });
        this.columnOptions.push({ data: "RagioneSociale" });
        this.columnOptions.push({ data: "CUAA" });
        this.columnOptions.push({ data: "CodiceAsl" });
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
    AllevamentiIndexPage = __decorate([
        Component({
            el: '#index-allevamenti-page',
            components: {
                ConfirmDialog: ConfirmDialog,
                NotificationDialog: NotificationDialog,
                EditazioneAllevamentoModal: EditazioneAllevamentoModal,
                DataTable: DataTable
            }
        }),
        __metadata("design:paramtypes", [])
    ], AllevamentiIndexPage);
    return AllevamentiIndexPage;
}(Vue));
export default AllevamentiIndexPage;
var page = new AllevamentiIndexPage();
Vue.config.devtools = true;
