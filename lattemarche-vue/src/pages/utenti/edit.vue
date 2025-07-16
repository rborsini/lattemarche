<template>
	<div>
		<!-- waiter -->
		<waiter ref="waiter"></waiter>

		<!-- Pannello editazione allevamento -->
		<editazione-allevamento-modal ref="editazioneAllevamentoModal" :allevamento="allevamento" v-on:salvato="onAllevamentoSaved"></editazione-allevamento-modal>

		<!-- Pannello editazione autocisterna -->
		<editazione-autocisterna-modal ref="editazioneAutocisternaModal" :autocisterna="autocisterna" v-on:salvato="onAutocisternaSaved"></editazione-autocisterna-modal>

		<!-- modale errore generico -->
		<notification-dialog ref="errorDialog" :title="'Errore imprevisto'" :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'" v-on:ok="reload()"></notification-dialog>

		<!-- modale conferma salvataggio -->
		<notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Utente salvato correttamente'" v-on:ok="reload()"></notification-dialog>

		<!-- Pannello modale conferma eliminazione utente -->
		<confirm-dialog ref="confirmDeleteDialog" :title="'Conferma eliminazione'" :message="'Sei sicuro di procedere con l\'eliminazione?'" v-on:confirmed="onRemove()"></confirm-dialog>

		<!-- Pannello notifica eliminazione commessa -->
		<notification-dialog ref="removedDialog" :title="'Conferma eliminazione'" :message="'Utente eliminato correttamente'" v-on:ok="redirect()"></notification-dialog>

		<!-- modale conferma cambio password -->
		<notification-dialog ref="pwdChangedDialog" :title="'Conferma cambio password'" :message="'Password aggiornata correttamente'" v-on:ok="reload()"></notification-dialog>

		<div>
			<div class="container-fluid">
				<ul class="nav nav-tabs" id="tabWrapper">
					<li class="active">
						<a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
					</li>
					<li v-if="isAdmin">
						<a data-toggle="tab" class="nav-link" href="#password">Password</a>
					</li>
					<li v-if="utente.IdProfilo == 3">
						<a data-toggle="tab" class="nav-link" href="#allevamenti">Allevamenti</a>
					</li>
					<li v-if="utente.IdProfilo == 5">
						<a data-toggle="tab" class="nav-link" href="#autocisterne">Autocisterne</a>
					</li>
				</ul>

				<div class="tab-content">
					<!-- Tab dettaglio -->
					<div id="dettaglio" class="tab-pane fade show active">
						<div class="container-fluid">
							<!-- tipo profilo -->
							<div class="row form-group pt-5">
								<label class="offset-1 col-sm-1">Tipo profilo</label>
								<div class="col-sm-4">
									<select2 class="form-control" :disabled="utente.Id != 0" :options="profilo.Items" :value.sync="utente.IdProfilo" :value-field="'Value'" :text-field="'Text'" />
								</div>							

								<!-- Acquirente -->
								<label v-if="utente.IdProfilo == 7" class="col-sm-1">Acquirente</label>
								<div v-if="utente.IdProfilo == 7" class="col-sm-4">
									<select2 class="form-control" :options="acquirente.Items" :value.sync="utente.IdAcquirente" :value-field="'Value'" :text-field="'Text'" />
								</div>

								<!-- Cessionario -->
								<label v-if="utente.IdProfilo == 8" class="col-sm-1">Cessionario</label>
								<div v-if="utente.IdProfilo == 8" class="col-sm-4">
									<select2 class="form-control" :options="cessionario.Items" :value.sync="utente.IdCessionario" :value-field="'Value'" :text-field="'Text'" />
								</div>

								<!-- Destinatario -->
								<label v-if="utente.IdProfilo == 6" class="col-sm-1">Destinatario</label>
								<div v-if="utente.IdProfilo == 6" class="col-sm-4">
									<select2 class="form-control" :options="destinatario.Items" :value.sync="utente.IdDestinatario" :value-field="'Value'" :text-field="'Text'" />
								</div>

								<!-- Allevatore -->
								<label v-if="utente.IdProfilo == 3" class="col-sm-1">Tipo latte</label>
								<div v-if="utente.IdProfilo == 3" class="col-sm-4">
									<select2 class="form-control" :options="tipoLatte.Items" :value.sync="utente.IdTipoLatte" :value-field="'Value'" :text-field="'Text'" />
								</div>
							</div>

							<!-- ragione sociale / username -->
							<div class="row form-group">
								<label class="offset-1 col-sm-1">Ragione sociale</label>
								<div class="col-sm-4">
									<input type="text" class="form-control" v-model="utente.RagioneSociale" />
								</div>
								<label class="col-sm-1">Username</label>
								<div class="col-sm-4">
									<input type="text" class="form-control" v-model="utente.Username" />
								</div>
							</div>

							<!-- nome / cognome -->
							<div class="row form-group">
								<label class="offset-1 col-sm-1">Nome</label>
								<div class="col-sm-4">
									<input type="text" class="form-control" v-model="utente.Nome" />
								</div>
								<label class="col-sm-1">Cognome</label>
								<div class="col-sm-4">
									<input type="text" class="form-control" v-model="utente.Cognome" />
								</div>
							</div>

							<!-- sesso / p.iva/cf -->
							<div class="row form-group">
								<label class="offset-1 col-sm-1">Sesso</label>
								<div class="col-sm-4">
									<select2 class="form-control" :placeholder="'-'" :options="sesso.Items" :value.sync="utente.Sesso" :value-field="'Value'" :text-field="'Text'" />
								</div>
								<label class="col-sm-1">P. Iva / C.F.</label>
								<div class="col-sm-4">
									<input type="text" class="form-control" v-model="utente.PivaCF" />
								</div>
							</div>

							<!-- indirizzo / provincia / città -->
							<div class="row form-group">
								<label class="offset-1 col-sm-1">Indirizzo</label>
								<div class="col-sm-4">
									<input type="text" class="form-control" v-model="utente.Indirizzo" />
								</div>

								<label class="col-sm-1">Provincia</label>
								<div class="col-sm-1">
									<select2 class="form-control" :options="provincia.Items" :value.sync="utente.SiglaProvincia" :value-field="'Value'" :text-field="'Text'" v-on:value-changed="loadComuni" />
								</div>
								<div class="col-sm-3">
									<select2 class="form-control" :options="comune.Items" :value.sync="utente.IdComune" :value-field="'Value'" :text-field="'Text'" />
								</div>
							</div>

							<!-- telefono / cellulare -->
							<div class="row form-group">
								<label class="offset-1 col-sm-1">Telefono</label>
								<div class="col-sm-4">
									<input type="text" class="form-control" v-model="utente.Telefono" />
								</div>
								<label class="col-sm-1">Cellulare</label>
								<div class="col-sm-4">
									<input type="text" class="form-control" v-model="utente.Cellulare" />
								</div>
							</div>

							<!-- tenant -->
							<div class="row form-group">
								<label class="offset-1 col-sm-1">Tenant</label>
								<div class="col-sm-4">
									<select2 class="form-control" :disabled="tenant != 'all'" :options="tenants.Items" :value.sync="utente.Tenant" :value-field="'Value'" :text-field="'Text'" />
								</div>
							</div>							

							<!-- note -->
							<div class="row form-group">
								<label class="offset-1 col-sm-1">Note</label>
								<div class="col-sm-9">
									<textarea class="form-control" v-model="utente.Note" rows="3"></textarea>
								</div>
							</div>

							<!-- Annulla / Salva -->
							<div class="row pt-3 justify-content-center">
								<div class="col-10 text-right">
									<button class="btn btn-secondary mr-2" role="button" v-on:click="reload()">Annulla</button>
									<button class="btn btn-success" role="button" v-on:click="onSave()">Salva</button>
								</div>
							</div>
						</div>
					</div>

					<!-- Tab password -->
					<div id="password" class="tab-pane fade">
						<div class="container-fluid">
							<!-- password -->
							<div class="row form-group pt-5">
								<label class="offset-2 col-2">Nuova Password</label>
								<div class="col-4">
									<input type="password" class="form-control" v-model="password_1" />
								</div>
							</div>

							<!-- ripeti password -->
							<div class="row form-group">
								<label class="offset-2 col-2">Ripeti password</label>
								<div class="col-4">
									<input type="password" class="form-control" v-model="password_2" />
								</div>
							</div>

							<!-- Imposta password -->
							<div class="row pt-1 justify-content-center">
								<div class="col-4 text-right">
									<button :disabled="password_1 === '' || password_2 === '' || password_1 != password_2" class="btn btn-success" role="button" v-on:click="onChangePassword()">Imposta password</button>
								</div>
							</div>
						</div>
					</div>

					<!-- Tab allevamenti -->
					<div id="allevamenti" class="tab-pane fade">
						<div class="row justify-content-center">
							<div class="col-10 text-right pt-4">
								<button v-on:click="onAllevamentoAdd" class="btn btn-success mb-2">Aggiungi</button>
							</div>

							<div class="col-10">
								<table class="table table-bordered">
									<thead class="table table-hover table-striped table-bordered">
										<tr>
											<th scope="rol">Codice ASL</th>
											<th scope="rol">Indirizzo</th>
											<th scope="rol">CUAA</th>
											<th scope="rol"></th>
										</tr>
									</thead>
									<tbody>
										<tr v-for="(allevamento, index) in utente.Allevamenti" :key="index">
											<td>{{ allevamento.CodiceAsl }}</td>
											<td>{{ allevamento.IndirizzoAllevamento }}</td>
											<td>{{ allevamento.CUAA }}</td>
											<td>
												<div class="text-center">
													<a class="edit" title="Modifica" v-on:click="onAllevamentoEdit(allevamento)" style="cursor: pointer">
														<i class="far fa-edit"></i>
													</a>
												</div>
											</td>
										</tr>
									</tbody>
								</table>
							</div>
						</div>

						<!-- Annulla / Salva -->
						<div class="row pt-3 justify-content-center">
							<div class="col-10 text-right">
								<button class="btn btn-secondary mr-2" role="button" v-on:click="reload()">Annulla</button>
								<button class="btn btn-success" role="button" v-on:click="onSave()">Salva</button>
							</div>
						</div>
					</div>

					<!-- Tab autocisterne -->
					<div id="autocisterne" class="tab-pane fade">
						<div class="row justify-content-center">
							<div class="col-sm-10 pt-4">
								<button v-on:click="onAutocisternaAdd" class="btn btn-success float-right">Aggiungi</button>
							</div>

							<div class="col-10 pt-2">
								<table class="table table-bordered">
									<thead class="table table-hover table-striped table-bordered">
										<tr>
											<th scope="rol">Marca</th>
											<th scope="rol">Modello</th>
											<th scope="rol">Targa</th>
											<th scope="rol"></th>
										</tr>
									</thead>
									<tbody>
										<tr v-for="(autocisterna, index) in utente.Autocisterne" :key="index">
											<td>{{ autocisterna.Marca }}</td>
											<td>{{ autocisterna.Modello }}</td>
											<td>{{ autocisterna.Targa }}</td>
											<td>
												<div class="text-center">
													<a class="edit" title="Modifica" v-on:click="onAutocisternaEdit(autocisterna)" style="cursor: pointer">
														<i class="far fa-edit"></i>
													</a>
												</div>
											</td>
										</tr>
									</tbody>
								</table>
							</div>
						</div>

						<!-- Annulla / Salva -->
						<div class="row pt-3 justify-content-center">
							<div class="col-10 text-right">
								<button class="btn btn-secondary mr-2" role="button" v-on:click="reload()">Annulla</button>
								<button class="btn btn-success" role="button" v-on:click="onSave()">Salva</button>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import * as jquery from "jquery";

import ConfirmDialog from "../../components/confirmDialog.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import EditazioneAllevamentoModal from "./editAllevamento.vue";
import EditazioneAutocisternaModal from "./editAutocisterna.vue";
import Waiter from "../../components/waiter.vue";
import Select2 from "../../components/select2.vue";
import { UtentiService } from "@/services/utenti.service";
import { DropdownService } from "@/services/dropdown.service";
import { Utente } from "@/models/utente.model";
import { UrlService } from "@/services/url.service";
import { AuthorizationsService } from "@/services/authorizations.service";
import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { Allevamento } from "../../models/allevamento.model";
import { Autocisterna } from "../../models/autocisterna.model";
import { ChangePassword } from "../../models/changePassword.model";

declare module "vue/types/vue" {
	interface Vue {
		open(): void;
		openAllevamento(allevamento: Allevamento): void;
		close(): void;
	}
}

@Component({
	components: {
		Select2,
		ConfirmDialog,
		Waiter,
		NotificationDialog,
		EditazioneAllevamentoModal,
		EditazioneAutocisternaModal,
	},
})
export default class App extends Vue {
	$refs: any = {
		savedDialog: Vue,
		pwdChangedDialog: Vue,
		errorDialog: Vue,
		waiter: Vue,
		confirmDeleteDialog: Vue,
		editazioneAllevamentoModal: Vue,
	};

	public itemNotFound: boolean = false;
	public isReadOnly: boolean = false;
	public isAdmin: boolean = false;
	public btnDeleteVisible: boolean = false;
	public tenant: string = "";

	public utentiService: UtentiService = new UtentiService();
	public dropdownService: DropdownService = new DropdownService();

	public utente: Utente = new Utente();
	public allevamento: Allevamento = new Allevamento();
	public autocisterna: Autocisterna = new Autocisterna();

	public profilo: Dropdown = new Dropdown();
	public sesso: Dropdown = new Dropdown();
	public provincia: Dropdown = new Dropdown();
	public comune: Dropdown = new Dropdown();
	public acquirente: Dropdown = new Dropdown();
	public cessionario: Dropdown = new Dropdown();
	public destinatario: Dropdown = new Dropdown();
	public tipoLatte: Dropdown = new Dropdown();
	public tenants: Dropdown = new Dropdown();

	public password_1: string = "";
	public password_2: string = "";

	constructor() {
		super();
	}

	public mounted() {
		this.readPermissions();

		var id = UrlService.getUrlParameter("id");
		if (id) {
			this.load(id);
		} else {
			this.utente = new Utente();
			this.utente.Tenant = AuthorizationsService.getCurrentTenant();
		}
		this.loadDropdown();

		this.keepSelectedTabOnRefresh();
	}

	// caricamento commessa
	private load(id: string) {
		this.$refs.waiter.open();
		this.utentiService.details(id).then((response) => {
			if (response.data != null) {
				this.utente = response.data;
				this.loadComuni(this.utente.SiglaProvincia);
			} else {
				this.itemNotFound = true;
			}

			this.$refs.waiter.close();
		});
	}

	// salvataggio commessa
	public onSave() {
		this.$refs.waiter.open();

		this.utentiService.save(this.utente).then(
			(response) => {
				this.utente = response.data;
				this.$refs.waiter.close();
				this.$refs.savedDialog.open();
			},
			(error) => {
				this.$refs.waiter.close();
				if (error.response.status == 400) {
					// Bad Request => messaggi di validazione
					this.$refs.validationDialog.openDialog(error.response.data.ModelState);
				} else {
					this.$refs.errorDialog.open();
				}
			}
		);
	}

	// cambio password
	public onChangePassword() {
		this.$refs.waiter.open();

		var model = new ChangePassword();
		model.Username = this.utente.Username;
		model.Password = this.password_1;
		model.RePassword = this.password_2;

		this.utentiService.changePassword(model).then((response) => {
			this.$refs.waiter.close();
			this.$refs.pwdChangedDialog.open();
		});
	}

	// caricamento dropdown
	private loadDropdown() {
		// sesso
		this.sesso.Items.push(new DropdownItem("M", "Maschio"));
		this.sesso.Items.push(new DropdownItem("F", "Femmina"));

		this.dropdownService.getDropdowns("acquirenti|cessionari|destinatari|province|tipiLatte|profili").then((response) => {
			this.acquirente = response.data["acquirenti"] as Dropdown;
			this.cessionario = response.data["cessionari"] as Dropdown;
			this.destinatario = response.data["destinatari"] as Dropdown;
			this.provincia = response.data["province"] as Dropdown;
			this.tipoLatte = response.data["tipiLatte"] as Dropdown;
			this.profilo = response.data["profili"] as Dropdown;
		});

		this.tenants = this.dropdownService.getTenants();
	}

	public loadComuni(provincia: string): void {
		this.dropdownService.getComuni(provincia).then((response) => {
			if (response.data != null) {
				this.comune = response.data;
			}
		});
	}

	// Mantengo la tab selezionata per il refresh della pagina
	public keepSelectedTabOnRefresh() {
		$("ul.nav-tabs > li > a").on("shown.bs.tab", function(e) {
			window.location.hash = String($(e.target).attr("href"));
		});

		$('#tabWrapper a[href="' + window.location.hash + '"]').tab("show");
	}

	// elimina utente
	public onRemove() {
		this.utentiService.delete(this.utente.Id).then((response) => {
			this.$refs.removedDialog.open();
		});
	}

	// popup nuovo allevamento
	public onAllevamentoAdd() {
		this.allevamento = new Allevamento();
		this.allevamento.IdUtente = this.utente.Id;
		this.$refs.editazioneAllevamentoModal.openAllevamento(this.allevamento);
	}

	// editazione allevamento
	public onAllevamentoEdit(allevamento: Allevamento) {
		this.allevamento = allevamento;
		this.$refs.editazioneAllevamentoModal.openAllevamento(this.allevamento);
	}

	// evento conferma salvataggio allevamento
	public onAllevamentoSaved() {
		if (this.allevamento.Id == 0) {
			this.utente.Allevamenti.push(this.allevamento);
		}
	}

	// popup nuova autocisterna
	public onAutocisternaAdd() {
		this.autocisterna = new Autocisterna();
		this.autocisterna.IdTrasportatore = this.utente.Id;
		this.$refs.editazioneAutocisternaModal.open();
	}

	// editazione autocisterna
	public onAutocisternaEdit(autocisterna: Autocisterna) {
		this.autocisterna = autocisterna;
		this.$refs.editazioneAutocisternaModal.open();
	}

	// evento conferma salvataggio autocisterna
	public onAutocisternaSaved() {
		if (this.autocisterna.Id == 0) {
			this.utente.Autocisterne.push(this.autocisterna);
		}
	}

	// lettura permessi da jwt
	private readPermissions() {
		this.isAdmin = AuthorizationsService.getCurrentRole() == "Admin";

		this.isReadOnly = !AuthorizationsService.isViewItemAuthorized("Utenti", "Edit", "Edit");
		this.btnDeleteVisible = AuthorizationsService.isViewItemAuthorized("Utenti", "Edit", "Delete");
		this.tenant = AuthorizationsService.getCurrentTenant();
	}

	// reload della pagina sullo stesso id
	public reload() {
		UrlService.redirect("/utenti/edit?id=" + this.utente.Id);
	}
}
</script>
