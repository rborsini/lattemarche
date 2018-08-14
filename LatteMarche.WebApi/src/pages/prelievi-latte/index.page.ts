import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import Select2 from "../../components/common/select2.vue";
import Datepicker from "../../components/common/datepicker.vue";
import EditazionePrelievoModal from "../prelievi-latte/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";

import { Allevatore } from "../../models/allevatore.model";
import { Trasportatore } from "../../models/trasportatore.model";
import { Acquirente } from "../../models/acquirente.model";
import { Destinatario } from "../../models/destinatario.model";
import { PrelievoLatte } from "../../models/prelievoLatte.model";

import { AllevatoriService } from "../../services/allevatori.service";
import { TrasportatoriService } from "../../services/trasportatori.service";
import { AcquirentiService } from "../../services/acquirenti.service";
import { DestinatariService } from "../../services/destinatari.service";
import { PrelieviLatteService } from "../../services/prelieviLatte.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#index-prelievi-latte-page',
    components: {
        Select2,
        ConfirmDialog,
        Datepicker,
        EditazionePrelievoModal,
        NotificationDialog,
        DataTable
    }
})


export default class PrelieviLatteIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        editazionePrelievoModal: Vue,
        confirmDeleteDialog: Vue,
        removedDialog: Vue
    }

    private prelieviLatteService: PrelieviLatteService;
    private allevatoriService: AllevatoriService;
    public trasporatoriService: TrasportatoriService;
    public destinatariService: DestinatariService;
    public acquirentiService: AcquirentiService;

    public columnOptions: any[] = [];
    public allevatori: Allevatore[] = [];
    public trasportatore: Trasportatore[] = [];
    public destinatario: Destinatario[] = [];
    public acquirente: Acquirente[] = [];
    public prelievi: PrelievoLatte[] = [];
    private idPrelievoDaEliminare: number;

    public idAllevatoreSelezionato: number = 0;
    public idTrasportatoreSelezionato: number = 0;
    public idDestinatarioSelezionato: number = 0;
    public idAcquirenteSelezionato: number = 0;


    public prelievoSelezionato: PrelievoLatte;
    public dal: string = "";
    public al: string = "";

    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;
    public canSearchAllevatore: boolean = false;
    public canSearchTrasportatore: boolean = false;
    public canSearchAcquirente: boolean = false;
    public canSearchDestinatario: boolean = false;

    public totale_prelievi_kg: number = 0;
    public totale_prelievi_lt: number = 0;

    constructor() {
        super();

        this.prelieviLatteService = new PrelieviLatteService();
        this.allevatoriService = new AllevatoriService();
        this.prelievoSelezionato = new PrelievoLatte();
        this.trasporatoriService = new TrasportatoriService();
        this.destinatariService = new DestinatariService();
        this.acquirentiService = new AcquirentiService();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
        this.canSearchAllevatore = $('#canSearchAllevatore').val() == "true";
        this.canSearchTrasportatore = $('#canSearchTrasportatore').val() == "true";
        this.canSearchAcquirente = $('#canSearchAcquirente').val() == "true";
        this.canSearchDestinatario = $('#canSearchDestinatario').val() == "true";

    }

    public mounted() {
        this.initTable();
        this.loadAllevatori();
        this.loadTrasportatori();
        this.loadDestinatari();
        this.loadAcquirenti();
        this.initSearchBox();
    }

    // Pulizia selezione
    public onAnnullaClick() {
        this.initSearchBox();
    }

    // Ricerca
    public onCercaClick() {
        this.totale_prelievi_kg = 0;
        this.totale_prelievi_lt = 0;
        var idAllevatoreStr = this.idAllevatoreSelezionato == 0 ? "" : this.idAllevatoreSelezionato.toString();
        var idTrasportatoreStr = this.idTrasportatoreSelezionato == 0 ? "" : this.idTrasportatoreSelezionato.toString();
        var idAcquirenteStr = this.idAcquirenteSelezionato == 0 ? "" : this.idAcquirenteSelezionato.toString();
        var idDestinatarioStr = this.idDestinatarioSelezionato == 0 ? "" : this.idDestinatarioSelezionato.toString();
        this.loadPrelievi(idAllevatoreStr, idTrasportatoreStr, idAcquirenteStr, idDestinatarioStr, (prelievi: PrelievoLatte[]) => {
            for (let prelievo of this.prelievi) {
                this.totale_prelievi_kg += prelievo.Quantita;
                this.totale_prelievi_lt += prelievo.QuantitaLitri;
                prelievo.QuantitaLitri = Math.round(prelievo.QuantitaLitri * 100) / 100;
            }
            this.totale_prelievi_lt = Math.round(this.totale_prelievi_lt * 100) / 100;
        });

    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.prelieviLatteService.getPrelievo(rowId)
                .then(response => {
                    this.prelievoSelezionato = response.data;
                    this.$refs.editazionePrelievoModal.open();
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idPrelievoDaEliminare = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });
    }

    // Evento richiesta esportazione excel
    public onExportClick() {
        console.log("on export click");
    }

    // nuova autocisterna
    public onAdd() {

        this.prelievoSelezionato = new PrelievoLatte();
        this.$refs.editazionePrelievoModal.open();

    }

    // rimozione autocisterna
    public onRemove() {

        this.prelieviLatteService.delete(this.idPrelievoDaEliminare)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }

    // inizializzazione tabella
    private initTable(): void {
        this.columnOptions.push({ data: "DataPrelievoStr" });
        this.columnOptions.push({ data: "DataConsegnaStr" });
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
                render: function (data: any, type: any, row: any) {

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

    }

    // inizializzazione parametri di ricerca
    private initSearchBox() {
        this.idAllevatoreSelezionato = 0;
        this.idTrasportatoreSelezionato = 0;
        this.idAcquirenteSelezionato = 0;
        this.idDestinatarioSelezionato = 0;


        var today = new Date();
        this.al = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

        var yesterday = this.addDays(today, -1);

        this.dal = yesterday.getDate() + '/' + (yesterday.getMonth() + 1) + '/' + yesterday.getFullYear();

    }

    // caricamento allevatori
    private loadAllevatori(): void {

        this.allevatoriService.getAllevatori()
            .then(response => {
                if (response.data != null) {
                    this.allevatori = response.data;
                }
            });

    }

    // caricamento trasportatori
    private loadTrasportatori() {
        this.trasporatoriService.getTrasportatori()
            .then(response => {
                if (response.data != null) {
                    this.trasportatore = response.data;
                }
            });
    }

    // caricamento destinatari
    private loadDestinatari() {
        this.destinatariService.index()
            .then(response => {
                if (response.data != null) {
                    this.destinatario = response.data;
                }
            });
    }

    // caricamento acquirenti
    private loadAcquirenti() {
        this.acquirentiService.index()
            .then(response => {
                if (response.data != null) {
                    this.acquirente = response.data;
                }
            });
    }

    private loadPrelievi(idAllevatoreStr: string, idTrasportatoreStr: string, idAcquirenteStr: string, idDestinatarioStr: string, done: (prelievi: PrelievoLatte[]) => void) {
        this.prelieviLatteService.getPrelievi(idAllevatoreStr, idTrasportatoreStr, idAcquirenteStr, idDestinatarioStr, this.dal, this.al)
            .then(response => {
                this.prelievi = response.data;

                done(this.prelievi);
            });
    }

    private addDays(date: Date, days: number): Date {
        //console.log('adding ' + days + ' days');
        //console.log(date);
        date.setDate(date.getDate() + days);
        //console.log(date);
        return date;
    }

}

let page = new PrelieviLatteIndexPage();
Vue.config.devtools = true;