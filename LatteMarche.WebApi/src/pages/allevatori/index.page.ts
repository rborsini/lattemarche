import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import EditazioneAllevatoreModal from "../allevatori/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";

import { Allevatore } from "../../models/allevatore.model";
import { AllevatoriService } from "../../services/allevatori.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#index-allevatori-page',
    components: {
        ConfirmDialog,
        NotificationDialog,
        DataTable
    }
})


export default class AllevatoriIndexPage extends Vue {

    $refs: {
        savedDialog: Vue,
        removedDialog: Vue,
        editazioneAllevatoreModal: Vue,
        confirmDeleteDialog: Vue
    }

    private allevatoriService: AllevatoriService;
    private allevatore: Allevatore;
    private idAllevatoreDaRimuovere: number;

    public columnOptions: any[] = [];
    public allevatori: Allevatore[] = [];

    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.allevatoriService = new AllevatoriService();
    }

    public mounted() {

        this.initTable();

        this.allevatoriService.getAllevatori()
            .then(response => {
                this.allevatori = response.data;
            });

    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.allevatoriService.details(rowId)
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

    // inizializzazione tabella
    private initTable(): void {

        this.columnOptions.push({ data: "Id" });
        this.columnOptions.push({ data: "RagioneSociale" });
        this.columnOptions.push({ data: "IndirizzoAllevamento" });
        this.columnOptions.push({ data: "Comune" });
        this.columnOptions.push({ data: "Provincia" });

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
                orderable: false
            });

        }
    }

}

let page = new AllevatoriIndexPage();
Vue.config.devtools = true;