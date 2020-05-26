<template>
  <div
    class="modal fade bd-example-modal-lg"
    id="editazione-cessionario-modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="myLargeModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-md">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Dettagli cessionario</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">
          <!-- P. IVA -->
          <div class="row form-group">
            <label class="col-3">P. IVA</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="cessionario.Piva" />
            </div>
          </div>

          <!-- Ragione sociale -->
          <div class="row form-group">
            <label class="col-3">Rag. sociale</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="cessionario.RagioneSociale" />
            </div>
          </div>

          <!-- Indirizzo -->
          <div class="row form-group">
            <label class="col-3">Indirizzo</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="cessionario.Indirizzo" />
            </div>
          </div>

          <!--  Provincia -->
          <div class="row form-group">
            <label class="col-3">Provincia</label>
            <div class="col-9">
              <select2
                class="form-control"
                :dropdownparent="'#editazione-cessionario-modal'"
                :options="provincia.Items"
                :value.sync="cessionario.SiglaProvincia"
                :value-field="'Value'"
                :text-field="'Text'"
                v-on:value-changed="loadComuni"
              />
            </div>
          </div>

          <!-- Comune -->
          <div class="row form-group">
            <label class="col-3">Comune</label>
            <div class="col-9">
              <select2
                class="form-control"
                :dropdownparent="'#editazione-cessionario-modal'"
                :options="comune.Items"
                :value.sync="cessionario.IdComune"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>
          </div>

        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary mr-2" data-dismiss="modal">Annulla</button>
          <button class="btn btn-primary" v-on:click="onSave()">Salva</button>
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
import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { Cessionario } from "../../models/cessionario.model";
import { Comune } from "../../models/comune.model";
import { CessionariService } from "../../services/cessionari.service";
import { ComuniService } from "../../services/comuni.service";
import { DropdownService } from "../../services/dropdown.service";

@Component({
  components: {
    Select2
  }
})
export default class EditazioneCessionarioModal extends Vue {
  @Prop()
  cessionario!: Cessionario;

  public comune: Dropdown = new Dropdown();
  public provincia: Dropdown = new Dropdown();

  public cessionariService: CessionariService;
  public dropdownService: DropdownService;

  constructor() {
    super();
    this.cessionariService = new CessionariService();
    this.dropdownService = new DropdownService();
  }

  mounted() {
    this.dropdownService.getProvince().then(response => {
        this.provincia = response.data;
    });
  }

  public openCessionario(cess: Cessionario): void {
    $(this.$el).modal("show");
    this.loadComuni(cess.SiglaProvincia);
  }

  public open(): void {
    $(this.$el).modal("show");
  }

  // carica comuni
  public loadComuni(provincia: string): void {
    this.dropdownService.getComuni(provincia).then(response => {
      if (response.data != null) {
        this.comune = response.data;
      }
    });
  }

  public onSave() {
    this.cessionariService.save(this.cessionario).then(response => {
      if (response.data != undefined) {
        this.$emit("salvato");
        this.close();
      } else {
        this.cessionario = response.data;
        this.close();
      }
    });
  }

  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>