<template>
    <div class="modal fade" id="status-history-dialog" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" >Informazioni stato documento</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <table class="offset-1 col-10 table table-striped table-hover table-bordered">
                        <thead class="thead-dark" >
                            <tr>
                                <th>Stato</th>
                                <th>Data</th>
                                <th>Utente</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="transition in transitions" :key="transition.Id" >
                                <td><span>{{transition.To_Status_Code}}</span></td>
                                <td><span>{{transition.LastChangeTimestmap_Str}}</span></td>
                                <td><span>{{transition.Author}}</span></td>
                            </tr>
                        </tbody>
                    </table>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Ok</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">

    import Vue from "vue";
    import $ from 'jquery';
    import * as bootstrap from 'bootstrap';
    import 'bootstrap/js/dist/modal';
    import Component from "vue-class-component";
    import { Prop, Watch, Emit } from "vue-property-decorator";
    import { DocumentTransition } from '@/models/documentTransition.model';

    @Component
    export default class StatusHistoryDialog extends Vue {

        @Prop() public transitions!: DocumentTransition[];

        mounted() {
            let vd = this;
            document.addEventListener("keyup", function (e: any) {
                if (e.keyCode === 27 || e.keyCode === 13) {
                    vd.close();
                }

            });
        }

        public open(): void {
            console.log("transitions", this.transitions);
            $(this.$el).modal('show');
        }

        public close(): void {
            $(this.$el).modal('hide');
        }

    }

</script>

