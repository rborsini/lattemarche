<template>
  <div
    class="modal fade bd-example-modal-lg"
    id="editazione-tipoLatte-modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="myLargeModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Dettagli tipo latte</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">
          <!--Descrizione-->
          <div class="row form-group">
            <label class="col-3">Descrizione</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="tipoLatte.Descrizione" />
            </div>
          </div>

          <!--Descrizione breve -->
          <div class="row form-group">
            <label class="col-3">Sigla</label>
            <div class="col-9">
              <input type="text" class="form-control" v-model="tipoLatte.DescrizioneBreve" />
            </div>
          </div>

          <!-- Fattore conversione -->
          <div class="row form-group">
            <label class="col-3">Fattore di conversione</label>
            <div class="col-9">
              <input
                type="number"
                step="0.001"
                class="form-control"
                v-model="tipoLatte.FattoreConversione"
              />
              <small class="form-text text-muted">Conversione da litri a kg.</small>
            </div>
          </div>

          <!-- Flag invio sitra -->
          <div class="row form-group">
            <label class="col-3"></label>
            <div class="col-9">
              <div class="form-group form-check">
                <input
                  type="checkbox"
                  class="form-check-input"
                  id="checkboxSitra"
                  v-model="tipoLatte.FlagInvioSitra"
                />
                <label class="form-check-label" for="checkboxSitra">Sincronizza con Sitra</label>
              </div>
            </div>
          </div>

                </div>
                    <div class="modal-footer">
                        <button class="btn btn-secondary mr-2" data-dismiss="modal">Annulla</button>
                        <button :disabled="tipoLatte.Descrizione === '' || tipoLatte.DescrizioneBreve === '' || tipoLatte.FattoreConversione == 0" class="btn btn-success" v-on:click="onSave()">Salva</button>
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
import { TipoLatte } from "../../models/tipoLatte.model";

import { TipiLatteService } from "../../services/tipiLatte.service";

@Component({
  components: {
    Select2
  }
})
export default class EditazioneTipoLatteModal extends Vue {
  @Prop()
  tipoLatte!: TipoLatte;

  public tipiLatteService: TipiLatteService;

        constructor() {
            super();
            this.tipiLatteService = new TipiLatteService();
        }

  mounted() {}

  public open(): void {
    $(this.$el).modal("show");
  }


        public onSave() {
            this.tipiLatteService.save(this.tipoLatte)
                .then(response => {
                    if (response.data != undefined) {
                        this.$emit("salvato");
                        this.close();
                    } else {
                        // save KO!!
                        this.tipoLatte = response.data;
                        // TODO: msg di validazione
                        //this.$emit("errore");
                        this.close();
                    }
                });
        }

  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>