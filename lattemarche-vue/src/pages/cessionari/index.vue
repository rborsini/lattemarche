<template>
  <div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Pannello editazione dettaglio -->
    <editazione-cessionario-modal
      ref="editazioneCessionarioModal"
      :cessionario="cessionario"
      v-on:salvato="$refs.savedDialog.open()"
    ></editazione-cessionario-modal>

    <!-- Pannello notifica salvatagggio -->
    <notification-dialog
      ref="savedDialog"
      :title="'Conferma salvataggio'"
      :message="'Cessionario salvato correttamente'"
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
      :message="'Sei sicuro di voler rimuovere il cessionario selezionato?'"
      v-on:confirmed="onRemove()"
    ></confirm-dialog>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="cessionari" v-on:data-loaded="onDataLoaded">
      <!-- Toolbox -->
      <template slot="toolbox" >
        <button class="toolbox btn btn-success float-right" v-on:click="onAdd()">Aggiungi</button>
      </template>

      <!-- Colonne -->
      <template slot="thead">
        <th>P. IVA</th>
        <th>Ragione sociale</th>
        <th></th>
      </template>
    </data-table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import DataTable from "../../components/dataTable.vue";
import Select2 from "../../components/select2.vue";
import Waiter from "../../components/waiter.vue";
import { UrlService } from "@/services/url.service";
import EditazioneCessionarioModal from "../cessionari/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";
import { Cessionario } from "../../models/cessionario.model";
import { CessionariService } from "../../services/cessionari.service";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    openCessionario(cess: Cessionario): void;
    close(): void;
  }
}

@Component({
  components: {
    Select2,
    ConfirmDialog,
    NotificationDialog,
    EditazioneCessionarioModal,
    DataTable,
    Waiter
  }
})
export default class CessionariIndexPage extends Vue {
  $refs: any = {
    savedDialog: Vue,
    removedDialog: Vue,
    editazioneAcquirenteModal: Vue,
    confirmDeleteDialog: Vue,
    waiter: Vue
  };

  private cessionariService: CessionariService;
  private cessionario: Cessionario;
  private idCessionarioDaRimuovere!: number;

  public tableOptions: any = {};
  public cessionari: Cessionario[] = [];

  constructor() {
    super();

    this.cessionariService = new CessionariService();
    this.cessionario = new Cessionario();

  }

  public mounted() {
    this.$refs.waiter.open();
    this.initTable();
    this.cessionariService.index().then(response => {
      this.cessionari = response.data;
      this.$refs.waiter.close();
    });
  }

  // Evento fine generazione tabella
  public onDataLoaded() {
    $(".edit").click(event => {
      var element = $(event.currentTarget);
      var rowId = $(element).data("row-id");

      this.cessionariService.details(rowId).then(response => {
        this.cessionario = response.data;
        this.$refs.editazioneCessionarioModal.openCessionario(this.cessionario);
      });
    });

    $(".delete").click(event => {
      var element = $(event.currentTarget);
      this.idCessionarioDaRimuovere = $(element).data("row-id");

      this.$refs.confirmDeleteDialog.open();
    });
  }

  // Nuovo cessionario
  public onAdd() {
    this.cessionario = new Cessionario();
    this.$refs.editazioneCessionarioModal.open();
  }

  // Rimuovi cessionario
  public onRemove() {
    this.cessionariService
      .delete(this.idCessionarioDaRimuovere)
      .then(response => {
        this.$refs.removedDialog.open();
      });
  }

  // Inizializzazione tabella
  private initTable(): void {
    var options: any = {};

    options.columns = [];

    options.columns.push({ data: "Piva" });
    options.columns.push({ data: "RagioneSociale" });



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