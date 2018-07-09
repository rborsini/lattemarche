<template>
    <date-picker v-model="date" :config="options"></date-picker>
</template>

<script>
    // Import required dependencies
    //import 'bootstrap/dist/css/bootstrap.css';

    // Import this component
    import datePicker from 'vue-bootstrap-datetimepicker';

    // Import date picker css
    //import 'pc-bootstrap4-datetimepicker/build/css/bootstrap-datetimepicker.css';

    export default {
        data() {
            return {
                date: new Date(),
                options: {
                    format: 'DD/MM/YYYY',
                    useCurrent: false,
                }
            }
        },
        components: {
            datePicker
        }
    }
</script>

<!--<script lang="ts">
    // Import required dependencies
    //import 'bootstrap/dist/css/bootstrap.css';

    import Vue from "vue";

    // Import this component
    import datePicker from 'vue-bootstrap-datetimepicker';

    // Import date picker css
    //import 'pc-bootstrap4-datetimepicker/build/css/bootstrap-datetimepicker.css';

    import Component from "vue-class-component";
    import { Prop, Watch, Emit } from "vue-property-decorator";

    @Component({
        components: {
            datePicker
        }
    })

    export default class DateTimePicker extends Vue {
        public date: Date;

        public options: object;

        constructor() {
            super();

            this.date = new Date()
            this.options = {
                format: 'DD/MM/YYYY',
                useCurrent: false
            }
        }

        mounted() {

        }

    }

</script>-->