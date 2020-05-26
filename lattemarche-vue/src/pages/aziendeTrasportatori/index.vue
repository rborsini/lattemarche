<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Pannello editazione dettaglio -->
    <editazione-trasportatore-modal
      ref="editazioneAziendaTrasportatoreModal"
      :trasportatore="trasportatore"
      v-on:salvato="$refs.savedDialog.open()"
    ></editazione-trasportatore-modal>

    <!-- Pannello notifica salvatagggio -->
    <notification-dialog
      ref="savedDialog"
      :title="'Conferma salvataggio'"
      :message="'Trasportatore salvato correttamente'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog
      ref="removedDialog"
      :title="'Conferma rimozione'"
      :message="'Trasportatore rimosso correttamente'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog
      ref="confirmDeleteDialog"
      :title="'Conferma eliminazione'"
      :message="'Sei sicuro di voler rimuovere il trasportatore selezionato?'"
      v-on:confirmed="onRemove()"
    ></confirm-dialog>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="acquirenti" v-on:data-loaded="onDataLoaded">
      <!-- Toolbox -->
      <template slot="toolbox">
        <button class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>
      </template>

      <!-- Colonne -->
      <template slot="thead">
        <th>P. IVA</th>
        <th>Ragione sociale</th>
        <th>Nome titolare</th>
        <th>Cognome titolare</th>
        <th></th>
      </template>
    </data-table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

import { UrlService } from "@/services/url.service";
import DataTable from "../../components/dataTable.vue";
import Select2 from "../../components/select2.vue";
import Waiter from "../../components/waiter.vue";
import EditazioneAziendaTrasportatoreModal from "../aziendaTrasportatori/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { AziendaTrasportatore } from "../../models/aziendaTrasportatore.model";
import { TrasportatoriService } from "../../services/trasportatori.service";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    openTrasportatore(tras: AziendaTrasportatore): void;
    close(): void;
  }
}

@Component({
  components: {
    Select2,
    ConfirmDialog,
    NotificationDialog,
    EditazioneAziendaTrasportatoreModal,
    DataTable,
    Waiter
  }
})
export default class AziendaTrasportatoriIndexPage extends Vue {
  $refs: any = {
    savedDialog: Vue,
    removedDialog: Vue,
    editazioneAziendaTrasportatoreModal: Vue,
    confirmDeleteDialog: Vue,
    waiter: Vue
  };

  private trasportatoriService: TrasportatoriService;
  private aziendaTrasportatore: AziendaTrasportatore;
  private idTrasportatoreDaRimuovere!: number;

  public tableOptions: any = {};
  public trasportatori: AziendaTrasportatore[] = [];

  constructor() {
    super();

    this.trasportatoriService = new TrasportatoriService();
    this.aziendaTrasportatore = new AziendaTrasportatore();
  }

  public mounted() {
    this.$refs.waiter.open();
    this.initTable();
    this.trasportatoriService.index().then(response => {
      this.trasportatori = response.data;
      this.$refs.waiter.close();
    });
  }

  // Evento fine generazione tabella
  public onDataLoaded() {
    $(".edit").click(event => {
      var element = $(event.currentTarget);
      var rowId = $(element).data("row-id");
      this.trasportatoriService.details(rowId).then(response => {
        this.aziendaTrasportatore = response.data;
        this.$refs.editazioneAziendaTrasportatoreModal.openTrasportatore(
          this.aziendaTrasportatore
        );
      });
    });

    $(".delete").click(event => {
      var element = $(event.currentTarget);
      this.idTrasportatoreDaRimuovere = $(element).data("row-id");
      this.$refs.confirmDeleteDialog.open();
    });
  }

  // nuovo acquirente
  public onAdd() {
    this.aziendaTrasportatore = new AziendaTrasportatore();
    this.$refs.editazioneAziendaTrasportatoreModal.open();
  }

  // rimozione acquirente
  public onRemove() {
    this.trasportatoriService
      .delete(this.idTrasportatoreDaRimuovere)
      .then(response => {
        this.$refs.removedDialog.open();
      });
  }

  // inizializzazione tabella
  private initTable(): void {
    var options: any = {};

    options.columns = [];

    options.columns.push({ data: "Piva" });
    options.columns.push({ data: "RagioneSociale" });
    options.columns.push({ data: "NomeTitolare" });
    options.columns.push({ data: "CognomeTitolare" });

    options.columns.push({
      render: function(data: any, type: any, row: any) {
        var html = '<div class="text-center">';

        html +=
          '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' +
          row.Id +
          '" ><i class="far fa-edit"></i></a>';

        html +=
          '<a class="pl-3 delete" title="elimina" style="cursor: pointer;" data-row-id="' +
          row.Id +
          '" ><i class="far fa-trash-alt"></i></a>';

        html += "</div>";

        return html;
      },
      className: "edit-column",
      orderable: false
    });

    this.tableOptions = options;
  }

  // reload della pagina sullo stesso id
  public reload() {
    UrlService.reload();
  }
}
</script>