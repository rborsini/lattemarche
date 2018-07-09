<template>
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <date-picker v-model="date" :config="options"></date-picker>
            </div>
        </div>
    </div>
</template>

<script>
  // Import required dependencies
  import 'bootstrap/dist/css/bootstrap.css';

  // Import this component
  import datePicker from 'vue-bootstrap-datetimepicker';

  // Import date picker css
  import 'pc-bootstrap4-datetimepicker/build/css/bootstrap-datetimepicker.css';

  export default {
    data () {
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