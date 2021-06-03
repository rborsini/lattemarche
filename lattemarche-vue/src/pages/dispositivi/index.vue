<template>
  <div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

		<!-- Pannello notifica rimozione -->
		<notification-dialog ref="removedDialog" :title="'Conferma rimozione'" :message="'Dispositivo rimosso correttamente'" v-on:ok="reload()"></notification-dialog>

		<!-- Pannello modale conferma eliminazione -->
		<confirm-dialog ref="confirmDeleteDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler rimuovere il dispositivo selezionato?'" v-on:confirmed="onRemove()"></confirm-dialog>


    <!-- Pannello editazione dettaglio -->
    <editazione-dispositivo-modal
      ref="editazioneDispositivoModal"
      :dispositivo="dispositivo"
      v-on:saved="onPopupSave"
    ></editazione-dispositivo-modal>

    <button id="btnPushMessage" class="d-none" v-on:click="onPushMessage" ></button>

    <!-- Tabella -->
    <data-table ref="table" :options="tableOptions" v-on:data-loaded="onDataLoaded">
      <!-- Colonne -->
      <template slot="thead">
        <th>Id</th>
        <th>Nome</th>
        <th>Marca</th>
        <th>Modello</th>
        <th>Vers. OS</th>
        <th>Trasportatore</th>
        <th>Automezzo</th>
        <th>Attivo</th>
        <th>Data reg.</th>
        <th>Data download</th>
        <th>Data upload</th>
        <th>Versione App</th>
        <th>Lat</th>
        <th>Lng</th>
        <th></th>
        <th></th>
      </template>
    </data-table>
  </div>
</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import Waiter from "../../components/waiter.vue";

import DataTable from "../../components/dataTable.vue";
import EditazioneDispositivoModal from "../dispositivi/edit.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";
import NotificationDialog from "../../components/notificationDialog.vue";

import { Dispositivo } from "../../models/dispositivo.model";
import { DispositiviService } from "../../services/dispositivi.service";
import { UrlService } from "@/services/url.service";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}


@Component({
  components: {
    DataTable,
    Waiter,
		NotificationDialog,
		ConfirmDialog,
    EditazioneDispositivoModal
  }
})
export default class DispositiviIndexPage extends Vue {
  $refs: any = {
    editazioneDispositivoModal: Vue,
    confirmDeleteDialog: Vue,
    removedDialog: Vue,
    waiter: Vue,
    table: Vue
  };

  private dispositiviService: DispositiviService;

  public tableOptions: any = {};
  public dispositivi: Dispositivo[] = [];
  public dispositivo: Dispositivo = new Dispositivo();
  public idDispositivo: string = "";

  constructor() {
    super();

    this.dispositiviService = new DispositiviService();
  }

  public mounted() {  
    this.initTable();
  }
  
  public onPushMessage() {
    this.$refs.waiter.open();
    this.$refs.table.load();
  }

	// rimozione dispositivo
	public onRemove() {
		this.dispositiviService.delete(this.idDispositivo).then((response) => {
			this.$refs.removedDialog.open();
		});
	}

  // inizializzazione tabella
  private initTable(): void {
    var options: any = {};

    options.serverSide = true;
    options.ajax = {
      url: '/api/dispositivi/search',
      type: 'POST'
    };

    options.columns = [];

    options.columns.push({ data: "Id" });
    options.columns.push({ data: "Nome" });
    options.columns.push({ data: "Marca" });
    options.columns.push({ data: "Modello" });
    options.columns.push({ data: "VersioneOS" });
    
    options.columns.push({ 
      data: "Trasportatore.RagioneSociale", 
      render: function(data: any, type: any, row: any) {
        return row.Trasportatore_RagioneSociale;
      }
    });

    options.columns.push({ 
      data: "Autocisterna.Targa", 
      render: function(data: any, type: any, row: any) {
        return row.Autocisterna_Targa;
      }
    });

    options.columns.push({ 
      data: "Attivo",
      render: function(data: any, type: any, row: any) {
        return row.Attivo ? "Si" : "No";
      }
    });

    options.columns.push({
      data: "DataRegistrazione",
      width: "90px",
      render: function(data: any, type: any, row: any) {
        return row.DataRegistrazione_Str;
      }
    });

    options.columns.push({
      data: "DataUltimoDownload",
      width: "90px",
      render: function(data: any, type: any, row: any) {
        return row.DataUltimoDownload_Str;
      }
    });

    options.columns.push({
      data: "DataUltimoUpload",
      width: "90px",
      render: function(data: any, type: any, row: any) {
        return row.DataUltimoUpload_Str;
      }
    });

    options.columns.push({ data: "VersioneApp" });
    options.columns.push({ data: "Latitudine" });
    options.columns.push({ data: "Longitudine" });

    options.columns.push({
      render: function(data: any, type: any, row: any) {
        var html = '<div class="text-center">';
        html += '<a class="edit" title="modifica" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-edit"></i></a>';
        html += "</div>";
        return html;
      },
      className: "edit-column",
      orderable: false
    });

		options.columns.push({
			render: function(data: any, type: any, row: any) {
				var html = '<div class="text-center">';
				html += '<a class="pl-3 delete" title="Elimina" style="cursor: pointer;" data-row-id="' + row.Id + '" ><i class="far fa-trash-alt"></i></a>';
				html += "</div>";
				return html;
			},
			className: "remove-column",
			orderable: false,
		});

    this.tableOptions = options;
  }

  // Evento fine generazione tabella
  public onDataLoaded() {

    this.$refs.waiter.close();

    $(".edit").click(event => {
      var element = $(event.currentTarget);
      var rowId = $(element).data("row-id");

      this.dispositiviService.details(rowId).then(response => {
        this.dispositivo = response.data;
        this.$refs.editazioneDispositivoModal.open();
      });
    });

		$(".delete").click((event) => {
			var element = $(event.currentTarget);
			this.idDispositivo = $(element).data("row-id");

			this.$refs.confirmDeleteDialog.open();
		});    
  }

  // evento chiusura popup
  public onPopupSave() {
    this.$refs.table.load();
    // window.location = window.location;
  }

  // reload della pagina sullo stesso id
  public reload() {
      UrlService.reload();
  }  
}
</script>