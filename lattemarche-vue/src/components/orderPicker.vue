<template>
  <div id="modal-order-picker" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" >
    <div class="modal-dialog modal-lg order-modal">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Selezione ordine</h5>
          <button type="button" class="close" data-dismiss="modal" v-on:click="$emit('cancel')" aria-label="Close" >
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">

          <!-- Ordine -->
          <div class="row form-group">
            <label class="col-2">Ordine</label>
            <div class="col-10">
              <select2 ref="orderSelect" :dropdownparent="'#modal-order-picker'" class="form-control" :placeholder="''" :options="orders.Items" :value.sync="orderId" :value-field="'Value'" :text-field="'Text'" />
            </div>
          </div>

        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary mr-2" data-dismiss="modal" v-on:click="$emit('cancel')" >Annulla</button>
          <button class="btn btn-primary" v-on:click="onOk()" >Ok</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Prop, Watch, Emit, Vue, Component } from "vue-property-decorator";
import Select2 from "./select2.vue";
import { Dropdown } from '@/models/dropdown.model';
import { DropdownService } from '@/services/dropdown.service';

@Component({
   components: {
      Select2
   }
})
export default class OrderPicker extends Vue {
   $refs: any = {};


    public dropdownService: DropdownService = new DropdownService();
    public orders: Dropdown = new Dropdown();

    private orderId: string = "";

    constructor() {
        super();
    }

    public onOk() {
        this.close();
        this.$emit("selected", this.orderId);
    }

    public open(): void {
        $(this.$el).modal("show");

        this.orderId = "";

        this.dropdownService.getOpenOrders()
            .then(response => {
                this.orders = response.data;
            });

    }

    // chiusura modale
    public close(): void {
        $(this.$el).modal("hide");
    }
}
</script>

<style>
.order-modal {
    max-width: 70%;
}

.modal-header {
    display: flex;
}
</style>

