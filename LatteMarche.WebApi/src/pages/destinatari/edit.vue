<template>
    <div class="modal fade bd-example-modal-lg" id="editazione-destinatario-modal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-md" >
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Dettagli destinatario</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body pl-5 pr-5">

                    <!-- P. IVA -->
                    <div class="row form-group">
                        <label class="col-3">P. IVA</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="destinatario.P_IVA" />
                        </div>
                    </div>

                    <!-- Ragione sociale -->
                    <div class="row form-group">
                        <label class="col-3">Rag. sociale</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="destinatario.RagioneSociale" />
                        </div>
                    </div>

                    <!-- Indirizzo -->
                    <div class="row form-group">
                        <label class="col-3">Indirizzo</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="destinatario.Indirizzo" />
                        </div>
                    </div>

                    <!--  Provincia -->
                    <div class="row form-group">
                        <label class="col-3">Provincia</label>
                        <div class="col-9">
                            <select2 class="form-control"
                                     :options="opzioniProvince"
                                     :value.sync="destinatario.SiglaProvincia"
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
                                     :options="comuni"
                                     :value.sync="destinatario.IdComune"
                                     :value-field="'Id'"
                                     :text-field="'Descrizione'"
                                     v-on:value-changed="onComuneSelezionato" />
                        </div>
                    </div>

                    <!-- Stabilimento -->
                    <div class="row form-group">
                        <label class="col-3">Stabilimento</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="destinatario.Stabilimento" />
                        </div>
                    </div>

                    <!-- progress bar -->
                    <div class="row" v-if="progressBarVisible">
                        <div class="col-sm-4 offset-4 pt-2">
                            <div class="progress">
                                <div class="progress-bar progress-bar-striped progress-bar-animated"
                                     role="progressbar"
                                     aria-valuenow="100"
                                     aria-valuemin="0"
                                     aria-valuemax="100"
                                     style="width: 100%"></div>
                            </div>
                        </div>
                        <div class="col-sm-4 offset-4 text-center pt-2">
                            <h4>Elaborazione in corso...</h4>
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
    import Select2 from "../../components/common/select2.vue";

    import { Dropdown, DropdownItem } from "../../models/dropdown.model";
    import { Destinatario } from "../../models/destinatario.model";
    import { Comune } from "../../models/comune.model";

    import { DestinatariService } from "../../services/destinatari.service";
    import { ComuniService } from "../../services/comuni.service";


    @Component({
        components: {
            Select2
        }
    })

    export default class EditazioneDestinatarioModal extends Vue {

        @Prop()
        destinatario: Destinatario = new Destinatario();

        public comuni: Comune[] = [];
        public opzioniProvince: DropdownItem[] = [];

        public destinatariService: DestinatariService;
        private comuniService: ComuniService;

        public progressBarVisible = false;


        constructor() {
            super();
            this.destinatariService = new DestinatariService();
            this.comuniService = new ComuniService();
        }

        mounted() {


        }

        public open(): void {

            $(this.$el).modal('show');

            this.loadComuni(this.destinatario.SiglaProvincia);

            this.comuniService.getProvince()
                .then(response => {
                    this.opzioniProvince = response.data;
                });

            
        }

        // carica comuni
        public loadComuni(provincia: string): void {
            this.comuniService.getComuni(provincia)
                .then(response => {
                    if (response.data != null) {
                        this.comuni = response.data;
                    }
                });
        }

        // carico provincia se seleziono comune (senza aver precedentemente selezionato la provincia)
        public onComuneSelezionato(idComune: string): void {
            if (this.destinatario.SiglaProvincia == '') {
                this.comuniService.getComuneDetails(idComune)
                    .then(response => {
                        this.destinatario.SiglaProvincia = response.data.Provincia;
                    })
            }
        }

        public onSave() {
            this.progressBarVisible = true;
            this.destinatariService.update(this.destinatario)
                .then(response => {
                    if (response.data != undefined) {
                        this.$emit("salvato");
                        this.progressBarVisible = false;
                        this.close();
                    } else {
                        // save KO!!
                        this.destinatario = response.data;
                        // TODO: msg di validazione
                        //this.$emit("errore");
                        this.close();
                    }
                });
        }



        public close(): void {
            $(this.$el).modal('hide');
        }


    }

</script>