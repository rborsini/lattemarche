import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import Waiter from "../../components/common/waiter.vue";
import Datepicker from "../../components/common/datepicker.vue";
import EditazionePrelievoModal from "../prelievi-latte/components/editazionePrelievoModal.vue";

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

    constructor() {
        super();
        this.id = $('#id').val() as string;
    }

    public mounted() {
        this.$refs.waiter.open();
        this.$refs.waiter.close();
    }
}

let page = new PrelieviLatteEditPage();
Vue.config.devtools = true;