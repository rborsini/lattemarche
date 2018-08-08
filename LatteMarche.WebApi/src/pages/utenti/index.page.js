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
import EditazioneUtenteModal from "../utenti/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import { Utente } from "../../models/utente.model";
import { UtentiService } from "../../services/utenti.service";
var UtentiIndexPage = /** @class */ (function (_super) {
    __extends(UtentiIndexPage, _super);
    function UtentiIndexPage() {
        var _this = _super.call(this) || this;
        _this.columnOptions = [];
        _this.utenti = [];
        _this.canAdd = false;
        _this.canEdit = false;
        _this.canRemove = false;
        _this.utentiService = new UtentiService();
        _this.utente = new Utente();
        _this.canAdd = $('#canAdd').val() == "true";
        _this.canEdit = $('#canEdit').val() == "true";
        _this.canRemove = $('#canRemove').val() == "true";
        return _this;
    }
    UtentiIndexPage.prototype.mounted = function () {
        var _this = this;
        this.initTable();
        this.utentiService.index()
            .then(function (response) {
            _this.utenti = response.data;
        });
    };
    // Evento fine generazione tabella
    UtentiIndexPage.prototype.onDataLoaded = function () {
        var _this = this;
        $('.edit').click(function (event) {
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");
            _this.utentiService.details(rowId)
                .then(function (response) {
                _this.utente = response.data;
                _this.$refs.editazioneUtenteModal.openUtente(_this.utente);
            });
        });
        $('.delete').click(function (event) {
            var element = $(event.currentTarget);
            _this.idUtente = $(element).data("row-id");
            _this.$refs.confirmDeleteDialog.open();
        });
    };
    // nuova utente
    UtentiIndexPage.prototype.onAdd = function () {
        this.utente = new Utente();
        this.$refs.editazioneUtenteModal.open();
    };
    // rimozione utente
    UtentiIndexPage.prototype.onRemove = function () {
        var _this = this;
        this.utentiService.delete(this.idUtente)
            .then(function (response) {
            _this.$refs.removedDialog.open();
        });
    };
    // inizializzazione tabella
    UtentiIndexPage.prototype.initTable = function () {
        this.columnOptions.push({ data: "RagioneSociale" });
        this.columnOptions.push({ data: "Nome" });
        this.columnOptions.push({ data: "Cognome" });
        this.columnOptions.push({ data: "Username" });
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
    UtentiIndexPage = __decorate([
        Component({
            el: '#utenti-page',
            components: {
                ConfirmDialog: ConfirmDialog,
                NotificationDialog: NotificationDialog,
                EditazioneUtenteModal: EditazioneUtenteModal,
                DataTable: DataTable
            }
        }),
        __metadata("design:paramtypes", [])
    ], UtentiIndexPage);
    return UtentiIndexPage;
}(Vue));
export default UtentiIndexPage;
var page = new UtentiIndexPage();
Vue.config.devtools = true;
