<template>

    <div>

        <data-table :options="tableOptions" :rows="viewItems" >
            
            <template slot="thead">
                <th>Type</th>
                <th>Controller</th>
                <th>Action</th>
                 <th>ViewItem</th>
                <th>Pagina</th>
                <th>Nome</th>
            </template>

        </data-table>

    </div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";

import { ViewItem } from '../../models/viewItem.model';
import { ViewItemsService } from '../../services/viewItems.service';

@Component({
  components: {
    DataTable
  }
})
export default class App extends Vue {

    public viewItems: ViewItem[] = [];
    public viewItemsService: ViewItemsService;
     public tableOptions: any = {};

    constructor() {
        super();

        this.viewItemsService = new ViewItemsService();
    }

    public mounted() {

        this.initTable();

        this.viewItemsService.index()
            .then(response => {
                this.viewItems = response.data;
            });
    }


    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Type" });
        options.columns.push({ data: "Controller" });
        options.columns.push({ data: "Action" });
        options.columns.push({ data: "Name" });
        options.columns.push({ data: "Page" });
        options.columns.push({ data: "DisplayName" });

        this.tableOptions = options;

    }

}
</script>