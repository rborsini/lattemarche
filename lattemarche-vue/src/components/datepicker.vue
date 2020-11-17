<template>
    <input type="text" v-model="value" class="form-control datepicker" />
</template>

<script>

    import $ from 'jquery';
    import { datepicker } from 'bootstrap-datepicker';       

    export default {
        props: ['value'],
 

        watch: {
            value: function (val, oldVal) {
                if(oldVal) {
                    this.$emit('update:value', val);
                    this.$emit('value-changed', val);
                }
            }
        },

        // init
        mounted: function () {

            $.fn.datepicker.dates['it'] = {
                days: ["Domenica", "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato"],
                daysShort: ["Dom", "Lun", "Mar", "Mer", "Gio", "Ven", "Sab"],
                daysMin: ["Do", "Lu", "Ma", "Me", "Gi", "Ve", "Sa"],
                months: ["Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"],
                monthsShort: ["Gen", "Feb", "Mar", "Apr", "Mag", "Giu", "Lug", "Ago", "Set", "Ott", "Nov", "Dic"],
                today: "Oggi",
                clear: "Clear",
                format: "dd-mm-yyyy",
                titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
                weekStart: 0
            };

            var vm = this
            $(this.$el).click(function () {

                $(this)
                    .datepicker({
                        weekStart: 1,
                        language: 'it',
                        autoclose: true
                    })
                    .on('changeDate', function () {
                        vm.$emit('update:value', this.value);
                        vm.$emit('value-changed', this.value); 
                    }).on('clearDate', function () {
                        vm.$emit('update:value', '');
                        vm.$emit('value-changed', '');
                    });

                $(this).datepicker('update', $(this).val());
                $(this).datepicker('show');

            });

        }
    }


</script>

<style>
    @import '../../public/css/bootstrap-datepicker.min.css'; 

    .datepicker {
        padding: 2px 5px;
    }

</style>