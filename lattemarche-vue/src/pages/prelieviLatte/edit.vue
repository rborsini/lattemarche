<template>
  <div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <!-- modale errore generico -->
    <notification-dialog ref="errorDialog" :title="'Errore imprevisto'" :message="'Si è verificato un errore imprevisto, contattare l\'amministratore del sistema'" v-on:ok="reload()" ></notification-dialog>

    <!-- modale degli errori -->
    <validation-dialog ref="validationDialog" :title="'Operazione non riuscita'" ></validation-dialog>    

    <!-- modale conferma salvataggio -->
    <notification-dialog ref="savedDialog" :title="'Conferma salvataggio'" :message="'Prelievo salvato correttamente'" v-on:ok="reload()" ></notification-dialog>

    <div>
      <div class="container-fluid">

        <ul class="nav nav-tabs" id="tabWrapper">
          <li class="active">
            <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
          </li>
          <li v-if="isMapVisible" >
            <a data-toggle="tab" class="nav-link" href="#mappa">Mappa</a>
          </li>             
        </ul>

        <div class="tab-content">

          <!-- Tab dettaglio -->
          <div id="dettaglio" class="tab-pane fade show active">
            <div class="container-fluid">

              <!-- Allevamento / Acquirente -->
              <div class="row form-group pt-5">

                <div class="offset-1 col-5 row" >
                  <label class="col-2">Allevamento</label>
                  <div class="col-10">
                    <select2 class="form-control" :disabled="isReadOnly" :options="allevatore.Items" :value.sync="prelievoLatte.IdAllevamento" :value-field="'Value'" :text-field="'Text'" />                  
                  </div>
                </div>

                <div class="col-5 row" >
                  <label class="col-2">Acquirente</label>
                  <div class="col-10">
                    <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="acquirente.Items" :value.sync="prelievoLatte.IdAcquirente" :value-field="'Value'" :text-field="'Text'" />
                  </div>                  
                </div>                

              </div>

              <!-- Cessionario / Destinatario -->
              <div class="row form-group">

                <div class="offset-1 col-5 row" >
                  <label class="col-2">Cessionario</label>
                  <div class="col-10">
                    <select2 class="form-control" :disabled="isReadOnly" :options="cessionario.Items" :value.sync="prelievoLatte.IdCessionario" :value-field="'Value'" :text-field="'Text'" />                  
                  </div>
                </div>

                <div class="col-5 row" >
                  <label class="col-2">Destinatario</label>
                  <div class="col-10">
                    <select2 class="form-control" :disabled="isReadOnly" :placeholder="'-'" :options="destinatario.Items" :value.sync="prelievoLatte.IdDestinatario" :value-field="'Value'" :text-field="'Text'" />
                  </div>                  
                </div>                

              </div>              

              <!-- Data prelievo / Data ult. mungitura -->
              <div class="row form-group">
                              
                <div class="offset-1 col-5 row" >

                  <label class="col-2">Data prelievo</label>
                  <div class="col-4">
                    <datepicker :disabled="isReadOnly" :value.sync="prelievoLatte.DataPrelievoStr" />
                    <div class="invalid-feedback">Inserire data prelievo.</div>
                  </div>

                  <label class="col-2">Ora prelievo</label>
                  <div class="col-4">
                    <time-editor :disabled="isReadOnly" v-model="prelievoLatte.OraPrelievo"></time-editor>
                  </div>

                </div>

                <div class="col-5 row" >
                  <label class="col-2">Data ult. mung.</label>
                  <div class="col-4">
                    <datepicker :disabled="isReadOnly" :value.sync="prelievoLatte.DataUltimaMungituraStr" />
                    <div class="invalid-feedback">Inserire data ultima mungitura.</div>
                  </div>

                  <label class="col-2">Ora ult. mung.</label>
                  <div class="col-4">
                    <time-editor :disabled="isReadOnly" v-model="prelievoLatte.OraUltimaMungitura"></time-editor>
                  </div>                  
                </div> 

              </div>

              <!-- Num mungiture / Temp / Quantità -->
              <div class="row form-group">

                <div class="offset-1 col-5 row" >
                  <label class="col-2">Num. mungiture</label>
                  <div class="col-4">
                    <input type="number" :disabled="isReadOnly" min="0" class="form-control" v-model="prelievoLatte.NumeroMungiture" />
                  </div>

                  <label class="col-2">Temp. in C°</label>
                  <div class="col-4">
                    <input type="number" :disabled="isReadOnly" min="-20" class="form-control" v-model="prelievoLatte.Temperatura" />
                  </div>  
                </div>

                <div class="col-5 row" >
                  <label class="col-2">Qta in Kg</label>
                  <div class="col-4">
                    <input type="number" :disabled="isReadOnly" min="0" class="form-control" v-model="prelievoLatte.Quantita" />
                  </div>                  
                  <label class="col-2">Lab. analisi</label>
                  <div class="col-4">
                    <select2 class="form-control" :disabled="isReadOnly" :options="laboratoriAnalisi.Items" :value.sync="prelievoLatte.IdLabAnalisi" :value-field="'Value'" :text-field="'Text'" />
                  </div>                  
                </div>

              </div>                    


              <!-- Trasportatore / Targa  / Lotto / Scomparto -->
              <div class="row form-group">
                
                <div class="offset-1 col-5 row" >
                  <label class="col-2">Trasportatore</label>
                  <div class="col-4">
                    <select2 class="form-control" :disabled="isReadOnly" :options="trasportatore.Items" :value.sync="prelievoLatte.IdTrasportatore" v-on:value-changed="loadAutocisterne" :value-field="'Value'" :text-field="'Text'" />
                  </div>

                  <label class="col-2">Targa mezzo</label>
                  <div class="col-4">
                    <select2 class="form-control" :disabled="isReadOnly" :options="autocisterna.Items" :value.sync="prelievoLatte.IdAutocisterna" :value-field="'Value'" :text-field="'Text'" />
                  </div>
                </div>

                <div class="col-5 row" >
                  <label class="col-2">Scomparto</label>
                  <div class="col-4">
                    <input type="text" class="form-control" :disabled="isReadOnly" v-model="prelievoLatte.Scomparto" />
                  </div>       
                  <label class="col-2">Dispositivo</label>
                  <div class="col-4">
                    <input disabled type="text" class="form-control" :disabled="isReadOnly" v-model="prelievoLatte.DeviceId" />
                  </div>                                 
                </div>

              </div>                       

              <!-- data/ora consegna -->
              <div class="row form-group">

                <div class="offset-1 col-5 row" >
                  <label class="col-2">Data consegna</label>
                  <div class="col-4">
                    <datepicker :disabled="isReadOnly" :value.sync="prelievoLatte.DataConsegnaStr" />
                    <div class="invalid-feedback">Inserire data consegna.</div>
                  </div>

                  <label class="col-2">Ora consegna</label>
                  <div class="col-4">
                    <time-editor :disabled="isReadOnly" v-model="prelievoLatte.OraConsegna"></time-editor>
                  </div>                  
                </div>

                <div class="col-5 row" >
                  <label class="col-2">Lotto di consegna</label>
                  <div class="col-4">
                    <input type="text" :disabled="isReadOnly" class="form-control" v-model="prelievoLatte.LottoConsegna" />
                  </div>       
                  <label class="col-2">Codice Sitra</label>
                  <div class="col-4">
                    <input type="text" :disabled="isReadOnly" class="form-control" v-model="prelievoLatte.CodiceSitra" />
                  </div>                                
                </div>  

              </div>                    


              <!-- Annulla / Salva -->
              <div v-if="!isReadOnly" class="row pt-3 justify-content-center">
                <div class="col-10 text-right">
                  <button class="btn btn-secondary mr-2" role="button" v-on:click="reload()">Annulla</button>
                  <button class="btn btn-success" role="button" v-on:click="onSave()">Salva</button>
                </div>
              </div>

            </div>
          </div>

          <!-- Tab mappa -->
          <div id="mappa" class="tab-pane fade">
            <div class="container-fluid">

              <!-- Lat / Lng -->
              <div class="row pt-3 form-group">
                
                <div class="offset-1 col-5" >
                  <div class="row">
                    <label class="col-2"><b>Prelievo</b></label>
                    <label class="col-1">Lat</label>
                    <div class="col-4">
                      <input :disabled="isReadOnly" type="number" class="form-control" v-model="prelievoLatte.Lat" />
                    </div>
                    <label class="col-1">Lng</label>
                    <div class="col-4">
                      <input :disabled="isReadOnly" type="number" class="form-control" v-model="prelievoLatte.Lng" />
                    </div>
                  </div>

                  <div class="row pt-1">
                    <label class="col-2"><b>Allevamento</b></label>
                    <label class="col-1">Lat</label>
                    <div class="col-4">
                      <input disabled type="number" class="form-control" v-model="prelievoLatte.Allevamento_Lat" />
                    </div>
                    <label class="col-1">Lng</label>
                    <div class="col-4">
                      <input disabled type="number" class="form-control" v-model="prelievoLatte.Allevamento_Lng" />
                    </div>                        
                  </div>                                    
                </div>
                
                <div class="col-5 row pt-3">
                  <label class="col-2"><b>Distanza</b></label>
                  <span class="col-6 pt-1">{{prelievoLatte.DistanzaAllevamento_Str}}</span>
                </div>

              </div>   
              

              <!-- Mappa -->
              <div class="row">
                <div class="offset-1 col-10">
                  <mappa-prelievi ref="mapViewer" style="height: 600px" />
                </div>                
              </div>
              
              <!-- Annulla / Salva -->
              <div v-if="!isReadOnly" class="row pt-3 justify-content-center">
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
  </div>
</template>
<script lang="ts">

import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";

import Waiter from "../../components/waiter.vue";
import Select2 from "../../components/select2.vue";
import Datepicker from "../../components/datepicker.vue";
import TimeEditor from "../../components/timeEditor.vue";
import NotificationDialog from "../../components/notificationDialog.vue";
import ValidationDialog from "../../components/validationDialog.vue";
import MappaPrelievi from "../../components/mappaPrelievi.vue";

import { Dropdown, DropdownItem } from "../../models/dropdown.model";
import { PrelievoLatte } from "../../models/prelievoLatte.model";

import { PrelieviLatteService } from "../../services/prelieviLatte.service";
import { DropdownService } from "../../services/dropdown.service";
import { AuthorizationsService } from '@/services/authorizations.service';
import { UrlService } from '@/services/url.service';
import { Marker, Position } from '@/models/map.model';

@Component({
  components: {
    Select2,
    Waiter,
    Datepicker,
    TimeEditor,
    NotificationDialog,
    ValidationDialog,
    MappaPrelievi    
  }
})
export default class EditazionePrelievoModal extends Vue {
  $refs: any = {
      waiter: Vue,
      savedDialog: Vue,
      errorDialog: Vue,
      validationDialog: Vue,   
      mapViewer: Vue
  };

  public itemNotFound: boolean = false;
  public isReadOnly: boolean = true;
  public isMapVisible: boolean = false;

  public prelievoLatte: PrelievoLatte = new PrelievoLatte();

  public prelieviLatteService: PrelieviLatteService;
  public dropdownService: DropdownService;

  public laboratoriAnalisi: Dropdown = new Dropdown();
  public allevatore: Dropdown = new Dropdown();
  public acquirente: Dropdown = new Dropdown();
  public autocisterna: Dropdown = new Dropdown();
  public cessionario: Dropdown = new Dropdown();
  public destinatario: Dropdown = new Dropdown();
  public trasportatore: Dropdown = new Dropdown();

  public id: string = "";
  public showMap: boolean = false;

  constructor() {
    super();
    this.prelieviLatteService = new PrelieviLatteService();
    this.dropdownService = new DropdownService();    
  }

  mounted() {
    this.readPermissions();
    this.loadDropdown();

    this.id = UrlService.getUrlParameter("id");
    if(this.id) {
      this.load(this.id);
    }
    this.keepSelectedTabOnRefresh();
  }

  // caricamento prelievo
  private load(id: string) {
    this.$refs.waiter.open();
    this.prelieviLatteService.details(id).then(response => {
      if (response.data != null) {
        this.prelievoLatte = response.data;
        this.loadAutocisterne(this.prelievoLatte.IdTrasportatore);
        this.initMap(response.data);
      } else {
        this.itemNotFound = true;
      }

      this.$refs.waiter.close();
    });
  }

  // inizializzazione mappa
  private initMap(prelievo: PrelievoLatte) {
    var center = new Position(43, 13);
    this.$refs.mapViewer.initMap(center, 8, [prelievo]);
  }

  // load dropdown
  private loadDropdown() {
    this.dropdownService.getDropdowns("acquirenti|allevatori|cessionari|destinatari|laboratoriAnalisi|trasportatori")
      .then(response => {
        this.acquirente = response.data["acquirenti"] as Dropdown;
        this.allevatore = response.data["allevatori"] as Dropdown;
        this.cessionario = response.data["cessionari"] as Dropdown;
        this.destinatario = response.data["destinatari"] as Dropdown;
        this.laboratoriAnalisi = response.data["laboratoriAnalisi"] as Dropdown;
        this.trasportatore = response.data["trasportatori"] as Dropdown;
      });
  }

  private loadAutocisterne(idTrasportatore: number) {
    this.dropdownService.getAutocisterne(idTrasportatore).then(response => {
      if (response.data != null) {
        this.autocisterna = response.data;
      }
    });    
  }

  public onSave() {
    this.$refs.waiter.open();
    this.prelieviLatteService.save(this.prelievoLatte).then(
      (response) => {
        this.$refs.waiter.close();
        this.$refs.savedDialog.open();
      },
      (error) => {
        this.$refs.waiter.close();
        if (error.response.status == 400) {
          this.$refs.validationDialog.openDialog(error.response.data.ModelState);
        } else {
          this.$refs.errorDialog.open();
        }
      }
    );
  }

  // lettura permessi da jwt
  private readPermissions() {
    this.isReadOnly = !AuthorizationsService.isViewItemAuthorized("Prelievi","Edit","Save");
    this.isMapVisible = AuthorizationsService.isViewItemAuthorized("Prelievi","Edit","Mappa");
  }  

  // reload della pagina sullo stesso id
  public reload() {
    UrlService.redirect('/prelievi/edit?id=' + this.id);
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