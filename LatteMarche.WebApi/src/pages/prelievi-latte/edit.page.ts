import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import Waiter from "../../components/common/waiter.vue";

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
    }
})

export default class PrelieviLatteEditPage extends Vue {

    $refs: {
        waiter: Vue,
        savedDialog: Vue
    }

    constructor() {
        super();


    }

    public mounted() {
        this.$refs.waiter.open();

        this.$refs.waiter.close();
    }

}

let page = new PrelieviLatteEditPage();
Vue.config.devtools = true;