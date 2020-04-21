<template>
    <div class="modal fade bd-example-modal-lg" id="editazione-dispositivo-modal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Dispositivo mobile</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body pl-5 pr-5">

                    <!-- IMEI -->
                    <div class="row form-group">
                        <label class="col-3">IMEI</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="dispositivo.Id" />
                        </div>
                    </div>

                    <!-- Attivo -->
                    <div class="row form-group">
                        <label class="col-3">Attivo</label>
                        <div class="col-9">
                            <div class="form-group form-check">
                                <input type="checkbox" class="form-check-input" v-model="attivo">
                            </div>
                        </div>
                    </div>

                    <!-- Trasportatore --> 
                    <div class="row form-group">
                        <label class="col-3">Trasportatore</label>
                        <div class="col-9">
                            <div class="form-group ">
                                <select2 class="form-control"
                                        :options="trasportatori"
                                        :value.sync="idTrasportatoreSelezionato"
                                        :value-field="'Id'"
                                        :text-field="'NomeCompleto'" />                                
                            </div>
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
    import { Dispositivo } from "../../models/dispositivo.model";
    import { DispositiviService } from "../../services/dispositivi.service";
    import { TrasportatoriService } from "../../services/trasportatori.service";
    import { Trasportatore } from "../../models/trasportatore.model";


    @Component({
        components: {
            Select2
        }
    })

    export default class EditazioneDispositivoModal extends Vue {

        @Prop()
        dispositivo: Dispositivo = new Dispositivo();

        public trasportatori: Trasportatore[] = [];
        public attivo: boolean = false;
        public idTrasportatoreSelezionato: number = 0;

        public dispositiviService: DispositiviService;
        public trasportatoriService: TrasportatoriService;    

        constructor() {
            super();
            this.dispositiviService = new DispositiviService();
            this.trasportatoriService = new TrasportatoriService();
        }

        @Watch('dispositivo')
        onDispositivoChanged(){
            this.attivo = this.dispositivo.Attivo;
            this.idTrasportatoreSelezionato = this.dispositivo.IdTrasportatore;
        }

        mounted() {
            this.trasportatoriService.getTrasportatori()
                .then(response => {
                    this.trasportatori = response.data;
                });
        }

        public open(): void {
            $(this.$el).modal('show');
        }

        public onSave() {

            this.dispositivo.Attivo = this.attivo;
            this.dispositivo.IdTrasportatore = this.idTrasportatoreSelezionato;

            this.dispositiviService.update(this.dispositivo)
                .then(response => {
                    this.$emit("saved");
                    this.close();
                });
        }

        public close(): void {
            $(this.$el).modal('hide');
        }

    }

</script>