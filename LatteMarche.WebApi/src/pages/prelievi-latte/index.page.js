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
import Datepicker from "../../components/common/datepicker.vue";
import EditazionePrelievoModal from "../prelievi-latte/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import { PrelievoLatte } from "../../models/prelievoLatte.model";
import { AllevatoriService } from "../../services/allevatori.service";
import { TrasportatoriService } from "../../services/trasportatori.service";
import { AcquirentiService } from "../../services/acquirenti.service";
import { DestinatariService } from "../../services/destinatari.service";
import { PrelieviLatteService } from "../../services/prelieviLatte.service";
var PrelieviLatteIndexPage = /** @class */ (function (_super) {
    __extends(PrelieviLatteIndexPage, _super);
    function PrelieviLatteIndexPage() {
        var _this = _super.call(this) || this;
        _this.columnOptions = [];
        _this.allevatori = [];
        _this.trasportatore = [];
        _this.destinatario = [];
        _this.acquirente = [];
        _this.prelievi = [];
        _this.idAllevatoreSelezionato = 0;
        _this.idTrasportatoreSelezionato = 0;
        _this.idDestinatarioSelezionato = 0;
        _this.idAcquirenteSelezionato = 0;
        _this.dal = "";
        _this.al = "";
        _this.canAdd = false;
        _this.canEdit = false;
        _this.canRemove = false;
        _this.canSearchAllevatore = false;
        _this.canSearchTrasportatore = false;
        _this.canSearchAcquirente = false;
        _this.canSearchDestinatario = false;
        _this.totale_prelievi_kg = 0;
        _this.totale_prelievi_lt = 0;
        _this.prelieviLatteService = new PrelieviLatteService();
        _this.allevatoriService = new AllevatoriService();
        _this.prelievoSelezionato = new PrelievoLatte();
        _this.trasporatoriService = new TrasportatoriService();
        _this.destinatariService = new DestinatariService();
        _this.acquirentiService = new AcquirentiService();
        _this.canAdd = $('#canAdd').val() == "true";
        _this.canEdit = $('#canEdit').val() == "true";
        _this.canRemove = $('#canRemove').val() == "true";
        _this.canSearchAllevatore = $('#canSearchAllevatore').val() == "true";
        _this.canSearchTrasportatore = $('#canSearchTrasportatore').val() == "true";
        _this.canSearchAcquirente = $('#canSearchAcquirente').val() == "true";
        _this.canSearchDestinatario = $('#canSearchDestinatario').val() == "true";
        return _this;
    }
    PrelieviLatteIndexPage.prototype.mounted = function () {
        this.initTable();
        this.loadAllevatori();
        this.loadTrasportatori();
        this.loadDestinatari();
        this.loadAcquirenti();
        this.initSearchBox();
    };
    // Pulizia selezione
    PrelieviLatteIndexPage.prototype.onAnnullaClick = function () {
        this.initSearchBox();
    };
    // Ricerca
    PrelieviLatteIndexPage.prototype.onCercaClick = function () {
        var _this = this;
        this.totale_prelievi_kg = 0;
        this.totale_prelievi_lt = 0;
        var idAllevatoreStr = this.idAllevatoreSelezionato == 0 ? "" : this.idAllevatoreSelezionato.toString();
        var idTrasportatoreStr = this.idTrasportatoreSelezionato == 0 ? "" : this.idTrasportatoreSelezionato.toString();
        var idAcquirenteStr = this.idAcquirenteSelezionato == 0 ? "" : this.idAcquirenteSelezionato.toString();
        var idDestinatarioStr = this.idDestinatarioSelezionato == 0 ? "" : this.idDestinatarioSelezionato.toString();
        this.loadPrelievi(idAllevatoreStr, idTrasportatoreStr, idAcquirenteStr, idDestinatarioStr, function (prelievi) {
            for (var _i = 0, _a = _this.prelievi; _i < _a.length; _i++) {
                var prelievo = _a[_i];
                _this.totale_prelievi_kg += prelievo.Quantita;
                _this.totale_prelievi_lt += prelievo.QuantitaLitri;
                prelievo.QuantitaLitri = Math.round(prelievo.QuantitaLitri * 100) / 100;
            }
            _this.totale_prelievi_lt = Math.round(_this.totale_prelievi_lt * 100) / 100;
        });
    };
    // Evento fine generazione tabella
    PrelieviLatteIndexPage.prototype.onDataLoaded = function () {
        var _this = this;
        $('.edit').click(function (event) {
            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");
            _this.prelieviLatteService.getPrelievo(rowId)
                .then(function (response) {
                _this.prelievoSelezionato = response.data;
                _this.$refs.editazionePrelievoModal.open();
            });
        });
        $('.delete').click(function (event) {
            var element = $(event.currentTarget);
            _this.idPrelievoDaEliminare = $(element).data("row-id");
            _this.$refs.confirmDeleteDialog.open();
        });
    };
    // Evento richiesta esportazione excel
    PrelieviLatteIndexPage.prototype.onExportClick = function () {
        console.log("on export click");
    };
    // nuova autocisterna
    PrelieviLatteIndexPage.prototype.onAdd = function () {
        this.prelievoSelezionato = new PrelievoLatte();
        this.$refs.editazionePrelievoModal.open();
    };
    // rimozione autocisterna
    PrelieviLatteIndexPage.prototype.onRemove = function () {
        var _this = this;
        this.prelieviLatteService.delete(this.idPrelievoDaEliminare)
            .then(function (response) {
            _this.$refs.removedDialog.open();
        });
    };
    // inizializzazione tabella
    PrelieviLatteIndexPage.prototype.initTable = function () {
        this.columnOptions.push({ data: "DataPrelievoStr" });
        this.columnOptions.push({ data: "DataConsegnaStr" });
        this.columnOptions.push({ data: "DataUltimaMungituraStr" });
        this.columnOptions.push({ data: "Quantita" });
        this.columnOptions.push({ data: "QuantitaLitri" });
        this.columnOptions.push({ data: "Temperatura" });
        this.columnOptions.push({ data: "Trasportatore" });
        this.columnOptions.push({ data: "Acquirente" });
        this.columnOptions.push({ data: "Destinatario" });
        this.columnOptions.push({ data: "Allevamento" });
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
    // inizializzazione parametri di ricerca
    PrelieviLatteIndexPage.prototype.initSearchBox = function () {
        this.idAllevatoreSelezionato = 0;
        this.idTrasportatoreSelezionato = 0;
        this.idAcquirenteSelezionato = 0;
        this.idDestinatarioSelezionato = 0;
        var today = new Date();
        this.al = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
        var lastMonth = this.subtractMonth(today);
        this.dal = lastMonth.getDate() + '/' + (lastMonth.getMonth() + 1) + '/' + lastMonth.getFullYear();
    };
    // caricamento allevatori
    PrelieviLatteIndexPage.prototype.loadAllevatori = function () {
        var _this = this;
        this.allevatoriService.getAllevatori()
            .then(function (response) {
            if (response.data != null) {
                _this.allevatori = response.data;
            }
        });
    };
    // caricamento trasportatori
    PrelieviLatteIndexPage.prototype.loadTrasportatori = function () {
        var _this = this;
        this.trasporatoriService.getTrasportatori()
            .then(function (response) {
            if (response.data != null) {
                _this.trasportatore = response.data;
            }
        });
    };
    // caricamento destinatari
    PrelieviLatteIndexPage.prototype.loadDestinatari = function () {
        var _this = this;
        this.destinatariService.index()
            .then(function (response) {
            if (response.data != null) {
                _this.destinatario = response.data;
            }
        });
    };
    // caricamento acquirenti
    PrelieviLatteIndexPage.prototype.loadAcquirenti = function () {
        var _this = this;
        this.acquirentiService.index()
            .then(function (response) {
            if (response.data != null) {
                _this.acquirente = response.data;
            }
        });
    };
    PrelieviLatteIndexPage.prototype.loadPrelievi = function (idAllevatoreStr, idTrasportatoreStr, idAcquirenteStr, idDestinatarioStr, done) {
        var _this = this;
        this.prelieviLatteService.getPrelievi(idAllevatoreStr, idTrasportatoreStr, idAcquirenteStr, idDestinatarioStr, this.dal, this.al)
            .then(function (response) {
            _this.prelievi = response.data;
            done(_this.prelievi);
        });
    };
    PrelieviLatteIndexPage.prototype.subtractMonth = function (date) {
        var days = 0;
        //get month ritorna un numero da 0 a 11. Dovendo considerare il mese precedente
        //da sottrarre, per comodità aggiungo un numero al case, considerando quindi lo 0 come dicembre
        switch (date.getMonth()) {
            case 4: //Aprile
            case 6: //Giugno
            case 9: //Settembre
            case 11: //Novembre
                {
                    days = 30;
                    break;
                }
            case 2: //febbraio
                {
                    if (date.getFullYear() % 4 != 0) //anno non bisestile
                     {
                        days = 28;
                    }
                    else {
                        days = 29;
                    }
                    break;
                }
            default: //altri mesi
                {
                    days = 31;
                    break;
                }
        }
        date.setDate(date.getDate() - days);
        return date;
    };
    PrelieviLatteIndexPage = __decorate([
        Component({
            el: '#index-prelievi-latte-page',
            components: {
                Select2: Select2,
                ConfirmDialog: ConfirmDialog,
                Datepicker: Datepicker,
                EditazionePrelievoModal: EditazionePrelievoModal,
                NotificationDialog: NotificationDialog,
                DataTable: DataTable
            }
        }),
        __metadata("design:paramtypes", [])
    ], PrelieviLatteIndexPage);
    return PrelieviLatteIndexPage;
}(Vue));
export default PrelieviLatteIndexPage;
var page = new PrelieviLatteIndexPage();
Vue.config.devtools = true;
