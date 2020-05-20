<template>
   <div>
      <!-- waiter -->
      <waiter ref="waiter"></waiter>

      <!-- modale errore generico -->
      <notification-dialog ref="errorDialog" :title="'Errore imprevisto'" :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'" v-on:ok="reload()" ></notification-dialog>

      <!-- Pannello modale degli errori -->
      <validation-dialog ref="validationDialog" :title="'Operazione non riuscita'"></validation-dialog>

      <!-- modale conferma salvataggio -->
      <notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Offerta salvata correttamente'" v-on:ok="reload()" ></notification-dialog>

      <!-- modale editazione articolo -->
      <article-dialog ref="articleDialog" :editable="!isReadOnly" :item.sync="selectedItem" :dateDocument.sync="offer.Date_Str" v-on:saved="onSavedArticleDialog()" ></article-dialog>

      <!-- modale selezione ordine -->
      <order-picker ref="orderPicker" v-on:selected="onOrderSelected" ></order-picker>

      <!--  modale conferma eliminazione articolo -->
      <confirm-dialog ref="confirmDeleteArticleDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler rimuovere l\'articolo ?'" v-on:confirmed="onRemoveArticle()" ></confirm-dialog>

      <!-- Pannello notifica eliminazione offerta -->
      <notification-dialog ref="removedDialog" :title="'Conferma eliminazione'" :message="'Offerta eliminata correttamente'" v-on:ok="redirect()" ></notification-dialog>

      <!-- Pannello modale conferma eliminazione offerta -->
      <confirm-dialog ref="confirmDeleteDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler eliminare l\'offerta?'" v-on:confirmed="onRemove()" ></confirm-dialog>

      <!-- Pannello modale conferma eliminazione allegato -->
      <confirm-dialog ref="confirmDeleteAttachmentDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler eliminare l\'allegato?'" v-on:confirmed="onAttachmentRemoveClicked()" ></confirm-dialog>

      <!-- modale informazioni stato documento -->
      <status-history-dialog ref="statusHistoryDialog" :transitions.sync="offer.Transitions"></status-history-dialog>

      <!-- breadcrumb -->
      <div class="jumbotron">
         <div class="row">
            <div class="col-6">
               <ol class="breadcrumb mb-0">
                  <li class="breadcrumb-item">
                     <a class="root" href="/">Home</a>
                  </li>
                  <li class="breadcrumb-item">
                     <a class="root" href="/offers">Offerta</a>
                  </li>
                  <li class="breadcrumb-item active">{{offer.Code}}</li>
               </ol>
            </div>
            <div class="col-6 text-right" v-if="!itemNotFound" >
               <button v-if="offer.Id && notAnyItemsTransformed && btnDeleteVisible" v-on:click="$refs.confirmDeleteDialog.open()" class="btn btn-primary mr-2" role="button" >Elimina</button>
            </div>
         </div>
      </div>

      <div v-if="itemNotFound" class="portlet light" >      
         <div class="portlet-body">
         <div class="row pl-4 pt-4">
               <h2>Nessuna offerta trovata</h2>
         </div>
         </div>
      </div>  

      <div v-else>

         <ul class="nav nav-tabs" id="tabWrapper">
            <li class="active">
               <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
            </li>
            <li>
               <a data-toggle="tab" class="nav-link" href="#allegati">Allegati ({{offer.Attachments.length + offer.PreviousAttachments.length}})</a>
            </li>
         </ul>

         <div class="tab-content jumbotron">

            <!-- Tab modifica offerta -->
            <div id="dettaglio" class="tab-pane fade show active">
               <!-- Stato / Codice / Richiesta offerta -->
               <div class="row pt-3">
                  <label class="col-1">Stato:</label>
                  <div class="col-2">
                     <div class="row">
                        <div class="col-10">
                           <select2 :disabled="!offer.StatusId || isReadOnly" class="form-control" :placeholder="'-'" :options="statuses" :value.sync="offer.StatusId" :value-field="'Id'" :text-field="'Code'" />
                        </div>
                        <div class="col-2">
                           <button class="btn btn-info" v-on:click="openStatusHistoryDialog()">
                              <i class="fa fa-info"></i>
                           </button>
                        </div>
                     </div>
                  </div>

                  <label class="col-1">Codice:</label>
                  <div class="col-2">
                     <input type="text" class="form-control" disabled v-model="offer.Code" />
                  </div>

                  <label v-if="offer.OfferRequestId" class="col-1">Richiesta offerta:</label>
                  <div v-if="offer.OfferRequestId" class="col-2 mt-1">
                     <a :href="'/offerrequests/edit?id=' + offer.OfferRequestId">{{offer.OfferRequestCode}}</a>
                  </div>
               </div>

               <!--  Sede / Cliente / Sottolinea / Data -->
               <div class="row pt-3">
                  <label class="col-1">Sede:</label>
                  <div class="col-2">
                     <select2 :disabled="isReadOnly" class="form-control" :placeholder="'-'" :options="headQuarters.Items" :value.sync="offer.HeadQuarterId" :value-field="'Value'" :text-field="'Text'" />
                  </div>

                  <label class="col-1">Cliente:</label>
                  <div class="col-2">
                     <select2 class="form-control" ajax="true" ref="customerSelect" :placeholder="''" :url="'/api/customers/search'" :value.sync="offer.CustomerId" value-field="Id" text-field="FullBusinessName" />
                  </div>

                  <label class="col-1">Sottolinea:</label>
                  <div class="col-2">
                     <select2 :disabled="isReadOnly" v-on:value-changed="updateCode" class="form-control" :placeholder="'-'" :options="businessSublines.Items" :value.sync="offer.BusinessSublineId" :value-field="'Value'" :text-field="'Text'" />
                  </div>

                  <label class="col-1">Data:</label>
                  <div class="col-2">
                     <datepicker :disabled="isReadOnly" v-on:value-changed="updateCode(); updateItemsPriceSuggested();" class="form-control" :value.sync="offer.Date_Str" />
                  </div>
               </div>

               <!-- Modalità di pagamento / Commerciale -->
               <div class="row pt-3">
                  <label class="col-1">Mod. pagamento:</label>
                  <div class="col-2">
                     <select2 :disabled="isReadOnly" class="form-control" :placeholder="'-'" :options="paymentTypes.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="offer.PaymentTypeId" />
                  </div>

                  <label class="col-1">Commerciale: </label>
                  <div class="col-2">
                     <select2 class="form-control" :placeholder="'-'" :options="sellers.Items" :value.sync="offer.Seller" :value-field="'Value'" :text-field="'Text'" />
                  </div>
               </div>

               <!-- Trasforma in ordine / Aggiunti articolo -->
               <div class="row pt-4 pb-1">
                  <div class="col-6">
                     <h4>Articoli</h4>
                  </div>
                  <div class="col-6 text-right">
                     <!-- <button v-if="notAllItemsTransformed && btnTransformVisible" v-on:click="onTransformClick()" :disabled="!anyItemSelected" class="btn btn-primary mr-2" type="button" >Trasforma in ordine</button> -->

                     <button v-if="notAllItemsTransformed && btnTransformVisible" :disabled="!anyItemSelected" class="btn btn-primary mr-2" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        Transforma in ordine
                     </button>
                     <ul class="dropdown-menu" aria-labelledby="dropdownMenu1" >
                        <li><a class="dropdown-item" href="#" v-on:click="onTransformClick()" >Nuovo</a></li>
                        <li><a class="dropdown-item" href="#" v-on:click="onTransform_ToExistingOrder_Click()" >Esistente</a></li>
                     </ul>

                     <button v-if="!isReadOnly" class="btn btn-primary" v-on:click="addItem()" role="button" >Aggiungi articolo</button>
                  </div>
               </div>

               <!-- Tabella articoli -->
               <div class="row">
                  <div class="col-12">
                     <table class="table table-striped">
                        <thead class="thead-dark">
                           <tr>
                              <th scope="col"></th>
                              <th scope="col">#</th>
                              <th scope="col">Codice articolo</th>
                              <th scope="col">Descr. articolo</th>
                              <th scope="col">Qta</th>
                              <th scope="col">Prezzo unitario</th>
                              <th scope="col">Prezzo suggerito</th>
                              <th scope="col">Importo totale</th>
                              <th scope="col">Un. Mis.</th>
                              <th scope="col">Note</th>
                              <th scope="col"></th>
                              <th scope="col"></th>
                           </tr>
                        </thead>
                        <tbody>
                           <tr v-for="(item, index) in offer.Items" :key="index">
                              <td>
                                 <input type="checkbox" v-on:change="onItemChecked(item)" v-model="item.isSelected" :disabled="item.Destination_Id || isReadOnly" />
                              </td>
                              <td>{{ index + 1 }}</td>
                              <td>{{ item.ArticleId }}</td>
                              <td>{{ item.Description }}</td>
                              <td>{{ item.Quantity }}</td>
                              <td>{{ item.UnitPrice }} €</td>
                              <td>{{ item.SuggestedPrice }} €</td>
                              <td>{{ item.TotalPrice }} €</td>
                              <td>{{ item.UOM }}</td>
                              <td>{{ item.Note }}</td>
                              <td>
                                 <a v-if="item.Destination_Id" :href="'/orders/edit?id=' + item.Destination_Id" >ordine</a>
                              </td>
                              <td class="text-center">
                                 <a v-if="!item.Destination_Id" v-on:click="onItemEdit(item, index)" title="Modifica" class="cursor-pointer" > <i class="far fa-edit"></i> </a>
                                 <a v-if="!item.Destination_Id && !isReadOnly" v-on:click="onItemDelete(item, index)" title="Rimuovi" class="cursor-pointer" > <i class="far fa-trash-alt ml-3"></i> </a>
                              </td>
                           </tr>
                        </tbody>
                     </table>
                  </div>
               </div>

               <!-- Note -->
               <div class="row pt-3">
                  <div class="col-12">
                     <h4>Note</h4>
                  </div>
                  <div class="col-12 text-right">
                     <textarea :disabled="isReadOnly" class="form-control" v-model="offer.Note" rows="3" ></textarea>
                  </div>
               </div>
            </div>

            <!-- Tab allegati -->
            <div id="allegati" class="tab-pane fade">
               <div class="row pt-3 justify-content-center">
                  <div v-if="!isReadOnly" class="col-10 text-right">
                     <file-uploader class="full-width btn-primary" :url="'/api/attachments/Upload?referenceType=Offer&referenceId=' + offer.Id" :title="'Carica allegato'" v-on:uploaded="onFileUploaded" ></file-uploader>
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
                           <tr>
                              <th width="20%">Allegato</th>
                              <th>Descrizione</th>
                              <th width="20%">Categoria</th>
                              <th width="5%"></th>
                           </tr>
                        </thead>

                        <tbody>
                           <tr v-for="(attachment, index) in offer.Attachments" :key="index">
                              <td>
                                 <a download v-bind:href="'/api/attachments/download?id=' + attachment.Id " >{{ attachment.OriginalFileName }}</a>
                              </td>
                              <td>
                                 <input type="text" class="form-control" v-model="attachment.Description" />
                              </td>
                              <td>
                                 <select2 class="form-control" :placeholder="'-'" :options="attachmentCategories" :value.sync="attachment.CategoryId" :value-field="'Id'" :text-field="'Description'" />
                              </td>
                              <td>
                                 <a v-on:click="attachmentSelectedIndex = index; $refs.confirmDeleteAttachmentDialog.open();" title="Rimuovi" class="cursor-pointer" ><i class="far fa-trash-alt ml-3"></i> </a>
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
                           <tr>
                              <th width="20%">Allegato</th>
                              <th>Descrizione</th>
                              <th width="20%">Categoria</th>
                              <th width="5%"></th>
                           </tr>
                        </thead>

                        <tbody>
                           <tr v-for="(attachment, index) in offer.PreviousAttachments.filter(a => a.ReferenceType == 'OfferRequest')" :key="index" >
                              <td>
                                 <a download v-bind:href="'/api/attachments/download?id=' + attachment.Id " >{{ attachment.OriginalFileName }}</a>
                              </td>
                              <td>{{ attachment.Description }}</td>
                              <td>{{ attachment.Category_Description }}</td>
                              <td></td>
                           </tr>
                        </tbody>
                     </table>
                  </div>
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
import ConfirmDialog from "../../components/confirmDialog.vue";
import FileUploader from "../../components/fileUploader.vue";
import ArticleDialog from "../../components/articleDialog.vue";
import Waiter from "../../components/waiter.vue";
import StatusHistoryDialog from "../../components/statusHistoryDialog.vue";
import OrderPicker from "../../components/orderPicker.vue";

// servizi
import { AttachmentsService } from "@/services/attachments.service";
import { OffersService } from "@/services/offers.service";
import { GuidService } from "@/services/guid.service";
import { DropdownService } from "@/services/dropdown.service";
import { AttachmentCategoriesService } from "@/services/attachmentCategories.service";
import { PermissionsService } from "@/services/permissions.service";

// modelli
import { Attachment } from "@/models/attachment.model";
import { DocumentItem } from "@/models/documentItem.model";
import { Dropdown } from "@/models/dropdown.model";
import { Offer } from "@/models/offer.model";
import { UrlService } from "@/services/url.service";
import { DocumentStatus } from "@/models/documentStatus.model";
import { AttachmentCategory } from "@/models/attachmentCategory.model";

@Component({
   components: {
      Select2,
      ConfirmDialog,
      Datepicker,
      StatusHistoryDialog,
      NotificationDialog,
      ValidationDialog,
      FileUploader,
      Waiter,
      ArticleDialog,
      OrderPicker
   }
})
export default class App extends Vue {
   $refs: any = {
      customerSelect: Vue,
      savedDialog: Vue,
      articleDialog: Vue,
      statusHistoryDialog: Vue,
      errorDialog: Vue,
      validationDialog: Vue,
      confirmDeleteArticleDialog: Vue,
      confirmDeleteDialog: Vue,
      confirmDeleteAttachmentDialog: Vue,
      orderPicker: Vue,
      waiter: Vue
   };

   public itemNotFound: boolean = false;
   public notAllItemsTransformed: boolean = true;
   public notAnyItemsTransformed: boolean = false;
   public isReadOnly: boolean = false;
   public btnDeleteVisible: boolean = false;
   public btnTransformVisible: boolean = false;

   public offer: Offer = new Offer();

   public attachmentsService: AttachmentsService = new AttachmentsService();
   public attachmentCategoriesService: AttachmentCategoriesService = new AttachmentCategoriesService();
   public offersService: OffersService = new OffersService();
   public dropdownService: DropdownService = new DropdownService();
   public statuses: DocumentStatus[] = [];
   public attachmentCategories: AttachmentCategory[] = [];

   public selectedItem: DocumentItem = new DocumentItem();

   public anyItemSelected: boolean = false;

   public headQuarters: Dropdown = new Dropdown();
   public businessSublines: Dropdown = new Dropdown();
   public paymentTypes: Dropdown = new Dropdown();
   public sellers: Dropdown = new Dropdown();
   public attachmentDescription = "";
   public attachmentSelectedIndex: number = 0;

   constructor() {
      super();
   }

   public mounted() {
      this.isReadOnly = !PermissionsService.isViewItemAuthorized(
         "Offers",
         "Edit",
         "Edit"
      );

      this.btnDeleteVisible = PermissionsService.isViewItemAuthorized(
         "Offers",
         "Edit",
         "Delete"
      );

      this.btnTransformVisible = PermissionsService.isViewItemAuthorized(
         "Offers",
         "Edit",
         "Transform"
      );

      var id: string = UrlService.getUrlParameter("id");
      if (id != "") {
         this.loadOffer(id);
      } else {
         var today = new Date();
         this.offer.Date_Str = today.getDate() + "-" + (today.getMonth() + 1) + "-" + today.getFullYear();
      }

      this.loadDropdown();
      this.keepSelectedTabOnRefresh();
   }

   // apertura modale info stato documento
   public openStatusHistoryDialog() {
      this.$refs.statusHistoryDialog.open();
   }

   // caricamento offerta
   private loadOffer(id: string) {

      this.$refs.waiter.open();
      this.offersService.details(id).then(response => {

         if(response.data != null) {
            this.offer = response.data;
            this.notAllItemsTransformed =
               response.data.Items.filter(
                  i =>
                     i.Destination_Id == null ||
                     i.Destination_Id == undefined ||
                     i.Destination_Id == ""
               ).length > 0;
            this.notAnyItemsTransformed =
               response.data.Items.filter(
                  i =>
                     i.Destination_Id != null &&
                     i.Destination_Id != undefined &&
                     i.Destination_Id != ""
               ).length == 0;
            this.updateItemsPriceSuggested();
            this.$refs.customerSelect.setItem(
               this.offer.CustomerId,
               this.offer.Customer_FullBusinessName
            );
            this.loadStatuses(this.offer.StatusId);
         } else {
            this.itemNotFound = true;
         }

         this.$refs.waiter.close();
      });
   }

   // Creazione codice e assegnazione all'offerta
   private updateCode() {
      this.offersService
         .getCode(
            this.offer.Id,
            this.offer.BusinessSublineId,
            this.offer.Date_Str
         )
         .then(response => {
            this.offer.Code = response.data;
         });
   }

  //  // Aggiornamento DocumentItem in base alla data selezionata
   private updateItemsPriceSuggested() {
      this.offersService.updateItems(this.offer).then(response => {
         this.offer = response;
      });
   }

   // caricamento dropdown
   private loadDropdown() {
      this.dropdownService.getPaymentTypes().then(response => {
         this.paymentTypes = response.data;
      });

      this.dropdownService.getHeadQuarters().then(response => {
         this.headQuarters = response.data;
      });

      this.dropdownService.getBusinessSublines().then(response => {
         this.businessSublines = response.data;
      });

      this.attachmentCategoriesService.index("Offer").then(response => {
         this.attachmentCategories = response.data;
      });

      this.dropdownService.getUsers("Commerciale").then(response => {
         this.sellers = response.data;
      });
   }

   // caricamento stati plausibili
   private loadStatuses(currentStatus: number) {
      this.offersService.statuses(currentStatus).then(response => {
         this.statuses = response.data;
      });
   }

   // nuovo item => apertura modale
   public addItem() {
      this.selectedItem = new DocumentItem();
      this.selectedItem.DocumentId = this.offer.Id;
      this.selectedItem.Position = this.offer.Items.length + 1;

      this.$refs.articleDialog.open();
   }

   // editazione item => apertura modale editazione
   public onItemEdit(item: DocumentItem, index: number) {
      this.selectedItem = item;
      this.$refs.articleDialog.open();
   }

   // rimozione item => apertura modale conferma
   public onItemDelete(item: DocumentItem, index: number) {
      this.selectedItem = item;
      this.$refs.confirmDeleteArticleDialog.open();
   }

   // evento checkbox tabella articoli => aggiornamento flag anyItemSelected
   public onItemChecked(item: DocumentItem) {
      this.anyItemSelected =
         this.offer.Items.filter(i => i.isSelected == true).length > 0;
   }

   public onSavedArticleDialog() {
      if (!this.selectedItem.Id) {
         this.selectedItem.Id = GuidService.generateGUID();
         this.offer.Items.push(this.selectedItem);
      }
   }

   // conferma rimozione articolo
   public onRemoveArticle() {
      var index: number = this.offer.Items.indexOf(this.selectedItem);
      this.offer.Items.splice(index, 1);
   }

   // elimina offerta
   public onRemove() {
      this.offersService.delete(this.offer).then(response => {
         this.$refs.removedDialog.open();
      });
   }

   // salvataggio offerta
   public onSave() {
      this.$refs.waiter.open();
      this.offersService
         .save(this.offer)
         .then(response => {
            this.offer = response.data;
            this.$refs.waiter.close();
            this.$refs.savedDialog.open();
         })
         .catch(error => {
            this.$refs.waiter.close();
            if (error.response.status == 400) {
               // Bad Request => messaggi di validazione
               this.$refs.validationDialog.openDialog(
                  error.response.data.ModelState
               );
            } else {
               this.$refs.errorDialog.open();
            }
         });
   }

   public onTransform_ToExistingOrder_Click() {
      this.$refs.orderPicker.open();
   }

   public onOrderSelected(orderId: string) {
      this.transformToOrder(orderId);
   }

   // transformazione in ordine
   public onTransformClick() {
      this.transformToOrder();
   }

   public transformToOrder(orderId: string = "") {
      var items: DocumentItem[] = [];
      //Recupero item da aggiungere all'Order
      this.offer.Items.forEach(i => {
         if (i.isSelected && (!i.Destination_Id || i.Destination_Id == "")) {
            items.push(i);
         }
      });
      // creazione del Order
      if (items.length > 0) {
         this.$refs.waiter.open();
         this.offersService.transformToOrder(items, orderId).then(response => {
            this.$refs.waiter.close();
            UrlService.redirect("/orders/edit?id=" + response.data.Id);
         });
      }
   }

   // evento allegato caricato
   public onFileUploaded(attachment: Attachment): void {
      this.offer.Attachments.push(attachment);
   }

   // evento rimozione allegato
   public onAttachmentRemoveClicked() {
      this.attachmentsService
         .delete(this.offer.Attachments[this.attachmentSelectedIndex].Id)
         .then(() => {
            this.offer.Attachments.splice(this.attachmentSelectedIndex, 1);
         });
   }

   // redirect della pagina di ricerca
   public redirect() {
      UrlService.redirect("/offers");
   }

   // redirect della pagina sullo stesso id
   public reload() {
      UrlService.redirect("/offers/edit?id=" + this.offer.Id);
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