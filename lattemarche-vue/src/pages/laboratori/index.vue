<template>
  <div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Pannello editazione dettaglio -->
    <editazione-laboratorio-modal ref="editazioneLaboratorioModal" :laboratorio="laboratorio" v-on:salvato="$refs.savedDialog.open()"></editazione-laboratorio-modal>

    <!-- Pannello notifica salvatagggio -->
    <notification-dialog
      ref="savedDialog"
      :title="'Conferma salvataggio'"
      :message="'Laboratorio salvato correttamente'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog
      ref="removedDialog"
      :title="'Conferma rimozione'"
      :message="'Laboratorio rimosso correttamente'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog
      ref="confirmDeleteDialog"
      :title="'Conferma eliminazione'"
      :message="'Sei sicuro di voler rimuovere il laboratorio selezionato?'"
      v-on:confirmed="onRemove()"
    ></confirm-dialog>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="laboratori" v-on:data-loaded="onDataLoaded">
      <!-- Toolbox -->
      <template slot="toolbox" >
        <button class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>
      </template>

      <!-- Colonne -->
      <template slot="thead">
        <th>Id</th>
        <th>Descrizione</th>
        <th></th>
      </template>
    </data-table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

import Waiter from "../../components/waiter.vue";
import DataTable from "../../components/dataTable.vue";
import Select2 from "../../components/select2.vue";
import EditazioneLaboratorioModal from "../laboratori/edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { UrlService } from "@/services/url.service";
import { LaboratoriService } from '../../services/laboratori.service';
import { Laboratorio } from '../../models/laboratorio.model';

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    openLaboratorio(lab: Laboratorio): void;
    close(): void;
  }
}

@Component({
  components: {
    Select2,
    ConfirmDialog,
    NotificationDialog,
    EditazioneLaboratorioModal,
    DataTable,
    Waiter
  }
})
export default class LaboratoriIndexPage extends Vue {
  $refs: any = {
    savedDialog: Vue,
    removedDialog: Vue,
    editazioneLaboratorioModal: Vue,
    confirmDeleteDialog: Vue,
    waiter: Vue
  };

  private laboratoriService: LaboratoriService;
  private laboratorio: Laboratorio;
  private idLaboratorioDaRimuovere!: number;

  public tableOptions: any = {};
  public laboratori: Laboratorio[] = [];

  constructor() {
    super();

    this.laboratoriService = new LaboratoriService();
    this.laboratorio = new Laboratorio();

  }

  public mounted() {
    this.initTable();
    this.$refs.waiter.open();
    this.laboratoriService.index().then(response => {      
      this.laboratori = response.data;
      this.$refs.waiter.close();
    });
  }

  // Evento fine generazione tabella
  public onDataLoaded() {
    $(".edit").click(event => {
      var element = $(event.currentTarget);
      var rowId = $(element).data("row-id");

      this.laboratoriService.details(rowId).then(response => {
        this.laboratorio = response.data;
        this.$refs.editazioneLaboratorioModal.openAcquirente(this.laboratorio);
      });
    });

    $(".delete").click(event => {
      var element = $(event.currentTarget);
      this.idLaboratorioDaRimuovere = $(element).data("row-id");
      this.$refs.confirmDeleteDialog.open();
    });
  }

  // nuovo acquirente
  public onAdd() {
    this.laboratorio = new Laboratorio();
    this.$refs.editazioneLaboratorioModal.open();
  }

  // rimozione acquirente
  public onRemove() {
    this.laboratoriService.delete(this.idLaboratorioDaRimuovere).then(response => {
        this.$refs.removedDialog.open();
      });
  }

  // inizializzazione tabella
  private initTable(): void {
    var options: any = {};

    options.columns = [];

    options.columns.push({ data: "Id" });
    options.columns.push({ data: "Descrizione" });


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