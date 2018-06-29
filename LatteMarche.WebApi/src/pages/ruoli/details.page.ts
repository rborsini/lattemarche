import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import Waiter from "../../components/common/waiter.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";

import { Ruolo } from "../../models/ruolo.model";

import { RuoliService } from "../../services/ruoli.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#ruoli-edit',
    components: {
        Waiter,
        NotificationDialog
    }
})

export default class UtentiDetailsPage extends Vue {

    $refs: {
        waiter: Vue,
        savedDialog: Vue
    }

    public ruolo: Ruolo;
    public id: string;

    private ruoliService: RuoliService;
    

    constructor() {
        super();

        this.id = $('#id').val() as string;
        this.ruolo = new Ruolo();
        this.ruoliService = new RuoliService();

    }

    public mounted() {
        this.loadRuolo((ruolo: Ruolo) => {
            // boh!
        });
    }

    // carica ruolo
    public loadRuolo(done: (ruolo: Ruolo) => void) {
        this.ruoliService.getDetails(this.id)
            .then(response => {
                console.log("response.data", response.data);
                this.ruolo = response.data;
                done(this.ruolo);
            });
    }


    // salvataggio utente
    public onSave() {
        this.$refs.waiter.open();
        this.ruoliService.update(this.ruolo)
            .then(response => {
                if (response.data != undefined) {
                    // TODO: msg di validazione
                    this.$refs.waiter.close();
                    this.$refs.savedDialog.open();
                } else {
                    // save OK !!
                    this.ruolo = response.data;
                    //this.$refs.waiter.close();
                    this.$refs.savedDialog.open();
                }
            });
    }


}

let page = new UtentiDetailsPage();
Vue.config.devtools = true;