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
import { TrasportatoreService } from "../../services/trasportatori.service";

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
        savedDialog: Vue
    }

    public trasportatoriService: TrasportatoreService;

    public trasportatori: Trasportatore;

    constructor() {
        super();
        this.trasportatori = new Trasportatore();
        this.trasportatoriService = new TrasportatoreService();
    }

    public mounted() {
        this.loadTrasportatori();
    }

    // caricamento trasportatori
    private loadTrasportatori() {
        this.trasportatoriService.getTrasportatori()
            .then(response => {
                if (response.data != null) {
                    this.trasportatori = response.data;
                }
            });
    }

    //// salvataggio utente
    //public onSave() {
    //    this.$refs.waiter.open();
    //    if (!this.isNew) {
    //        this.utentiServices.update(this.utente)
    //            .then(response => {
    //                if (response.data != undefined) {
    //                    // TODO: msg di validazione
    //                    this.$refs.waiter.close();
    //                    this.$refs.savedDialog.open();
    //                } else {
    //                    // save OK !!
    //                    this.utente = response.data;
    //                    //this.$refs.waiter.close();
    //                    this.$refs.savedDialog.open();
    //                }
    //            });
    //    } else {
    //        this.utentiServices.create(this.utente)
    //            .then(response => {
    //                if (response.data != undefined) {
    //                    // TODO: msg di validazione
    //                    this.$refs.waiter.close();
    //                    this.$refs.savedDialog.open();
    //                } else {
    //                    // save OK !!
    //                    this.utente = response.data;
    //                    //this.$refs.waiter.close();
    //                    this.$refs.savedDialog.open();
    //                }
    //            });
    //    }

    //}


}

let page = new TrasportatoriEditPage();
Vue.config.devtools = true;