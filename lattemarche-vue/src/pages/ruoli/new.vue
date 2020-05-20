<template>


    <div id="ruoli-new">

    
        <!-- Pannello modale del caricamento -->
        <waiter ref="waiter"></waiter>

        <!-- Pannelli modali di conferma azioni -->
        <notification-dialog ref="savedDialog"
                            :title="'Conferma creazione ruolo'"
                            :message="'Il ruolo è stato creato correttamente'"
                            v-on:ok="window.location = '/ruoli'"></notification-dialog>

        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <nav>
                        <div class="nav nav-tabs" id="nav-tab" role="tablist">
                            <a class="nav-item nav-link active" id="tab-nuovo-ruolo" data-toggle="tab" href="#nav-nuovo-ruolo" role="tab" aria-controls="nav-nuovo-ruolo" aria-selected="true">Nuovo ruolo</a>
                        </div>
                    </nav>

                    <div class="tab-content" id="nav-tabContent">
                        <!-- Tab Nuovo ruolo -->
                        <div class="tab-pane fade show active" id="nav-nuovo-ruolo" role="tabpanel" aria-labelledby="nav-nuovo-ruolo-tab">

                            <div class="form-group row pt-3">
                                <label class="col-sm-1 offset-2 col-form-label">Codice</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" v-model="ruolo.Codice">
                                </div>
                            </div>

                            <div class="form-group row">
                                <label class="col-sm-1 offset-2 col-form-label">Descrizione</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" v-model="ruolo.Descrizione">
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="col-12 text-right">
                                    <button class="btn btn-secondary mr-2" v-on:click="window.location = '/ruoli'">Annulla</button>
                                    <button class="btn btn-success" v-on:click="salvaRuoloCreato()" type="submit">Salva</button>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>


</template>

<script lang="ts">

    import Vue from "vue";
    import Component from "vue-class-component";
    import { Prop, Watch, Emit } from "vue-property-decorator";

    import Waiter from "../../components/waiter.vue";
    import ConfirmDialog from "../../components/confirmDialog.vue";
    import NotificationDialog from "../../components/notificationDialog.vue";

    import { Ruolo } from "../../models/ruolo.model";
    import { RuoliService } from "../../services/ruoli.service";

    declare module 'vue/types/vue' {
        interface Vue {
            open(): void
            close(): void
        }
    }

    @Component({
        components: {
            Waiter,
            NotificationDialog
        }
    })

    export default class RouliNew extends Vue {

        $refs: any = {
            waiter: Vue,
            savedDialog: Vue
        }

        public ruolo: Ruolo;
        public id: string;

        private ruoliService: RuoliService;

        constructor() {
            super();

            this.id = $('#id').val() as string;
            this.ruolo = new Ruolo();
            this.ruoliService = new RuoliService();

        }

        public mounted() {

        }

        // salva ruolo creato
        public salvaRuoloCreato() {
            this.$refs.waiter.open();
            
            this.ruoliService.create(this.ruolo)
                .then(response => {
                    if (response.data != undefined) {
                        this.$refs.waiter.close();
                        this.$refs.savedDialog.open();
                    } else {
                        this.ruolo = response.data;
                        this.$refs.savedDialog.open();
                    }
                });
        }


    }


</script>