<template>
    <div class="modal fade" id="validation-messages-dialog" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <!-- validationDialog -->
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">{{title}}</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div class="row form-group">
                        <dl class="pt-4 pl-4 ">
                            <dd v-for="(message, index) in messages" :key="index">
                                <h5><i class="fa fa-warning red"></i>&nbsp<strong><span class="text-danger">{{message}}</span></strong></h5>
                            </dd>
                        </dl>
                    </div>

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

    @Component
    export default class ValidationDialog extends Vue {

        public messages: string[] = [];

        @Prop() private title!: string;

        mounted() {
            let vd = this;
            document.addEventListener("keyup", function (e: any) {
                if (e.keyCode === 27 || e.keyCode === 13) {
                    vd.close();
                }

            });
        }


        public openDialog(modelState: any): void {

            this.messages = [];

            var properties = Object.getOwnPropertyNames(modelState);
            for(var i = 0; i < properties.length; i++) {
                var propertyName = properties[i];
                this.messages.push(modelState[propertyName][0]);
            }

            $(this.$el).modal('show');
        }

        public close(): void {
            $(this.$el).modal('hide');
        }

    }

</script>
