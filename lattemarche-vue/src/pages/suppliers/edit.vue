<template>
  
<div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>      

    <!-- modale errore generico -->
    <notification-dialog ref="errorDialog" :title="'Errore imprevisto'" :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'" v-on:ok="reload()"></notification-dialog>

    <!-- modale conferma salvataggio -->
    <notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Fornitore salvato correttamente'" v-on:ok="reload()" ></notification-dialog>

    <!-- breadcrumb -->
    <div class="jumbotron">
        <div class="row">
        <div class="col-6">
            <ol class="breadcrumb mb-0">
                <li class="breadcrumb-item">
                    <a class="root" href="/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a class="root" href="/suppliers">Fornitori</a>
                </li>
                <li class="breadcrumb-item active">{{supplier.Name}}</li>
            </ol>
        </div>
        <div class="col-6 text-right">
            <button v-if="supplier.Id && btnDeleteVisible" v-on:click="$refs.confirmDeleteDialog.open()" class="btn btn-primary mr-2" role="button" >Elimina</button>
        </div>
        </div>
    </div>

    <div v-if="itemNotFound" class="portlet light" >      
      <div class="portlet-body">
        <div class="row pl-4 pt-4">
            <h2>Nessuna auditor trovato</h2>
        </div>
      </div>
    </div>    

    <div v-else>

        <ul class="nav nav-tabs" id="tabWrapper">
            <li class="active">
                <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
            </li>
            <li>
                <a data-toggle="tab" class="nav-link" href="#skills">Skills</a>
            </li>
            <li>
                <a data-toggle="tab" class="nav-link" href="#rates">Tariffa oraria</a>
            </li>           
            <li>
                <a data-toggle="tab" class="nav-link" href="#auditors">Tecnici</a>
            </li>        
        </ul>

        <div class="tab-content jumbotron">

            <!-- Tab dettaglio -->
            <div id="dettaglio" class="tab-pane fade show active">

                <!-- Ragione sociale / Indirizzo -->
                <div class="row pt-3">
                    <div class="col-6 row">
                        <label class="col-2">Rag. sociale:</label>
                        <div class="col-10">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.BusinessName" />
                        </div>
                    </div>
                </div>

                <!-- Indirizzo / CAP / Comune / Indirizzo  -->
                <div class="row pt-3">

                    <div class="col-6 row" >
                        <label class="col-2">Indirizzo:</label>
                        <div class="col-7">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.Address" />
                        </div>
                        <label class="col-1">CAP:</label>
                        <div class="col-2">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.PostalCode" />
                        </div>   
                    </div>

                    <div class="col-6 row" >
                        <label class="col-2" >Comune:</label>
                        <div class="col-5">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.City" />
                        </div>   

                        <label class="col-1">Prov:</label>
                        <div class="col-2">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.Province" />
                        </div>                       
                    </div>                                    

                </div>

                <!-- P. IVA / Codice fiscale / Telefono / Cellulare  -->
                <div class="row pt-3">

                    <div class="col-6 row">
                        <label class="col-2">P. IVA:</label>
                        <div class="col-4">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.VAT_Number" />
                        </div>
                        <label class="col-1">C.F.:</label>
                        <div class="col-5">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.FiscalCode" />
                        </div>                      
                    </div>

                    <div class="col-6 row">
                        <label class="col-2">Telefono:</label>
                        <div class="col-5">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.Phone" />
                        </div>
                        <label class="col-1">Cellulare:</label>
                        <div class="col-4">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.CellPhone" />
                        </div>  
                    </div>

                </div>

                <!-- Fax / Email / PEC  -->
                <div class="row pt-3">

                    <div class="col-6 row">
                        <label class="col-2">Fax:</label>
                        <div class="col-4">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.Fax" />
                        </div>
                        <label class="col-1">Email.:</label>
                        <div class="col-5">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.Email" />
                        </div>                      
                    </div>

                    <div class="col-6 row">
                        <label class="col-2">PEC:</label>
                        <div class="col-5">
                            <input :disabled="isReadOnly" type="text" class="form-control" v-model="supplier.PEC" />
                        </div>
                    </div>

                </div>

            </div>

            <!-- Tab auditors -->
            <div id="auditors" class="tab-pane fade row">

                <div class="offset-2 col-8 pt-4">

                    <table class="table table-bordered">

                        <!-- intestazione tabella -->
                        <thead class="thead-dark">
                            <tr>
                                <th scope="rol">Nome</th>
                                <th scope="rol">Cognome</th>
                                <th scope="rol">Interno</th>
                                <th scope="rol"></th>
                            </tr>
                        </thead>

                        <!-- corpo tabella -->
                        <tbody>
                            <tr v-for="(auditor, index) in supplier.Auditors" :key="index">
                                <td>
                                    {{auditor.FirstName}}
                                </td>
                                <td>
                                    {{auditor.FirstName}}
                                </td>
                                <td>
                                    {{auditor.IsDefault}}
                                </td>          
                                <td>
                                    <div class="text-center">
                                        <a v-bind:href="'/auditors/edit?id=' + auditor.Id " class="edit text-primary" title="modifica" style="cursor: pointer;" ><i class="far fa-edit"></i></a>
                                    </div>
                                </td>            
                            </tr>

                        </tbody>

                    </table>

                </div>            

            </div>

            <!-- Tab skills -->
            <div id="skills" class="tab-pane fade">

                <div class="offset-1 col-10 pt-4">

                    <div class="row pt-1" v-for="i in Math.ceil(skills.length / SKILL_ROWS_SIZE)" :key="i" >
                        <div class="col" v-for="skill in skills.slice((i - 1) * SKILL_ROWS_SIZE, i * SKILL_ROWS_SIZE)" :key="skill.Id" >
                            <div class="row form-check">
                                <input class="form-check-input" type="checkbox" v-model="skill.Checked">
                                <div class="form-check-label">{{skill.Description}}</div>
                            </div>             
                        </div>
                    </div> 
                    
                </div>

            </div>

            <!-- Tab rates -->
            <div id="rates" class="tab-pane fade row">

                <div class="offset-2 col-8 pt-4">

                    <table class="table table-bordered">
                        <!-- intestazione tabella -->
                        <thead class="thead-dark">
                            <tr>
                                <th scope="rol">Dal</th>
                                <th scope="rol">A</th>
                                <th scope="rol">Tariffa oraria</th>
                                <th scope="rol"></th>
                            </tr>
                        </thead>

                        <!-- corpo tabella -->
                        <tbody>
                            <tr v-for="(rate, index) in supplier.Rates" :key="index">
                                <td>
                                    <!-- {{rate.From_Str}} -->
                                    <datepicker class="form-control" :disabled="isReadOnly" :value.sync="rate.From_Str"  />
                                </td>
                                <td>
                                    <!-- {{rate.To_Str}} -->
                                    <datepicker class="form-control" :disabled="isReadOnly" :value.sync="rate.To_Str"  />
                                </td>
                                <td>
                                    <!-- {{rate.Value}} -->
                                    <div class="row">
                                        <div class="col-10">
                                            <input type="number" :disabled="isReadOnly" v-model="rate.Value" class="form-control" step="0.01" style="text-align: right;" > 
                                        </div>
                                        <div class="col-2">
                                            <span> €</span>
                                        </div>
                                    </div>                                
                                </td>
                                <td class="text-center">
                                    <a v-if="!isReadOnly" v-on:click="supplier.Rates.splice(index, 1)" title="Rimuovi" class="cursor-pointer text-primary" >
                                        <i class="far fa-trash-alt mt-2"></i>
                                    </a>
                                </td>                            
                            </tr>
                            <!-- Riga vuota per inserimento nuova tariffa -->
                            <tr>
                                <td>
                                    <datepicker class="form-control" :disabled="isReadOnly" :value.sync="newRate.From_Str"  />
                                </td>
                                <td>
                                    <datepicker class="form-control" :disabled="isReadOnly" :value.sync="newRate.To_Str" />
                                </td>                   
                                <td>
                                    <div class="row">
                                        <div class="col-10">
                                            <input type="number" :disabled="isReadOnly" v-model="newRate.Value" class="form-control" step="0.01" style="text-align: right;" > 
                                        </div>
                                        <div class="col-2">
                                            <span> €</span>
                                        </div>
                                    </div>                                
                                </td>                                         
                                <td>
                                    <button class="btn btn-primary" v-on:click="onRate_Add(newRate)" >Aggiungi</button>
                                </td>
                            </tr>
                        </tbody>

                    </table>

                </div>

            </div>        
        </div>

        <!-- Annulla / Salva -->
        <div v-if="!isReadOnly" class="row pt-3">
            <div class="col-12 text-right">
            <button class="btn btn-secondary mr-2" role="button" v-on:click="reload()">Annulla</button>
            <button class="btn btn-primary" v-on:click="onSave()" role="button">Salva</button>
            </div>
        </div>

    </div>


</div>

</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

// componenti
import Waiter from "../../components/waiter.vue";
import Datepicker from "../../components/datepicker.vue";
import NotificationDialog from "../../components/notificationDialog.vue";

// servizi
import { SuppliersService } from "@/services/suppliers.service";
import { DropdownService } from "@/services/dropdown.service";
import { PermissionsService } from "@/services/permissions.service";
import { UrlService } from "@/services/url.service";

// modelli
import { Supplier } from "@/models/supplier.model";
import { Dropdown } from "@/models/dropdown.model";
import { Skill } from "@/models/skill.model";
import { AssigneeRate } from "@/models/assigneeRate.model";

@Component({
    components: {
        Waiter,
        Datepicker,
        NotificationDialog
    }
})
export default class App extends Vue {
    $refs: any = {
        waiter: Vue,
        errorDialog: Vue,
        savedDialog: Vue
    };

    public itemNotFound: boolean = false;
    public isReadOnly: boolean = false;
    public btnDeleteVisible: boolean = false;

    public supplier: Supplier = new Supplier();

    public suppliersService: SuppliersService = new SuppliersService();
    public dropdownService: DropdownService = new DropdownService();

    public SKILL_ROWS_SIZE: number = 4;
    public dropdownSkills: Dropdown = new Dropdown();
    public skills: Skill[] = [];
    public newRate: AssigneeRate = new AssigneeRate();

    constructor() {
        super();
    }

    public mounted() {
        this.isReadOnly = !PermissionsService.isViewItemAuthorized(
            "Suppliers",
            "Edit",
            "Edit"
        );

        this.btnDeleteVisible = PermissionsService.isViewItemAuthorized(
            "Suppliers",
            "Edit",
            "Delete"
        );

        var id: string = UrlService.getUrlParameter("id");

        if (id != "") {
            this.loadSupplier(id);
        }

        this.loadDropdown();
        this.keepSelectedTabOnRefresh();
    }

    // caricamento fornitore
    private loadSupplier(id: string) {

        this.$refs.waiter.open();
        this.suppliersService.details(id).then(response => {

            if(response.data != null) {
                this.supplier = response.data;
                this.bindSkills();
            } else {
                this.itemNotFound = true;
            }

            this.$refs.waiter.close();
        });
    }

    // caricamento dropdown
    private loadDropdown() {
        this.dropdownService.getSkills().then(response => {
            this.dropdownSkills = response.data;
            this.bindSkills();
        });
    }

    private bindSkills() {
            this.skills = [];
            for (var i = 0; i < this.dropdownSkills.Items.length; i++) {
                var skill = new Skill();

                skill.Id = this.dropdownSkills.Items[i].Value;
                skill.Description = this.dropdownSkills.Items[i].Text;
                skill.Checked = this.supplier.Skills.find(s => s.Id == skill.Id) != null;

                this.skills.push(skill);
            }
    }

    // elimina fornitore
    public onRemove() {
        this.suppliersService.delete(this.supplier.Id).then(response => {
            this.$refs.removedDialog.open();
        });
    }

    // salvataggio auditor
    public onSave() {
        this.supplier.Skills = [];
        for (var i = 0; i < this.skills.length; i++) {
            if (this.skills[i].Checked) {
                this.supplier.Skills.push(this.skills[i]);
            }
        }

        this.$refs.waiter.open();
        this.suppliersService
            .save(this.supplier)
            .then(response => {
                this.supplier = response.data;
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

    public onRate_Add(rate: AssigneeRate) {
        rate.AssigneeId = this.supplier.Id;
        var clonedRate = JSON.parse(JSON.stringify(rate));
        this.supplier.Rates.push(clonedRate);

        this.newRate.From_Str = "";
        this.newRate.To_Str = "";
        this.newRate.Value = 0;
    }

    // redirect della pagina di ricerca
    public redirect() {
        UrlService.redirect("/suppliers");
    }

    // redirect della pagina sullo stesso id
    public reload() {
        UrlService.redirect("/suppliers/edit?id=" + this.supplier.Id + window.location.hash);
    }

    // Mantengo la tab selezionata per il refresh della pagina
    public keepSelectedTabOnRefresh() {
        $("ul.nav-tabs > li > a").on("shown.bs.tab", function(e) {
            window.location.hash = String($(e.target).attr("href"));
        });

        $('#tabWrapper a[href="' + window.location.hash + '"]').tab("show");
    }
}
</script>