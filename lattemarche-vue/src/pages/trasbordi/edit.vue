<template>
  <div>

    <!-- waiter -->
    <waiter ref="waiter"></waiter>

    <div>
      <div class="container-fluid">

        <ul class="nav nav-tabs" id="tabWrapper">
          <li class="active">
            <a data-toggle="tab" class="nav-link active" href="#dettaglio">Dettaglio</a>
          </li>            
        </ul>

        <div class="tab-content">

          <!-- Tab dettaglio -->
          <div id="dettaglio" class="tab-pane fade show active">
            <div class="container-fluid">

              <!-- Data / Giro -->
              <div class="row form-group pt-5">

                <div class="offset-1 col-5 row" >
                  <label class="col-2">Data</label>
                  <div class="col-10">
                    <input type="text" disabled class="form-control" v-model="trasbordo.Data_Str" />
                  </div>
                </div>

                <div class="col-5 row" >
                  <label class="col-2">Giro</label>
                  <div class="col-10">
                    <input type="text" disabled class="form-control" v-model="trasbordo.DenominazioneGiro" />
                  </div>                  
                </div>                

              </div>

              <!-- Targa origine / Targa destinatario -->
              <div class="row form-group">

                <div class="offset-1 col-5 row" >
                  <label class="col-2">Origine</label>
                  <div class="col-10">
                    <input type="text" disabled class="form-control" v-model="trasbordo.Targa_Origine" />
                  </div>
                </div>

                <div class="col-5 row" >
                  <label class="col-2">Destinazione</label>
                  <div class="col-10">
                    <input type="text" disabled class="form-control" v-model="trasbordo.Targa_Destinazione" />
                  </div>                  
                </div>                

              </div>

              <!-- Mappa -->
              <div class="row form-group">
                <div class="offset-1 col-10">
                  <map-viewer ref="mapViewer" style="height: 600px" />
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
import MapViewer from "../../components/map.vue";

import { UrlService } from '@/services/url.service';
import { Marker, Position } from '@/models/map.model';

import { Trasbordo } from "@/models/trasbordo.model";
import { TrasbordiService } from "@/services/trasbordi.service";

@Component({
  components: {
    Select2,
    Waiter,
    MapViewer    
  }
})
export default class EditazionePrelievoModal extends Vue {
  $refs: any = {
      waiter: Vue,
      mapViewer: Vue
  };

  public trasbordo: Trasbordo = new Trasbordo();

  public trasbordiService: TrasbordiService;

  public id: string = "";
  public showMap: boolean = false;

  constructor() {
    super();
    this.trasbordiService = new TrasbordiService();
  }

  mounted() {
    this.id = UrlService.getUrlParameter("id");
    if(this.id) {
      this.load(this.id);
    }
  }

  // caricamento trasbordo
  private load(id: string) {
    this.$refs.waiter.open();
    this.trasbordiService.details(id).then(response => {
        this.trasbordo = response.data;
        this.initMap(response.data);

        this.$refs.waiter.close();
    });
  }

  // inizializzazione mappa
  private initMap(trasbordo: Trasbordo) {
    var center = new Position(43, 13);
    var marker = new Marker(trasbordo.Lat, trasbordo.Lng, trasbordo.Data_Str);
    this.$refs.mapViewer.initMap(center, 8, [marker]);
  }


}
</script>