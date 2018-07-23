import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import Select2 from "../../components/common/select2.vue";
import Waiter from "../../components/common/waiter.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";
import GiroTrasportatoriModal from "./components/giroTrasportatoriModal.vue";

import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { Trasportatore } from "../../models/trasportatore.model";
import { Giro, Item } from "../../models/giro.model";

import { GiriService } from "../../services/giri.service";
import { TrasportatoriService } from "../../services/trasportatori.service";

declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#trasportatori-page',
    components: {
        Select2,
        Waiter,
        NotificationDialog,
        GiroTrasportatoriModal
    }
})

export default class TrasportatoriEditPage extends Vue {

    $refs: {
        waiter: Vue,
        salvataggioDettaglioGiroModal: Vue,
        salvataggioGiroModal: Vue,
        giroTrasportatoriModal: Vue
    }

    public trasportatoriService: TrasportatoriService;
    public trasportatore: Trasportatore;
    public trasportatori: Trasportatore[] = [];
    public trasportatoreSelezionato: boolean = true;
    public selectedGiro: number = 0;
    public selectedTrasportatore: number = 0;
    public giro: Giro;
    public giriService: GiriService;

    constructor() {
        super();
        this.trasportatore = new Trasportatore();
        this.trasportatore.Giri[0] = new Giro();
        this.trasportatoriService = new TrasportatoriService();
        this.giro = new Giro();
        this.giriService = new GiriService();
    }

    public mounted() {
        this.loadTrasportatori();
    }

    // caricamento trasportatori
    public loadTrasportatori() {
        this.trasportatoriService.getTrasportatori()
            .then(response => {
                if (response.data != null) {
                    this.trasportatori = response.data;
                }
            });
    }

    // carico allevamenti se seleziono trasportatore
    public onTrasportatoreSelezionato(idTrasportatore: string): void {
        this.trasportatoreSelezionato = false;
        this.giro = new Giro();
        this.selectedGiro = 0;
        this.trasportatoriService.getTrasportatoreDetails(idTrasportatore)
            .then(response => {
                this.trasportatore = response.data;
            })
    }

    // Selezione / Deselezione item del giro
    public onItemSelectedChanged(event: any, item: Item) {

        if (!item.Selezionato) {
            item.Priorita = undefined;
        }

    }


    // carico allevatori
    public loadGiro(id: number) {
        this.giriService.getGiroDetails(id)
            .then(response => {
                if (response.data != null) {
                    this.giro = response.data;
                    for (let i = 0; i < this.giro.Items.length; i++) {
                        if (this.giro.Items[i].Priorita != null) {
                            this.giro.Items[i].Selezionato = true;
                        } else {
                            this.giro.Items[i].Selezionato = false;
                        }
                    }
                }

            });
    }

    // salva giro trasportatori
    public salvaGiro() {
        this.$refs.waiter.open();
        this.giriService.save(this.giro)
            .then(response => {
                if (response.data != undefined) {
                    this.$refs.waiter.close();
                    this.$refs.salvataggioGiroModal.open();
                } else {
                    this.giro = response.data;
                    //this.$refs.waiter.close();
                    this.$refs.salvataggioGiroModal.open();
                }
            });
    }


}

let page = new TrasportatoriEditPage();
Vue.config.devtools = true;