import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import Select2 from "../../components/common/select2.vue";
import EditazioneDestinatarioModal from "../destinatari/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";

import { Destinatario } from "../../models/destinatario.model";
import { DestinatariService } from "../../services/destinatari.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        openDestinatario(destinatario: Destinatario): void
        close(): void
    }
}

@Component({
    el: '#destinatari-page',
    components: {
        Select2,
        ConfirmDialog,
        NotificationDialog,
        EditazioneDestinatarioModal,
        DataTable
    }
})


export default class DestinatariIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        removedDialog: Vue,
        editazioneDestinatarioModal: Vue,
        confirmDeleteDialog: Vue
    }

    private destinatariService: DestinatariService;
    private destinatario: Destinatario;
    private idDestinatarioDaRimuovere: number;

    public columnOptions: any[] = [];
    public destinatari: Destinatario[] = [];
    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.destinatariService = new DestinatariService();
        this.destinatario = new Destinatario();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {
        this.initTable();

        this.destinatariService.index()
            .then(response => {
                this.destinatari = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.destinatariService.details(rowId)
                .then(response => {
                    this.destinatario = response.data;
                    this.$refs.editazioneDestinatarioModal.openDestinatario(this.destinatario);
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idDestinatarioDaRimuovere = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }

    // nuovo destinatario
    public onAdd() {

        this.destinatario = new Destinatario();
        this.$refs.editazioneDestinatarioModal.open();

    }

    // rimozione destinatario
    public onRemove() {

        this.destinatariService.delete(this.idDestinatarioDaRimuovere)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }

    // inizializzazione tabella
    private initTable(): void {
        this.columnOptions.push({ data: "P_IVA" });
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
                },
                className: "edit-column",
                orderable: false
            });

        }

    }

}

let page = new DestinatariIndexPage();
Vue.config.devtools = true;