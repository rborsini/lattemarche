<template>
    <div class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" style="max-width:68%">
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
                            <input type="text" class="form-control">
                        </div>
                        <label class="col-2">Ora prelievo</label>
                        <div class="col-sm-4">
                            <!--<date-time-picker v-model="date" @dp-hide="doSomethingOnHide" @dp-change="doSomethingOnChange"></date-time-picker>-->
                            <date-time-picker v-model="prelivoLatte.OraPrelievo"> </date-time-picker>

                            <!--<input type="text" class="form-control">-->
                        </div>
                    </div>
                    <!-- data/ora ultima mungitura -->
                    <div class="row form-group">
                        <label class="col-2">Data ultima mungitura</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
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
                            <input type="text" class="form-control">
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
                            <input type="number" class="form-control">
                        </div>
                        <label class="col-2">Quantità in Kg</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control">
                        </div>
                    </div>
                    <!-- temperatura C° / trasportatore -->
                    <div class="row form-group">
                        <label class="col-2">Temperatura in C°</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control">
                        </div>
                        <label class="col-2">Trasportatore</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
                        </div>
                    </div>
                    <!-- acquirente / destinatario -->
                    <div class="row form-group">
                        <label class="col-2">Acquirente</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
                        </div>
                        <label class="col-2">Destinatario</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
                        </div>
                    </div>
                    <!-- laboratorio analisi / seriale laboratorio -->
                    <div class="row form-group">
                        <label class="col-2">Laboratorio analisi</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
                        </div>
                        <label class="col-2">Seriale laboratorio</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
                        </div>
                    </div>
                    <!-- scomparto / lotto di consegna -->
                    <div class="row form-group">
                        <label class="col-2">Scomparto</label>
                        <div class="col-sm-4">
                            <input type="number" class="form-control">
                        </div>
                        <label class="col-2">Lotto di consegna</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control">
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
    import DateTimePicker from "../../../components/common/datetimepicker.vue"

    //Vue.use(DateTimePicker);

    @Component({
        components: {
            DateTimePicker
        }
    })

    export default class EditazionePrelievoModal extends Vue {


        constructor() {
            super();
        }

        mounted() {
        }

        public open(): void {
            $(this.$el).modal('show');
        }

        public close(): void {
            $(this.$el).modal('hide');
        }


    }

</script>