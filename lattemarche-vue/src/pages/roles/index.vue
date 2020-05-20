<template>
  
<div id="roles-page">

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="roles" >

        <!-- Toolbox -->
        <template slot="toolbox" >
            <button class="toolbox btn btn-primary float-right" v-on:click="redirect()">Aggiungi</button>
        </template>

        <!-- Colonne -->
        <template slot="thead">
            <th>Nome</th>
            <th>Descrizione</th>
            <th></th>
        </template>

    </data-table>

</div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import $ from 'jquery';

import DataTable from "../../components/dataTable.vue";

import { Role } from "../../models/role.model";
import { RolesService } from "../../services/roles.service";

@Component({
  components: {
    DataTable
  }
})
export default class App extends Vue {

    private rolesService: RolesService;

    public roles: Role[] = [];
    public tableOptions: any = {};

    constructor() {
        super();

        this.rolesService = new RolesService();
    }

    public mounted() {
        this.initTable();

        this.rolesService.getRoles()
            .then(response => {
                this.roles = response.data;
            });   
    }

    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Code" });
        options.columns.push({ data: "Description" });

        options.columns.push({
            render: function (data: any, type: any, row: any) {                

                var html = '<div class="text-center">';
                html += '<a title="dettaglio text-primary" style="cursor: pointer;" href="/roles/edit?id=' + row.Id + '" ><i class="far fa-edit"></i></i></a>';
                html += '</div>';

                return html;
            },
            orderable: false
        });

        this.tableOptions = options;

    }

    public redirect() {
        window.location.assign('/roles/new');
    }

}
</script>