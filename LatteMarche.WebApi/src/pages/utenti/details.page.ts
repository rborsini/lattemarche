import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import Select2 from "../../components/common/select2.vue";
import Waiter from "../../components/common/waiter.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";

import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { Utente } from "../../models/utente.model";
import { TipoLatte } from "../../models/tipoLatte.model";

import { UtentiService } from "../../services/utenti.service";
import { TipiLatteService } from "../../services/tipiLatte.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#utenti-allevatori-details',
    components: {
        Select2,
        Waiter,
        NotificationDialog
    }
})

export default class UtentiDetailsPage extends Vue {

    $refs: {
        waiter: Vue,
        savedDialog: Vue
    }

    public utentiServices: UtentiService;
    public utente: Utente;
    public id: string;

    public tipiLatte: TipoLatte;
    public comune: Comune;

    public opzioniSesso: DropdownItem[] = [];
    public opzioniAbilitato: DropdownItem[] = [];
    public opzioniVisibile: DropdownItem[] = [];

    private comuniService: ComuniService;
    private tipiLatteService: TipiLatteService;
    private utentiServices: UtentiService;
    

    constructor() {
        super();

        this.comune = new Comune;
        this.id = $('#id').val() as string;
        this.tipiLatte = new TipoLatte;
        this.utente = new Utente();


        this.comuniService = new ComuniService();
        this.tipiLatteService = new TipiLatteService();
        this.utentiServices = new UtentiService();

    }

    public mounted() {
        this.loadUtente((utente: Utente) => {
            this.opzioniSesso = this.getOpzioniSessoUtente();
            this.opzioniAbilitato = this.getOpzioniAbilitato();
            this.opzioniVisibile = this.getOpzioniAbilitato();
            this.loadTipiLatte();
        });
    }

    // carica utente
    public loadUtente(done: (utente: Utente) => void) {
        this.utentiServices.getDetails(this.id)
            .then(response => {
                this.utente = response.data;
                done(this.utente);
            });
    }

    // carica dropdown sesso
    public getOpzioniSessoUtente(): DropdownItem[] {
        let opzioniSesso: DropdownItem[] = [];
        opzioniSesso.push(new DropdownItem("M", "Maschio"));
        opzioniSesso.push(new DropdownItem("F", "Femmina"));
        return opzioniSesso;
    }

    // carica opzioni abilitato
    public getOpzioniAbilitato(): DropdownItem[] {
        let opzioniAbilitato: DropdownItem[] = [];
        opzioniAbilitato.push(new DropdownItem("true", "Si"));
        opzioniAbilitato.push(new DropdownItem("false", "No"));
        return opzioniAbilitato;
    }

    // carica opzioni visibile
    public getOpzioniVisibile(): DropdownItem[] {
        let opzioniVisibile: DropdownItem[] = [];
        opzioniVisibile.push(new DropdownItem("true", "Si"));
        opzioniVisibile.push(new DropdownItem("false", "No"));
        return opzioniVisibile;
    }

    // caricamento tipi latte
    private loadTipiLatte() {
        this.tipiLatteService.getTipiLatte()
            .then(response => {
                if (response.data != null) {
                    this.tipiLatte = response.data;
                }
            });
    }

    // salvataggio utente
    public onSave() {
        //this.$refs.waiter.open();
        this.utentiServices.update(this.utente)
            .then(response => {
                if (response.data != undefined) {
                    // TODO: msg di validazione
                    //this.$refs.waiter.close();
                } else {
                    // save OK !!
                    this.utente = response.data;
                    //this.$refs.waiter.close();
                    //this.$refs.savedDialog.open();
                }
            });
    }


}

let page = new UtentiDetailsPage();
Vue.config.devtools = true;