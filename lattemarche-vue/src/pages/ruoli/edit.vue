<template>

    <div id="ruoli-edit" class="container-fluid">

        <!-- Pannello modale del caricamento -->
        <waiter ref="waiter"></waiter>

        <!-- Pannelli modali di conferma azioni -->
        <notification-dialog ref="savedDialog"
                            :title="'Conferma salvataggio'"
                            :message="'Ruolo salvato correttamente'"
                            v-on:ok="reload()"></notification-dialog>


        <!-- Nav tabs -->
        <nav>
            <div class="nav nav-tabs" id="nav-tab" role="tablist">
                <a class="nav-item nav-link active" id="tab-mvc" data-toggle="tab" href="#nav-mvc" role="tab" aria-controls="nav-home" aria-selected="true">Autorizzazioni MVC</a>
                <a class="nav-item nav-link" id="tab-api" data-toggle="tab" href="#nav-api" role="tab" aria-controls="nav-profile" aria-selected="false">Autorizzazioni API</a>
                <a class="nav-item nav-link" id="tab-dettaglio-ruolo" data-toggle="tab" href="#nav-dettaglio-ruolo" role="tab" aria-controls="nav-contact" aria-selected="false">Dettaglio ruolo</a>
                <a class="nav-item nav-link" id="tab-utenti" data-toggle="tab" href="#nav-utenti" role="tab" aria-controls="nav-contact" aria-selected="false">Utenti</a>
            </div>
        </nav>

        <!-- Nav content -->
        <div class="tab-content pl-5 pt-3" id="nav-tabContent">

            <!-- Tab Autorizzazioni MVC -->
            <div class="tab-pane fade show active" id="nav-mvc" role="tabpanel" aria-labelledby="nav-home-tab">

                <div class="form-group row pt-3" v-for="(pagina, index) in ruolo.Pagine_MVC" :key="index">
                    <div class="col-sm-1 font-weight-bold">{{pagina.Title}}</div>
                    <div class="col-sm-4">
                        <div class="form-check" v-for="(item, index2) in pagina.Items" :key="index2">
                            <input class="form-check-input" type="checkbox" v-model="item.Enabled">
                            <label class="form-check-label">
                                {{item.DisplayName}}
                            </label>
                        </div>
                    </div>
                </div>

            </div>

            <!-- Tab Autorizzazioni API -->
            <div class="tab-pane fade" id="nav-api" role="tabpanel" aria-labelledby="nav-profile-tab">

                <div class="form-group row pt-3" v-for="(pagina, index) in ruolo.Pagine_API">
                    <div class="col-sm-1 font-weight-bold">{{pagina.Title}}</div>
                    <div class="col-sm-4">
                        <div class="form-check" v-for="(item) in pagina.Items">
                            <input class="form-check-input" type="checkbox" v-model="item.Enabled">
                            <label class="form-check-label">
                                {{item.DisplayName}}
                            </label>
                        </div>
                    </div>
                </div>

            </div>

            <!-- Dettaglio ruolo -->
            <div class="tab-pane fade" id="nav-dettaglio-ruolo" role="tabpanel" aria-labelledby="nav-contact-tab">

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

            </div>

            <!-- Utenti -->
            <div class="tab-pane fade" id="nav-utenti" role="tabpanel" aria-labelledby="nav-contact-tab">

                <table class="table table-striped table-bordered mt-3">
                    <thead>
                        <tr>
                            <th>Username</th>
                            <th>Nome</th>
                            <th>Cognome</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(utente, index) in ruolo.Utenti" :key="index">
                            <td>{{utente.Username}}</td>
                            <td>{{utente.Nome}}</td>
                            <td>{{utente.Cognome}}</td>
                        </tr>
                    </tbody>
                </table>

            </div>

        </div>

        <!-- salva -->
        <div class="form-group row">
            <div class="col-sm-12 text-right">
                <button class="btn btn-secondary mr-2" v-on:click="window.location = '/ruoli'">Annulla</button>
                <button class="btn btn-primary" v-on:click="onSave()" type="submit">Salva</button>
            </div>
        </div>

    </div>

</template>
<script lang="ts">

    import Vue from "vue";
    import Component from "vue-class-component";
    import { Prop, Watch, Emit } from "vue-property-decorator";

    import { UrlService } from "@/services/url.service";
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

    export default class UtentiDetailsPage extends Vue {

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
            this.$refs.waiter.open();
            this.loadRuolo((ruolo: Ruolo) => {
                // boh!
            });
            this.$refs.waiter.close();
        }

        // carica ruolo
        public loadRuolo(done: (ruolo: Ruolo) => void) {
            this.ruoliService.getDetails(this.id)
                .then(response => {
                    console.log("response.data", response.data);
                    this.ruolo = response.data;
                    done(this.ruolo);
                });
        }


        // salvataggio utente
        public onSave() {
            this.$refs.waiter.open();
            this.ruoliService.update(this.ruolo)
                .then(response => {
                    if (response.data != undefined) {
                        // TODO: msg di validazione
                        this.$refs.waiter.close();
                        this.$refs.savedDialog.open();
                    } else {
                        // save OK !!
                        this.ruolo = response.data;
                        this.$refs.savedDialog.open();
                    }
                });
        }

        
        // reload della pagina sullo stesso id
        public reload() {
            UrlService.reload();
        }

    }



</script>