<template>
    
    <tr>                
        <td>
            <span v-if="rowMode == 'Edit'" >{{index + 1}}</span> 
        </td>

        <!-- Articolo -->
        <td>
            <select2 class="form-control" :disabled="isReadOnly || rowMode == 'New'" :options="articles.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="movement.ArticleId" />
        </td>
        
        <!-- Importo Unitario -->
        <td>
            <input type="number" :disabled="isReadOnly || rowMode == 'New'" v-model="movement.UnitAmount" @change="updateTotalAmount" class="form-control input-no-padding" step="0.01" style="text-align: right;" >
        </td>

        <!-- Quantità -->
        <td>
            <input type="number" :disabled="isReadOnly || rowMode == 'New'" v-model="movement.Quantity" @change="updateTotalAmount" class="form-control input-no-padding" step="1" style="text-align: right;" >
        </td>

        <!-- Importo -->
        <td>
            <input type="number" disabled v-model="movement.TotalAmount" class="form-control input-no-padding" step="0.01" style="text-align: right;" >
        </td>

        <!-- Data -->
        <td>
            <datepicker class="form-control" :disabled="isReadOnly || rowMode == 'New'" :value.sync="movement.Date_Str" v-on:update:value="onDateChanged" />
        </td>

        <!-- Fornitore -->
        <td>
            <select2 class="form-control" :disabled="isReadOnly || rowMode == 'New'" :options="suppliers.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="movement.Recipient"  v-on:value-changed="onSupplierSelected" />
        </td>

        <!-- Auditor -->
        <td>
            <select2 class="form-control" :disabled="isReadOnly || rowMode == 'New'" :options="auditors.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="movement.SubRecipient" v-on:value-changed="onAuditorSelected" />
        </td>
        
        <!-- Tipo di movimento -->
        <td>
            <select2 class="form-control" :disabled="isReadOnly || rowMode == 'New'" :options="movementTypes.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="movement.MovementTypeId" />
        </td>

        <!-- N.Fattura -->
        <td>
            <input type="text" :disabled="isReadOnly || rowMode == 'New'" v-model="movement.InvoiceNumber" class="form-control input-no-padding" >
        </td>
        
        <!-- Data Fattura -->
        <td>
            <datepicker class="form-control" :disabled="isReadOnly || rowMode == 'New'" :value.sync="movement.InvoiceDate_Str" v-on:update:value="onDateChanged" />
        </td>
        
        <!-- Note -->
        <td>
            <input type="text" :disabled="isReadOnly || rowMode == 'New'" v-model="movement.Note" class="form-control input-no-padding" >
        </td>

        <!-- Tools -->
        <td class="text-center">
            <a v-if="!isReadOnly && rowMode == 'Edit'" v-on:click="onDelete(movement)" title="Rimuovi" class="cursor-pointer text-primary" >
                <i class="far fa-trash-alt mt-2"></i>
            </a>

            <a v-if="!isReadOnly && rowMode == 'New'" v-on:click="onAdd(movement)" title="Aggiungi" class="cursor-pointer text-primary" >
                <i class="fa fa-plus mt-2"></i>                          
            </a>  
        </td>
    </tr>


</template>

<script lang="ts">
import { Prop, Watch, Emit, Vue, Component } from "vue-property-decorator";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import { Movement } from '@/models/movement.model';
import { DropdownService } from '@/services/dropdown.service';
import { Dropdown } from '@/models/dropdown.model';
import { AuditorsService } from '../../services/auditors.service';


@Component({
    components: {
        Select2,
        Datepicker        
    }
})
export default class MovementOutRow extends Vue{
    
    public dropdownService: DropdownService = new DropdownService();
    public auditorsService: AuditorsService = new AuditorsService();

    public auditors: Dropdown = new Dropdown();

    @Prop() public movement!: Movement;
    @Prop() public index!: number;
    @Prop() public isReadOnly!: boolean;
    @Prop() public rowMode!: string;
    @Prop() public articles!: Dropdown;
    @Prop() public movementTypes!: Dropdown;
    @Prop() public suppliers!: Dropdown;

    constructor() {
        super();
    }

    public mounted() {
        this.loadDropDowns();
    }

    @Emit('add')
    public onAdd(): Movement {
        return this.movement;
    }

    @Emit('delete')
    public onDelete(): Movement {
        return this.movement;
    }

    @Emit('dateChanged')
    public onDateChanged(date: any) {

    }

    public onSupplierSelected(supplierId: number) {
        this.dropdownService.getAuditors(supplierId).then(response => {
            this.auditors = response.data;
        });
    }

    public onAuditorSelected(auditorId: string) {
        this.auditorsService.details(auditorId).then(response => {
            if(response.data.IsDefault)
                this.movement.MovementTypeId = "COLLABORATORE_INTERNO";
            else
                this.movement.MovementTypeId = "COLLABORATORE_ESTERNO";
        });
    }

    public loadDropDowns() {

        // auditors
        this.onSupplierSelected(parseInt(this.movement.Recipient));        

    }

    public updateTotalAmount() {
        this.movement.TotalAmount = this.movement.UnitAmount * this.movement.Quantity;
    }

}
</script>


<style scoped>

    .input-no-padding {
        padding-left: 5px;
        padding-right: 5px;
    }

</style>