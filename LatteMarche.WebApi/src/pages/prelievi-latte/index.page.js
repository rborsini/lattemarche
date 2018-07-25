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
import EditazionePrelievoModal from "../prelievi-latte/components/editazionePrelievoModal.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import { PrelievoLatte } from "../../models/prelievoLatte.model";
import { AllevatoriService } from "../../services/allevatori.service";
import { PrelieviLatteService } from "../../services/prelieviLatte.service";
var PrelieviLatteIndexPage = /** @class */ (function (_super) {
    __extends(PrelieviLatteIndexPage, _super);
    function PrelieviLatteIndexPage() {
        var _this = _super.call(this) || this;
        _this.columnOptions = [];
        _this.allevatori = [];
        _this.prelievi = [];
        _this.idAllevatoreSelezionato = 0;
        _this.dal = "";
        _this.al = "";
        _this.prelieviLatteService = new PrelieviLatteService();
        _this.allevatoriService = new AllevatoriService();
        _this.prelievoSelezionato = new PrelievoLatte();
        return _this;
    }
    PrelieviLatteIndexPage.prototype.mounted = function () {
        this.initTable();
        this.loadAllevatori();
        this.initSearchBox();
    };
    // Pulizia selezione
    PrelieviLatteIndexPage.prototype.onAnnullaClick = function () {
        this.initSearchBox();
    };
    // Ricerca
    PrelieviLatteIndexPage.prototype.onCercaClick = function () {
        var _this = this;
        var idAllevatoreStr = this.idAllevatoreSelezionato == 0 ? "" : this.idAllevatoreSelezionato.toString();
        this.prelieviLatteService.getPrelievi(idAllevatoreStr, this.dal, this.al)
            .then(function (response) {
            _this.prelievi = response.data;
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
    };
    // Evento richiesta esportazione excel
    PrelieviLatteIndexPage.prototype.onExportClick = function () {
        console.log("on export click");
    };
    // inizializzazione tabella
    PrelieviLatteIndexPage.prototype.initTable = function () {
        this.columnOptions.push({ data: "DataPrelievoStr" });
        this.columnOptions.push({ data: "DataConsegnaStr" });
        this.columnOptions.push({ data: "Quantita" });
        this.columnOptions.push({ data: "Temperatura" });
        this.columnOptions.push({ data: "IdTrasportatore" });
        this.columnOptions.push({ data: "IdAllevamento" });
        this.columnOptions.push({
            render: function (data, type, row) {
                return '<a class="edit" style="cursor: pointer;" data-row-id="' + row.Id + '" >Dettagli</a>';
            }
        });
    };
    // inizializzazione parametri di ricerca
    PrelieviLatteIndexPage.prototype.initSearchBox = function () {
        this.idAllevatoreSelezionato = 0;
        var today = new Date();
        this.al = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
        var yesterday = this.addDays(today, -1);
        this.dal = yesterday.getDate() + '/' + (yesterday.getMonth() + 1) + '/' + yesterday.getFullYear();
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
    PrelieviLatteIndexPage.prototype.addDays = function (date, days) {
        //console.log('adding ' + days + ' days');
        //console.log(date);
        date.setDate(date.getDate() + days);
        //console.log(date);
        return date;
    };
    PrelieviLatteIndexPage = __decorate([
        Component({
            el: '#index-prelievi-latte-page',
            components: {
                Select2: Select2,
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
