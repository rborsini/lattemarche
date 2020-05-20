<template>
  <div>

    <!-- Pannello modale del caricamento -->
    <waiter ref="waiter"></waiter>

    <!-- Pannelli modali di conferma azioni -->
    <notification-dialog ref="savedDialog"    
                         title="Conferma creazione ruolo"
                         message = "Il ruolo è stato creato correttamente"
                         v-on:ok="redirect()"></notification-dialog>
                      

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
                            <input type="text" class="form-control" v-model="role.Code">
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-sm-1 offset-2 col-form-label">Descrizione</label>
                        <div class="col-sm-6">
                            <input type="text" class="form-control" v-model="role.Description">
                        </div>
                    </div>

                    <div class="form-group row">
                        <div class="col-12 text-right">
                            <button class="btn btn-secondary mr-2" v-on:click="window.location = '/ruoli'">Annulla</button>
                            <button class="btn btn-primary" v-on:click="salvaRuoloCreato()" type="submit">Salva</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

  </div>
</template>

<script lang="ts">

import { Component, Vue } from 'vue-property-decorator';

import Waiter from "../../components/waiter.vue";
import NotificationDialog from "../../components/notificationDialog.vue";

import { Role } from "../../models/role.model";

import { RolesService } from "@/services/roles.service";
import { UrlService } from '@/services/url.service';

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
  },
})
export default class App extends Vue {

    $refs: any = {
        waiter: Vue,
        savedDialog: Vue
    }

    public role: Role = new Role();
    public rolesService: RolesService = new RolesService();
    public urlService: UrlService = new UrlService();
    public id: string = "";

    constructor() {
        super();              
    }

    public mounted() {
    }

    // salva ruolo creato
    public salvaRuoloCreato() {
        this.$refs.waiter.open();
        this.rolesService.save(this.role)
            .then(response => {
                if (response.data != undefined) {
                    this.$refs.waiter.close();
                    this.$refs.savedDialog.open();
                } else {
                    this.role = response.data;
                    this.$refs.savedDialog.open();
                }
            });
    }

    public redirect() {
        UrlService.redirect('/roles');
    }

}

</script>