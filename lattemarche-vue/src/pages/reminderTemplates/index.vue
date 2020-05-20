<template>
  
<div id="templates-page">

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="templates" >

        <!-- Toolbox -->
        <template slot="toolbox" >
            <button class="toolbox btn btn-primary float-right" v-on:click="redirect()">Aggiungi</button>
        </template>

        <!-- Colonne -->
        <template slot="thead">
            <th>Nome</th>
            <th></th>
        </template>

    </data-table>

</div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import $ from 'jquery';

import { Template } from "../../models/ir/template.model";
import { TemplatesService } from "../../services/ir/templates.service";

import DataTable from "../../components/dataTable.vue";

@Component({
  components: {
    DataTable
  }
})
export default class App extends Vue {

    private templatesService: TemplatesService;

    public templates: Template[] = [];
    public tableOptions: any = {};

    constructor() {
        super();

        this.templatesService = new TemplatesService();
    }

    public mounted() {
        this.initTable();

        this.templatesService.index()
            .then(response => {
                this.templates = response.data;
            });   
    }

    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Name" });

        options.columns.push({
            render: function (data: any, type: any, row: any) {                

                var html = '<div class="text-center">';
                html += '<a title="dettaglio text-primary" style="cursor: pointer;" href="/reminderTemplates/edit?id=' + row.Id + '" ><i class="far fa-edit"></i></i></a>';
                html += '</div>';

                return html;
            },
            orderable: false
        });

        this.tableOptions = options;

    }

    public redirect() {
        window.location.assign('/reminderTemplates/edit');
    }

}
</script>