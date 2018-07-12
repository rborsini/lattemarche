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
import { Comune } from "../../models/comune.model";
import { Profilo } from "../../models/profilo.model";

import { UtentiService } from "../../services/utenti.service";
import { TipiLatteService } from "../../services/tipiLatte.service";
import { ComuniService } from "../../services/comuni.service";
import { ProfiliService } from "../../services/profili.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#utenti-edit',
    components: {
        Select2,
        Waiter,
        NotificationDialog
    }
})


export default class UtentiEditPage extends Vue {

    $refs: {
        waiter: Vue,
        savedDialog: Vue
    }

    public utente: Utente;
    public id: string;

    public tipoLatte: TipoLatte;
    public comune: Comune;
    public profilo: Profilo[] = [];

    public opzioniSesso: DropdownItem[] = [];
    public opzioniAbilitato: DropdownItem[] = [];
    public opzioniVisibile: DropdownItem[] = [];
    public comuni: Comune[] = [];
    public opzioniProvince: DropdownItem[] = [];

    private comuniService: ComuniService;
    private tipiLatteService: TipiLatteService;
    private utentiService: UtentiService;
    private profiliService: ProfiliService;

    private isNew: boolean = true;

    constructor() {
        super();

        this.comune = new Comune;
        this.id = $('#id').val() as string;
        this.tipoLatte = new TipoLatte;
        this.utente = new Utente();
        this.comuniService = new ComuniService();
        this.tipiLatteService = new TipiLatteService();
        this.utentiService = new UtentiService();
        this.profiliService = new ProfiliService();

    }

    public mounted() {
        this.$refs.waiter.open();
        if (this.id != '') {
            this.loadUtente((utente: Utente) => {
                this.isNew = false;
                this.loadComuni(this.utente.SiglaProvincia);
                this.comuniService.getProvince()
                    .then(response => {
                        this.opzioniProvince = response.data;
                    });
                this.opzioniSesso = this.getOpzioniSessoUtente();
                this.opzioniAbilitato = this.getOpzioniAbilitato();
                this.opzioniVisibile = this.getOpzioniAbilitato();
                this.loadTipiLatte();
                this.loadProfili();
                this.$refs.waiter.close();
            });
        }
    }

    // carica utente
    public loadUtente(done: (utente: Utente) => void) {
        this.utentiService.getDetails(this.id)
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
                    this.tipoLatte = response.data;
                }
            });
    }

    // carica comuni
    public loadComuni(provincia: string): void {
        this.comuniService.getComuni(provincia)
            .then(response => {
                if (response.data != null) {
                    this.comuni = response.data;
                }
            });
    }

    // carica tipi profilo
    public loadProfili(): void {
        this.profiliService.getProfili()
            .then(response => {
                if (response.data != null) {
                    this.profilo = response.data;
                }
            });
    }

    // carico provincia se seleziono comune (senza aver precedentemente selezionato la provincia)
    public onComuneSelezionato(idComune: string): void {
        if (this.utente.SiglaProvincia == '') {
            this.comuniService.getComuneDetails(idComune)
                .then(response => {
                    this.utente.SiglaProvincia = response.data.Provincia;
                })
        }
    }

    // salvataggio utente
    public onSave() {
        this.$refs.waiter.open();
        if (!this.isNew) {
            this.utentiService.update(this.utente)
                .then(response => {
                    if (response.data != undefined) {
                        // TODO: msg di validazione
                        this.$refs.waiter.close();
                        this.$refs.savedDialog.open();
                    } else {
                        // save OK !!
                        this.utente = response.data;
                        //this.$refs.waiter.close();
                        this.$refs.savedDialog.open();
                    }
                });
        } else {
            this.utentiService.create(this.utente)
                .then(response => {
                    if (response.data != undefined) {
                        // TODO: msg di validazione
                        this.$refs.waiter.close();
                        this.$refs.savedDialog.open();
                    } else {
                        // save OK !!
                        this.utente = response.data;
                        //this.$refs.waiter.close();
                        this.$refs.savedDialog.open();
                    }
                });
        }

    }


}

let page = new UtentiEditPage();
Vue.config.devtools = true;