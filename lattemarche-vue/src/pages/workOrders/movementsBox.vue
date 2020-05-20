<template>
    <div>

        <!--  Ricavi -->
        <div>
            <div class="row pt-3">
                <h2 class="col-12">Ricavi</h2>
            </div>

            <div class="row pt-1">
                <div class="col-12">

                <table class="table table-bordered">
                    
                    <thead class="thead-dark">
                    <tr>
                        <th scope="col" width="2%">#</th>
                        <th scope="col" width="12%">Articolo</th> 
                        <th scope="col" width="5%">Importo Un.</th>
                        <th scope="col" width="4%">Qta</th>
                        <th scope="col" width="5%">Importo Tot.</th>
                        <th scope="col" width="5%">Data</th>
                        <th scope="col" width="13%">Cliente</th>
                        <th scope="col" width="6%">Tipo</th>
                        <th scope="col" width="4%">N. Fattura</th>
                        <th scope="col" width="5%">Data Fattura</th>
                        <th scope="col" width="12%">Note</th>
                        <th scope="col" width="3%"></th>
                    </tr>
                    </thead>

                    <!-- Corpo tabella -->
                    <tbody>

                        <movement-in-row   v-for="(movement, index) in movements_In" :key="index" 
                                            v-on:delete="onMovementDelete"  
                                            v-on:dateChanged="onMovementDateChanged"
                                            :row-mode="'Edit'" 
                                            :movement.sync="movement" 
                                            :is-read-only="isReadOnly" 
                                            :customer="workOrder.Customer_FullBusinessName"
                                            :index="index"
                                            :articles="articles"
                                            :movement-types="movementTypesIn"
                                            :suppliers="suppliers" 
                                            >
                        </movement-in-row>

                        <movement-in-row   v-on:add="onMovementInAdd" 
                                            :row-mode="'New'" 
                                            :movement.sync="newMovement_In" 
                                            :is-read-only="isReadOnly" 
                                            :customer="''"
                                            :articles="articles"
                                            :movement-types="movementTypesIn"
                                            :suppliers="suppliers"                                         
                                            >
                        </movement-in-row>

                    </tbody>
                </table>
                </div>
            </div>
        </div>

          <!--  Costi -->
          <div>
            <div class="row pt-3">
              <h2 class="col-12">Costi</h2>
            </div>

            <div class="row pt-1">
              <div class="col-12">
                <table class="table table-bordered">

                  <thead class="thead-dark">
                    <tr>
                        <th scope="col" width="2%">#</th>
                        <th scope="col" width="12%">Articolo</th>
                        <th scope="col" width="5%">Importo Un.</th>
                        <th scope="col" width="4%">Qta</th>
                        <th scope="col" width="5%">Importo Tot.</th>
                        <th scope="col" width="5%">Data</th>                      
                        <th scope="col" width="7%">Fornitore</th>
                        <th scope="col" width="7%">Tecnico</th>
                        <th scope="col" width="6%">Tipo</th>                      
                        <th scope="col" width="3%">N. Fattura</th>
                        <th scope="col" width="5%">Data Fattura</th>
                        <th scope="col" width="12%">Note</th>
                        <th scope="col" width="3%"></th>
                    </tr>
                  </thead>

                  <!-- Corpo tabella -->
                  <tbody>
                    
                    <movement-out-row   v-for="(movement, index) in movements_Out" :key="index" 
                                        v-on:delete="onMovementDelete"  
                                        :row-mode="'Edit'" 
                                        :movement.sync="movement" 
                                        :is-read-only="isReadOnly" 
                                        :index="index"
                                        :articles="articles"
                                        :movement-types="movementTypesOut"
                                        :suppliers="suppliers" 
                                        >
                    </movement-out-row>

                    <movement-out-row   v-on:add="onMovementOutAdd" 
                                        :row-mode="'New'" 
                                        :movement.sync="newMovement_Out" 
                                        :is-read-only="isReadOnly" 
                                        :articles="articles"
                                        :movement-types="movementTypesOut"
                                        :suppliers="suppliers"                                         
                                        >
                    </movement-out-row>

                  </tbody>
                </table>
              </div>
            </div>
          </div>

    </div>
</template>

<script lang="ts">
import { Prop, Watch, Emit, Vue, Component } from "vue-property-decorator";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import MovementOutRow from "./movementOutRow.vue";
import MovementInRow from "./movementInRow.vue";

import { Movement } from "@/models/movement.model";
import { DropdownService } from "@/services/dropdown.service";
import { Dropdown } from "@/models/dropdown.model";
import { WorkOrder } from "@/models/workOrder.model";

@Component({
    components: {
        Select2,
        Datepicker,
        MovementInRow,
        MovementOutRow
    }
})
export default class MovementBox extends Vue {
    public dropdownService: DropdownService = new DropdownService();

    @Prop() public workOrder!: WorkOrder;
    @Prop() public movements!: Movement[];
    @Prop() public isReadOnly!: boolean;

    public articles: Dropdown = new Dropdown();
    public movementTypesIn: Dropdown = new Dropdown();
    public movementTypesOut: Dropdown = new Dropdown();
    public suppliers: Dropdown = new Dropdown();

    public movements_In: Movement[] = [];
    public movements_Out: Movement[] = [];

    public newMovement_In: Movement = new Movement();
    public newMovement_Out: Movement = new Movement();

    constructor() {
        super();
    }

    public mounted() {
        this.loadDropDowns();
    }

    @Watch("movements")
    public onMovementsChanged(newVal: Movement[], oldVal: Movement[]) {
        this.movements_In = newVal.filter(
            m => m.MovementType_Direction == "In"
        );
        this.movements_Out = newVal.filter(
            m => m.MovementType_Direction == "Out"
        );
    }

    // Aggiunta nuovo movimento IN
    public onMovementInAdd(movement: Movement) {
        movement.WorkOrderId = this.workOrder.Id;
        movement.MovementType_Direction = "In";
        movement.Quantity = movement.Quantity == 0 ? 1 : movement.Quantity; // Se la quantità è 0 la imposto a 1 automaticamente

        if(!movement.MovementTypeId)
            movement.MovementTypeId = "FATTURA";

        this.movements.push(movement);
        this.newMovement_In = new Movement();
    }

    // Aggiunta nuovo movimento OUT
    public onMovementOutAdd(movement: Movement) {
        movement.WorkOrderId = this.workOrder.Id;
        movement.MovementType_Direction = "Out";
        movement.Quantity = movement.Quantity == 0 ? 1 : movement.Quantity; // Se la quantità è 0 la imposto a 1 automaticamente

        if(!movement.MovementTypeId)
            movement.MovementTypeId = "COLLABORATORE_ESTERNO";

        this.movements.push(movement);
        this.newMovement_Out = new Movement();
    }

    // Elimina Movimento In / Out
    public onMovementDelete(movement: Movement) {
        this.movements.splice(this.movements.indexOf(movement), 1);
    }

    // Evento modifica data singolo movimento #325583
    public onMovementDateChanged(date: any) {
        if(this.workOrder.Status == 'New' && date) {
            this.workOrder.Status = 'InProgress';
        }
    }

    public loadDropDowns() {
        // tipi movimento IN
        this.dropdownService.getMovementTypes("in").then(response => {
            this.movementTypesIn = response.data;
        });

        // tipi movimento OUT
        this.dropdownService.getMovementTypes("out").then(response => {
            this.movementTypesOut = response.data;
        });

        // articoli
        this.dropdownService.getArticles().then(response => {
            this.articles = response.data;
        });

        // fornitori
        this.dropdownService.getSuppliers().then(response => {
            this.suppliers = response.data;
        });
    }
}
</script>
