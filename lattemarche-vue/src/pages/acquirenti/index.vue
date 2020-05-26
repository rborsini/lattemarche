<template>
  <div>
    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Pannello editazione dettaglio -->
    <editazione-acquirente-modal ref="editazioneAcquirenteModal" :acquirente="acquirente" v-on:salvato="$refs.savedDialog.open()" ></editazione-acquirente-modal>

    <!-- Pannello notifica salvatagggio -->
    <notification-dialog
      ref="savedDialog"
      :title="'Conferma salvataggio'"
      :message="'Acquirente salvato correttamente'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog
      ref="removedDialog"
      :title="'Conferma rimozione'"
      :message="'Acquirente rimosso correttamente'"
       v-on:ok="reload()"
    ></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog
      ref="confirmDeleteDialog"
      :title="'Conferma eliminazione'"
      :message="'Sei sicuro di voler rimuovere l\'acquirente selezionato?'"
      v-on:confirmed="onRemove()"
    ></confirm-dialog>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="acquirenti" v-on:data-loaded="onDataLoaded">
      <!-- Toolbox -->
      <template slot="toolbox" v-if="canAdd">
        <button class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>
      </template>

      <!-- Colonne -->
      <template slot="thead">
        <th>P. IVA</th>
        <th>Ragione sociale</th>
        <th v-if="canEdit || canRemove"></th>
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
import EditazioneAcquirenteModal from "../acquirenti/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { Acquirente } from "../../models/acquirente.model";
import { AcquirentiService } from "../../services/acquirenti.service";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    openAcquirente(acqu: Acquirente): void;
    close(): void;
  }
}

@Component({
  components: {
    Select2,
    ConfirmDialog,
    NotificationDialog,
    EditazioneAcquirenteModal,
    DataTable,
    Waiter
  }
})
export default class AcquirentiIndexPage extends Vue {
  $refs: any = {
    savedDialog: Vue,
    removedDialog: Vue,
    editazioneAcquirenteModal: Vue,
    confirmDeleteDialog: Vue,
    waiter: Vue
  };

  private acquirentiService: AcquirentiService;
  private acquirente: Acquirente;
  private idAcquirenteDaRimuovere!: number;

  public tableOptions: any = {};
  public acquirenti: Acquirente[] = [];
  public canAdd: boolean = false;
  public canEdit: boolean = false;
  public canRemove: boolean = false;

  constructor() {
    super();

    this.acquirentiService = new AcquirentiService();
    this.acquirente = new Acquirente();

    this.canAdd = $("#canAdd").val() == "true";
    this.canEdit = $("#canEdit").val() == "true";
    this.canRemove = $("#canRemove").val() == "true";
  }

  public mounted() {
    this.$refs.waiter.open();
    this.initTable();

    this.acquirentiService.index().then(response => {
      this.acquirenti = response.data;
      this.$refs.waiter.close();
    });
  }

  // Evento fine generazione tabella
  public onDataLoaded() {
    $(".edit").click(event => {
      var element = $(event.currentTarget);
      var rowId = $(element).data("row-id");

      this.acquirentiService.details(rowId).then(response => {
        this.acquirente = response.data;
        this.$refs.editazioneAcquirenteModal.openAcquirente(this.acquirente);
      });
    });

    $(".delete").click(event => {
      var element = $(event.currentTarget);
      this.idAcquirenteDaRimuovere = $(element).data("row-id");

      this.$refs.confirmDeleteDialog.open();
    });
  }

  // nuovo acquirente
  public onAdd() {
    this.acquirente = new Acquirente();
    this.$refs.editazioneAcquirenteModal.open();
  }

  // rimozione acquirente
  public onRemove() {
    this.acquirentiService
      .delete(this.idAcquirenteDaRimuovere)
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

    var ce = this.canEdit;
    var cr = this.canRemove;

    if (ce || cr) {
      options.columns.push({
        render: function(data: any, type: any, row: any) {
          var html = '<div class="text-center">';

          if (ce)
            html +=
              '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' +
              row.Id +
              '" ><i class="far fa-edit"></i></a>';

          if (cr)
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
    }

    this.tableOptions = options;
  }
}
</script>