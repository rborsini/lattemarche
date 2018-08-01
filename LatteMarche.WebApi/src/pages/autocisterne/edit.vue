<template>
    <div class="modal fade bd-example-modal-lg" id="editazione-autocisterna-modal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" >
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
                            <select2 class="form-control"
                                     :options="trasportatori"
                                     :value.sync="autocisterna.IdTrasportatore"
                                     :value-field="'Id'"
                                     :text-field="'Cognome'" />
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
    import { Autocisterna } from "../../models/autocisterna.model";
    import { Trasportatore } from "../../models/trasportatore.model";

    import { AutocisterneService } from "../../services/autocisterne.service";
    import { TrasportatoriService } from "../../services/trasportatori.service";


    @Component({
        components: {
            Select2
        }
    })

    export default class EditazioneDestinatarioModal extends Vue {

        @Prop()
        autocisterna: Autocisterna = new Autocisterna();

        public trasportatori: Trasportatore[] = [];

        public autocisterneService: AutocisterneService;
        public trasportatoriService: TrasportatoriService;

        public progressBarVisible = false;


        constructor() {
            super();
            this.autocisterneService = new AutocisterneService();
            this.trasportatoriService = new TrasportatoriService();
        }

        mounted() {


        }

        public open(): void {

            $(this.$el).modal('show');

            this.trasportatoriService.getTrasportatori()
                .then(response => {
                    this.trasportatori = response.data;
                });

            
        }


        public onSave() {
            this.progressBarVisible = true;
            this.autocisterneService.update(this.autocisterna)
                .then(response => {
                    if (response.data != undefined) {
                        this.$emit("salvato");
                        this.progressBarVisible = false;
                        this.close();
                    } else {
                        // save KO!!
                        this.autocisterna = response.data;
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