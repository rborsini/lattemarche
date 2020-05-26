<template>
    <div class="modal fade bd-example-modal-lg" id="editazione-allevamento-modal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" >
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Dettagli allevamento</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body pl-5 pr-5">

                    <!-- Codice ASL -->
                    <div class="row form-group">
                        <label class="col-3">Codice ASL</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="allevamento.CodiceAsl" />
                        </div>
                    </div>

                    <!-- CUAA -->
                    <div class="row form-group">
                        <label class="col-3">CUAA</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="allevamento.CUAA" />
                        </div>
                    </div>

                    <!-- Indirizzo -->
                    <div class="row form-group">
                        <label class="col-3">Indirizzo</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="allevamento.IndirizzoAllevamento" />
                        </div>
                    </div>

                    <!--  Provincia -->
                    <div class="row form-group">
                        <label class="col-3">Provincia</label>
                        <div class="col-9">
                            <select2 class="form-control"
                                     :dropdownparent="'#editazione-allevamento-modal'"
                                     :options="provincia.Items"
                                     :value.sync="allevamento.SiglaProvincia"
                                     :value-field="'Value'"
                                     :text-field="'Text'"
                                     v-on:value-changed="loadComuni" />
                        </div>
                    </div>

                    <!-- Comune -->
                    <div class="row form-group">

                        <label class="col-3">Comune</label>
                        <div class="col-9">
                            <select2 class="form-control"
                                     :dropdownparent="'#editazione-allevamento-modal'"
                                     :options="comune.Items"
                                     :value.sync="allevamento.IdComune"
                                     :value-field="'Value'"
                                     :text-field="'Text'"  />
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary mr-2" data-dismiss="modal">Annulla</button>
                    <button :disabled="allevamento.CodiceAsl === '' || allevamento.Indirizzo === '' || allevamento.SiglaProvincia === '' || allevamento.IdComune == 0" class="btn btn-primary" v-on:click="onSave()">Salva</button>
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
    import { Allevamento } from "../../models/allevamento.model";

    import { DropdownService } from "../../services/dropdown.service";


    @Component({
        components: {
            Select2
        }
    })

    export default class EditazioneAllevamentoModal extends Vue {

        @Prop()
        allevamento!: Allevamento;

        public provincia: Dropdown = new Dropdown();
        public comune: Dropdown = new Dropdown();

        public dropdownService: DropdownService = new DropdownService();        

        constructor() {
            super();
        }

        mounted() {

            this.dropdownService.getProvince()
                .then(response => {
                    this.provincia = response.data;
                });
        }

        public openAllevamento(all: Allevamento): void {
            $(this.$el).modal('show');
            this.loadComuni(all.SiglaProvincia);
        }

        public open(): void {
            $(this.$el).modal('show');
        }

        // carica comuni
        public loadComuni(provincia: string): void {
            this.dropdownService.getComuni(provincia)
                .then(response => {
                    if (response.data != null) {
                        this.comune = response.data;
                    }
                });
        }

        public onSave() {
            this.$emit("salvato");
            this.close();
        }

        public close(): void {
            $(this.$el).modal('hide');
        }

    }

</script>