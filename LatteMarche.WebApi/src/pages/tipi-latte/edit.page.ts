import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import Waiter from "../../components/common/waiter.vue";
import ConfirmDialog from "../../components/common/confirmDialog.vue";
import NotificationDialog from "../../components/common/notificationDialog.vue";

import { TipoLatte } from "../../models/tipoLatte.model";

import { TipiLatteService } from "../../services/tipiLatte.service";


declare module 'vue/types/vue' {
    interface Vue {
        open(): void
        close(): void
    }
}

@Component({
    el: '#tipi-latte-edit',
    components: {
        Waiter,
        NotificationDialog
    }
})

export default class TipiLatteEditPage extends Vue {

    $refs: {
        waiter: Vue,
        savedDialog: Vue
    }

    public id: string;
    public tipiLatte: TipoLatte;
    private tipiLatteService: TipiLatteService;
    private isNew: boolean = true;

    constructor() {
        super();

        this.id = $('#id').val() as string;
        this.tipiLatte = new TipoLatte;
        this.tipiLatteService = new TipiLatteService();
    }

    public mounted() {
        this.$refs.waiter.open();

        if (this.id != '') {
            this.loadTipiLatte();
        }

        this.$refs.waiter.close();
    }

    // caricamento tipi latte
    private loadTipiLatte() {
        this.tipiLatteService.getTipoLatte(this.id)
            .then(response => {
                if (response.data != null) {
                    this.tipiLatte = response.data;
                    this.isNew = false;
                }
            });
    }

    // salvataggio tipi latte
    public onSave() {
        this.$refs.waiter.open();
        this.tipiLatteService.save(this.tipiLatte, this.isNew)
            .then(response => {
                if (response.data != undefined) {
                    // TODO: msg di validazione
                    this.$refs.waiter.close();
                    this.$refs.savedDialog.open();
                } else {
                    // save OK !!
                    this.tipiLatte = response.data;
                    //this.$refs.waiter.close();
                    this.$refs.savedDialog.open();
                }
            });
    }


}

let page = new TipiLatteEditPage();
Vue.config.devtools = true;