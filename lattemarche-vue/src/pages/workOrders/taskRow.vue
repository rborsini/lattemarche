<template>
    
    <tr>                
        <td>
            {{index + 1}} 
        </td>

        <!-- Articolo -->
        <td>
            {{task.Article_ArticleCode}} - {{task.Article_Description}}
        </td>
        
        <!-- Fornitore -->
        <td>
            <select2 class="form-control" :options="suppliers.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="task.SupplierId" v-on:value-changed="onSupplierSelected" />
        </td>
        
        <!-- Tecnico -->
        <td>
            <select2 class="form-control" :options="auditors.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="task.AuditorId" />
        </td>
        
        <!-- Data -->
        <td>
            <datepicker class="form-control" :value.sync="task.StartDate_Str" v-on:update:value="onDateChanged" />
        </td>

        <!-- Ora inizio -->
        <td>
            <select2 class="form-control" :options="timeOptions.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="task.StartDate_Time" />     
        </td>

        <!-- Ora fine -->
        <td>
            <select2 class="form-control" :options="timeOptions.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="task.EndDate_Time" />
        </td>        

        <!-- Bottone assegna -->
        <!-- <td class="text-center" >
            <button class="btn btn-primary">Invia incarico</button>
        </td> -->

    </tr>

</template>

<script lang="ts">
import { Prop, Watch, Emit, Vue, Component } from "vue-property-decorator";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import { DropdownService } from '@/services/dropdown.service';
import { Dropdown } from '@/models/dropdown.model';
import { Task } from '../../models/task.model';


@Component({
    components: {
        Select2,
        Datepicker  
    }
})
export default class TaskRow extends Vue{
    
    public dropdownService: DropdownService = new DropdownService();

    public auditors: Dropdown = new Dropdown();

    @Prop() public task!: Task;
    @Prop() public index!: number;
    @Prop() public suppliers!: Dropdown;
    @Prop() public timeOptions!: Dropdown;

    constructor() {
        super();
    }

    public mounted() {
        this.loadDropDowns();
    }

    public onSupplierSelected(supplierId: number) {
        this.dropdownService.getAuditors(supplierId).then(response => {
            this.auditors = response.data;
        });
    }

    public loadDropDowns() {
        // auditors
        this.onSupplierSelected(this.task.SupplierId);        
    }

    public onDateChanged(date: any) {
        this.task.EndDate_Str = date;
    }

}
</script>