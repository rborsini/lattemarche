<template>
  <div>
    <!-- Pannello modale del caricamento -->
    <waiter ref="waiter"></waiter>

    <!-- Pannelli modali di conferma azioni -->
    <notification-dialog
      ref="salvataggioDettaglioGiroModal"
      :title="'Conferma salvataggio'"
      :message="'Dettaglio giro salvato correttamente'"
    ></notification-dialog>

    <!-- Pannelli modali di conferma azioni -->
    <notification-dialog
      ref="salvataggioGiroModal"
      :title="'Conferma salvataggio'"
      :message="'Giro salvato correttamente'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- modale modifica/aggiungi giro -->
    <giro-trasportatori-modal ref="giroTrasportatoriModal" :giro="giro" v-on:salvato="reload()"></giro-trasportatori-modal>

    <div class="container pt-4">
      <!-- selezione trasportatore -->
      <div class="row form-group">
        <label class="col-sm-2 offset-1">Trasportatore</label>
        <div class="col-sm-6">
          <select2
            class="form-control"
            :options="trasportatori.Items"
            :value.sync="selectedTrasportatore"
            :value-field="'Value'"
            :text-field="'Text'"
            v-on:value-changed="onTrasportatoreSelezionato"
          />
        </div>
      </div>

      <!-- selezione giro allevamenti -->
      <div class="row form-group">
        <label class="col-sm-2 offset-1">Giro degli allevamenti</label>
        <div class="col-sm-6">
          <select2
            class="form-control"
            :options="giri.Items"
            :value.sync="selectedGiro"
            :value-field="'Value'"
            :text-field="'Text'"
            :disabled="trasportatoreSelezionato"
          />
        </div>
      </div>

      <!-- bottoni modifica/aggiungi giro -->
      <div class="row form-group">
        <div class="col-sm-6 offset-3 text-right">
          <button
            class="btn btn-primary mr-2"
            v-on:click="modificaGiro(selectedGiro)"
            :disabled="selectedGiro == 0"
          >Modifica giro</button>
          <button
            class="btn btn-success"
            v-on:click="aggiungiGiro()"
            :disabled="selectedTrasportatore == 0"
          >Aggiungi giro</button>
        </div>
      </div>

      <!-- bottone carica allevamenti trasportatore -->
      <div class="row form-group">
        <div class="col-sm-6 offset-3">
          <button
            class="btn btn-success"
            v-on:click="loadGiro(selectedGiro)"
            :disabled="selectedGiro == 0"
            style="width:100%"
          >Carica gli allevamenti in cui si reca il trasportatore</button>
        </div>
      </div>
    </div>

    <div class="container-fluid">
      <!-- tabella priorità allevamenti -->
      <div class="row form-group" v-if="giro.Items.length > 0">
        <div class="col-sm-12">
          <hr />
        </div>
        <div class="col-sm-12">
          <h3>Copertura territoriale dei trasportatori</h3>
          <p>L'indice di priorità indica il percorso seguito dal trasportatore; Inserire una sequenza numerica dei soli allevamenti segnati del check.</p>
        </div>
        <div class="col-sm-12">
          <table class="table table-striped table-bordered c-table-trasportatori">
            <thead>
              <tr>
                <th>Ragione sociale</th>
                <th>Allevatore</th>
                <th>Indirizzo allevamento</th>
                <th>Priorità</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="allevatore in giro.Items" :key="allevatore.Id">
                <td>{{allevatore.RagioneSociale}}</td>
                <td>{{allevatore.Allevatore}}</td>
                <td>{{allevatore.Indirizzo}}</td>
                <td>
                  <div class="input-group mb-3">
                    <div class="input-group-prepend">
                      <div class="input-group-text">
                        <input
                          type="checkbox"
                          v-on:change="onItemSelectedChanged($event, allevatore)"
                          v-model="allevatore.Selezionato"
                        />
                      </div>
                    </div>
                    <input
                      :disabled="allevatore.Selezionato == false"
                      type="number"
                      class="form-control"
                      v-model="allevatore.Priorita"
                    />
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <!-- bottoni annulla/salva priorità giro dei trasportatori -->
        <div class="col-sm-12 text-right">
          <button
            class="btn btn-secondary mr-2"
            v-on:click="window.location = '/trasportatori'"
          >Annulla</button>
          <button class="btn btn-success" v-on:click="salvaGiro()">Salva</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import Select2 from "../../components/select2.vue";
import Waiter from "../../components/waiter.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import GiroTrasportatoriModal from "./edit.vue";

import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { Giro, Item } from "../../models/giro.model";

import { UrlService } from "@/services/url.service";
import { GiriService } from "../../services/giri.service";
import { DropdownService } from "../../services/dropdown.service";
import { Trasportatore } from "@/models/trasportatore.model";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    openGiro(giro: Giro): void;
    close(): void;
  }
}

@Component({
  components: {
    Select2,
    Waiter,
    NotificationDialog,
    GiroTrasportatoriModal
  }
})
export default class GiriIndexPage extends Vue {
  $refs: any = {
    waiter: Vue,
    salvataggioDettaglioGiroModal: Vue,
    salvataggioGiroModal: Vue,
    giroTrasportatoriModal: Vue
  };

  public dropdownService: DropdownService;
  // public trasportatoriService: TrasportatoriService;
  public trasportatore: Trasportatore;

  public trasportatori: Dropdown = new Dropdown();
  public giri: Dropdown = new Dropdown();

  public trasportatoreSelezionato: boolean = true;
  public selectedGiro: number = 0;
  public selectedTrasportatore: number = 0;
  public giro: Giro;
  public giriService: GiriService;

  constructor() {
    super();
    this.trasportatore = new Trasportatore();
    this.trasportatore.Giri[0] = new Giro();
    this.dropdownService = new DropdownService();
    // this.trasportatoriService = new TrasportatoriService();
    this.giro = new Giro();
    this.giriService = new GiriService();
  }

  public mounted() {
    this.$refs.waiter.open();
    this.loadTrasportatori();
  }

  // caricamento trasportatori
  public loadTrasportatori() {
    this.dropdownService.getTrasportatori().then(response => {
      this.trasportatori = response.data;
      this.$refs.waiter.close();
    });
  }

  // carico allevamenti se seleziono trasportatore
  public onTrasportatoreSelezionato(idTrasportatore: string) {
    this.trasportatoreSelezionato = false;
    this.giro = new Giro();
    this.selectedGiro = 0;

    this.dropdownService.getGiri(idTrasportatore).then(response => {
      this.giri = response.data;
    });
  }

  // Selezione / Deselezione item del giro
  public onItemSelectedChanged(event: any, item: Item) {
    if (!item.Selezionato) {
      item.Priorita = undefined;
    }
  }

  // modifica giro
  public modificaGiro(id: number) {
    this.giriService.getGiroDetails(id).then(response => {
      if (response.data != null) {
        this.giro = response.data;
        this.$refs.giroTrasportatoriModal.open();
      } else {
        return null;
      }
    });
  }

  // aggiungo giro
  public aggiungiGiro() {
    this.giro = new Giro();
    this.$refs.giroTrasportatoriModal.open();
    this.giro.IdTrasportatore = this.selectedTrasportatore;
    this.giro.CodiceGiro = "";
    this.giro.Denominazione = "";
  }

  // carico allevatori
  public loadGiro(id: number) {
    this.giriService.getGiroDetails(id).then(response => {
      if (response.data != null) {
        this.giro = response.data;
        for (let i = 0; i < this.giro.Items.length; i++) {
          if (this.giro.Items[i].Priorita != null) {
            this.giro.Items[i].Selezionato = true;
          } else {
            this.giro.Items[i].Selezionato = false;
          }
        }
      }
    });
  }

  // salva giro trasportatori
  public salvaGiro() {
    this.$refs.waiter.open();
    this.giriService.save(this.giro).then(response => {
      if (response.data != undefined) {
        this.$refs.waiter.close();
        this.$refs.salvataggioGiroModal.open();
      } else {
        this.giro = response.data;
        this.$refs.salvataggioGiroModal.open();
      }
    });
  }

  public reload() {
    UrlService.reload();
  }
}
</script>