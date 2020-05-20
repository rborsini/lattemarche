<template>
  <div
    class="modal fade bd-example-modal-lg"
    id="editazione-utente-modal"
    tabindex="-1"
    role="dialog"
    aria-labelledby="myLargeModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Dettaglio utente</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">

          <ul class="nav nav-tabs" id="tabWrapper">
              <li class="active">
                  <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
              </li>
              <li>
                  <a data-toggle="tab" class="nav-link" href="#ruoli">Ruoli</a>
              </li>
          </ul>

          <div class="tab-content">

            <!-- Tab dettaglio -->
            <div id="dettaglio" class="tab-pane fade show active pt-4">
              
              <!--  username -->
              <div class="row form-group">
                <label class="col-sm-3 text-right">Username</label>
                <div class="col-sm-9">
                  <input type="text" class="form-control" v-model="user.Username" />
                </div>
              </div>

              <!--  email -->
              <div class="row form-group">
                <label class="col-sm-3 text-right">Email</label>
                <div class="col-sm-9">
                  <input type="text" class="form-control" v-model="user.Email" />
                </div>
              </div>

              <!-- nome -->
              <div class="row form-group">
                <label class="col-sm-3 text-right">Nome</label>
                <div class="col-sm-9">
                  <input type="text" class="form-control" v-model="user.FirstName" />
                </div>
              </div>

              <!-- cognome -->
              <div class="row form-group">
                <label class="col-sm-3 text-right">Cognome</label>
                <div class="col-sm-9">
                  <input type="text" class="form-control" v-model="user.LastName" />
                </div>
              </div>

            </div>

            <!-- Tab ruoli -->
            <div class="tab-pane fade pt-4" id="ruoli">

              <div class="row form-group" v-for="(role, index) in user.Roles" :key="index">
                <div class="offset-2 col-1 padding-right-5">
                  <input type="checkbox" class="float-right" v-model="role.Enabled" />
                </div>
                <span class="col-2">{{role.Title}}</span>
              </div>       
  
            </div>
          </div>


          <!-- progress bar -->
          <div class="row" v-if="progressBarVisible">
            <div class="col-sm-3 offset-4 pt-2">
              <div class="progress">
                <div
                  class="progress-bar progress-bar-striped progress-bar-animated"
                  role="progressbar"
                  aria-valuenow="100"
                  aria-valuemin="0"
                  aria-valuemax="100"
                  style="width: 100%"
                ></div>
              </div>
            </div>
            <div class="col-sm-3 offset-4 text-center pt-2">
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
import { Prop } from "vue-property-decorator";

import { User } from "../../models/user.model";
import { UsersService } from "../../services/users.service";

@Component({})
export default class EditazioneUtenteModal extends Vue {
  @Prop()
  public user!: User;

  private usersService: UsersService;

  public progressBarVisible = false;

  constructor() {
    super();
    this.usersService = new UsersService();
  }

  mounted() {}

  public openUser(user: User): void {
    $(this.$el).modal("show");
  }

  public open(): void {
    $(this.$el).modal("show");
  }

  public onSave() {
    this.progressBarVisible = true;
    this.usersService.save(this.user).then(response => {
      if (response.data != undefined) {
        this.$emit("salvato");
        this.progressBarVisible = false;
        this.close();
      } else {
        // save KO!!
        this.user = response.data;
        // TODO: msg di validazione
        //this.$emit("errore");
        this.close();
      }
    });
  }

  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>
