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
    el: '#ruoli-new',
    components: {
        Waiter,
        NotificationDialog
    }
})

export default class RouliNew extends Vue {

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

    }

    // salva ruolo creato
    public salvaRuoloCreato() {
        this.$refs.waiter.open();
        this.ruoliService.create(this.ruolo)
            .then(response => {
                if (response.data != undefined) {
                    this.$refs.waiter.close();
                    this.$refs.savedDialog.open();
                } else {
                    this.ruolo = response.data;
                    this.$refs.savedDialog.open();
                }
            });
    }


}

let page = new RouliNew();
Vue.config.devtools = true;