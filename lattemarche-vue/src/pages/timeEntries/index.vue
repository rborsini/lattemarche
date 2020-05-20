<template>
  <div>

    <!-- Pannello modale conferma eliminazione riga -->
    <confirm-dialog
      ref="confirmDeleteDialog"
      :title="'Conferma eliminazione'"
      :message="'Sei sicuro di voler rimuovere la riga contente le ore della commessa?'"
      v-on:confirmed="onRemove(taskToRemove, selectedAuditor, selectedWeek )"
    ></confirm-dialog>

    <!-- Selezione utente e settimana -->
    <div class="jumbotron">
      <div class="row">
        <label class="col-1">Tecnico:</label>
        <div class="col-2">
          <select2 :disabled="!editMultiUsers" class="form-control"  :placeholder="'Seleziona un tecnico'" :value.sync="selectedAuditor" :options="auditors.Items" :value-field="'Value'" :text-field="'Text'" v-on:value-changed="selectedAuditorChanged()" />
        </div>
        <div class="col-1">
          <button class="btn btn-primary btn-block" v-on:click="--selectedWeek; selectedWeekChanged()" >Precedente</button>
        </div>
        <div class="col-4">
          <select2  class="form-control" :placeholder="'-'" :options="weeks" :value.sync="selectedWeek" v-on:value-changed="selectedWeekChanged()" :value-field="'Index'" :text-field="'WeekAndDateForSelectBox'" />
        </div>
        <div class="col-1">
          <button class="btn btn-primary btn-block" v-on:click="++selectedWeek; selectedWeekChanged()" >Successiva</button>
        </div>
      </div>
    </div>

    <!-- Tabella immissione dati -->
    <div class="row">
      <div class="col-12">
        <table class="table table-striped">

          <!-- Intestazione tabella -->
          <thead class="thead-dark">
            <tr>
              <th scope="col" style="width: 20%" >Commessa</th>
              <th v-for="(day, index) in weeks[selectedWeek].DaysStr" :key="index" >{{day}}</th>
              <th scope="col">Totale ore</th>
              <th scope="col"></th>
            </tr>
          </thead>

          <!-- Corpo tabella -->
          <tbody>

            <!-- Righe tabella utilizzate-->
            <tr v-for="(timeEntryRow, index) in timeEntryWeek.Tasks" :key="index">

              <!-- Commessa -->
              <td >
                <a v-bind:href="'/workOrders/edit?id=' + timeEntryRow.WorkOrderId " >{{timeEntryRow.Description}}</a>
              </td>

              <!-- Giorni -->
              <td v-for="(row, index) in timeEntryRow.Days" :key="index">
                <input :disabled="isReadOnly || timeEntryRow.WorkOrderId === ''"  type="number" class="form-control" v-model="row.Quantity"  min="0" max="16" step="0.25" v-on:change="calculateRowAndColumn(timeEntryRow,index)"  >
              </td>

              <!-- Totale ore -->
              <td class="text-center" >{{timeEntryRow.Sum}}</td>

              <!-- Rimozione riga -->
              <td class="tools-col text-center" style="width:4%" >
                <div v-if="!isReadOnly && timeEntryRow.WorkOrderId !== ''">
                  <a class="cursor-pointer" v-on:click="$refs.confirmDeleteDialog.open(); taskToRemove = timeEntryRow.TaskId" title="Rimuovi" >
                    <i class="far fa-trash-alt"></i>
                  </a>
                </div>
              </td>

            </tr>

            <!-- Riga vuota-->
            <tr v-if="selectedAuditor" >

              <!-- Commessa -->
              <td>
                <select2 :disabled="isReadOnly" class="form-control" ref="workOrderDropdown" :options="tasks.Items" :value-field="'Value'" :text-field="'Text'" v-on:value-changed="onTaskSelected" />
              </td>

              <!-- Giorni -->
              <td v-for="index in 7" :key="index">
                <input type="number" class="form-control" :disabled=true >
              </td>

              <!-- Totale ore -->
              <td class="text-center" >0</td>

              <!-- Rimozione riga -->
              <td class="tools-col" style="width:4%">
                
              </td>

            </tr>

          </tbody>

          <!-- Footer tabella -->
          <tfoot class="tfoot-dark">
            <tr>
              <td class="font-weight-bold">Totale</td>
              <td class="font-weight-bold" v-for="(tot, index) in totalRow.Days" :key="index" ><span style="padding-left: 13px">{{tot.Quantity}}</span></td>
              <td class="font-weight-bold text-center">{{totalRow.Sum}}</td>
              <td></td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>
  </div>
</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import $ from "jquery";

// componenti
import ConfirmDialog from "@/components/confirmDialog.vue";
import Select2 from "@/components/select2.vue";

// modelli
import { WorkOrderRow, WorkOrderSearchModel } from "@/models/workOrderRow.model";
import { Auditor } from "@/models/auditor.model";
import { DropdownItem, Dropdown } from "@/models/dropdown.model";
import { TimeEntryWeek, TimeEntryTaskWeek, TimeEntry } from "@/models/timeEntry.model";
import { Week } from "@/models/week.model";

// servizi
import { DateService } from "@/services/date.service";
import { TimeEntriesService } from "@/services/timeEntries.service";
import { UrlService } from '@/services/url.service';
import { PermissionsService } from '@/services/permissions.service';
import { DropdownService } from '../../services/dropdown.service';
import { TasksService } from '../../services/tasks.service';

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}

@Component({
  components: {
    ConfirmDialog,
    Select2
  }
})
export default class App extends Vue {
  $refs: any = {
    confirmDeleteDialog: Vue,
    workOrderDropdown:Vue
  };

  public isReadOnly: boolean = false;
  public editSingleUser: boolean = false;
  public editMultiUsers: boolean = false;

  public currentUsername: string = "";

  public timeEntryWeek: TimeEntryWeek = new TimeEntryWeek();

  public selectedAuditor: number = 0;
  public tasks: Dropdown = new Dropdown();
  public workOrderRemove: string = "";
  public auditors: Dropdown = new Dropdown();

  public weeks: Week[] = [new Week()];
  public selectedWeek: number = 0;

  public totalRow: TimeEntryTaskWeek = new TimeEntryTaskWeek();

  private timeEntriesService: TimeEntriesService = new TimeEntriesService();
  private dateService: DateService = new DateService();
  private dropdownService: DropdownService = new DropdownService();
  private tasksService: TasksService = new TasksService();

  constructor() {
    super();
  }

  public mounted() {

    var editSingle = PermissionsService.isViewItemAuthorized("TimeEntries", "Index", "EditSingleUser");
    var editMulti = PermissionsService.isViewItemAuthorized("TimeEntries", "Index", "EditMultiUsers");

    this.currentUsername = PermissionsService.getCurrentUser();
    this.isReadOnly = !editSingle && !editMulti;    
    this.editSingleUser = editSingle && !editMulti;    
    this.editMultiUsers = editMulti;    

    this.getWeeks(() => {

      // caricamento parametri da hash
      if(UrlService.getHashParamareter('auditor')) {
        this.selectedAuditor = parseInt(UrlService.getHashParamareter('auditor'));
        UrlService.setHashParamareter('auditor', this.selectedAuditor.toString());
      }

      if(UrlService.getHashParamareter('week')) {
        this.selectedWeek = parseInt(UrlService.getHashParamareter('week'));
        UrlService.setHashParamareter('week', this.selectedWeek.toString());
      }      

      this.loadTable();

      this.loadAuditors();
      this.loadTasks();

    });

  }

  // carico ore
  public loadTable() {

    this.resetQuantity(this.totalRow);

    var mondaySelected = this.dateService.getMonday().toJSON();
    if (this.weeks[this.selectedWeek]) {
      mondaySelected = this.weeks[this.selectedWeek].Days[0].toString();
    }

    this.timeEntriesService
      .details(this.selectedAuditor, mondaySelected)
      .then(response => {
        this.timeEntryWeek = response.data;
        this.timeEntryWeek.Tasks.forEach(row => {
          for (var i = 0; i < 7; i++) {            
            if(!row.Days[i].Id){
              row.Days[i].Date = this.weeks[this.selectedWeek].Days[i];
              row.Days[i].TaskId = row.TaskId;
              row.Days[i].AssigneeId = this.selectedAuditor;
            } 
          } 

          this.sumRowQuantity(row);
        });
        for (var i = 0; i < 7; i++) {
          this.sumColumnHours(this.totalRow, i);
        }
      });

  }

  //Calcolo incrociato riga colonna al cambiamento della cella
  public calculateRowAndColumn(row: TimeEntryTaskWeek, column: number) {
    row.Days[column].Quantity = Number(row.Days[column].Quantity);
    this.sumRowQuantity(row);
    this.sumColumnHours(row, column);
    this.save(row.Days[column]);
  }

  // Effettuo la somma delle righe
  public sumRowQuantity(row: TimeEntryTaskWeek) {
    row.Sum = 0;
    row.Days.forEach(day => {
      row.Sum += day.Quantity;
    });
  }

  // Effettuo la somma della colonna
  public sumColumnHours(row: TimeEntryTaskWeek, day: number) {
    this.totalRow.Days[day].Quantity = 0;
    this.timeEntryWeek.Tasks.forEach(row => {
      this.totalRow.Days[day].Quantity += row.Days[day].Quantity;
    });
    this.sumRowQuantity(this.totalRow); //aggiorno totali
  }

  //Alla selezione del task
  public onTaskSelected(selectedId: string) {

    if(selectedId!=null && selectedId!=""){

      this.tasksService.details(selectedId).then(response => {
        var row: TimeEntryTaskWeek = new TimeEntryTaskWeek();
        for (var i = 0; i < 7; i++) {
          row.Days[i].Date = this.weeks[this.selectedWeek].Days[i];
        } 
        row.TaskId = selectedId;
        row.WorkOrderId = response.data.WorkOrderId
        row.Description = response.data.Description;
        row.Days.forEach(d => d.TaskId = selectedId);
        row.Days.forEach(d => d.AssigneeId = this.selectedAuditor);
        this.timeEntryWeek.Tasks.push(row);
      });
      this.$refs.workOrderDropdown.setItem("","");

      // aggiornamento lista task
      this.loadTasks();
    }
  }

  // Rimozione ore della commessa
  private onRemove(workOrderId: string, employeeId: number, selectedWeek: number): void {
    // setHours serve per impostare l'orario 00:00:00
    var from :Date = new Date(this.weeks[selectedWeek].Days[0]);
    var to:Date=new Date ();
    to.setDate(from.getDate() + 7);

    this.timeEntriesService.delete(workOrderId, employeeId, from.toString(), to.toString() );
    this.loadTable();
  }

  // Salvataggio ore
  private save(timeEntry: TimeEntry): void {
    this.timeEntriesService.save(timeEntry).then(response=>{
      timeEntry.Id = response.data.Id;
    })
  }

  // evento selezione auditor
  private selectedAuditorChanged() {
    UrlService.setHashParamareter('auditor', this.selectedAuditor.toString());
    this.loadTable();
  }

  // evento selezione settimana
  private selectedWeekChanged() {
    UrlService.setHashParamareter('week', this.selectedWeek.toString());
    this.loadTable();
  }

  // Carico le settimane
  private getWeeks(done: () => void) {
    this.dateService.getWeeks().then(response => {
      this.weeks = response.data;
      this.selectedWeek = 52; //current week
      done();
    });
  }

  // caricamento elenco auditors
  private loadAuditors() {
    this.dropdownService.getAuditors().then(response => {
      this.auditors = response.data;
    });
  }

  //Imposto a 0 i valori delle quantità della riga
  private resetQuantity(row: TimeEntryTaskWeek) {
    row.Days.forEach(day => {
      day.Quantity = 0;
    });
  }

  // Caricamento tasks
  public loadTasks() {
    this.dropdownService.getTasks()
    .then(response => {
      this.tasks = response.data;
    });
  }
}
</script>