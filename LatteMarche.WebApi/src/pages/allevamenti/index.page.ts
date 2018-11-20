import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import EditazioneAllevamentoModal from "../allevamenti/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";

import { Allevamento } from "../../models/allevamento.model";
import { AllevamentiService } from "../../services/allevamenti.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        openAllevamento(allevamento: Allevamento): void
        close(): void
    }
}

@Component({
    el: '#index-allevamenti-page',
    components: {
        ConfirmDialog,
        NotificationDialog,
        EditazioneAllevamentoModal,
        DataTable
    }
})


export default class AllevamentiIndexPage extends Vue {

    $refs: {
        savedDialog: Vue,
        removedDialog: Vue,
        editazioneAllevamentoModal: Vue,
        confirmDeleteDialog: Vue
    }

    private allevamentiService: AllevamentiService;
    private allevamento: Allevamento;
    private idAllevamentoDaRimuovere: number;

    public tableOptions: any = {};
    public allevamenti: Allevamento[] = [];

    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.allevamentiService = new AllevamentiService();
        this.allevamento = new Allevamento();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {

        this.initTable();

        this.allevamentiService.index()
            .then(response => {
                this.allevamenti = response.data;
            });

    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.allevamentiService.details(rowId)
                .then(response => {
                    this.allevamento = response.data;
                    this.$refs.editazioneAllevamentoModal.openAllevamento(this.allevamento);
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idAllevamentoDaRimuovere = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }

    // nuovo allevamento
    public onAdd() {

        this.allevamento = new Allevamento();
        this.$refs.editazioneAllevamentoModal.open();

    }


    // rimozione acquirente
    public onRemove() {

        this.allevamentiService.delete(this.idAllevamentoDaRimuovere)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }

    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};

        options.columns = [];

        options.columns.push({ data: "Id" });
        options.columns.push({ data: "RagioneSociale" });
        options.columns.push({ data: "CUAA" });
        options.columns.push({ data: "CodiceAsl" });

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

let page = new AllevamentiIndexPage();
Vue.config.devtools = true;