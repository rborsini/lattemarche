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
import { PrelievoLatte } from "../../models/prelievoLatte.model";

import { AllevatoriService } from "../../services/allevatori.service";
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

    public columnOptions: any[] = [];
    public allevatori: Allevatore[] = [];
    public prelievi: PrelievoLatte[] = [];
    private idPrelievoDaEliminare: number;

    public idAllevatoreSelezionato: number = 0;
    public prelievoSelezionato: PrelievoLatte;
    public dal: string = "";
    public al: string = "";

    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.prelieviLatteService = new PrelieviLatteService();
        this.allevatoriService = new AllevatoriService();
        this.prelievoSelezionato = new PrelievoLatte();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {
        this.initTable();
        this.loadAllevatori();
        this.initSearchBox();
    }

    // Pulizia selezione
    public onAnnullaClick() {     
        this.initSearchBox();
    }

    // Ricerca
    public onCercaClick() {
        var idAllevatoreStr = this.idAllevatoreSelezionato == 0 ? "" : this.idAllevatoreSelezionato.toString();
        this.prelieviLatteService.getPrelievi(idAllevatoreStr, this.dal, this.al)
            .then(response => {
                this.prelievi = response.data;
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
        this.columnOptions.push({ data: "Temperatura" });
        this.columnOptions.push({ data: "Trasportatore" });
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