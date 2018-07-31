import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import Select2 from "../../components/common/select2.vue";
import EditazioneDestinatarioModal from "../destinatari/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";

import { Destinatario } from "../../models/destinatario.model";

import { DestinatariService } from "../../services/destinatari.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#destinatari-page',
    components: {
        Select2,
        NotificationDialog,
        EditazioneDestinatarioModal,
        DataTable
    }
})


export default class DestinatariIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        editazioneDestinatarioModal: Vue
    }

    private destinatariService: DestinatariService;
    private destinatario: Destinatario;

    public columnOptions: any[] = [];
    public destinatari: Destinatario[] = [];

    constructor() {
        super();

        this.destinatariService = new DestinatariService();
        this.destinatario = new Destinatario();
    }

    public mounted() {
        this.initTable();

        this.destinatariService.getDestinatari()
            .then(response => {
                this.destinatari = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.destinatariService.getDetails(rowId)
                .then(response => {
                    this.destinatario = response.data;
                    this.$refs.editazioneDestinatarioModal.open();
                });

        });

    }

    // inizializzazione tabella
    private initTable(): void {
        this.columnOptions.push({ data: "P_IVA" });
        this.columnOptions.push({ data: "RagioneSociale" });

        this.columnOptions.push({
            render: function (data: any, type: any, row: any) {
                return '<a class="edit" style="cursor: pointer;" data-row-id="' + row.Id + '" >Dettagli</a>';
            }
        });

    }

}

let page = new DestinatariIndexPage();
Vue.config.devtools = true;