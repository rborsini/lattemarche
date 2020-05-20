<template>
  
<div id="notifications-page">

    <!-- Tabella -->
    <data-table :options="tableOptions" :rows="notifications" >

        <!-- Colonne -->
        <template slot="thead">
            <th>Livello</th>
            <th>Username</th>
            <th>Data</th>
            <th>Titolo</th>
            <th>Testo</th>
            <th>Letta</th>
        </template>

    </data-table>

</div>

</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator";
import $ from 'jquery';

import DataTable from "../../components/dataTable.vue";

import { Notification } from "../../models/notification.model";
import { NotificationsService } from "../../services/notifications.service";
import { UrlService } from '@/services/url.service';

@Component({
  components: {
    DataTable
  }
})
export default class App extends Vue {

    private notificationsService: NotificationsService;

    public notifications: Notification[] = [];
    public tableOptions: any = {};

    constructor() {
        super();

        this.notificationsService = new NotificationsService();
    }

    public mounted() {
        this.initTable();    

        this.notificationsService.getAll(UrlService.getUrlParameter("username"))
            .then(response => {
                this.notifications = response.data;
            });   
    }

    // inizializzazione tabella
    private initTable(): void {

        var options: any = {};
        options.columns = [];

        options.columns.push({ data: "Level" });
        options.columns.push({ data: "Username" });
        options.columns.push({ data: "TimestampStr", type: "date-eu" });
        options.columns.push({ data: "Subject" });
        options.columns.push({ data: "Body" });
        options.columns.push({ data: "Hidden" });

        this.tableOptions = options;

    }

}
</script>