<template>
  <div>
    <!-- Pannello modale del caricamento -->
    <waiter ref="waiter"></waiter>

    <!-- Pannelli modali di conferma azioni -->
    <notification-dialog
      ref="savedDialog"
      :title="'Conferma salvataggio'"
      :message="'Ruolo salvato correttamente'"
      v-on:ok="reload()"
    ></notification-dialog>

    <!-- Nav tabs -->
    <ul class="nav nav-tabs" id="tabWrapper">
      <li class="active">
        <a class="nav-link active" data-toggle="tab" href="#nav-mvc">Autorizzazioni MVC</a>
      </li>
      <li>
        <a class="nav-link" data-toggle="tab" href="#nav-api">Autorizzazioni API</a>
      </li>
      <li>
        <a class="nav-link" data-toggle="tab" href="#nav-dettaglio-ruolo">Dettaglio ruolo</a>
      </li>
      <li>
        <a class="nav-link" data-toggle="tab" href="#nav-utenti">Utenti</a>
      </li>
    </ul>

    <!-- Nav content -->
    <div class="tab-content" id="nav-tabContent">
      <!-- Tab Autorizzazioni MVC -->
      <div
        class="tab-pane fade show active"
        id="nav-mvc"
        role="tabpanel"
        aria-labelledby="nav-home-tab"
      >
        <div class="form-group row pt-3" v-for="page in role.MVC_Pages" :key="page.Id">
          <div class="col-sm-1 font-weight-bold">{{page.Title}}</div>
          <div class="col-sm-4">
            <div class="form-check" v-for="item in page.Items" :key="item.Id">
              <input class="form-check-input" type="checkbox" v-model="item.Enabled">
              <label class="form-check-label">{{item.DisplayName}}</label>
            </div>
          </div>
        </div>
      </div>

      <!-- Tab Autorizzazioni API -->
      <div class="tab-pane fade" id="nav-api" role="tabpanel" aria-labelledby="nav-profile-tab">
        <div class="form-group row pt-3" v-for="page in role.API_Pages" :key="page.Id">
          <div class="col-sm-1 font-weight-bold">{{page.Title}}</div>
          <div class="col-sm-4">
            <div class="form-check" v-for="(item) in page.Items" :key="item.Id">
              <input class="form-check-input" type="checkbox" v-model="item.Enabled">
              <label class="form-check-label">{{item.DisplayName}}</label>
            </div>
          </div>
        </div>
      </div>

      <!-- Dettaglio ruolo -->
      <div
        class="tab-pane fade"
        id="nav-dettaglio-ruolo"
        role="tabpanel"
        aria-labelledby="nav-contact-tab"
      >
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
      </div>

      <!-- Utenti -->
      <div
        class="tab-pane fade p-3"
        id="nav-utenti"
        role="tabpanel"
        aria-labelledby="nav-contact-tab"
      >
        <table class="table table-striped table-bordered">
          <thead>
            <tr>
              <th>Username</th>
              <th>Nome</th>
              <th>Cognome</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="user in role.Users" :key="user.Id">
              <td>{{user.Username}}</td>
              <td>{{user.FirstName}}</td>
              <td>{{user.LastName}}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- salva -->
      <div class="form-group row mt-3">
        <div class="col-12 text-right">
          <button class="btn btn-secondary mr-2" v-on:click="window.location = '/roles'">Annulla</button>
          <button class="btn btn-primary" v-on:click="onSave()" type="submit">Salva</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import * as jquery from 'jquery';

import Waiter from "../../components/waiter.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";
import NotificationDialog from "../../components/notificationDialog.vue";

import { Role } from "../../models/role.model";

import { RolesService } from "../../services/roles.service";
import { UrlService } from "../../services/url.service";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}

@Component({
  components: {
    Waiter,
    NotificationDialog
  }
})
export default class App extends Vue {
  $refs: any = {
    waiter: Vue,
    savedDialog: Vue
  };

  public roleId: string = "";
  public role: Role = new Role();

  private rolesService: RolesService = new RolesService();
  public urlService: UrlService = new UrlService();

  constructor() {
    super();
  }

  public mounted() {
    this.roleId = UrlService.getUrlParameter("id");
    this.$refs.waiter.open();
    this.load();
    this.$refs.waiter.close();
    this.keepSelectedTabOnRefresh();
  }

  // carica ruolo
  public load() {
    this.rolesService.getDetails(this.roleId).then(response => {
      this.role = response.data;
    });
  }

  // salvataggio utente
  public onSave() {
    this.$refs.waiter.open();
    this.rolesService.save(this.role).then(response => {
      if (response.data != undefined) {
        // TODO: msg di validazione
        this.$refs.waiter.close();
        this.$refs.savedDialog.open();
      } else {
        // save OK !!
        this.role = response.data;
        this.$refs.savedDialog.open();
      }
    });
  }

  public reload() {
    UrlService.reload();
  }

  // Mantengo la tab selezionata al refresh della pagina
  public keepSelectedTabOnRefresh() {
    $("ul.nav-tabs > li > a").on("shown.bs.tab", function(e) {
      window.location.hash = String($(e.target).attr('href'));
    });

    $('#tabWrapper a[href="' + window.location.hash + '"]').tab('show');
  }
}
</script>