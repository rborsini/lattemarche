<template>
  <div class="container-fluid p-0">

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- Pannello notifica rimozione -->
    <notification-dialog
      ref="removedDialog"
      :title="'Conferma rimozione'"
      :message="'Utente rimosso correttamente'"
      v-on:ok="window.location = '/Utenti'"
    ></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog
      ref="confirmDeleteDialog"
      :title="'Conferma eliminazione'"
      :message="'Sei sicuro di voler rimuovere l\'utente selezionato?'"
      v-on:confirmed="onRemove()"
    ></confirm-dialog>

    <!-- Box ricerca -->
    <div class="jumbotron">
      <div class="row pt-1">
        <label class="col-1">Tipo profilo:</label>
        <div class="col-3">
          <select2
            class="form-control"
            :placeholder="'-'"
            :options="profilo.Items"
            :value-field="'Value'"
            :text-field="'Text'"
            :value.sync="parameters.IdProfilo"
            v-on:value-changed="onProfiloValueChanged"
          />
        </div>
      </div>
    </div>

    <!-- Tabella -->
    <data-table ref="table" :options="tableOptions" v-on:data-loaded="onDataLoaded">
      <!-- Toolbox -->
      <template slot="toolbox">
        <a class="toolbox btn btn-success float-right" href="/utenti/edit">Aggiungi</a>
      </template>

      <!-- Colonne -->
      <template slot="thead">
        <th>Ragione sociale</th>
        <th>Nome</th>
        <th>Cognome</th>
        <th>Username</th>
        <th>Profilo</th>
        <th></th>
      </template>
    </data-table>

  </div>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import Select2 from "../../components/select2.vue";
import DataTable from "../../components/dataTable.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import Waiter from "../../components/waiter.vue";

import { Utente, UtentiSearchModel } from "../../models/utente.model";
import { UtentiService } from "../../services/utenti.service";
import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { DropdownService } from "../../services/dropdown.service";

@Component({
  components: {
    Select2,
    NotificationDialog,
    ConfirmDialog,
    Waiter,
    DataTable
  }
})
export default class UtentiIndexPage extends Vue {
  $refs: any = {
    savedDialog: Vue,
    confirmDeleteDialog: Vue,
    waiter: Vue,
    removedDialog: Vue,
    table: Vue
  };

  private dropdownService: DropdownService;
  private utentiService: UtentiService;
  public utente: Utente;
  private idUtente!: number;

  public profilo: Dropdown = new Dropdown();

  public parameters: UtentiSearchModel = new UtentiSearchModel();

  public tableOptions: any = {};
  public utenti: Utente[] = [];

  constructor() {
    super();

    this.dropdownService = new DropdownService();
    this.utentiService = new UtentiService();
    this.utente = new Utente();
  }

  public mounted() {
    this.initTable();
    this.loadDropdown();

    this.$refs.waiter.open();
    // this.$refs.table.load(this.parameters.ToUrlQueryString());
  }

  // selezione profilo di filtro
  public onProfiloValueChanged(value: number) {
    this.$refs.waiter.open();
    this.$refs.table.load(this.parameters.ToUrlQueryString());
  }  

  // Evento fine generazione tabella
  public onDataLoaded() {

    this.$refs.waiter.close();

    $(".delete").click(event => {
      var element = $(event.currentTarget);
      this.idUtente = $(element).data("row-id");

      this.$refs.confirmDeleteDialog.open();
    });
  }

  // rimozione utente
  public onRemove() {
    this.utentiService.delete(this.idUtente).then(response => {
      this.$refs.removedDialog.open();
    });
  }

  // inizializzazione tabella
  private initTable(): void {
    var options: any = {};

    options.serverSide = true;
    options.ajax = {
      url: '/api/utenti/search',
      type: 'POST'
    };

    options.columns = [];

    options.columns.push({ data: "RagioneSociale" });
    options.columns.push({ data: "Nome" });
    options.columns.push({ data: "Cognome" });
    options.columns.push({ data: "Username" });
    options.columns.push({ data: "Profilo", orderable: false });


      options.columns.push({
        render: function(data: any, type: any, row: any) {
          var html = '<div class="text-center">';

            html +=
              '<a class="edit" title="Modifica" href="/utenti/edit?id=' +
              row.Id +
              '" ><i class="far fa-edit"></i></a>';

            html +=
              '<a class="pl-3 delete" title="Elimina" style="cursor: pointer;" data-row-id="' +
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

  // caricamento dropdown
  private loadDropdown(){

    this.dropdownService.getProfili().then(response => {
      this.profilo = response.data;
    });

  }

}
</script>