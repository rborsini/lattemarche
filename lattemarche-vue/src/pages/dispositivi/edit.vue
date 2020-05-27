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

                    <!-- Id -->
                    <div class="row form-group">
                        <label class="col-3">Id</label>
                        <div class="col-9">
                            <input disabled type="text" class="form-control" v-model="dispositivo.Id" />
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

                    <!-- Nome -->
                    <div class="row form-group">
                        <label class="col-3">Nome</label>
                        <div class="col-9">
                            <input type="text" class="form-control" v-model="dispositivo.Nome" />
                        </div>
                    </div>

                    <!-- Trasportatore --> 
                    <div class="row form-group">
                        <label class="col-3">Trasportatore</label>
                        <div class="col-9">
                            <select2 class="form-control"
                                    :options="trasportatori.Items"
                                    :value.sync="idTrasportatoreSelezionato"
                                    :value-field="'Value'"
                                    :text-field="'Text'" 
                                    v-on:value-changed="onTrasporatoreChanged()"
                                    />
                        </div>                        
                    </div>

                    <!-- Automezzo -->
                    <div class="row form-group">
                        <label class="col-3">Automezzo</label>
                        <div class="col-9">
                            <select2 class="form-control"
                                    :options="autocisterne.Items"
                                    :value.sync="idAutocisternaSelezionata"
                                    :value-field="'Value'"
                                    :text-field="'Text'" />
                        </div>                        
                    </div>       

                    <!-- Marca -->
                    <div class="row form-group">
                        <label class="col-3">Marca</label>
                        <div class="col-9">
                            <input disabled type="text" class="form-control" v-model="dispositivo.Marca" />
                        </div>
                    </div>

                    <!-- Modello -->
                    <div class="row form-group">
                        <label class="col-3">Modello</label>
                        <div class="col-9">
                            <input disabled type="text" class="form-control" v-model="dispositivo.Modello" />
                        </div>
                    </div>

                    <!-- Versione OS -->
                    <div class="row form-group">
                        <label class="col-3">Versione OS</label>
                        <div class="col-9">
                            <input disabled type="text" class="form-control" v-model="dispositivo.VersioneOS" />
                        </div>
                    </div>

                    <!-- Versione App -->
                    <div class="row form-group">
                        <label class="col-3">Versione App</label>
                        <div class="col-9">
                            <input disabled type="text" class="form-control" v-model="dispositivo.VersioneApp" />
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
    import { Dispositivo } from "../../models/dispositivo.model";
    import { DispositiviService } from "../../services/dispositivi.service";
    import { Trasportatore } from "../../models/trasportatore.model";
    import { DropdownService} from "../../services/dropdown.service";


    @Component({
        components: {
            Select2
        }
    })

    export default class EditazioneDispositivoModal extends Vue {

        @Prop()
        dispositivo!: Dispositivo;

        public trasportatori: Dropdown = new Dropdown();
        public autocisterne: Dropdown = new Dropdown();
        public attivo: boolean = false;
        public idTrasportatoreSelezionato: number = 0;
        public idAutocisternaSelezionata: number = 0;

        public dispositiviService: DispositiviService;
        public dropdownService: DropdownService;

        constructor() {
            super();
            this.dispositiviService = new DispositiviService();
            this.dropdownService = new DropdownService();
        }

        @Watch('dispositivo')
        onDispositivoChanged(){
            this.attivo = this.dispositivo.Attivo;
            this.idTrasportatoreSelezionato = this.dispositivo.IdTrasportatore;
            this.idAutocisternaSelezionata = this.dispositivo.IdAutocisterna;
            this.loadAutocisterne(this.dispositivo.IdTrasportatore);
        }

        mounted() {
            this.dropdownService.getTrasportatori()
                .then(response => {
                    this.trasportatori = response.data;
                });
        }

        public open(): void {
            $(this.$el).modal('show');
        }

        public onTrasporatoreChanged() {
            this.loadAutocisterne(this.idTrasportatoreSelezionato);
        }

        private loadAutocisterne(idTrasportatore?: number) {

            if(idTrasportatore) {
                this.dropdownService.getAutocisterne(idTrasportatore)
                    .then(response => {
                        this.autocisterne = response.data;
                    });
            }

        }

        public onSave() {

            this.dispositivo.Attivo = this.attivo;
            this.dispositivo.IdTrasportatore = this.idTrasportatoreSelezionato;
            this.dispositivo.IdAutocisterna = this.idAutocisternaSelezionata;

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