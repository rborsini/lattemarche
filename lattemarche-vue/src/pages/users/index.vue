<template>
  
<div>

    <!-- Pannello editazione dettaglio -->
    <editazione-utente-modal ref="editazioneUtenteModal"
                             :user="user"
                             v-on:salvato="$refs.savedDialog.open()"></editazione-utente-modal>

    <!-- Pannello notifica salvatagggio -->
    <notification-dialog ref="savedDialog"
                         :title="'Conferma salvataggio'"
                         :message="'Utente salvato correttamente'"
                         v-on:ok="redirect()"></notification-dialog>

    <!-- Pannello notifica rimozione -->
    <notification-dialog ref="removedDialog"
                         :title="'Conferma rimozione'"
                         :message="'Utente rimosso correttamente'"
                         v-on:ok="redirect()"></notification-dialog>

    <!-- Pannello modale conferma eliminazione -->
    <confirm-dialog ref="confirmDeleteDialog"
                    :title="'Conferma eliminazione'"
                    :message="'Sei sicuro di voler rimuovere l\'utente selezionato?'"
                    v-on:confirmed="onRemove()"></confirm-dialog>

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="users" v-on:data-loaded="onDataLoaded">

        <!-- Toolbox -->
        <template slot="toolbox" v-if="canAdd">
            <button class="toolbox btn btn-primary float-right" v-on:click="onAdd()">Aggiungi</button>
        </template>

        <!-- Colonne -->
        <template slot="thead">
            <th>Nome</th>
            <th>Cognome</th>
            <th>Username</th>
            <th></th>
        </template>

    </data-table>
</div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";
import EditazioneUtenteModal from "./edit.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

import { User } from "../../models/user.model";
import { UsersService } from "../../services/users.service";
import { UrlService } from '@/services/url.service';

@Component({
  components: {
    ConfirmDialog,
    NotificationDialog,
    EditazioneUtenteModal,
    DataTable
  }
})
export default class App extends Vue {

    $refs: any = {
        savedDialog: Vue,
        editazioneUtenteModal: Vue,
        confirmDeleteDialog: Vue,
        removedDialog: Vue
    }

    private usersService: UsersService;
    public user: User;
    private userId: number = 0;

    public tableOptions: any = {};
    public users: User[] = [];
    public canAdd: boolean = false;
    public canEdit: boolean = false;
    public canRemove: boolean = false;

    constructor() {
        super();

        this.usersService = new UsersService();
        this.user = new User();
    }

    public mounted() {
        this.initTable();

        this.usersService.index()
            .then(response => {
                this.users = response.data;
            });
    }

    // Evento fine generazione tabella
    public onDataLoaded() {

        $('.edit').click((event) => {

            var element = $(event.currentTarget);
            var rowId = $(element).data("row-id");

            this.usersService.details(rowId)
                .then(response => {
                    this.user = response.data;
                    this.$refs.editazioneUtenteModal.openUser(this.user);
                });

        });

        $('.delete').click((event) => {

            var element = $(event.currentTarget);
            this.userId = $(element).data("row-id");

            this.$refs.confirmDeleteDialog.open();

        });

    }


    // nuova utente
    public onAdd() {

        this.user = new User();
        this.$refs.editazioneUtenteModal.open();

    }

    // rimozione utente
    public onRemove() {

        this.usersService.delete(this.userId)
            .then(response => {
                this.$refs.removedDialog.open();
            });
    }


    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "FirstName" });
        options.columns.push({ data: "LastName" });
        options.columns.push({ data: "Username" });
        options.columns.push({
            render: function (data: any, type: any, row: any) {

                var html = '<div class="text-center">';
                html += '<a class="edit text-primary" title="modifica" style="cursor: pointer;" data-row-id="' + row.Username + '" ><i class="far fa-edit"></i></a>';
                html += '</div>';

                return html;
            },
            className: "edit-column",
            orderable: false
        });

        this.tableOptions = options;
    }

    public redirect() {
        UrlService.redirect('/users');
    }



}
</script>