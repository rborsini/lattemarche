import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import Select2 from "../../components/common/select2.vue";
import EditazioneAcquirenteModal from "../acquirenti/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";

import { Acquirente } from "../../models/acquirente.model";

import { AcquirentiService } from "../../services/acquirenti.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        openAcquirente(acqu: Acquirente): void
        close(): void
    }
}

@Component({
    el: '#acquirenti-page',
    components: {
        Select2,
        ConfirmDialog,
        NotificationDialog,
        EditazioneAcquirenteModal,
        DataTable
    }
})


export default class AcquirentiIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        removedDialog: Vue,
        editazioneAcquirenteModal: Vue,
        confirmDeleteDialog: Vue
    }

    private acquirentiService: AcquirentiService;
    private acquirente: Acquirente;
    private idAcquirenteDaRimuovere: number;

    public columnOptions: any[] = [];
    public acquirenti: Acquirente[] = [];
    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.acquirentiService = new AcquirentiService();
        this.acquirente = new Acquirente();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {
        this.initTable();

        this.acquirentiService.index()
            .then(response => {
                this.acquirenti = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.acquirentiService.details(rowId)
                .then(response => {
                    this.acquirente = response.data;
                    this.$refs.editazioneAcquirenteModal.openAcquirente(this.acquirente);
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idAcquirenteDaRimuovere = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }

    // nuovo acquirente
    public onAdd() {

        this.acquirente = new Acquirente();
        this.$refs.editazioneAcquirenteModal.open();

    }

    // rimozione acquirente
    public onRemove() {

        this.acquirentiService.delete(this.idAcquirenteDaRimuovere)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }

    // inizializzazione tabella
    private initTable(): void {
        this.columnOptions.push({ data: "Piva" });
        this.columnOptions.push({ data: "RagioneSociale" });

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
                }
            });

        }




    }

}

let page = new AcquirentiIndexPage();
Vue.config.devtools = true;