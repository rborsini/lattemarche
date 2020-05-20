<template>
  
    <div id="signup" class="container">

        <h3 class="pt-5 mb-2">Nuovo utente</h3>

        <!-- Pannelli modali di conferma azioni -->
        <notification-dialog ref="savedDialog"
                            :title="'Conferma creazione utente'"
                            :message="'Utente creato correttamente'"
                            v-on:ok="redirect()"></notification-dialog>

        <!-- nome -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Nome</label>
            <div class="col-sm-6">
                <input type="text" class="form-control" v-model="user.FirstName" />
            </div>
        </div>

        <!-- cognome -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Cognome</label>
            <div class="col-sm-6">
                <input type="text" class="form-control" v-model="user.LastName" />
            </div>
        </div>

        <!--  username -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Username</label>
            <div class="col-sm-6">
                <input type="text" class="form-control" v-model="user.Username" />
            </div>
        </div>

        <!--  email -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Email</label>
            <div class="col-sm-6">
                <input type="text" class="form-control" v-model="user.Email" />
            </div>
        </div>

        <!--  password -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Password</label>
            <div class="col-sm-6">
                <input type="password" class="form-control" v-model="user.Password" />
            </div>
        </div>

        <!--  Registrati -->
        <div class="row form-group">
            <div class="col-9">
                <button class="btn btn-primary float-right" v-on:click="onSave()">Invia</button>
            </div>
        </div>

    </div>


</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
// import $ from 'jquery';

import NotificationDialog from "../../components/notificationDialog.vue";

import { User } from "../../models/user.model";
import { UsersService } from "../../services/users.service";

declare module "vue/types/vue" {
  interface Vue {
    open(): void;
    close(): void;
  }
}

@Component({
  components: {
    NotificationDialog
  }
})
export default class App extends Vue {

    $refs: any = {
        savedDialog: Vue
    }

    private usersService: UsersService;

    public user: User;

    constructor() {
        super();

        this.usersService = new UsersService();
        this.user = new User();

    }

    public onSave() {
        this.usersService.save(this.user)
            .then(response => {
                this.$refs.savedDialog.open();
            });
    }

    public redirect() {
        window.location.assign('/');
    }

}
</script>

