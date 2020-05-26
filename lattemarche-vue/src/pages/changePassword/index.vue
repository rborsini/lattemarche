<template>
  
    <div id="signup" class="container">
        <h3 class="pt-5 mb-2">Cambio password</h3>

        <!-- Pannelli modali di conferma azioni -->
        <notification-dialog ref="changedPasswordSuccessDialog"
                            :title="'Conferma della modifica della password'"
                            :message="'La password è stata modificata correttamente'"
                            v-on:ok="redirect()"></notification-dialog> 

        <!-- Pannelli modali di conferma azioni -->
        <notification-dialog ref="changedPasswordFailedDialog"
                            :title="'Errore nella modifica della password'"
                            :message="result"></notification-dialog> 

        <!--  username -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Username</label>
            <div class="col-sm-6">
                <input type="text" class="form-control" v-model="model.Username" />
            </div>
        </div>

        <!--  old password -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Vecchia Password</label>
            <div class="col-sm-6">
                <input type="password" class="form-control" v-model="model.OldPassword" />
            </div>
        </div>

        <!--  password -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Nuova Password</label>
            <div class="col-sm-6">
                <input type="password" class="form-control" v-model="model.Password" />
            </div>
        </div>

        <!--  rePassword -->
        <div class="row form-group">
            <label class="col-sm-3 text-right">Conferma la nuova Password</label>
            <div class="col-sm-6">
                <input type="password" class="form-control" v-model="model.RePassword" />
            </div>
        </div>

        <!--  Registrati -->
        <div class="row form-group">
            <div class="col-9">
                <button class="btn btn-primary float-right" v-on:click="onSave()">Cambia Password</button>
            </div>
        </div>

    </div>


</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import $ from 'jquery';

import NotificationDialog from "../../components/notificationDialog.vue";

import { ChangePassword } from "../../models/changePassword.model";
import { UtentiService } from "../../services/utenti.service";

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
        changedPasswordSuccessDialog: Vue,
        changedPasswordFailedDialog:Vue
    }

    private usersService: UtentiService;

    public model: ChangePassword;
    public result:string="";

    constructor() {
        super();

        this.usersService = new UtentiService();
        this.model = new ChangePassword();

    }

    public onSave() {
        this.usersService.changePassword(this.model)
            .then(response => {
                if(response.data=="ok"){                    
                    this.$refs.changedPasswordSuccessDialog.open();
                }else{
                    this.result=response.data;
                    this.$refs.changedPasswordFailedDialog.open();
                }
            });
    }

    public redirect() {
        window.location.assign('/');
    }

}
</script>