<template>
  <div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Pannello modale degli errori -->
    <validation-dialog ref="validationDialog" :title="'Operazione non riuscita'" ></validation-dialog>

    <!-- modale errore generico -->
    <notification-dialog ref="errorDialog" :title="'Errore imprevisto'" :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'" v-on:ok="reload()"></notification-dialog>

    <!-- modale conferma salvataggio -->
    <notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Commessa salvata correttamente'" v-on:ok="reload()" ></notification-dialog>

    <!-- Pannello modale conferma eliminazione commessa -->
    <confirm-dialog ref="confirmDeleteDialog"
                    :title="'Conferma eliminazione'"
                    :message="'Eliminando la commessa verranno eliminate anche le pianificazioni, le ore scaricate e i movimenti associati. Sei sicuro di procedere con l\'eliminazione?'"
                    v-on:confirmed="onRemove()"></confirm-dialog>

    <!-- Pannello notifica eliminazione commessa -->
    <notification-dialog ref="removedDialog"
                         :title="'Conferma eliminazione'"
                         :message="'Commessa eliminata correttamente'"
                         v-on:ok="redirect()"></notification-dialog>

    <!-- Pannello modale conferma eliminazione allegato -->
    <confirm-dialog ref="confirmDeleteAttachmentDialog"
                    :title="'Conferma eliminazione'"
                    :message="'Sei sicuro di voler eliminare l\'allegato?'"
                    v-on:confirmed="onAttachmentRemoveClicked()"></confirm-dialog>

    <!-- breadcrumb -->
    <div class="jumbotron" >
      <div class="row">
        <div class="col-6">
          <ol class="breadcrumb mb-0"  >
            <li class="breadcrumb-item"><a class="root" href="/" >Home</a></li>
            <li class="breadcrumb-item"><a class="root" href="/workOrders" >Commessa</a></li>
            <li class="breadcrumb-item active">{{workOrder.Code}}</li>
          </ol>
        </div>
        <div class="col-6 text-right">
          <button v-if="btnDeleteVisible && workOrder.Id" v-on:click="$refs.confirmDeleteDialog.open()" class="btn btn-primary mr-2 mt-1" role="button">Elimina</button>
        </div>
      </div>
    </div>

    <div v-if="itemNotFound" class="portlet light" >      
      <div class="portlet-body">
        <div class="row pl-4 pt-4">
            <h2>Nessuna commessa trovata</h2>
        </div>
      </div>
    </div>

    <div v-else >
      <ul class="nav nav-tabs" id="tabWrapper">
        <li class="active">
          <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
        </li>
        <li>
          <a v-if="tabIncarichiVisible" data-toggle="tab" class="nav-link" href="#incarichi">Incarichi</a>
        </li>  
        <li>        
          <a data-toggle="tab" class="nav-link" href="#allegati" >Allegati ({{workOrder.Attachments.length + workOrder.PreviousAttachments.length}})</a>
        </li>
      </ul>

      <div class="tab-content" >

        <!-- Tab dettaglio -->
        <div id="dettaglio" class="tab-pane fade show active">
          
            <!-- Stato / Codice -->
            <div class="row pt-3">
              
              <label class="col-1">Stato:</label>
              <div class="col-2">
                <select2 :disabled="isReadOnly" class="form-control" :placeholder="'-'" :options="statuses.Items" :value.sync="workOrder.Status" :value-field="'Value'" :text-field="'Text'" /> 
              </div>

              <label class="col-1">Codice:</label>
              <div class="col-2">
                <input disabled type="text" class="form-control" v-model="workOrder.Code">
              </div>

              <label class="col-1">Ordine:</label>
              <div class="col-2 pt-1">
                  <a :href="'/orders/edit?id=' + workOrder.Order_Id" >{{workOrder.Order_Code}}</a>
              </div>

            </div>

            <!-- Sede / Cliente / Sottolinea / Data acquisizione -->
            <div class="row pt-3">

              <label class="col-1">Sede:</label>
              <div class="col-2">
                <select2 :disabled="true" class="form-control" :placeholder="'-'" :options="headQuarters.Items" :value.sync="workOrder.HeadQuarterId" :value-field="'Value'" :text-field="'Text'" />
              </div>

              <label class="col-1">Cliente:</label>
              <div class="col-2">
                <select2 :disabled="true" class="form-control" ajax="true" ref="customerSelect" :placeholder="''"  :url="'/api/customers/search'" :value.sync="workOrder.CustomerId" value-field="Id" text-field="FullBusinessName" />          
              </div>

              <label class="col-1">Sottolinea:</label>
              <div class="col-2">
                <select2 :disabled="true" class="form-control" :placeholder="'-'" :options="businessSublines.Items" :value.sync="workOrder.BusinessSublineId" :value-field="'Value'" :text-field="'Text'" /> 
              </div>            

            <label class="col-1">Commerciale:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="true" :options="sellers.Items" :value.sync="workOrder.Seller" :value-field="'Value'" :text-field="'Text'" /> 
            </div>

            </div>

            <!-- Data inizio / Data scadenza / Data chiusura-->
            <div class="row pt-3">            
              <label class="col-1">Data acquisizione:</label>
              <div class="col-2">
                <datepicker :disabled="isReadOnly" class="form-control" :value.sync="workOrder.TakeoverDate_Str"/>
              </div>
              <label class="col-1">Data inizio:</label>
              <div class="col-2">
                <datepicker :disabled="isReadOnly" class="form-control" :value.sync="workOrder.StartDate_Str"/>
              </div>
              <label class="col-1">Data scadenza:</label>
              <div class="col-2">
                <datepicker :disabled="isReadOnly" class="form-control" :value.sync="workOrder.DueDate_Str"/>
              </div>
              <label class="col-1">Data chiusura:</label>
              <div class="col-2">
                <datepicker :disabled="isReadOnly" class="form-control" :value.sync="workOrder.EndDate_Str"/>
              </div>
            </div>

            <!-- Note Ordine -->
            <div class="row pt-3">
              <label class="col-1">Note ordine:</label>
              <div class="col-11 text-right">
                <textarea class="form-control" disabled v-model="workOrder.Order_Note" rows="3"></textarea>
              </div>
            </div>

            <!-- Note -->
            <div class="row pt-3">
              <label class="col-1">Note:</label>
              <div class="col-11 text-right">
                <textarea class="form-control" :disabled="isReadOnly" v-model="workOrder.Note" rows="3"></textarea>
              </div>
            </div>

            <!-- Widgets Stato Economico -->
            <div class="row pt-4">
              <div class="col-4">
                <div class="card">
                  <div class="card-body">
                    <div class="row">
                      <h3 class="card-title col-4 pt-2">Ricavi</h3>
                      <h1 class="card-text col-8 text-center text-success">{{workOrder.Revenue | currency}}</h1>
                    </div>
                  </div>
                </div>
              </div>

              <div class="col-4">
                <div class="card">
                  <div class="card-body">
                    <div class="row">
                      <h3 class="card-title col-4 pt-2">Costi</h3>
                      <h1 class="card-text col-8 text-center text-danger">{{workOrder.Cost | currency}}</h1>
                    </div>
                  </div>
                </div>
              </div>

              <div class="col-4">
                <div class="card">
                  <div class="card-body">
                    <div class="row">
                      <h3 class="card-title col-4 pt-2">Margini</h3>
                      <h1 class="card-text col-8 text-center" v-bind:class="{'text-success' : workOrder.Margin > 0, 'text-danger' : workOrder.Margin < 0}" >{{workOrder.Margin | currency}} ({{workOrder.MarginPerc | percentage}})</h1>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Box Movimenti -->
            <div>
              <movements-box :work-order="this.workOrder" :movements.sync="workOrder.Movements" :is-read-only="isReadOnly" ></movements-box>
            </div>


        </div>

        <!-- Tab incarichi -->
        <div id="incarichi" class="tab-pane fade pt-4">

          <table class="table table-bordered pt-4">

            <thead class="thead-dark">
              <tr>
                  <th scope="col" width="5%">#</th>
                  <th scope="col" width="25%">Articolo</th>
                  <th scope="col" width="20%">Fornitore</th>
                  <th scope="col" width="20%">Tecnico</th>
                  <th scope="col" width="10%">Data</th>
                  <th scope="col" width="10%">Ora inizio</th>
                  <th scope="col" width="10%">Ora fine</th>
                  <!-- <th scope="col" width="10%"></th> -->
              </tr>
            </thead>

            <!-- Corpo tabella -->
            <tbody>
              <task-row v-for="(task, index) in workOrder.Tasks" :key="index" :task="task" :index="index" :suppliers="suppliers" :time-options="timeOptions" ></task-row>
            </tbody>
          </table>   

        </div>

        <!-- Tab allegati -->
        <div id="allegati" class="tab-pane fade">

          <div class="row pt-3 justify-content-center">
            <div v-if="!isReadOnly" class="col-10 text-right">
              <file-uploader class="full-width btn-primary" :url="'/api/attachments/Upload?referenceType=WorkOrder&referenceId=' + workOrder.Id" :title="'Carica allegato'" v-on:uploaded="onFileUploaded"></file-uploader> 
            </div>           
          </div>

          <!-- Tabella allegati commessa -->
          <div>
            <div class="row">
              <div class="offset-md-1 col-3">
                <h5>Commessa</h5>
              </div>
            </div>
            <div class="row justify-content-center">

              <table class="table table-bordered col-10">
                
                <thead class="thead-dark">
                  <tr >
                    <th width="20%">Allegato</th>
                    <th>Descrizione</th>
                    <th width="20%">Categoria</th>
                    <th  width="5%"></th>
                  </tr>
                </thead>

                
                <tbody>
                  <tr v-for="(attachment, index) in workOrder.Attachments" :key="index">
                    <td ><a download v-bind:href="'/api/attachments/download?id=' + attachment.Id">{{attachment.OriginalFileName}}</a></td> 
                    <td><input type="text" class="form-control" v-model="attachment.Description" /></td>
                    <td><select2 class="form-control" :placeholder="'-'" :options="attachmentCategories" :value.sync="attachment.CategoryId" :value-field="'Id'" :text-field="'Description'" /></td>
                    <td>
                      <a v-on:click="attachmentSelectedIndex=index; $refs.confirmDeleteAttachmentDialog.open()" title="Rimuovi" class="cursor-pointer" >
                          <i class="far fa-trash-alt ml-3"></i>
                      </a>
                    </td>
                  </tr>
                </tbody>          
              </table>  
            </div>          
          </div>

          <!--Tabella allegati Ordine -->
          <div>
            <div class="row">
              <div class="offset-md-1 col-3">
                <h5>Ordine</h5>
              </div>
            </div>
            <div class="row justify-content-center">

              <table class="table table-bordered col-10">

                <thead class="thead-dark">
                  <tr >
                    <th width="20%">Allegato</th>
                    <th>Descrizione</th>
                    <th width="20%">Categoria</th>
                    <th  width="5%"></th>
                  </tr>
                </thead>

                <tbody>
                  <tr v-for="(attachment, index) in workOrder.PreviousAttachments.filter(a => a.ReferenceType == 'Order')" :key="index">
                    <td ><a download v-bind:href="'/api/attachments/download?id=' + attachment.Id">{{attachment.OriginalFileName}}</a></td> 
                    <td>{{attachment.Description}}</td>
                    <td>{{attachment.Category_Description}}</td>
                    <td>
                    </td>
                  </tr>
                </tbody>          
              </table>            

            </div>
          </div>  

          <!-- Tabella allegati offerta --> 
          <div>
            <div class="row">
              <div class="offset-md-1 col-3">
                <h5>Offerta</h5>
              </div>
            </div>
            <div class="row justify-content-center">

              <table class="table table-bordered col-10">

                <thead class="thead-dark">
                  <tr >
                    <th width="20%">Allegato</th>
                    <th>Descrizione</th>
                    <th width="20%">Categoria</th>
                    <th  width="5%"></th>
                  </tr>
                </thead>

                <tbody>
                  <tr v-for="(attachment, index) in workOrder.PreviousAttachments.filter(a => a.ReferenceType == 'Offer')" :key="index">
                    <td ><a download v-bind:href="'/api/attachments/download?id=' + attachment.Id">{{attachment.OriginalFileName}}</a></td> 
                    <td>{{attachment.Description}}</td>
                    <td>{{attachment.Category_Description}}</td>
                    <td>
                    </td>
                  </tr>
                </tbody>          
              </table>  
            </div>
          </div>

          <!-- Tabella allegati richiesta offerta -->
          <div>
            <div class="row">
              <div class="offset-md-1 col-3">
                <h5>Richiesta Offerta</h5>
              </div>
            </div>
            <div class="row justify-content-center">

              <table class="table table-bordered col-10">

                <thead class="thead-dark">
                  <tr >
                    <th width="20%">Allegato</th>
                    <th>Descrizione</th>
                    <th width="20%">Categoria</th>
                    <th  width="5%"></th>
                  </tr>
                </thead>

                <tbody>
                  <tr v-for="(attachment, index) in workOrder.PreviousAttachments.filter(a => a.ReferenceType == 'OfferRequest')" :key="index">
                    <td ><a download v-bind:href="'/api/attachments/download?id=' + attachment.Id">{{attachment.OriginalFileName}}</a></td> 
                    <td>{{attachment.Description}}</td>
                    <td>{{attachment.Category_Description}}</td>
                    <td>
                    </td>
                  </tr>
                </tbody>          
              </table>  
            </div>
          </div>                       

        </div>

        <!-- Annulla / Salva -->
        <div v-if="!isReadOnly" class="row pt-3">
          <div class="col-12 text-right">
            <button class="btn btn-secondary mr-2" role="button" v-on:click="reload()">Annulla</button>
            <button class="btn btn-primary" role="button" v-on:click="onSave()">Salva</button>
          </div>
        </div>

      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import * as jquery from "jquery";

// Components
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ValidationDialog from "../../components/validationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";
import Waiter from "../../components/waiter.vue";
import FileUploader from "../../components/fileUploader.vue";
import MovementsBox from "./movementsBox.vue";
import TaskRow from "./taskRow.vue";

// Modelli
import { Attachment } from "@/models/attachment.model";
import { WorkOrder } from "../../models/workOrder.model";
import { Movement } from "../../models/movement.model";
import { Dropdown } from "../../models/dropdown.model";
import { AttachmentCategory } from "@/models/attachmentCategory.model";

// Servizi
import { DropdownService } from "@/services/dropdown.service";
import { WorkOrdersService } from "../../services/workOrders.service";
import { UrlService } from "@/services/url.service";
import { PermissionsService } from "@/services/permissions.service";
import { AttachmentCategoriesService } from "@/services/attachmentCategories.service";
import { AttachmentsService } from '../../services/attachments.service';

@Component({
    components: {
        Select2,
        ConfirmDialog,
        ValidationDialog,        
        FileUploader,
        Datepicker,
        Waiter,
        NotificationDialog,
        MovementsBox,
        TaskRow        
    },
    filters: {
        currency(value: number) {
            return value ?  value.toFixed(0).toLocaleString() + ' €' : '-';
        },
        percentage(value: any) {
            return value ?  value.toFixed(1) + ' %' : '-';
        }
    }    
})
export default class App extends Vue {
    $refs: any = {
        savedDialog: Vue,
        errorDialog: Vue,
        customerSelect: Vue,
        waiter: Vue,
        validationDialog: Vue,           
        confirmDeleteDialog: Vue,
        confirmDeleteAttachmentDialog: Vue
    };

    public itemNotFound: boolean = false;
    public isReadOnly: boolean = false;
    public btnDeleteVisible: boolean = false;
    public tabIncarichiVisible: boolean = false;

    public workOrdersService: WorkOrdersService = new WorkOrdersService();
    public dropdownService: DropdownService = new DropdownService();
    public attachmentCategoriesService: AttachmentCategoriesService = new AttachmentCategoriesService();
    public attachmentsService: AttachmentsService = new AttachmentsService();

    public attachmentSelectedIndex: number = 0;

    public workOrder: WorkOrder = new WorkOrder();

    public headQuarters: Dropdown = new Dropdown();
    public businessSublines: Dropdown = new Dropdown();
    public statuses: Dropdown = new Dropdown();
    public sellers: Dropdown = new Dropdown();
    public suppliers: Dropdown = new Dropdown();
    public timeOptions: Dropdown = new Dropdown();

    public newMovement_In: Movement = new Movement();
    public newMovement_Out: Movement = new Movement();

    public attachmentCategories: AttachmentCategory[] = [];

    constructor() {
        super();
    }

    public mounted() {

        this.readPermissions();
        this.load(UrlService.getUrlParameter("id"));
        this.loadDropdown();

        this.keepSelectedTabOnRefresh();
    }

    // caricamento commessa
    private load(id: string) {
        this.$refs.waiter.open();
        this.workOrdersService.details(id).then(response => {
          
          if(response.data != null) {            
            this.workOrder = response.data;
            this.$refs.customerSelect.setItem(
                this.workOrder.CustomerId,
                this.workOrder.Customer_FullBusinessName
            );              
          } else {
            this.itemNotFound = true;
          }

          this.$refs.waiter.close();

        });
    }

    // evento allegato caricato
    public onFileUploaded(attachment: Attachment): void {
        this.workOrder.Attachments.push(attachment);
    }

    // salvataggio commessa
    public onSave() {
        this.$refs.waiter.open();
       
        this.workOrdersService.save(this.workOrder).then(
          (response) => {
            this.workOrder = response.data;
            this.$refs.waiter.close();
            this.$refs.savedDialog.open();
          },
          (error) => { 

            this.$refs.waiter.close();
            if(error.response.status == 400){   // Bad Request => messaggi di validazione
              this.$refs.validationDialog.openDialog(error.response.data.ModelState);
            } else {
              this.$refs.errorDialog.open();
            }
          }
        );
    }

    // caricamento dropdown
    private loadDropdown() {

        this.dropdownService.getHeadQuarters().then(response => {
            this.headQuarters = response.data;
        });

        this.dropdownService.getBusinessSublines().then(response => {
            this.businessSublines = response.data;
        });

        this.dropdownService.getWorkOrderStatuses().then(response => {
            this.statuses = response.data;
        });

        this.attachmentCategoriesService.index("WorkOrder").then(response => {
            this.attachmentCategories = response.data;
        });

        this.dropdownService.getUsers("Commerciale").then(response => {
            this.sellers = response.data;
        });     

        this.dropdownService.getSuppliers().then(response => {
            this.suppliers = response.data;
        });               
        
        this.dropdownService.getTimeOptions().then(response => {
            this.timeOptions = response.data;
        });             

    }

    // Mantengo la tab selezionata per il refresh della pagina
    public keepSelectedTabOnRefresh() {
        $("ul.nav-tabs > li > a").on("shown.bs.tab", function(e) {
            window.location.hash = String($(e.target).attr("href"));
        });

        $('#tabWrapper a[href="' + window.location.hash + '"]').tab("show");
    }

    // elimina commessa
    public onRemove() {
        this.workOrdersService.delete(this.workOrder).then(response => {
            this.$refs.removedDialog.open();
        });
    }

   // evento rimozione allegato
   public onAttachmentRemoveClicked() {
      this.attachmentsService
         .delete(this.workOrder.Attachments[this.attachmentSelectedIndex].Id)
         .then(() => {
            this.workOrder.Attachments.splice(this.attachmentSelectedIndex, 1);
         });
   }

    // lettura permessi da jwt
    private readPermissions() {

        this.isReadOnly = !PermissionsService.isViewItemAuthorized(
            "WorkOrders",
            "Edit",
            "Edit"
        );
        this.btnDeleteVisible = PermissionsService.isViewItemAuthorized(
            "WorkOrders",
            "Edit",
            "Delete"
        );
        this.tabIncarichiVisible = PermissionsService.isViewItemAuthorized(
            "WorkOrders",
            "Edit",
            "Tasks"
        );        

    }

    // reload della pagina sullo stesso id
    public reload() {
        UrlService.reload();
    }

    public redirect() {
        UrlService.redirect("/workOrders");
    }    

}
</script>

<style lang="scss">
</style>

