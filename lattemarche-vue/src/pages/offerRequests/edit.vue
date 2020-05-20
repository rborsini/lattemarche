<template>
  <div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- modale errore generico -->
    <notification-dialog ref="errorDialog" :title="'Errore imprevisto'" :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'" v-on:ok="reload()"></notification-dialog>

    <!-- modale conferma salvataggio -->
    <notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Richiesta offerta salvata correttamente'" v-on:ok="reload()"></notification-dialog>

    <!-- Pannello modale degli errori -->
    <validation-dialog ref="validationDialog" :title="'Operazione non riuscita'" ></validation-dialog>

    <!-- Pannello notifica eliminazione -->
    <notification-dialog ref="removedDialog"
                         :title="'Conferma eliminazione'"
                         :message="'Richiesta offerta eliminata correttamente'"
                         v-on:ok="redirect()"></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog ref="confirmDeleteDialog"
                    :title="'Conferma eliminazione'"
                    :message="'Sei sicuro di voler eliminare la richiesta offerta?'"
                    v-on:confirmed="onRemove()"></confirm-dialog>

    <!-- Pannello modale conferma eliminazione allegato -->
    <confirm-dialog ref="confirmDeleteAttachmentDialog"
                    :title="'Conferma eliminazione'"
                    :message="'Sei sicuro di voler eliminare l\'allegato?'"
                    v-on:confirmed="onAttachmentRemoveClicked()"></confirm-dialog>

    <!-- modale informazioni stato documento -->
    <status-history-dialog ref="statusHistoryDialog" :transitions.sync="offerRequest.Transitions" ></status-history-dialog>

    <!-- breadcrumb -->
    <div class="jumbotron" >
      <div class="row">
        <div class="col-6">
          <ol class="breadcrumb mb-0"  >
            <li class="breadcrumb-item"><a class="root" href="/" >Home</a></li>
            <li class="breadcrumb-item"><a class="root" href="/offerRequests" >Richieste offerta</a></li>
            <li class="breadcrumb-item active">{{offerRequest.Code}}</li>
          </ol>
        </div>
        <div class="col-6 text-right" v-if="!itemNotFound" >
          <button v-if="!isNew && !offerRequest.OfferId && btnDeleteVisible" v-on:click="$refs.confirmDeleteDialog.open()" class="btn btn-primary mr-2 mt-1" role="button">Elimina</button>
          <button v-if="!isNew && !offerRequest.OfferId && btnTransformVisible" v-on:click="onTransformClick()" class="btn btn-primary mt-1" type="button">Transforma in offerta</button>
        </div>
        
      </div>
    </div>

    <div v-if="itemNotFound" class="portlet light" >      
      <div class="portlet-body">
        <div class="row pl-4 pt-4">
            <h2>Nessuna richiesta trovata</h2>
        </div>
      </div>
    </div>    

    <div v-else >

      <ul class="nav nav-tabs" id="tabWrapper">
        <li class="active">
          <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
        </li>
        <li>
          <a data-toggle="tab" class="nav-link" href="#allegati">Allegati ({{offerRequest.Attachments.length}})</a>
        </li>
      </ul>

      <div class="tab-content jumbotron">      
        <!-- Tab modifica richiesta offerta -->
        <div id="dettaglio" class="tab-pane fade show active">
          
          <!-- Stato / Offerta generata -->
          <div v-if="offerRequest.Id" class="row pt-3">               
            <label class="col-1">Stato:</label>
            <div class="col-2">
              <div class="row" >
                <div class="col-10">
                  <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="statuses" :value.sync="offerRequest.StatusId" :value-field="'Id'" :text-field="'Code'" /> 
                </div>
                <div class="col-2">
                  <button class="btn btn-info" v-on:click="openStatusHistoryDialog()" ><i class="fa fa-info"></i></button>
                </div>            
              </div>
            </div>   
            <label class="col-1">Codice:</label>
            <div class="col-2 mt-1">          
                <input type="text" class="form-control" disabled v-model="offerRequest.Code" />
            </div>                        
            <label v-if="offerRequest.OfferId" class="col-1">Offerta generata:</label>
            <div v-if="offerRequest.OfferId" class="col-2 mt-1">          
              <a  :href="'/offers/edit?id=' + offerRequest.OfferId" >{{offerRequest.Code}}</a>
            </div>
          </div>

          <!-- Sede / Cliente / Sottolinea / Data -->
          <div class="row pt-3">

            <label class="col-1">Sede:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="headQuarters.Items" :value.sync="offerRequest.HeadQuarterId" :value-field="'Value'" :text-field="'Text'" /> 
            </div>

            <label class="col-1">Cliente:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="isReadOnly" ajax="true" ref="customerSelect" :placeholder="''" :url="'/api/customers/search'" :value.sync="offerRequest.CustomerId" value-field="Id" text-field="FullBusinessName" />          
            </div>

            <label class="col-1">Sottolinea:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="businessSublines.Items" :value.sync="offerRequest.BusinessSublineId" :value-field="'Value'" :text-field="'Text'" /> 
            </div> 

            <label class="col-1">Data:</label>
            <div class="col-2">
              <datepicker class="form-control" :disabled="isReadOnly" :value.sync="offerRequest.Date_Str" />
            </div>
            
          </div>

          <!-- Referente / Commerciale -->
          <div class="row pt-3">

            <label class="col-1">Referente:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="referents.Items" :value.sync="offerRequest.Referent" :value-field="'Value'" :text-field="'Text'" />
            </div>

            <label class="col-1">Commerciale:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="sellers.Items" :value.sync="offerRequest.Seller" :value-field="'Value'" :text-field="'Text'" />
            </div>

          </div>

          <!-- Note -->
          <div class="row pt-3">
            <div class="col-12">
              <h4>Note</h4>
            </div>
            <div class="col-12 text-right">
              <textarea class="form-control" :disabled="isReadOnly" v-model="offerRequest.Note" rows="3"></textarea>
            </div>
          </div>
        </div>

        <!-- Tab allegati -->
        <div id="allegati" class="tab-pane fade">
          <div class="row pt-3 justify-content-center">
            <div v-if="!isReadOnly" class="col-10 text-right">
              <file-uploader  class="full-width btn-primary" :url="'/api/attachments/Upload?referenceType=OfferRequest&referenceId=' + offerRequest.Id" :title="'Carica allegato'" v-on:uploaded="onFileUploaded"></file-uploader>
            </div>           
          </div>

          <div class="row justify-content-center">
            <!--Tabella allegati-->
            <table class="table table-bordered mt-3 col-10">
              <!-- Intestazione tabella -->
              <thead class="thead-dark">
                <tr>
                  <th width="20%">Allegato</th>
                  <th>Descrizione</th>
                  <th width="20%">Categoria</th>
                  <th width="5%"></th>
                </tr>
              </thead>
              <!-- Corpo tabella -->
              <tbody>
                <tr v-for="(attachment, index) in offerRequest.Attachments" :key="index">
                  <td><a download v-bind:href="'/api/attachments/download?id=' + attachment.Id">{{attachment.OriginalFileName}}</a></td>
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
      </div>

      <!-- Annulla / Salva -->
      <div v-if="!isReadOnly" class="row pt-3">
        <div class="col-12 text-right">
          <button class="btn btn-secondary mr-2" role="button" v-on:click="reload()">Annulla</button>
          <button class="btn btn-primary" v-on:click="onSave()" role="button">Salva</button>
        </div>      
      </div>

    </div>

  </div>
</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

// componenti
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ValidationDialog from "../../components/validationDialog.vue";
import StatusHistoryDialog from "../../components/statusHistoryDialog.vue";
import FileUploader from "../../components/fileUploader.vue";
import Waiter from "../../components/waiter.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

// servizi
import { AttachmentsService } from "@/services/attachments.service";
import { OfferRequestsService } from "@/services/offerRequests.service";
import { GuidService } from "@/services/guid.service";
import { UsersService } from "@/services/users.service";
import { DropdownService } from "@/services/dropdown.service";

// modelli
import { Attachment } from "@/models/attachment.model";
import { Dropdown } from "@/models/dropdown.model";
import { OfferRequest } from "@/models/offerRequest.model";
import { DocumentStatus } from '@/models/documentStatus.model';
import { AttachmentCategoriesService } from '../../services/attachmentCategories.service';
import { AttachmentCategory } from '../../models/attachmentCategory.model';
import { PermissionsService } from '@/services/permissions.service';
import { UrlService } from '@/services/url.service';

@Component({
  components: {
    ConfirmDialog,
    Select2,
    Datepicker,
    NotificationDialog,
    ValidationDialog,
    StatusHistoryDialog,
    FileUploader,
    Waiter
  }
})
export default class App extends Vue {
  $refs: any = {
    customerSelect: Vue,
    savedDialog: Vue,
    statusHistoryDialog: Vue,
    errorDialog: Vue,
    validationDialog: Vue,
    waiter: Vue,
    confirmDeleteDialog: Vue,
    confirmDeleteAttachmentDialog:Vue,
    removedDialog: Vue
  };

  public itemNotFound: boolean = false;
  public isNew: boolean = true;
  public offerRequest: OfferRequest = new OfferRequest();
  public referents: Dropdown = new Dropdown();
  public sellers: Dropdown = new Dropdown();
  public attachmentCategories: AttachmentCategory[] = [];
  public attachmentDescription = "";
  public attachmentSelectedIndex:number = 0;

  public attachmentsService: AttachmentsService = new AttachmentsService();
  public attachmentCategoriesService: AttachmentCategoriesService = new AttachmentCategoriesService();
  public offerRequestsService: OfferRequestsService = new OfferRequestsService();
  public dropdownService: DropdownService = new DropdownService();
  public usersService: UsersService = new UsersService();

  public headQuarters: Dropdown = new Dropdown();
  public businessSublines: Dropdown = new Dropdown();
  public statuses: DocumentStatus[] = [];

  public isReadOnly: boolean = false;
  public btnDeleteVisible: boolean = false;
  public btnTransformVisible: boolean = false;

  constructor() {
    super();
  }

  public mounted() {

    this.isReadOnly = !PermissionsService.isViewItemAuthorized("OfferRequests", "Edit", "Edit");    
    this.btnDeleteVisible = PermissionsService.isViewItemAuthorized("OfferRequests", "Edit", "Delete");    
    this.btnTransformVisible = PermissionsService.isViewItemAuthorized("OfferRequests", "Edit", "Transform");    

    var id: string = UrlService.getUrlParameter('id');
    if(id != ''){
      this.isNew = false; // bottone trasformazione offerta visibile solo in edit
      this.loadOfferRequest(id);
    } else {
      this.isNew = true;
      var today = new Date();
      this.offerRequest.Date_Str = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
    }
    this.loadDropdown();
    this.keepSelectedTabOnRefresh();
  }

  // apertura modale info stato documento
  public openStatusHistoryDialog() {
    this.$refs.statusHistoryDialog.open();
  }

  // caricamento richiesta offerta
  private loadOfferRequest(id: string){

    this.$refs.waiter.open();
    this.offerRequestsService.details(id)
    .then(response => {
      
      if(response.data != null) {
        this.offerRequest = response.data;      
        this.$refs.customerSelect.setItem(this.offerRequest.CustomerId, this.offerRequest.Customer_FullBusinessName);
        this.loadStatuses(this.offerRequest.StatusId);
      } else {
        this.itemNotFound = true;
      }

      this.$refs.waiter.close();

    });
  }

  // caricamento dropdown
  private loadDropdown(){

    this.dropdownService.getHeadQuarters().then(response => {
      this.headQuarters=response.data
    });

    this.dropdownService.getBusinessSublines().then(response => {
      this.businessSublines=response.data
    });

    this.dropdownService.getUsers().then(response => {
      this.referents = response.data;
    });
    
    this.dropdownService.getUsers("Commerciale").then(response => {
      this.sellers = response.data;
    });

    this.attachmentCategoriesService.index('OfferRequest').then(response => {
      this.attachmentCategories = response.data;
    });

  }

  // caricamento stati plausibili
  private loadStatuses(currentStatus: number){
    this.offerRequestsService.statuses(currentStatus).then(response => {
      this.statuses = response.data;
    });
  }

  // salvataggio richiesta offerta
  public onSave() {    
    
    this.$refs.waiter.open();
    this.offerRequestsService.save(this.offerRequest)
      .then(response => {
        this.offerRequest = response.data;
        this.$refs.waiter.close();
        this.$refs.savedDialog.open();
      })
      .catch(error => {

        this.$refs.waiter.close();
        if(error.response.status == 400){   // Bad Request => messaggi di validazione
          this.$refs.validationDialog.openDialog(error.response.data.ModelState);
        } else {
          this.$refs.errorDialog.open();
        }
        
      });
  }

  // transformazione in offerta
  public onTransformClick() {
    this.$refs.waiter.open();
    this.offerRequestsService.transformToOrder(this.offerRequest)
      .then(response => {
        this.$refs.waiter.close();
        UrlService.redirect('/offers/edit?id=' + response.data.Id);
      })
  }

  // elimina richiesta offerta
  public onRemove() {
    this.offerRequestsService.delete(this.offerRequest)
      .then(response => {
        this.$refs.removedDialog.open();
      })
  }

  // evento allegato caricato
  public onFileUploaded(attachment: Attachment): void {
    this.offerRequest.Attachments.push(attachment);
  }

  // evento rimozione allegato
  public onAttachmentRemoveClicked() {
    this.attachmentsService.delete(this.offerRequest.Attachments[this.attachmentSelectedIndex].Id)
      .then(() => { this.offerRequest.Attachments.splice(this.attachmentSelectedIndex, 1); })    
  }

  // redirect della pagina di ricerca
  public redirect() {
    UrlService.redirect('/offerRequests');
  }

  // redirect della pagina sullo stesso id
  public reload() {
    UrlService.redirect('/offerRequests/edit?id='+this.offerRequest.Id);
  }

  // Mantengo la tab selezionata per il refresh della pagina
  public keepSelectedTabOnRefresh() {
    $("ul.nav-tabs > li > a").on("shown.bs.tab", function(e) {
      window.location.hash = String($(e.target).attr('href'));
    });

    $('#tabWrapper a[href="' + window.location.hash + '"]').tab('show');
  }

}
</script>
