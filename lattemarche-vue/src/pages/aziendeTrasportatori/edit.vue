<template>
  <div
    class="modal fade bd-example-modal-lg"
    id="editazione-trasportatore-modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="myLargeModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-md">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Dettagli azienda trasportatore</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">
          <!-- P. IVA -->
          <div class="row form-group">
            <label class="col-3">P. IVA</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="trasportatore.Piva" />
            </div>
          </div>

          <!-- Ragione sociale -->
          <div class="row form-group">
            <label class="col-3">Rag. sociale</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="trasportatore.RagioneSociale" />
            </div>
          </div>

          <!-- Nome -->
          <div class="row form-group">
            <label class="col-3">Nome titolare</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="trasportatore.NomeTitolare" />
            </div>
          </div>

          <!-- Cognome titolare -->
          <div class="row form-group">
            <label class="col-3">Cognome titolare</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="trasportatore.CognomeTitolare" />
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
import { Acquirente } from "../../models/acquirente.model";

import { TrasportatoriService } from "../../services/trasportatori.service";
import { AziendaTrasportatore } from '../../models/aziendaTrasportatore.model';

@Component({
  components: {
    Select2
  }
})
export default class EditazioneAziendaTrasportatoreModal extends Vue {
  @Prop()
  aziendaTrasportatore!: AziendaTrasportatore;

  public trasportatoriService: TrasportatoriService;

  constructor() {
    super();
    this.trasportatoriService = new TrasportatoriService();
  }

  public openTrasportatore(): void {
    $(this.$el).modal("show");
  }

  public open(): void {
    $(this.$el).modal("show");
  }

  public onSave() {
    this.trasportatoriService.save(this.aziendaTrasportatore).then(response => {
      if (response.data != undefined) {
        this.close();
      } else {
        this.aziendaTrasportatore = response.data;
        this.close();
      }
    });
  }

  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>