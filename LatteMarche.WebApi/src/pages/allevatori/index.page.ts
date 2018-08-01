import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import DataTable from "../../components/common/dataTable.vue";

import { Allevatore } from "../../models/allevatore.model";
import { AllevatoriService } from "../../services/allevatori.service";

@Component({
    el: '#index-allevatori-page',
    components: {
        DataTable
    }
})


export default class AllevatoriIndexPage extends Vue {

    private allevatoriService: AllevatoriService;
    private allevatore: Allevatore;

    public columnOptions: any[] = [];
    public allevatori: Allevatore[] = [];

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


    // inizializzazione tabella
    private initTable(): void {

        this.columnOptions.push({ data: "Id" });
        this.columnOptions.push({ data: "RagioneSociale" });
        this.columnOptions.push({ data: "IndirizzoAllevamento" });
        this.columnOptions.push({ data: "Comune" });
        this.columnOptions.push({ data: "Provincia" });
    }

}

let page = new AllevatoriIndexPage();
Vue.config.devtools = true;