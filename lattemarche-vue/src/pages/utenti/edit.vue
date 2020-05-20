<template>
    <div class="modal fade bd-example-modal-lg" id="editazione-utente-modal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" >
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Dettagli utente</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body pl-5 pr-5">

                    <!-- ragione sociale / username -->
                    <div class="row form-group">
                        <label class="col-sm-2">Ragione sociale</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.RagioneSociale" />
                        </div>
                        <label class="col-sm-2">Username</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.Username" />
                        </div>
                    </div>

                    <!-- nome / cognome -->
                    <div class="row form-group">
                        <label class="col-sm-2">Nome</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.Nome" />
                        </div>
                        <label class="col-sm-2">Cognome</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.Cognome" />
                        </div>
                    </div>

                    <!-- sesso / p.iva/cf -->
                    <div class="row form-group">
                        <label class="col-sm-2">Sesso</label>
                        <div class="col-sm-4">
                            <select2 class="form-control"
                                     :placeholder="'-'"
                                     :options="opzioniSesso"
                                     :value.sync="utente.Sesso"
                                     :value-field="'Value'"
                                     :text-field="'Text'" />
                        </div>
                        <label class="col-sm-2">P. Iva / C.F.</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.PivaCF" />
                        </div>
                    </div>

                    <!-- indirizzo / provincia / città -->
                    <div class="row form-group">
                        <label class="col-sm-2">Indirizzo</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.Indirizzo" />
                        </div>

                        <label class="col-sm-2">Provincia</label>
                        <div class="col-sm-1">
                            <select2 class="form-control"
                                     :options="opzioniProvince"
                                     :value.sync="utente.SiglaProvincia"
                                     :value-field="'Value'"
                                     :text-field="'Text'"
                                     v-on:value-changed="loadComuni" />
                        </div>
                        <div class="col-sm-3">
                            <select2 class="form-control"
                                     :options="comuni"
                                     :value.sync="utente.IdComune"
                                     :value-field="'Id'"
                                     :text-field="'Descrizione'"
                                     v-on:value-changed="onComuneSelezionato" />
                        </div>
                    </div>

                    <!-- telefono / cellulare -->
                    <div class="row form-group">
                        <label class="col-sm-2">Telefono</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.Telefono" />
                        </div>
                        <label class="col-sm-2">Cellulare</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.Cellulare" />
                        </div>
                    </div>

                    <!-- codice allevatore / tipo di latte -->
                    <div class="row form-group">
                        <label class="col-sm-2">Codice allevatore</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.CodiceAllevatore" />
                        </div>
                        <label class="col-sm-2">Tipo latte</label>
                        <div class="col-sm-4">
                            <select2 class="form-control"
                                     :options="tipiLatte"
                                     :value.sync="utente.IdTipoLatte"
                                     :value-field="'Id'"
                                     :text-field="'Descrizione'" />
                        </div>
                    </div>

                    <!-- quota del latte / comunicazione quota del latte -->
                    <div class="row form-group">
                        <label class="col-sm-2">Quota latte(Kg)</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.QuantitaLatte" />
                        </div>
                        <label class="col-sm-2">Quota latte N°</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="utente.NumeroComunicazione" />
                        </div>
                    </div>

                    <!-- tipo di profilo / comunicazione quota del latte -->
                    <div class="row form-group">
                        <label class="col-sm-2">Tipo profilo</label>
                        <div class="col-sm-4">
                            <select2 class="form-control"
                                     :options="ruoli"
                                     :value.sync="utente.IdProfilo"
                                     :value-field="'Id'"
                                     :text-field="'Descrizione'" />
                        </div>
                        <label class="col-sm-2">Abilitato</label>
                        <div class="col-sm-1">
                            <select2 class="form-control"
                                     :placeholder="'-'"
                                     :options="opzioniAbilitato"
                                     :value.sync="utente.Abilitato"
                                     :value-field="'Value'"
                                     :text-field="'Text'" />
                        </div>
                        <label class="col-sm-2">Visibile</label>
                        <div class="col-sm-1">
                            <select2 class="form-control"
                                     :placeholder="'-'"
                                     :options="opzioniVisibile"
                                     :value.sync="utente.Visibile"
                                     :value-field="'Value'"
                                     :text-field="'Text'" />
                        </div>
                    </div>

                    <!-- note -->
                    <div class="row form-group">
                        <label class="col-sm-2">Note</label>
                        <div class="col-sm-10">
                            <textarea class="form-control" v-model="utente.Note" rows="3"></textarea>
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
    import Select2 from "../../components/select2.vue";

    import { Dropdown, DropdownItem } from "../../models/dropdown.model";
    import { Utente } from "../../models/utente.model";
    import { TipoLatte } from "../../models/tipoLatte.model";
    import { Comune } from "../../models/comune.model";
    import { Ruolo } from "../../models/ruolo.model";

    import { UtentiService } from "../../services/utenti.service";
    import { TipiLatteService } from "../../services/tipiLatte.service";
    import { ComuniService } from "../../services/comuni.service";
    import { RuoliService } from "../../services/ruoli.service";


    @Component({
        components: {
            Select2
        }
    })

    export default class EditazioneUtenteModal extends Vue {

        @Prop()
        utente: Utente = new Utente();

        public tipiLatte: TipoLatte[] = [];
        public comune: Comune;
        public ruoli: Ruolo[] = [];

        public opzioniSesso: DropdownItem[] = [];
        public opzioniAbilitato: DropdownItem[] = [];
        public opzioniVisibile: DropdownItem[] = [];
        public comuni: Comune[] = [];
        public opzioniProvince: DropdownItem[] = [];

        private comuniService: ComuniService;
        private tipiLatteService: TipiLatteService;
        private utentiService: UtentiService;
        private ruoliService: RuoliService;

        public progressBarVisible = false;


        constructor() {
            super();

            this.comune = new Comune;
            this.utente = new Utente();
            this.comuniService = new ComuniService();
            this.tipiLatteService = new TipiLatteService();
            this.utentiService = new UtentiService();
            this.ruoliService = new RuoliService();
        }

        mounted() {

            this.comuniService.getProvince()
                .then(response => {
                    this.opzioniProvince = response.data;
                });
            this.opzioniSesso = this.getOpzioniSessoUtente();
            this.opzioniAbilitato = this.getOpzioniAbilitato();
            this.opzioniVisibile = this.getOpzioniAbilitato();
            this.loadTipiLatte();
            this.loadProfili();

        }

        public openUtente(utente: Utente): void {

            $(this.$el).modal('show');

            this.loadComuni(utente.SiglaProvincia);
        }

        public open(): void {
            $(this.$el).modal('show');            
        }

        // carica dropdown sesso
        public getOpzioniSessoUtente(): DropdownItem[] {
            let opzioniSesso: DropdownItem[] = [];
            opzioniSesso.push(new DropdownItem("M", "Maschio"));
            opzioniSesso.push(new DropdownItem("F", "Femmina"));
            return opzioniSesso;
        }

        // carica opzioni abilitato
        public getOpzioniAbilitato(): DropdownItem[] {
            let opzioniAbilitato: DropdownItem[] = [];
            opzioniAbilitato.push(new DropdownItem("true", "Si"));
            opzioniAbilitato.push(new DropdownItem("false", "No"));
            return opzioniAbilitato;
        }

        // carica opzioni visibile
        public getOpzioniVisibile(): DropdownItem[] {
            let opzioniVisibile: DropdownItem[] = [];
            opzioniVisibile.push(new DropdownItem("true", "Si"));
            opzioniVisibile.push(new DropdownItem("false", "No"));
            return opzioniVisibile;
        }

        // caricamento tipi latte
        private loadTipiLatte() {
            this.tipiLatteService.index()
                .then(response => {
                    if (response.data != null) {
                        this.tipiLatte = response.data;
                    }
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

        // carica tipi profilo
        public loadProfili(): void {
            this.ruoliService.getRuoli()
                .then(response => {
                    if (response.data != null) {
                        this.ruoli = response.data;
                    }
                });
        }

        // carico provincia se seleziono comune (senza aver precedentemente selezionato la provincia)
        public onComuneSelezionato(idComune: string): void {
            if (this.utente.SiglaProvincia == '') {
                this.comuniService.getComuneDetails(idComune)
                    .then(response => {
                        this.utente.SiglaProvincia = response.data.Provincia;
                    })
            }
        }

        public onSave() {
            this.progressBarVisible = true;
            this.utentiService.save(this.utente)
                .then(response => {
                    if (response.data != undefined) {
                        this.$emit("salvato");
                        this.progressBarVisible = false;
                        this.close();
                    } else {
                        // save KO!!
                        this.utente = response.data;
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