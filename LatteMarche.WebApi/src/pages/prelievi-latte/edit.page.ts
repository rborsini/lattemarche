import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import Waiter from "../../components/common/waiter.vue";
import Datepicker from "../../components/common/datepicker.vue";
import EditazionePrelievoModal from "../prelievi-latte/components/editazionePrelievoModal.vue";

import { PrelievoLatte } from "../../models/prelievoLatte.model";
import { Utente } from "../../models/utente.model";

import { PrelieviLatteService } from "../../services/prelieviLatte.service";
import { UtentiService } from "../../services/utenti.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#prelievi-latte-edit',
    components: {
        Waiter,
        Datepicker,
        EditazionePrelievoModal
    }
})

export default class PrelieviLatteEditPage extends Vue {

    $refs: {
        waiter: Vue,
        savedDialog: Vue,
        editazionePrelievoModal: Vue
    }

    public id: string;

    public dataInzio: string = "";
    public dataFine: string = "";
    private today: Date;

    public prelievi: PrelievoLatte[] = [];
    public utente: Utente;

    public prelieviLatteService: PrelieviLatteService;
    public utentiService: UtentiService;

    constructor() {
        super();
        this.id = $('#id').val() as string;
        this.utente = new Utente();
        this.today = new Date();

        this.prelieviLatteService = new PrelieviLatteService();
        this.utentiService = new UtentiService();
    }

    public mounted() {
        this.$refs.waiter.open();
        this.dataFine = String(this.today.getDate()) + '/' + String(this.today.getMonth() + 1) + '/' + String(this.today.getFullYear());
        //this.today = this.today.setMonth(this.today.getMonth() - 1);
        this.loadUtente();
        this.dataInzio = '25-04-2018';//String(this.today.getDate()) + '-' + String(this.today.getMonth() + 1) + '-' + String(this.today.getFullYear());
        this.loadPrelievi((prelievi: PrelievoLatte[]) => {
            this.$refs.waiter.close();
        });
    }

    private loadPrelievi(done: (prelievi: PrelievoLatte[]) => void) {
        console.log('Chiamata servizio');
        this.prelieviLatteService.getPrelievi(this.id, this.dataInzio, this.dataFine)
            .then(response => {
                this.prelievi = response.data;
                done(this.prelievi);
            });

    }

    public loadUtente(): void {
        this.utentiService.getDetails(this.id)
            .then(response => {
                this.utente = response.data;
            });
    }
}

let page = new PrelieviLatteEditPage();
Vue.config.devtools = true;