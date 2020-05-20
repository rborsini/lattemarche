<template>
  
<div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>      

    <!-- modale errore generico -->
    <notification-dialog ref="errorDialog" :title="'Errore imprevisto'" :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'" v-on:ok="reload()"></notification-dialog>

    <!-- modale conferma salvataggio -->
    <notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Modello comunicazione salvato correttamente'" v-on:ok="redirect()" ></notification-dialog>

    <!-- Pannello notifica eliminazione offerta -->
    <notification-dialog ref="removedDialog" :title="'Conferma eliminazione'" :message="'Modello eliminato correttamente'" v-on:ok="redirect()" ></notification-dialog>

    <!-- Pannello modale conferma eliminazione offerta -->
    <confirm-dialog ref="confirmDeleteDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di voler eliminare il modello?'" v-on:confirmed="onRemove()" ></confirm-dialog>


    <!-- breadcrumb -->
    <div class="jumbotron">
        <div class="row">
        <div class="col-6">
            <ol class="breadcrumb mb-0">
                <li class="breadcrumb-item">
                    <a class="root" href="/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a class="root" href="/reminderTemplates">Comunicazioni</a>
                </li>
                <li class="breadcrumb-item active">{{template.Name}}</li>
            </ol>
        </div>
        <div class="col-6 text-right">
            <button v-if="template.Id && btnDeleteVisible" v-on:click="$refs.confirmDeleteDialog.open()" class="btn btn-primary mr-2 mt-2" role="button" >Elimina</button>
        </div>
        </div>
    </div>

    <div class="row">

        <!-- Col sx editazione -->
        <div class="card col-8 border-light mb-3" >
            <div class="card-header">Editazione</div>
            <div class="card-body">

                <div class="row pt-3">

                <label class="col-2">Nome:</label>
                    <div class="col-9">
                        <input :disabled="isReadOnly" type="text" class="form-control" v-model="template.Name" />
                    </div>

                </div>

                <div class="row pt-3">

                    <label class="col-2">A:</label>
                    <div class="col-9">
                        <input :disabled="isReadOnly" type="text" class="form-control" v-model="template.To" />
                    </div>

                </div>

                <div class="row pt-3">

                    <label class="col-2">Cc:</label>
                    <div class="col-9">
                        <input :disabled="isReadOnly" type="text" class="form-control" v-model="template.Cc" />
                    </div>

                </div>                 

                <div class="row pt-3">

                    <label class="col-2">Oggetto:</label>
                    <div class="col-9">
                        <input :disabled="isReadOnly" type="text" class="form-control" v-model="template.Subject" />
                    </div>

                </div>                

                <div class="row pt-3">

                    <label class="col-2">Corpo:</label>
                    <div class="col-9">
                        <textarea :disabled="isReadOnly" class="form-control" v-model="template.Body" rows="3" ></textarea>
                    </div>

                </div>                                                    

                <!-- Annulla / Salva -->
                <div v-if="!isReadOnly" class="row pt-5">
                    <div class="col-11 text-right">
                    <button class="btn btn-secondary mr-2" role="button" v-on:click="reload()">Annulla</button>
                    <button class="btn btn-primary" v-on:click="onSave()" role="button">Salva</button>
                    </div>
                </div>

            </div>
        </div>
        
        <!-- Col dx legenda -->
        <div class="card col-4 border-light mb-3" >
            <div class="card-header">Legenda</div>
            <div class="card-body">
                <div class="row">
                    <div class="col-5"><h5 class="card-title">{data}</h5></div>
                    <div class="col-5">Data fattura</div>
                </div>
                <div class="row">
                    <div class="col-5"><h5 class="card-title">{dettagli-pagamento}</h5></div>
                    <div class="col-5">Dettagli pagamento</div>
                </div>
                <div class="row">
                    <div class="col-5"><h5 class="card-title">{num}</h5></div>
                    <div class="col-5">Numero fattura</div>
                </div>
                <div class="row">
                    <div class="col-5"><h5 class="card-title">{email}</h5></div>
                    <div class="col-5">Email cliente</div>
                </div>
                <div class="row">
                    <div class="col-5"><h5 class="card-title">{ragione-sociale}</h5></div>
                    <div class="col-5">Ragione sociale cliente</div>
                </div>                                                                                                                                            
            </div>
        </div>

    </div>


</div>

</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

// componenti
import Waiter from "../../components/waiter.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ConfirmDialog from "../../components/confirmDialog.vue";

// servizi
import { PermissionsService } from "@/services/permissions.service";
import { UrlService } from "@/services/url.service";
import { Template } from "@/models/ir/template.model";
import { TemplatesService } from "@/services/ir/templates.service";

// modelli

@Component({
    components: {
        Waiter,
        NotificationDialog,
        ConfirmDialog
    }
})
export default class App extends Vue {
    $refs: any = {
        waiter: Vue,
        errorDialog: Vue,
        confirmDeleteDialog: Vue,
        savedDialog: Vue
    };

    public isReadOnly: boolean = false;
    public btnDeleteVisible: boolean = false;

    public template: Template = new Template();

    public templatesService: TemplatesService = new TemplatesService();

    constructor() {
        super();
    }

    public mounted() {
        this.isReadOnly = !PermissionsService.isViewItemAuthorized(
            "ReminderTemplates",
            "Edit",
            "Edit"
        );

        this.btnDeleteVisible = PermissionsService.isViewItemAuthorized(
            "ReminderTemplates",
            "Edit",
            "Delete"
        );

        var id: string = UrlService.getUrlParameter("id");

        if (id != "") {
            this.loadTemplate(id);
        }
    }

    // caricamento auditor
    private loadTemplate(id: string) {
        this.templatesService.details(id).then(response => {
            this.template = response.data;
        });
    }

    // elimina auditor
    public onRemove() {
        this.templatesService.delete(this.template.Id).then(response => {
            this.$refs.removedDialog.open();
        });
    }

    // salvataggio auditor
    public onSave() {
        this.$refs.waiter.open();
        this.templatesService
            .save(this.template)
            .then(response => {
                this.template = response.data;
                this.$refs.waiter.close();
                this.$refs.savedDialog.open();
            })
            .catch(error => {
                this.$refs.waiter.close();
                if (error.response.status == 400) {
                    // Bad Request => messaggi di validazione
                    this.$refs.validationDialog.openDialog(
                        error.response.data.ModelState
                    );
                } else {
                    this.$refs.errorDialog.open();
                }
            });
    }

    // redirect della pagina di ricerca
    public redirect() {
        UrlService.redirect("/reminderTemplates");
    }

    // redirect della pagina sullo stesso id
    public reload() {
        UrlService.redirect("/reminderTemplates/edit?id=" + this.template.Id);
    }
}
</script>