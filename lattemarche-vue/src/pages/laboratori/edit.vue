<template>
  <div
    class="modal fade bd-example-modal-lg"
    id="editazione-laboratorio-modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="myLargeModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-md">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Dettaglio laboratorio</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">
          <!-- Id -->
          <div class="row form-group">
            <label class="col-3">Id</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="laboratorio.Id" />
            </div>
          </div>

          <!-- Descrizione -->
          <div class="row form-group">
            <label class="col-3">Descrizione</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="laboratorio.Descrizione" />
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
import { Laboratorio } from "../../models/laboratorio.model";

import { LaboratoriService } from "../../services/laboratori.service";
import { DropdownService } from "../../services/dropdown.service";

@Component({
  components: {
    Select2
  }
})
export default class EditazioneLaboratorioModal extends Vue {
  @Prop()
  laboratorio!: Laboratorio;

  public laboratoriService: LaboratoriService;
  private dropdownService: DropdownService;

  constructor() {
    super();
    this.laboratoriService = new LaboratoriService();
    this.dropdownService = new DropdownService();
  }

  public openLaboratorio(lab: Laboratorio): void {
    $(this.$el).modal("show");
  }

  public open(): void {
    $(this.$el).modal("show");
  }

  public onSave() {
    this.laboratoriService.save(this.laboratorio).then(response => {
      if (response.data != undefined) {
        this.close();
      } else {
        this.laboratorio = response.data;
        this.close();
      }
    });
  }

  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>