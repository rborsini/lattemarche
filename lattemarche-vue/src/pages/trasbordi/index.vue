<template>
	<div class="container-fluid p-0">
		<!-- waiter -->
		<waiter ref="waiter"></waiter>

		<!-- Box ricerca -->
		<div class="jumbotron">
			<!-- dal / al -->
			<div class="row pt-1">
				<label class="col-1">Dal:</label>
				<div class="col-1">
					<datepicker v-on:value-changed="onParametersChanged" class="form-control" :value.sync="parameters.DataInizio_Str" />
				</div>

				<label class="col-1">Al:</label>
				<div class="col-1">
					<datepicker v-on:value-changed="onParametersChanged" class="form-control" :value.sync="parameters.DataFine_Str" />
				</div>

                <div class="col-8">
                    <button class="btn btn-primary" v-on:click="search" >Cerca</button>
                </div>
			</div>
		</div>

		<!-- Tabella -->
		<data-table ref="table" class="pt-4" :options="tableOptions" :rows="trasbordi" v-on:data-loaded="onDataLoaded">
			<!-- Colonne -->
			<template slot="thead">
				<th>Targa origine</th>
				<th>Targa destinazione</th>
				<th>Data</th>
				<th></th>
			</template>
		</data-table>
	</div>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

// components
import Datepicker from "../../components/datepicker.vue";
import DataTable from "../../components/dataTable.vue";
import Waiter from "../../components/waiter.vue";

// models
import { TrasbordiSearchModel, Trasbordo } from "@/models/trasbordo.model";

// services
import { TrasbordiService } from "../../services/trasbordi.service";

@Component({
	components: {
		Waiter,
		DataTable,
        Datepicker
	},
})
export default class UtentiIndexPage extends Vue {
	$refs: any = {
		savedDialog: Vue,
		confirmDeleteDialog: Vue,
		waiter: Vue,
		removedDialog: Vue,
		table: Vue,
	};

    public trasbordi: Trasbordo[] = [];

	private trasbordiService: TrasbordiService;

	public parameters: TrasbordiSearchModel = new TrasbordiSearchModel();

	public tableOptions: any = null;

	constructor() {
		super();

		this.trasbordiService = new TrasbordiService();
	}

	public mounted() {
        this.initTable();
        this.initSearchBox();  
		if (window.location.hash.length > 0) {
			this.parameters.decodeUrl(window.location.hash);
		}
	}

	// evento modifica parametri ricerca
	private onParametersChanged() {
		window.location.hash = this.parameters.toUrlQueryString();
	}

	public search() {
		this.$refs.waiter.open();

        this.trasbordiService.search(this.parameters).then(response => {
            console.log("response.data", response.data);
            this.trasbordi = response.data;
            this.$refs.waiter.close();
        });
	}

	// Evento fine generazione tabella
	public onDataLoaded() {
		this.$refs.waiter.close();
	}

	// inizializzazione tabella
	private initTable(): void {
		var options: any = {};

		options.serverSide = false;
		options.columns = [];

		options.columns.push({ data: "Targa_Origine" });
		options.columns.push({ data: "Targa_Destinazione" });
		options.columns.push({ data: "Data_Str" });

		options.columns.push({
			render: function(data: any, type: any, row: any) {
				var html = '<div class="text-center">';
				html += '<a class="edit" title="Modifica" href="/trasbordi/edit?id=' + row.Id + '" ><i class="far fa-edit"></i></a>';
				html += "</div>";

				return html;
			},
			className: "edit-column",
			orderable: false,
		});

		this.tableOptions = options;
	}

    // inizializzazione parametri di ricerca
    private initSearchBox() {
        // this.parameters.clear();
        this.parameters = new TrasbordiSearchModel();
        this.parameters.DataFine_Str = this.formatDate(new Date());
        this.parameters.DataInizio_Str = this.formatDate(this.subtractMonth(new Date()));
    }    

  private formatDate(date: Date): string {
    var returnDate = "";

    var dd = date.getDate();
    var mm = date.getMonth() + 1; //because January is 0!
    var yyyy = date.getFullYear();

    if (dd < 10) {
      returnDate += `0${dd}-`;
    } else {
      returnDate += `${dd}-`;
    }

    if (mm < 10) {
      returnDate += `0${mm}-`;
    } else {
      returnDate += `${mm}-`;
    }
    returnDate += yyyy;
    return returnDate;
  }    

  private subtractMonth(date: Date): Date {
    var days = 0;
    //get month ritorna un numero da 0 a 11. Dovendo considerare il mese precedente
    //da sottrarre, per comodità aggiungo un numero al case, considerando quindi lo 0 come dicembre
    switch (date.getMonth()) {
      case 4: //Aprile
      case 6: //Giugno
      case 9: //Settembre
      case 11: {
        //Novembre
        days = 30;
        break;
      }
      case 2: {
        //febbraio
        if (date.getFullYear() % 4 != 0) {
          //anno non bisestile
          days = 28;
        } else {
          days = 29;
        }
        break;
      }
      default: //altri mesi
      {
        days = 31;
        break;
      }
    }
    date.setDate(date.getDate() - days);
    return date;
  }

}
</script>
