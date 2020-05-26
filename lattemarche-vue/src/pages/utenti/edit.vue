<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- modale errore generico -->
    <notification-dialog
      ref="errorDialog"
      :title="'Errore imprevisto'"
      :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- modale conferma salvataggio -->
    <notification-dialog
      ref="savedDialog"
      :title="'Conferma salvataggio'"
      :message="'Utente salvato correttamente'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- Pannello modale conferma eliminazione utente -->
    <confirm-dialog
      ref="confirmDeleteDialog"
      :title="'Conferma eliminazione'"
      :message="'Sei sicuro di procedere con l\'eliminazione?'"
      v-on:confirmed="onRemove()"
    ></confirm-dialog>

    <!-- Pannello notifica eliminazione commessa -->
    <notification-dialog
      ref="removedDialog"
      :title="'Conferma eliminazione'"
      :message="'Utente eliminato correttamente'"
      v-on:ok="redirect()"
    ></notification-dialog>

    <div>
      <ul class="nav nav-tabs" id="tabWrapper">
        <li class="active">
          <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
        </li>
        <li >
          <a v-if="utente.IdProfilo == 3" data-toggle="tab" class="nav-link" href="#allevamenti">Allevamenti</a>
        </li>        
      </ul>

      <div class="tab-content">

        <!-- Tab dettaglio -->
        <div id="dettaglio" class="tab-pane fade show active">

          <!-- tipo profilo -->
          <div class="row form-group pt-5">
            <label class="offset-1 col-sm-1">Tipo profilo</label>
            <div class="col-sm-4">
              <select2
                class="form-control"
                :disabled="utente.Id != 0"
                :options="profilo.Items"
                :value.sync="utente.IdProfilo"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>

            <!-- Acquirente -->
            <label v-if="utente.IdProfilo == 7" class="col-sm-1">Acquirente</label>
            <div v-if="utente.IdProfilo == 7" class="col-sm-4">
              <select2
                class="form-control"
                :options="acquirente.Items"
                :value.sync="utente.IdAcquirente"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>

            <!-- Cessionario -->
            <label v-if="utente.IdProfilo == 8" class="col-sm-1">Cessionario</label>
            <div v-if="utente.IdProfilo == 8" class="col-sm-4">
              <select2
                class="form-control"
                :options="cessionario.Items"
                :value.sync="utente.IdCessionario"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>

            <!-- Destinatario -->
            <label v-if="utente.IdProfilo == 6" class="col-sm-1">Destinatario</label>
            <div v-if="utente.IdProfilo == 6" class="col-sm-4">
              <select2
                class="form-control"
                :options="destinatario.Items"
                :value.sync="utente.IdDestinatario"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>
          </div>

          <!-- ragione sociale / username -->
          <div class="row form-group">
            <label class="offset-1 col-sm-1">Ragione sociale</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="utente.RagioneSociale" />
            </div>
            <label class="col-sm-1">Username</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="utente.Username" />
            </div>
          </div>

          <!-- nome / cognome -->
          <div class="row form-group">
            <label class="offset-1 col-sm-1">Nome</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="utente.Nome" />
            </div>
            <label class="col-sm-1">Cognome</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="utente.Cognome" />
            </div>
          </div>

          <!-- sesso / p.iva/cf -->
          <div class="row form-group">
            <label class="offset-1 col-sm-1">Sesso</label>
            <div class="col-sm-4">
              <select2
                class="form-control"
                :placeholder="'-'"
                :options="sesso.Items"
                :value.sync="utente.Sesso"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>
            <label class="col-sm-1">P. Iva / C.F.</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="utente.PivaCF" />
            </div>
          </div>

          <!-- indirizzo / provincia / città -->
          <div class="row form-group">
            <label class="offset-1 col-sm-1">Indirizzo</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="utente.Indirizzo" />
            </div>

            <label class="col-sm-1">Provincia</label>
            <div class="col-sm-1">
              <select2
                class="form-control"
                :options="provincia.Items"
                :value.sync="utente.SiglaProvincia"
                :value-field="'Value'"
                :text-field="'Text'"
                v-on:value-changed="loadComuni"
              />
            </div>
            <div class="col-sm-3">
              <select2
                class="form-control"
                :options="comune.Items"
                :value.sync="utente.IdComune"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>
          </div>

          <!-- telefono / cellulare -->
          <div class="row form-group">
            <label class="offset-1 col-sm-1">Telefono</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="utente.Telefono" />
            </div>
            <label class="col-sm-1">Cellulare</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="utente.Cellulare" />
            </div>
          </div>

          <!-- note -->
          <div class="row form-group">
            <label class="offset-1 col-sm-1">Note</label>
            <div class="col-sm-9">
              <textarea class="form-control" v-model="utente.Note" rows="3"></textarea>
            </div>
          </div>
        </div>

        <!-- Tab allevamenti -->
        <div id="allevamenti" class="tab-pane fade">

          <div class="row">
              <div class="offset-1 col-sm-10 pt-4">

                <table class="table table-bordered">

                    <thead class="table table-hover table-striped table-bordered">
                        <tr>
                            <th scope="rol">Codice ASL</th>
                            <th scope="rol">Indirizzo</th>
                            <th scope="rol">CUAA</th>
                            <th scope="rol"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(allevamento, index) in utente.Allevamenti" :key="index">
                            <td>
                                {{allevamento.CodiceAsl}}
                            </td>
                            <td>
                                {{allevamento.IndirizzoAllevamento}}
                            </td>
                            <td>
                                {{allevamento.CUAA}}
                            </td>          
                            <td>
                                <div class="text-center">
                                    <!-- <a v-bind:href="'/auditors/edit?id=' + auditor.Id " class="edit text-primary" title="modifica" style="cursor: pointer;" ><i class="far fa-edit"></i></a> -->
                                </div>
                            </td>            
                        </tr>
                    </tbody>

                </table>    

              </div>
          </div>
      

        </div>

        <!-- Annulla / Salva -->
        <div class="row pt-3">
          <div class="col-11 text-right">
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

import ConfirmDialog from "../../components/confirmDialog.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import Waiter from "../../components/waiter.vue";
import Select2 from "../../components/select2.vue";
import { UtentiService } from "@/services/utenti.service";
import { DropdownService } from "@/services/dropdown.service";
import { Utente } from "@/models/utente.model";
import { UrlService } from "@/services/url.service";
import { PermissionsService } from "@/services/permissions.service";
import { Dropdown, DropdownItem } from "../../models/dropdown.model";

@Component({
  components: {
    Select2,
    ConfirmDialog,
    Waiter,
    NotificationDialog
  }
})
export default class App extends Vue {
  $refs: any = {
    savedDialog: Vue,
    errorDialog: Vue,
    waiter: Vue,
    confirmDeleteDialog: Vue
  };

  public itemNotFound: boolean = false;
  public isReadOnly: boolean = false;
  public btnDeleteVisible: boolean = false;

  public utentiService: UtentiService = new UtentiService();
  public dropdownService: DropdownService = new DropdownService();

  public utente: Utente = new Utente();

  public profilo: Dropdown = new Dropdown();
  public sesso: Dropdown = new Dropdown();
  public provincia: Dropdown = new Dropdown();
  public comune: Dropdown = new Dropdown();
  public acquirente: Dropdown = new Dropdown();
  public cessionario: Dropdown = new Dropdown();
  public destinatario: Dropdown = new Dropdown();

  constructor() {
    super();
  }

  public mounted() {
    this.readPermissions();

    var id = UrlService.getUrlParameter("id");
    if (id) {
      this.load(id);
    }
    this.loadDropdown();

    this.keepSelectedTabOnRefresh();
  }

  // caricamento commessa
  private load(id: string) {
    this.$refs.waiter.open();
    this.utentiService.details(id).then(response => {
      if (response.data != null) {
        this.utente = response.data;
        this.loadComuni(this.utente.SiglaProvincia);
      } else {
        this.itemNotFound = true;
      }

      this.$refs.waiter.close();
    });
  }

  // salvataggio commessa
  public onSave() {
    this.$refs.waiter.open();

    this.utentiService.save(this.utente).then(
      response => {
        this.utente = response.data;
        this.$refs.waiter.close();
        this.$refs.savedDialog.open();
      },
      error => {
        this.$refs.waiter.close();
        if (error.response.status == 400) {
          // Bad Request => messaggi di validazione
          this.$refs.validationDialog.openDialog(
            error.response.data.ModelState
          );
        } else {
          this.$refs.errorDialog.open();
        }
      }
    );
  }

  // caricamento dropdown
  private loadDropdown() {
    // tipi profilo
    this.dropdownService.getProfili().then(response => {
      this.profilo = response.data;
    });

    // sesso
    this.sesso.Items.push(new DropdownItem("M", "Maschio"));
    this.sesso.Items.push(new DropdownItem("F", "Femmina"));

    // province
    this.dropdownService.getProvince().then(response => {
      this.provincia = response.data;
    });

    // acquirente
    this.dropdownService.getAcquirenti().then(response => {
      this.acquirente = response.data;
    });

    // Cesionario
    this.dropdownService.getCessionari().then(response => {
      this.cessionario = response.data;
    });

    // Destinatario
    this.dropdownService.getDestinatari().then(response => {
      this.destinatario = response.data;
    });
  }

  public loadComuni(provincia: string): void {
    this.dropdownService.getComuni(provincia).then(response => {
      if (response.data != null) {
        this.comune = response.data;
      }
    });
  }

  // Mantengo la tab selezionata per il refresh della pagina
  public keepSelectedTabOnRefresh() {
    $("ul.nav-tabs > li > a").on("shown.bs.tab", function(e) {
      window.location.hash = String($(e.target).attr("href"));
    });

    $('#tabWrapper a[href="' + window.location.hash + '"]').tab("show");
  }

  // elimina utente
  public onRemove() {
    this.utentiService.delete(this.utente.Id).then(response => {
      this.$refs.removedDialog.open();
    });
  }

  // lettura permessi da jwt
  private readPermissions() {
    this.isReadOnly = !PermissionsService.isViewItemAuthorized(
      "Utenti",
      "Edit",
      "Edit"
    );
    this.btnDeleteVisible = PermissionsService.isViewItemAuthorized(
      "Utenti",
      "Edit",
      "Delete"
    );
  }

  // reload della pagina sullo stesso id
  public reload() {
    UrlService.reload();
  }

  public redirect() {
    UrlService.redirect("/utenti");
  }
}
</script>