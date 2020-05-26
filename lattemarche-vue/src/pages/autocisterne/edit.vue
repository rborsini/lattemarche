<template>
  <div
    class="modal fade bd-example-modal-lg"
    id="editazione-autocisterna-modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="myLargeModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Dettagli autocisterna</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">
          <!-- Marca -->
          <div class="row form-group">
            <label class="col-3">Marca</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="autocisterna.Marca" />
            </div>
          </div>

          <!-- Modello -->
          <div class="row form-group">
            <label class="col-3">Modello</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="autocisterna.Modello" />
            </div>
          </div>

          <!-- Targa -->
          <div class="row form-group">
            <label class="col-3">Targa</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="autocisterna.Targa" />
            </div>
          </div>

          <!--  Autotrasportatore -->
          <div class="row form-group">
            <label class="col-3">Autotrasportatore</label>
            <div class="col-9">
              <select2
                class="form-control"
                :dropdownparent="'#editazione-autocisterna-modal'"
                :options="trasportatori.Items"
                :value.sync="autocisterna.IdTrasportatore"
                :value-field="'Value'"
                :text-field="'Text'"
              />
            </div>
          </div>

          <!-- Portata -->
          <div class="row form-group">
            <label class="col-3">Portata</label>
            <div class="col-9">
              <input type="number" class="form-control" v-model="autocisterna.Portata" />
            </div>
          </div>

          <!-- Num scomparti -->
          <div class="row form-group">
            <label class="col-3">Num. scomparti</label>
            <div class="col-9">
              <input type="number" class="form-control" v-model="autocisterna.NumScomparti" />
            </div>
          </div>

        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary mr-2" data-dismiss="modal">Annulla</button>
          <button :disabled="autocisterna.Marca === '' || autocisterna.Modello === '' ||  autocisterna.Targa === '' || 
                   autocisterna.IdTrasportatore == 0 || autocisterna.Portata == 0 || autocisterna.NumScomparti == 0" class="btn btn-primary" v-on:click="onSave()">Salva</button>
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
import { Autocisterna } from "../../models/autocisterna.model";
import { Trasportatore } from "../../models/trasportatore.model";

import { AutocisterneService } from "../../services/autocisterne.service";
import { TrasportatoriService } from "../../services/trasportatori.service";
import { DropdownService } from "../../services/dropdown.service";

@Component({
  components: {
    Select2
  }
})
export default class EditazioneAutocisternaModal extends Vue {
  @Prop()
  autocisterna!: Autocisterna;

  public trasportatori: Dropdown = new Dropdown();

  public autocisterneService: AutocisterneService;
  private dropdownService: DropdownService;

  constructor() {
    super();
    this.autocisterneService = new AutocisterneService();
    this.dropdownService = new DropdownService();
  }

  mounted() {
    this.dropdownService.getTrasportatori().then(response => {
      this.trasportatori = response.data;
    });
  }

  public open(): void {
    $(this.$el).modal("show");
  }

  public onSave() {

    this.autocisterneService.save(this.autocisterna).then(response => {
      if (response.data != undefined) {
        this.$emit("salvato");
        this.close();
      } else {
        this.autocisterna = response.data;
        this.close();
      }
    });
  }

  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>