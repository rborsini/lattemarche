import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import EditazioneTipoLatteModal from "../tipi-latte/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";

import { TipoLatte } from "../../models/tipoLatte.model";
import { TipiLatteService } from "../../services/tipiLatte.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#tipilatte-page',
    components: {
        ConfirmDialog,
        NotificationDialog,
        EditazioneTipoLatteModal,
        DataTable
    }
})


export default class TipiLatteIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        editazioneTipoLatteModal: Vue,
        confirmDeleteDialog: Vue,
        removedDialog: Vue
    }

    private tipiLatteService: TipiLatteService;
    public tipoLatte: TipoLatte;
    private idTipoLatte: number;

    public tableOptions: any = {};
    public tipiLatte: TipoLatte[] = [];
    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.tipiLatteService = new TipiLatteService();
        this.tipoLatte = new TipoLatte();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {
        this.initTable();

        this.tipiLatteService.index()
            .then(response => {
                this.tipiLatte = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.tipiLatteService.details(rowId)
                .then(response => {
                    this.tipoLatte = response.data;
                    this.$refs.editazioneTipoLatteModal.open();
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idTipoLatte = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }


    // nuova tipoLatte
    public onAdd() {

        this.tipoLatte = new TipoLatte();
        this.$refs.editazioneTipoLatteModal.open();

    }

    // rimozione tipoLatte
    public onRemove() {

        this.tipiLatteService.delete(this.idTipoLatte)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }


    // inizializzazione tabella
    private initTable(): void {
        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Id" });
        options.columns.push({ data: "Descrizione" });
        options.columns.push({ data: "DescrizioneBreve" });
        options.columns.push({ data: "FattoreConversione" });

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

let page = new TipiLatteIndexPage();
Vue.config.devtools = true;