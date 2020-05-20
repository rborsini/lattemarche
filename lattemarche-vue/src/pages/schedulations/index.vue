<template>
  <div>

    <!-- Selezione settimana -->
    <div class="jumbotron">
      <div class="row">
        <div class="offset-3 col-1">
          <button class="btn btn-primary btn-block" v-on:click="--selectedWeek; selectedWeekChanged()" >Precedente</button>
        </div>
        <div class="col-4">
          <select2 class="form-control" :placeholder="'-'" v-on:value-changed="selectedWeekChanged()" :options="weeks" :value.sync="selectedWeek" :value-field="'Index'" :text-field="'WeekAndDateForSelectBox'" />
        </div>
        <div class="col-1">
          <button class="btn btn-primary btn-block" v-on:click="++selectedWeek; selectedWeekChanged()" >Successiva</button>
        </div>
      </div>
    </div>

    <!-- Tabella immissione dati -->
    <div class="row">
      <div class="col-12">
        <table class="table table-bordered">
          
          <!-- Intestazione tabella -->
          <thead class="thead-dark">
            <tr>
              
              <!-- Tecnico -->
              <th scope="col">Tecnico</th>

              <!-- Giorni della settimana -->
              <th v-for="(day, index) in weeks[selectedWeek].DaysStr" :key="index" >{{day}}</th>

            </tr>
          </thead>

          <!-- Corpo tabella -->
          <tbody>
            
            <tr v-for="(row, rowIndex) in weeklyPlan.AuditorPlans" :key="rowIndex">

              <td>
                {{row.AuditorName}}
              </td>
            
              <td v-for="(day, dayIndex) in row.Days" :key="dayIndex" >
                <div class="row row-inside-td">
                  <div class="col-12" v-for="(task, taskIndex) in day.Tasks" :key="taskIndex">
                    <div class="row mr-0">
                      <div class="col-5">
                        <span class="font-weight-bold" >{{task.StartDate_Time.slice(0,5)}}-{{task.EndDate_Time.slice(0,5)}}</span>
                      </div>
                      <div class="col-5">
                        <a v-bind:href="'/workOrders/edit?id=' + task.WorkOrderId ">{{task.WorkOrder_Code}}</a>
                      </div>
                    </div>
                  </div>
                </div>
              </td>
              
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>


<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import $ from "jquery";

// componenti
import Select2 from "@/components/select2.vue";

// servizi
import { DateService } from "@/services/date.service";
import { UrlService } from "@/services/url.service";

// modelli
import { Week } from "@/models/week.model";

import { Dropdown } from '../../models/dropdown.model';
import { DropdownService } from '../../services/dropdown.service';
import { PlanningService } from '../../services/planning.service';
import { WeeklyPlan } from '../../models/weeklyPlan.model';

@Component({
  components: {
    Select2
  }
})
export default class App extends Vue {

  private dateService: DateService = new DateService();
  private dropDownService: DropdownService = new DropdownService();
  private planningService: PlanningService = new PlanningService();

  public weeks: Week[] = [new Week()];
  public selectedWeek: number = 0;
  public weeklyPlan: WeeklyPlan = new WeeklyPlan();

  constructor() {
    super();
  }

  public mounted() {


    this.getWeeks(() => {

      if(UrlService.getHashParamareter('week')) {
        this.selectedWeek = parseInt(UrlService.getHashParamareter('week'));
        UrlService.setHashParamareter('week', this.selectedWeek.toString());
      }    

      this.loadTable();

    });

  }

  // carico le settimane
  private getWeeks(done:() => void) {
    this.dateService.getWeeks().then(response => {
      this.weeks = response.data;
      this.selectedWeek = 52; //current week
      done();
    });
  }

  // evento selezione settimana
  private selectedWeekChanged() {
    UrlService.setHashParamareter('week', this.selectedWeek.toString());
    this.loadTable();
  }

  // carico i dati nella tabella
  public loadTable() {
    var mondaySelected = this.dateService.getMonday().toJSON();
    
    if (this.weeks[this.selectedWeek]) {
      mondaySelected = this.weeks[this.selectedWeek].Days[0].toString();
    }

    this.planningService.loadWeek(mondaySelected).then(response => {
      this.weeklyPlan = response.data;
    });
  }

}
</script>

<style lang="scss">
.row-inside-td {
  .fa-trash-alt:hover,
  .fa-plus:hover {
    cursor: pointer;
    background: rgb(224, 224, 224);
  }
  .fa-trash-alt,
  .fa-plus {
    padding: 6px;
  }
}
</style>



