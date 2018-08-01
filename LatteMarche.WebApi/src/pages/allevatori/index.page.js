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
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import { AllevatoriService } from "../../services/allevatori.service";
var AllevatoriIndexPage = /** @class */ (function (_super) {
    __extends(AllevatoriIndexPage, _super);
    function AllevatoriIndexPage() {
        var _this = _super.call(this) || this;
        _this.columnOptions = [];
        _this.allevatori = [];
        _this.canAdd = false;
        _this.canEdit = false;
        _this.canRemove = false;
        _this.allevatoriService = new AllevatoriService();
        return _this;
    }
    AllevatoriIndexPage.prototype.mounted = function () {
        var _this = this;
        this.initTable();
        this.allevatoriService.getAllevatori()
            .then(function (response) {
            _this.allevatori = response.data;
        });
    };
    // Evento fine generazione tabella
    AllevatoriIndexPage.prototype.onDataLoaded = function () {
        var _this = this;
        $('.edit').click(function (event) {
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");
            _this.allevatoriService.details(rowId)
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
    // inizializzazione tabella
    AllevatoriIndexPage.prototype.initTable = function () {
        this.columnOptions.push({ data: "Id" });
        this.columnOptions.push({ data: "RagioneSociale" });
        this.columnOptions.push({ data: "IndirizzoAllevamento" });
        this.columnOptions.push({ data: "Comune" });
        this.columnOptions.push({ data: "Provincia" });
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
    AllevatoriIndexPage = __decorate([
        Component({
            el: '#index-allevatori-page',
            components: {
                ConfirmDialog: ConfirmDialog,
                NotificationDialog: NotificationDialog,
                DataTable: DataTable
            }
        }),
        __metadata("design:paramtypes", [])
    ], AllevatoriIndexPage);
    return AllevatoriIndexPage;
}(Vue));
export default AllevatoriIndexPage;
var page = new AllevatoriIndexPage();
Vue.config.devtools = true;
