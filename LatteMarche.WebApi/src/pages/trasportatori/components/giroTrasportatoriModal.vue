<template>
    <div class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Dettaglio giro</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body pl-5 pr-5">
                    <div class="row form-group">
                        <label class="col-2">Codice</label>
                        <div class="col-10">
                            <input type="text" class="form-control" v-model="giro.CodiceGiro">
                        </div>
                    </div>
                    <div class="row form-group">
                        <label class="col-2">Denominazione</label>
                        <div class="col-10">
                            <input type="text" class="form-control" v-model="giro.Denominazione">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary mr-2" data-dismiss="modal">Annulla</button>
                    <button v-on:click="onSave()" class="btn btn-success">Salva</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">

    import Vue from "vue";
    import Component from "vue-class-component";
    import { Prop, Watch, Emit } from "vue-property-decorator";
    import { Giro } from "../../../models/giro.model";

    @Component({
        components: { }
    })

    export default class GiroTrasportatoriModal extends Vue {

        @Prop()
        giro: Giro;

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