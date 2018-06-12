<template>

    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">{{title}}</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    {{message}}
                </div>
                <div class="modal-footer">
                    <button type="button" v-on:click="$emit('ok')" class="btn btn-primary" data-dismiss="modal">Ok</button>
                </div>
            </div>
        </div>
    </div>

</template>

<script lang="ts">

    import Vue from "vue";
    import Component from "vue-class-component"
    import { Prop, Watch, Emit } from "vue-property-decorator"

    @Component
    export default class NotificationDialog extends Vue {

        @Prop()
        public title: string = "";

        @Prop()
        public message: string = "";

        mounted() {
            let nd = this;
            document.addEventListener("keyup", function (e: any) {

                if ($(nd.$el).hasClass('in')) {
                    if (e.keyCode === 27 || e.keyCode === 13) {
                        nd.$emit('ok');
                    }
                }

            });
        }

        public open(): void {
            $(this.$el).modal('show');
        }

        public close(): void {
            $(this.$el).modal('hide');
        }

    }

</script>
