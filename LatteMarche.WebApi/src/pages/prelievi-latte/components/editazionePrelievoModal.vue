<template>
    <div class="modal fade bd-example-modal-lg" id="editazione-prelievo-modal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" style="max-width:90%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Dettaglio prelievo</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body pl-5 pr-5">
                    <!-- data/ora prelievo -->
                    <div class="row form-group">
                        <label class="col-2">Data prelievo</label>
                        <div class="col-sm-4">
                            <datepicker :value.sync="prelievoLatte.DataPrelievoStr" />
                        </div>
                        <label class="col-2">Ora prelievo</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
                        </div>
                    </div>
                    <!-- data/ora ultima mungitura -->
                    <div class="row form-group">
                        <label class="col-2">Data ultima mungitura</label>
                        <div class="col-sm-4">
                            <datepicker :value.sync="prelievoLatte.DataUltimaMungituraStr" />
                        </div>
                        <label class="col-2">Ora ultima mungitura</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
                        </div>
                    </div>
                    <!-- data/ora consegna -->
                    <div class="row form-group">
                        <label class="col-2">Data consegna</label>
                        <div class="col-sm-4">
                            <datepicker :value.sync="prelievoLatte.DataConsegnaStr" />
                        </div>
                        <label class="col-2">Ora consegna</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
                        </div>
                    </div>
                    <!-- numero mungiture / quantità in kg -->
                    <div class="row form-group">
                        <label class="col-2">Numero mungiture</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control" v-model="prelievoLatte.NumeroMungiture">
                        </div>
                        <label class="col-2">Quantità in Kg</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control" v-model="prelievoLatte.Quantita">
                        </div>
                    </div>
                    <!-- temperatura C° / trasportatore -->
                    <div class="row form-group">
                        <label class="col-2">Temperatura in C°</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control" v-model="prelievoLatte.Temperatura">
                        </div>
                        <label class="col-2">Trasportatore</label>
                        <div class="col-sm-4">
                            <select2 class="form-control"
                                     :dropdownparent="'#editazione-prelievo-modal'"
                                     :options="trasportatore"
                                     :value.sync="prelievoLatte.IdTrasportatore"
                                     :value-field="'Id'"
                                     :text-field="'Cognome'" />
                        </div>
                    </div>
                    <!-- acquirente / destinatario -->
                    <div class="row form-group">
                        <label class="col-2">Acquirente</label>
                        <div class="col-sm-4">
                            <select2 class="form-control"
                                     :dropdownparent="'#editazione-prelievo-modal'"
                                     :options="acquirente"
                                     :value.sync="prelievoLatte.IdAcquirente"
                                     :value-field="'Id'"
                                     :text-field="'RagioneSociale'" />
                        </div>
                        <label class="col-2">Destinatario</label>
                        <div class="col-sm-4">
                            <select2 class="form-control"
                                     :dropdownparent="'#editazione-prelievo-modal'"
                                     :options="destinatario"
                                     :value.sync="prelievoLatte.IdDestinatario"
                                     :value-field="'Id'"
                                     :text-field="'RagioneSociale'" />
                        </div>
                    </div>
                    <!-- laboratorio analisi / seriale laboratorio -->
                    <div class="row form-group">
                        <label class="col-2">Laboratorio analisi</label>
                        <div class="col-sm-4">
                            <select2 class="form-control"
                                     :dropdownparent="'#editazione-prelievo-modal'"
                                     :options="laboratorioAnalisi"
                                     :value.sync="prelievoLatte.IdLabAnalisi"
                                     :value-field="'Id'"
                                     :text-field="'Descrizione'" />
                        </div>
                        <label class="col-2">Seriale laboratorio</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="prelievoLatte.SerialeLabAnalisi">
                        </div>
                    </div>
                    <!-- scomparto / lotto di consegna -->
                    <div class="row form-group">
                        <label class="col-2">Scomparto</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control" v-model="prelievoLatte.Scomparto">
                        </div>
                        <label class="col-2">Lotto di consegna</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" v-model="prelievoLatte.LottoConsegna">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary mr-2" data-dismiss="modal">Annulla</button>
                    <button class="btn btn-success">Salva</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">

    import Vue from "vue";
    import Component from "vue-class-component";
    import { Prop, Watch, Emit } from "vue-property-decorator";
    import Select2 from "../../../components/common/select2.vue";
    import Datepicker from "../../../components/common/datepicker.vue";

    import { Dropdown, DropdownItem } from "../../../models/dropdown.model";
    import { PrelievoLatte } from "../../../models/prelievoLatte.model";
    import { LaboratorioAnalisi } from "../../../models/laboratorioAnalisi.model";
    import { Trasportatore } from "../../../models/trasportatore.model";
    import { Acquirente } from "../../../models/acquirente.model";
    import { Destinatario } from "../../../models/destinatario.model";

    import { PrelieviLatteService } from "../../../services/prelieviLatte.service";
    import { TrasportatoriService } from "../../../services/trasportatori.service";
    import { AcquirentiService } from "../../../services/acquirenti.service";
    import { DestinatariService } from "../../../services/destinatari.service";
    

    @Component({
        components: {
            Select2,
            Datepicker
        }
    })

    export default class EditazionePrelievoModal extends Vue {

        @Prop()
        prelievoLatte: PrelievoLatte = new PrelievoLatte();

        public prelieviLatteService: PrelieviLatteService;
        public trasporatoriService: TrasportatoriService;
        public destinatariService: DestinatariService;
        public acquirentiService: AcquirentiService;

        public laboratorioAnalisi: LaboratorioAnalisi;
        public trasportatore: Trasportatore[] = [];
        public destinatario: Destinatario[] = [];
        public acquirente: Acquirente[] = [];


        constructor() {
            super();
            this.prelieviLatteService = new PrelieviLatteService();
            this.trasporatoriService = new TrasportatoriService();
            this.destinatariService = new DestinatariService();
            this.acquirentiService = new AcquirentiService();
        }

        mounted() {
            this.loadLaboratoriAnalisi();
            this.loadTrasportatori();
            this.loadDestinatari();
            this.loadAcquirenti();
        }

        // caricamento laboratori analisi
        public loadLaboratoriAnalisi() {
            this.prelieviLatteService.getLaboratoriAnalisi()
                .then(response => {
                    if (response.data != null) {
                        this.laboratorioAnalisi = response.data;
                    }
                });
        }

        // caricamento trasportatori
        public loadTrasportatori() {
            this.trasporatoriService.getTrasportatori()
                .then(response => {
                    if (response.data != null) {
                        this.trasportatore = response.data;
                    }
                });
        }

        // caricamento destinatari
        public loadDestinatari() {
            this.destinatariService.getDestinatari()
                .then(response => {
                    if (response.data != null) {
                        this.destinatario = response.data;
                    }
                });
        }

        // caricamento acquirenti
        public loadAcquirenti() {
            this.acquirentiService.getAcquirenti()
                .then(response => {
                    if (response.data != null) {
                        this.acquirente = response.data;
                    }
                });
        }

        public open(): void {
            $(this.$el).modal('show');
        }

        public close(): void {
            $(this.$el).modal('hide');
        }


    }

</script>