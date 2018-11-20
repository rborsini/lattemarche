import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import EditazioneAutocisternaModal from "../autocisterne/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";

import { Autocisterna } from "../../models/autocisterna.model";
import { AutocisterneService } from "../../services/autocisterne.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#autocisterne-page',
    components: {
        ConfirmDialog,
        NotificationDialog,
        EditazioneAutocisternaModal,
        DataTable
    }
})


export default class AutocisterneIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        editazioneAutocisternaModal: Vue,
        confirmDeleteDialog: Vue,
        removedDialog: Vue
    }
    
    private autocisterneService: AutocisterneService;
    private autocisterna: Autocisterna;
    private idAutocisterna: number;

    public tableOptions: any = {};
    public autocisterne: Autocisterna[] = [];
    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.autocisterneService = new AutocisterneService();
        this.autocisterna = new Autocisterna();

        this.canAdd = $('#canAdd').val() == "true";
        this.canEdit = $('#canEdit').val() == "true";
        this.canRemove = $('#canRemove').val() == "true";
    }

    public mounted() {
        this.initTable();

        this.autocisterneService.index()
            .then(response => {
                this.autocisterne = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.autocisterneService.details(rowId)
                .then(response => {
                    this.autocisterna = response.data;
                    this.$refs.editazioneAutocisternaModal.open();
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.idAutocisterna = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }


    // nuova autocisterna
    public onAdd() {

        this.autocisterna = new Autocisterna();
        this.$refs.editazioneAutocisternaModal.open();

    }

    // rimozione autocisterna
    public onRemove() {

        this.autocisterneService.delete(this.idAutocisterna)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }


    // inizializzazione tabella
    private initTable(): void {
        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Marca" });
        options.columns.push({ data: "Modello" });
        options.columns.push({ data: "Targa" });
        options.columns.push({ data: "Portata" });

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

let page = new AutocisterneIndexPage();
Vue.config.devtools = true;