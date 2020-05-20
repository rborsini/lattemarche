<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- modale errore generico -->
    <notification-dialog ref="errorDialog" :title="'Errore imprevisto'" :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'" v-on:ok="reload()"></notification-dialog>

    <!-- Pannello modale degli errori -->
    <validation-dialog ref="validationDialog" :title="'Operazione non riuscita'" ></validation-dialog>

    <!-- modale conferma salvataggio -->
    <notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Ordine salvato correttamente'" v-on:ok="reload()" ></notification-dialog>

    <!-- modale editazione articolo -->
    <article-dialog ref="articleDialog" :editable="!isReadOnly" :item.sync="selectedItem" :dateDocument.sync="order.Date_Str" v-on:saved="onSavedArticleDialog()" ></article-dialog>

    <!--  modale conferma eliminazione articolo -->
    <confirm-dialog ref="confirmDeleteArticleDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler rimuovere l\'articolo ?'" v-on:confirmed="onRemoveArticle()" ></confirm-dialog>

    <!-- Pannello notifica eliminazione ordine -->
    <notification-dialog ref="removedDialog" :title="'Conferma eliminazione'" :message="'Ordine eliminato correttamente'" v-on:ok="redirect()" ></notification-dialog>

    <!-- Pannello modale conferma eliminazione ordine -->
    <confirm-dialog ref="confirmDeleteDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler eliminare l\'ordine?'" v-on:confirmed="onRemove()" ></confirm-dialog>

    <!-- Pannello modale conferma eliminazione allegato -->
    <confirm-dialog ref="confirmDeleteAttachmentDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler eliminare l\'allegato?'" v-on:confirmed="onAttachmentRemoveClicked()" ></confirm-dialog>

    <!-- modale informazioni stato documento -->
    <status-history-dialog ref="statusHistoryDialog" :transitions.sync="order.Transitions"></status-history-dialog>

    <!-- breadcrumb -->
    <div class="jumbotron">
      <div class="row">
        <div class="col-6">
          <ol class="breadcrumb mb-0">
            <li class="breadcrumb-item">
              <a class="root" href="/">Home</a>
            </li>
            <li class="breadcrumb-item">
              <a class="root" href="/orders">Ordine</a>
            </li>
            <li class="breadcrumb-item active">{{order.Code}}</li>
          </ol>
        </div>
        <div class="col-6 text-right" v-if="!itemNotFound" >
          <button v-if="order.Id && notAnyItemsTransformed && btnDeleteVisible" v-on:click="$refs.confirmDeleteDialog.open()" class="btn btn-primary mr-2 mt-1" role="button" >Elimina</button>
        </div>
      </div>
    </div>

    <div v-if="itemNotFound" class="portlet light" >      
      <div class="portlet-body">
        <div class="row pl-4 pt-4">
            <h2>Nessun ordine trovata</h2>
        </div>
      </div>
    </div>   

    <div v-else>

      <ul class="nav nav-tabs" id="tabWrapper">
        <li class="active">
          <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
        </li>
        <li>
          <a data-toggle="tab" class="nav-link" href="#allegati" >Allegati ({{order.Attachments.length + order.PreviousAttachments.length}})</a>
        </li>
      </ul>

      <div class="tab-content jumbotron">
        <!-- Tab modifica ordine -->
        <div id="dettaglio" class="tab-pane fade show active">
          <!-- Stato / Codice-->
          <div class="row pt-3">
            <label v-if="order.Id" class="col-1">Stato:</label>
            <div v-if="order.Id" class="col-2">
              <div class="row">
                <div class="col-10">
                  <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="statuses" :value.sync="order.StatusId" :value-field="'Id'" :text-field="'Code'" />
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
              <input type="text" class="form-control" disabled v-model="order.Code" />
            </div>
          </div>

          <!-- Sede / Cliente / Sottolinea / Data -->
          <div class="row pt-3">
            <label class="col-1">Sede:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="headQuarters.Items" :value.sync="order.HeadQuarterId" :value-field="'Value'" :text-field="'Text'" />
            </div>

            <label class="col-1">Cliente:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="true" ajax="true" ref="customerSelect" :placeholder="''" :url="'/api/customers/search'" :value.sync="order.CustomerId" value-field="Id" text-field="FullBusinessName" />
            </div>

            <label class="col-1">Sottolinea:</label>
            <div class="col-2">
              <select2 v-on:value-changed="updateCode" :disabled="isReadOnly" class="form-control" :placeholder="'-'" :options="businessSublines.Items" :value.sync="order.BusinessSublineId" :value-field="'Value'" :text-field="'Text'" />
            </div>

            <label class="col-1">Data:</label>
            <div class="col-2">
              <datepicker :disabled="isReadOnly" v-on:value-changed="updateCode(); updateItemsPriceSuggested();" class="form-control" :value.sync="order.Date_Str" />
            </div>
          </div>

          <!-- Modalità di pagamento / Commerciale -->
          <div class="row pt-3">
            <label class="col-1">Mod. pagamento:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="paymentTypes.Items" :value-field="'Value'" :text-field="'Text'" :value.sync="order.PaymentTypeId" />
            </div>

            <label class="col-1">Commerciale:</label>
            <div class="col-2">
              <select2 class="form-control" :disabled="isReadOnly || order.OfferId != ''" :placeholder="'-'" :options="sellers.Items" :value.sync="order.Seller" :value-field="'Value'" :text-field="'Text'" />
            </div>
          </div>

          <!-- Genera commessa / Aggiungi articolo -->
          <div class="row pt-4 pb-1">
            <div class="col-6">
              <h4>Articoli</h4>
            </div>
            <div class="col-6 text-right">
              <button v-if="notAllItemsTransformed && btnTransformVisible && order.Status_Code == 'Revisionato'" :disabled="!anyItemSelected || !btnTransformEnabled" v-on:click="onTransformClick()" class="btn btn-primary mr-2" type="button" >Genera commessa</button>
              <button class="btn btn-primary" :disabled="isReadOnly" v-on:click="addItem()" role="button" >Aggiungi articolo</button>
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
                    <th scope="col"></th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item, index) in order.Items" :key="index">
                    <td>
                      <input type="checkbox" v-on:change="onItemChecked(item)" :disabled="isReadOnly || item.WorkOrderId" v-model="item.isSelected" />
                    </td>
                    <td>{{index+1}}</td>
                    <td>{{item.ArticleId}}</td>
                    <td>{{item.Description}}</td>
                    <td>{{item.Quantity}}</td>
                    <td>{{item.UnitPrice}} €</td>
                    <td>{{item.SuggestedPrice}} €</td>
                    <td>{{item.TotalPrice}} €</td>
                    <td>{{item.UOM}}</td>
                    <td>{{item.Note}}</td>
                    <td>
                      <a v-if="item.Origin_Id" :href="'/offers/edit?id=' + item.Origin_Id">offerta</a>
                    </td>
                    <td>
                      <a v-if="item.Destination_Item_Id" :href="'/workOrders/edit?id=' + item.WorkOrderId" >commessa</a>
                    </td>
                    <td class="text-center">
                      <a v-if="!item.WorkOrderId" v-on:click="onItemEdit(item, index)" title="Modifica" class="cursor-pointer" >
                        <i class="far fa-edit"></i>
                      </a>
                      <a v-if="!item.WorkOrderId" :disabled="isReadOnly" v-on:click="onItemDelete(item, index)" title="Rimuovi" class="cursor-pointer" >
                        <i class="far fa-trash-alt ml-3"></i>
                      </a>
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
              <textarea class="form-control" :disabled="isReadOnly" v-model="order.Note" rows="3"></textarea>
            </div>
          </div>
        </div>

        <!-- Tab allegati -->
        <div id="allegati" class="tab-pane fade">
          <div class="row pt-3 justify-content-center">
            <div v-if="!isReadOnly" class="col-10 text-right">
              <file-uploader class="full-width btn-primary" :url="'/api/attachments/Upload?referenceType=Order&referenceId=' + order.Id" :title="'Carica allegato'" v-on:uploaded="onFileUploaded" ></file-uploader>
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
                  <tr>
                    <th width="20%">Allegato</th>
                    <th>Descrizione</th>
                    <th width="20%">Categoria</th>
                    <th width="5%"></th>
                  </tr>
                </thead>

                <tbody>
                  <tr v-for="(attachment, index) in order.Attachments" :key="index">
                    <td>
                      <a download v-bind:href="'/api/attachments/download?id=' + attachment.Id" >{{attachment.OriginalFileName}}</a>
                    </td>
                    <td>
                      <input type="text" class="form-control" v-model="attachment.Description" />
                    </td>
                    <td>
                      <select2 class="form-control" :placeholder="'-'" :options="attachmentCategories" :value.sync="attachment.CategoryId" :value-field="'Id'" :text-field="'Description'" />
                    </td>
                    <td>
                      <a v-on:click="attachmentSelectedIndex=index; $refs.confirmDeleteAttachmentDialog.open()" title="Rimuovi" class="cursor-pointer" > <i class="far fa-trash-alt ml-3"></i> </a>
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
                  <tr>
                    <th width="20%">Allegato</th>
                    <th>Descrizione</th>
                    <th width="20%">Categoria</th>
                    <th width="5%"></th>
                  </tr>
                </thead>

                <tbody>
                  <tr v-for="(attachment, index) in order.PreviousAttachments.filter(a => a.ReferenceType == 'Offer')" :key="index" >
                    <td>
                      <a download v-bind:href="'/api/attachments/download?id=' + attachment.Id" >{{attachment.OriginalFileName}}</a>
                    </td>
                    <td>{{attachment.Description}}</td>
                    <td>{{attachment.Category_Description}}</td>
                    <td></td>
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
                  <tr v-for="(attachment, index) in order.PreviousAttachments.filter(a => a.ReferenceType == 'OfferRequest')" :key="index" >
                    <td>
                      <a download v-bind:href="'/api/attachments/download?id=' + attachment.Id" >{{attachment.OriginalFileName}}</a>
                    </td>
                    <td>{{attachment.Description}}</td>
                    <td>{{attachment.Category_Description}}</td>
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
import StatusHistoryDialog from "../../components/statusHistoryDialog.vue";
import DataTable from "../../components/dataTable.vue";
import FileUploader from "../../components/fileUploader.vue";
import ArticleDialog from "../../components/articleDialog.vue";
import Waiter from "../../components/waiter.vue";

// servizi
import { AttachmentsService } from "@/services/attachments.service";
import { OrdersService } from "@/services/orders.service";
import { GuidService } from "@/services/guid.service";
import { DropdownService } from "@/services/dropdown.service";

// modelli
import { Attachment } from "@/models/attachment.model";
import { DocumentItem } from "@/models/documentItem.model";
import { Dropdown } from "@/models/dropdown.model";
import { Order } from "@/models/order.model";
import { UrlService } from "@/services/url.service";
import { DocumentStatus } from "@/models/documentStatus.model";
import { AttachmentCategory } from "@/models/attachmentCategory.model";
import { AttachmentCategoriesService } from "@/services/attachmentCategories.service";
import { PermissionsService } from "@/services/permissions.service";

@Component({
  components: {
    Select2,
    ConfirmDialog,
    Datepicker,
    NotificationDialog,
    ValidationDialog,
    StatusHistoryDialog,
    DataTable,
    FileUploader,
    Waiter,
    ArticleDialog
  }
})
export default class App extends Vue {
  $refs: any = {
    customerSelect: Vue,
    savedDialog: Vue,
    articleDialog: Vue,
    confirmDeleteArticleDialog: Vue,
    statusHistoryDialog: Vue,
    errorDialog: Vue,
    validationDialog: Vue,    
    confirmDeleteDialog: Vue,
    confirmDeleteAttachmentDialog: Vue,
    waiter: Vue
  };

  public itemNotFound: boolean = false;
  public order: Order = new Order();
  public notAllItemsTransformed: boolean = true;
  public notAnyItemsTransformed: boolean = false;
  public isReadOnly: boolean = false;
  public btnDeleteVisible: boolean = false;
  public btnTransformVisible: boolean = false;
  public btnTransformEnabled: boolean = false;

  public attachmentsService: AttachmentsService = new AttachmentsService();
  public attachmentCategoriesService: AttachmentCategoriesService = new AttachmentCategoriesService();
  public ordersService: OrdersService = new OrdersService();
  private urlService: UrlService = new UrlService();
  public dropdownService: DropdownService = new DropdownService();

  public attachmentSelectedIndex: number = 0;

  public anyItemSelected: boolean = false;

  public headQuarters: Dropdown = new Dropdown();
  public businessSublines: Dropdown = new Dropdown();
  public paymentTypes: Dropdown = new Dropdown();
  public sellers: Dropdown = new Dropdown();
  public statuses: DocumentStatus[] = [];
  public attachmentCategories: AttachmentCategory[] = [];

  public selectedItem: DocumentItem = new DocumentItem();

  constructor() {
    super();
  }

  public mounted() {
    this.isReadOnly = !PermissionsService.isViewItemAuthorized(
      "Orders",
      "Edit",
      "Edit"
    );
    this.btnDeleteVisible = PermissionsService.isViewItemAuthorized(
      "Orders",
      "Edit",
      "Delete"
    );
    this.btnTransformVisible = PermissionsService.isViewItemAuthorized(
      "Orders",
      "Edit",
      "Transform"
    );

    var id: string = UrlService.getUrlParameter("id");
    if (id != "") {
      this.load(id);  
    } else {
      var today = new Date();
      debugger;
      this.order.Date_Str = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
    }

    this.loadDropdown();
      this.keepSelectedTabOnRefresh();    
  }

  // apertura modale info stato documento
  public openStatusHistoryDialog() {
    this.$refs.statusHistoryDialog.open();
  }

  // caricamento ordine
  private load(id: string) {

    this.$refs.waiter.open();
    this.ordersService.details(id).then(response => {

      if(response.data != null) {
        this.order = response.data;      
        this.notAllItemsTransformed = response.data.Items.filter(
            i =>
              i.WorkOrderId == null ||
              i.WorkOrderId == undefined ||
              i.WorkOrderId == ""
          ).length > 0;

        this.notAnyItemsTransformed = response.data.Items.filter(
            i =>
              i.WorkOrderId != null &&
              i.WorkOrderId != undefined &&
              i.WorkOrderId != ""
          ).length == 0;

        this.btnTransformEnabled = this.order.Status_Code == "Revisionato";
        this.updateItemsPriceSuggested();
        this.$refs.customerSelect.setItem(
          this.order.CustomerId,
          this.order.Customer_FullBusinessName
        );
        this.loadStatuses(this.order.StatusId);
      } else {
        this.itemNotFound = true;
      }
       
      this.$refs.waiter.close();       

    });

  }

  // Aggiornamento DocumentItem in base alla data selezionata
  private updateItemsPriceSuggested() {
    this.ordersService.updateItems(this.order).then(response => {
      this.order = response;
    });
  }

  // Aggiornamento codice ordine
  private updateCode() {
    this.ordersService
      .getCode(this.order.Id, this.order.BusinessSublineId, this.order.Date_Str)
      .then(response => {
        this.order.Code = response.data;
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

    this.attachmentCategoriesService.index("Order").then(response => {
      this.attachmentCategories = response.data;
    });

    this.dropdownService.getUsers("Commerciale").then(response => {
      this.sellers = response.data;
    });
  }

  // caricamento stati plausibili
  private loadStatuses(currentStatus: number) {
    this.ordersService.statuses(currentStatus).then(response => {
      this.statuses = response.data;
    });
  }

  // nuovo item => apertura modale
  public addItem() {
    this.selectedItem = new DocumentItem();
    this.selectedItem.DocumentId = this.order.Id;
    this.selectedItem.Position = this.order.Items.length + 1;

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
      this.order.Items.filter(i => i.isSelected == true).length > 0;
  }

  // conferma rimozione articolo
  public onRemoveArticle() {
    var index: number = this.order.Items.indexOf(this.selectedItem);
    this.order.Items.splice(index, 1);
  }

  // elimina ordine
  public onRemove() {
    this.ordersService.delete(this.order).then(response => {
      this.$refs.removedDialog.open();
    });
  }

  // evento allegato caricato
  public onFileUploaded(attachment: Attachment): void {
    this.order.Attachments.push(attachment);
  }

  // salvataggio ordine
  public onSave() {
    this.$refs.waiter.open();
    this.ordersService.save(this.order)
      .then(response => {
        this.order = response.data;
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

  // generazione commessa
  public onTransformClick() {
    var items: DocumentItem[] = [];
    //Recupero item da aggiungere agli workOrder
    this.order.Items.forEach(i => {
      console.log(i);
      if (i.isSelected && (!i.WorkOrderId || i.WorkOrderId == "")) {
        items.push(i);
      }
    });
    // creazione del workOrder
    if (items.length > 0) {
      this.ordersService.transformToWorkOrders(items).then(response => {
        this.load(this.order.Id);
      });
    }
  }

  // redirect della pagina di ricerca
  public redirect() {
    UrlService.redirect("/orders");
  }

  // redirect della pagina sullo stesso id
  public reload() {
    UrlService.redirect("/orders/edit?id=" + this.order.Id);
  }

  public onSavedArticleDialog() {
    if (!this.selectedItem.Id) {
      this.selectedItem.Id = GuidService.generateGUID();
      this.order.Items.push(this.selectedItem);
    }
  }

  // evento rimozione allegato
  public onAttachmentRemoveClicked() {
    this.attachmentsService
      .delete(this.order.Attachments[this.attachmentSelectedIndex].Id)
      .then(() => {
        this.order.Attachments.splice(this.attachmentSelectedIndex, 1);
      });
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
