import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";
import EditazioneAutocisternaModal from "../autocisterne/edit.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";

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
        NotificationDialog,
        EditazioneAutocisternaModal,
        DataTable
    }
})


export default class AutocisterneIndexPage extends Vue {


    $refs: {
        savedDialog: Vue,
        editazioneAutocisternaModal: Vue
    }

    private autocisterneService: AutocisterneService;
    private autocisterna: Autocisterna;

    public columnOptions: any[] = [];
    public autocisterne: Autocisterna[] = [];

    constructor() {
        super();

        this.autocisterneService = new AutocisterneService();
        this.autocisterna = new Autocisterna();
    }

    public mounted() {
        this.initTable();

        this.autocisterneService.getAutocisterne()
            .then(response => {
                this.autocisterne = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.autocisterneService.getDetails(rowId)
                .then(response => {
                    this.autocisterna = response.data;
                    this.$refs.editazioneAutocisternaModal.open();
                });

        });

    }

    // inizializzazione tabella
    private initTable(): void {
        this.columnOptions.push({ data: "Marca" });
        this.columnOptions.push({ data: "Modello" });
        this.columnOptions.push({ data: "Targa" });
        this.columnOptions.push({ data: "Portata" });
        this.columnOptions.push({ data: "NumScomparti" });

        this.columnOptions.push({
            render: function (data: any, type: any, row: any) {
                return '<a class="edit" style="cursor: pointer;" data-row-id="' + row.Id + '" >Dettagli</a>';
            }
        });

    }

}

let page = new AutocisterneIndexPage();
Vue.config.devtools = true;