<template>
  <div
    class="modal fade bd-example-modal-lg"
    id="editazione-prelievo-modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="myLargeModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-lg" style="max-width:90%">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Dettagli prelievo</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">
          <!-- data/ora prelievo -->
          <div class="row form-group">
            <label class="col-2">Data prelievo</label>
            <div class="col-sm-4">
              <datepicker :value.sync="prelievoLatte.DataPrelievoStr" />
              <div class="invalid-feedback">Inserire data prelievo.</div>
            </div>
            <label class="col-2">Ora prelievo</label>
            <div class="col-sm-4">
              <time-editor v-model="prelievoLatte.OraPrelievo"></time-editor>
            </div>
          </div>

          <!-- data/ora ultima mungitura -->
          <div class="row form-group">
            <label class="col-2">Data ultima mungitura</label>
            <div class="col-sm-4">
              <datepicker :value.sync="prelievoLatte.DataUltimaMungituraStr" />
              <div class="invalid-feedback">Inserire data ultima mungitura.</div>
            </div>
            <label class="col-2">Ora ultima mungitura</label>
            <div class="col-sm-4">
              <time-editor v-model="prelievoLatte.OraUltimaMungitura"></time-editor>
            </div>
          </div>

          <!-- data/ora consegna -->
          <div class="row form-group">
            <label class="col-2">Data consegna</label>
            <div class="col-sm-4">
              <datepicker :value.sync="prelievoLatte.DataConsegnaStr" />
              <div class="invalid-feedback">Inserire data consegna.</div>
            </div>
            <label class="col-2">Ora consegna</label>
            <div class="col-sm-4">
              <time-editor v-model="prelievoLatte.OraConsegna"></time-editor>
            </div>
          </div>

          <!-- numero mungiture / quantità in kg -->
          <div class="row form-group">
            <label class="col-2">Numero mungiture</label>
            <div class="col-sm-4">
              <input
                type="number"
                min="0"
                class="form-control"
                v-model="prelievoLatte.NumeroMungiture"
              />
            </div>
            <label class="col-2">Quantità in Kg</label>
            <div class="col-sm-4">
              <input type="number" min="0" class="form-control" v-model="prelievoLatte.Quantita" />
            </div>
          </div>

          <!-- temperatura C° / trasportatore -->
          <div class="row form-group">
            <label class="col-2">Temperatura in C°</label>
            <div class="col-sm-4">
              <input
                type="number"
                min="-20"
                class="form-control"
                v-model="prelievoLatte.Temperatura"
              />
            </div>
            <label class="col-2">Trasportatore</label>
            <div class="col-sm-4">
              <select2
                class="form-control"
                :dropdownparent="'#editazione-prelievo-modal'"
                :options="trasportatore.Items"
                :value.sync="prelievoLatte.IdTrasportatore"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>
          </div>

          <!-- acquirente / destinatario -->
          <div class="row form-group">
            <label class="col-2">Acquirente</label>
            <div class="col-sm-4">
              <select2
                class="form-control"
                :placeholder="'-'"
                :options="acquirente.Items"
                :value.sync="prelievoLatte.IdAcquirente"
                :value-field="'Value'"
                :text-field="'Text'"
              />

            </div>
            <label class="col-2">Destinatario</label>
            <div class="col-sm-4">
              <select2
                class="form-control"
                :dropdownparent="'#editazione-prelievo-modal'"
                :options="destinatario.Items"
                :value.sync="prelievoLatte.IdDestinatario"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>
          </div>

          <!-- laboratorio analisi / seriale laboratorio -->
          <div class="row form-group">
            <label class="col-2">Laboratorio analisi</label>
            <div class="col-sm-4">
              <select2
                class="form-control"
                :dropdownparent="'#editazione-prelievo-modal'"
                :options="laboratoriAnalisi.Items"
                :value.sync="prelievoLatte.IdLabAnalisi"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>
            <label class="col-2">Seriale laboratorio</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="prelievoLatte.SerialeLabAnalisi" />
            </div>
          </div>

          <!-- scomparto / lotto di consegna -->
          <div class="row form-group">
            <label class="col-2">Scomparto</label>
            <div class="col-sm-4">
              <input type="number" min="0" class="form-control" v-model="prelievoLatte.Scomparto" />
            </div>
            <label class="col-2">Lotto di consegna</label>
            <div class="col-sm-4">
              <input type="text" class="form-control" v-model="prelievoLatte.LottoConsegna" />
            </div>
          </div>

          <!-- progress bar -->
          <div class="row" v-if="progressBarSalvaPrelievo">
            <div class="col-sm-4 offset-4 pt-2">
              <div class="progress">
                <div
                  class="progress-bar progress-bar-striped progress-bar-animated"
                  role="progressbar"
                  aria-valuenow="100"
                  aria-valuemin="0"
                  aria-valuemax="100"
                  style="width: 100%"
                ></div>
              </div>
            </div>
            <div class="col-sm-4 offset-4 text-center pt-2">
              <h4>Elaborazione in corso...</h4>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary mr-2" data-dismiss="modal">Annulla</button>
          <button class="btn btn-primary" v-on:click="salvaDettaglioPrelievo()">Salva</button>
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
import Datepicker from "../../components/datepicker.vue";
import TimeEditor from "../../components/timeEditor.vue";

import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { PrelievoLatte } from "../../models/prelievoLatte.model";
import { LaboratorioAnalisi } from "../../models/laboratorioAnalisi.model";

import { PrelieviLatteService } from "../../services/prelieviLatte.service";
import { DropdownService } from "../../services/dropdown.service";

@Component({
  components: {
    Select2,
    Datepicker,
    TimeEditor
  }
})
export default class EditazionePrelievoModal extends Vue {
  @Prop()
  prelievoLatte!: PrelievoLatte;

  public prelieviLatteService: PrelieviLatteService;
  public dropdownService: DropdownService;
  public laboratoriAnalisi: Dropdown = new Dropdown();
  public trasportatore: Dropdown = new Dropdown();
  public destinatario: Dropdown = new Dropdown();
  public acquirente: Dropdown = new Dropdown();

  public id: string;
  public progressBarSalvaPrelievo = false;

  constructor() {
    super();
    this.prelieviLatteService = new PrelieviLatteService();
    this.dropdownService = new DropdownService();
    this.id = $("#id").val() as string;
  }

  mounted() {
    this.loadLaboratoriAnalisi();
    this.loadTrasportatori();
    this.loadDestinatari();
    this.loadAcquirenti();
  }

  // caricamento laboratori analisi
  public async  loadLaboratoriAnalisi() {
      const dd = await this.dropdownService.getLaboratori();

    if (dd.data != null) {
      this.laboratoriAnalisi = dd.data;
    }

  }

  // caricamento trasportatori
  public async loadTrasportatori() {
      const dd = await this.dropdownService.getTrasportatori();
    if (dd.data != null) {
      this.trasportatore = dd.data;
    }

  }

  // caricamento destinatari
  public async loadDestinatari() {
      const dd = await this.dropdownService.getDestinatari();

    if (dd.data != null) {
      this.destinatario = dd.data;
    }

  }

  // caricamento acquirenti
  public async loadAcquirenti() {
    const dd = await this.dropdownService.getAcquirenti();

    if (dd.data != null) {
      this.acquirente = dd.data;
    }

  }

  public salvaDettaglioPrelievo() {
    this.progressBarSalvaPrelievo = true;
    this.prelieviLatteService.save(this.prelievoLatte).then(response => {
      if (response.data != undefined) {
        this.$emit("salvato");
        this.progressBarSalvaPrelievo = false;
        this.close();
      } else {
        // save KO!!
        this.prelievoLatte = response.data;
        // TODO: msg di validazione
        //this.$emit("errore");
        this.close();
      }
    });
  }

  public open(): void {
    $(this.$el).modal("show");
  }

  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>