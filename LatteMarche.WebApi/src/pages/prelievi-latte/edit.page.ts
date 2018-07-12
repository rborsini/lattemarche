import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import Waiter from "../../components/common/waiter.vue";
import Datepicker from "../../components/common/datepicker.vue";
import EditazionePrelievoModal from "../prelievi-latte/components/editazionePrelievoModal.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";


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
        EditazionePrelievoModal,
        NotificationDialog
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
    public prelievoSelezionato: PrelievoLatte;
    public prelieviLatteService: PrelieviLatteService;
    public utentiService: UtentiService;

    constructor() {
        super();
        this.id = $('#id').val() as string;
        this.utente = new Utente();
        this.today = new Date();
        this.prelieviLatteService = new PrelieviLatteService();
        this.utentiService = new UtentiService();
        this.prelievoSelezionato = new PrelievoLatte();
    }

    public mounted() {
        this.$refs.waiter.open();
        this.dataFine = String(this.today.getDate()) + '/' + String(this.today.getMonth() + 1) + '/' + String(this.today.getFullYear());

        this.loadUtente();
        //restituisce i prelievi dall'inizio del mese corrente
        this.dataInzio = String('01/' + String(this.today.getMonth()) + '/' + String(this.today.getFullYear()));
        this.loadPrelievi((prelievi: PrelievoLatte[]) => {
            this.$refs.waiter.close();
        });
    }

    // carico prelievi
    private loadPrelievi(done: (prelievi: PrelievoLatte[]) => void) {
        this.prelieviLatteService.getPrelievi(this.id, this.dataInzio, this.dataFine)
            .then(response => {
                this.prelievi = response.data;
                done(this.prelievi);
            });

    }

    // carico gli utenti
    public loadUtente(): void {
        this.utentiService.getDetails(this.id)
            .then(response => {
                this.utente = response.data;
            });
    }

    // carica il prelievo selezionato nella modale
    public onPrelievoSelezionato(prelievo: PrelievoLatte): void {
        this.prelievoSelezionato = prelievo;
        this.$refs.editazionePrelievoModal.open()
    }
}

let page = new PrelieviLatteEditPage();
Vue.config.devtools = true;