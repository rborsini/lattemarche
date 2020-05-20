<template>
  
    <div class="container-fluid">

        <!-- Tabella -->
        <data-table :options="columnOptions" :rows="articles">
            <!-- Toolbox -->
            <template slot="toolbox">
                <div class="toolbox">
                    <button v-if="btnExcelVisible" class="btn btn-primary float-right mr-3" v-on:click="downloadExcel()">Esporta excel</button>
                </div>
            </template>
            
            <!-- Colonne -->
            <template slot="thead">
                <th>Id</th>
                <th>Descrizione</th>
                <th>Categoria</th>
                <th>Sotto categoria</th>
                <th>UOM</th>
            </template>

        </data-table>

    </div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";

import DataTable from "../../components/dataTable.vue";

import { Article } from "../../models/article.model";
import { ArticlesService } from "../../services/articles.service";
import { UrlService } from '@/services/url.service';
import { PermissionsService } from '@/services/permissions.service';

@Component({
  components: {
    DataTable
  }
})
export default class App extends Vue {

    public articles: Article[] = [];
    private articlesService: ArticlesService;
    public columnOptions: any[] = [];

      public btnExcelVisible: boolean = false;

    constructor() {
        super();
        this.articlesService = new ArticlesService();
    }

    public mounted() {

        this.btnExcelVisible = PermissionsService.isViewItemAuthorized("Articles", "Excel", "Excel", "API");    

        this.initTable();

        this.articlesService.index()
            .then(response => {
                this.articles = response.data;
            });
    }

    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Id" });
        options.columns.push({ data: "Description" });
        options.columns.push({ data: "Category" });
        options.columns.push({ data: "SubCategory" });
        options.columns.push({ data: "UOM" });

        this.columnOptions = options;

    }
    //Esportazione excell
    public downloadExcel() {
        UrlService.redirect('/api/articles/excel');
    }

}
</script>