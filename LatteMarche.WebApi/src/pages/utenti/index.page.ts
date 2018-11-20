import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import EditazioneUtenteModal from "../utenti/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";

import { Utente } from "../../models/utente.model";
import { UtentiService } from "../../services/utenti.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        openUtente(utente: Utente): void
        close(): void
    }
}

@Component({
    el: '#utenti-page',
    components: {
        ConfirmDialog,
        NotificationDialog,
        EditazioneUtenteModal,
        DataTable
    }
})


export default class UtentiIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        editazioneUtenteModal: Vue,
        confirmDeleteDialog: Vue,
        removedDialog: Vue
    }

    private utentiService: UtentiService;
    public utente: Utente;
    private idUtente: number;

    public tableOptions: any = {};
    public utenti: Utente[] = [];
    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.utentiService = new UtentiService();
        this.utente = new Utente();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {
        this.initTable();

        this.utentiService.index()
            .then(response => {
                this.utenti = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.utentiService.details(rowId)
                .then(response => {
                    this.utente = response.data;
                    this.$refs.editazioneUtenteModal.openUtente(this.utente);
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idUtente = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }


    // nuova utente
    public onAdd() {

        this.utente = new Utente();
        this.$refs.editazioneUtenteModal.open();

    }

    // rimozione utente
    public onRemove() {

        this.utentiService.delete(this.idUtente)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }


    // inizializzazione tabella
    private initTable(): void {
        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "RagioneSociale" });
        options.columns.push({ data: "Nome" });
        options.columns.push({ data: "Cognome" });
        options.columns.push({ data: "Username" });

        var ce = this.canEdit;
        var cr = this.canRemove;

        if (ce || cr) {

            options.columns.push({
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
        this.tableOptions = options;
    }

}

let page = new UtentiIndexPage();
Vue.config.devtools = true;