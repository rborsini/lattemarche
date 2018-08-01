import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import Select2 from "../../components/common/select2.vue";
import EditazioneAcquirenteModal from "../acquirenti/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";

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
        NotificationDialog,
        EditazioneAcquirenteModal,
        DataTable
    }
})


export default class AcquirentiIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        editazioneAcquirenteModal: Vue
    }

    private acquirentiService: AcquirentiService;
    private acquirente: Acquirente;

    public columnOptions: any[] = [];
    public acquirenti: Acquirente[] = [];

    constructor() {
        super();

        this.acquirentiService = new AcquirentiService();
        this.acquirente = new Acquirente();
    }

    public mounted() {
        this.initTable();

        this.acquirentiService.getAcquirenti()
            .then(response => {
                this.acquirenti = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.acquirentiService.getDetails(rowId)
                .then(response => {
                    this.acquirente = response.data;
                    this.$refs.editazioneAcquirenteModal.openAcquirente(this.acquirente);
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            console.log("remove");

        });

    }

    // nuovo acquirente
    public onAggiungi() {

        console.log("nuovo");

    }

    // inizializzazione tabella
    private initTable(): void {
        this.columnOptions.push({ data: "Piva" });
        this.columnOptions.push({ data: "RagioneSociale" });

        this.columnOptions.push({
            render: function (data: any, type: any, row: any) {

                var html = '<div class="text-center">';
                html += '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-edit"></i></a>';
                html += '<a class="pl-3 delete" title="elimina" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-trash-alt"></i></a>';
                html += '</div>';

                return html;
            }
        });


    }

}

let page = new AcquirentiIndexPage();
Vue.config.devtools = true;