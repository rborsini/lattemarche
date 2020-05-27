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
                    <button type="button" class="btn btn-secondary cancel" data-dismiss="modal">No</button>
                    <button type="submit" v-on:click="$emit('confirmed')" class="btn btn-success" data-dismiss="modal">Si</button>
                </div>
            </div>
        </div>
    </div>

</template>

<script lang="ts">

    import Vue from "vue";
    import Component from "vue-class-component";
    import { Prop, Watch, Emit } from "vue-property-decorator";

    @Component
    export default class ConfirmDialog extends Vue {
        @Prop() private title!: string;
        @Prop() private message!: string;

        mounted() {
            let cd = this;
            document.addEventListener("keyup", function (e: any) {

                if ($(cd.$el).hasClass('in')) {
                    if (e.keyCode === 27) {
                        cd.close();
                    } else if (e.keyCode === 13) {
                        cd.$emit('confirmed');
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

