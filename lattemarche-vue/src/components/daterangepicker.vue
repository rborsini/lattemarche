<template>
    <input type="text" name="period" class="form-control" />
</template>
<script>

import $ from 'jquery';
import { daterangepicker } from 'daterangepicker';     
import moment  from 'moment';     
import 'daterangepicker/daterangepicker.css'

export default {
    
    mounted: function() {

        var options = {
            ranges: {
                'Da inizio anno': [moment().startOf('year'), moment()],
                'Mese corrente': [moment().startOf('month'), moment().endOf('month')],
                'Mese precedente': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'Anno corrente': [moment().startOf('year'), moment().endOf('year')],            
                'Anno precedente': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
                'Ultimi 365 gioni': [moment().subtract(365, 'days'), moment()],
            },
            maxSpan: { year: 1 },
            alwaysShowCalendars: true,
            locale: {
                format: 'DD/MM/YYYY',
                separator: ' - ',
                applyLabel: 'Applica',
                cancelLabel: 'Cancella',
                fromLabel: 'Da',
                toLabel: 'A',
                customRangeLabel: 'Periodo personalizzato',
                weekLabel: 'W',
                daysOfWeek: [
                    'Dom',
                    'Lun',
                    'Mar',
                    'Mer',
                    'Gio',
                    'Ven',
                    'Sab'
                ],
                monthNames: [
                    'Gennaio',
                    'Febbraio',
                    'Marzo',
                    'Aprile',
                    'Maggio',
                    'Giugno',
                    'Luglio',
                    'Agosto',
                    'Settembre',
                    'Ottobre',
                    'Novembre',
                    'Dicembre'
                ],
                firstDay: 1
            }
        };

        options.startDate = moment().startOf('year');
        options.endDate = moment().endOf('year');

        $('input[name="period"]').daterangepicker(options);

        this.$emit('apply', {
            From: options.startDate.format('YYYY-MM-DD'),
            To: options.endDate.format('YYYY-MM-DD')
        });

        var vm = this;
        $('input[name="period"]').on('apply.daterangepicker', function (ev, picker) {
            vm.$emit('apply', {
                From: picker.startDate.format('YYYY-MM-DD'),
                To: picker.endDate.format('YYYY-MM-DD')
             });
        });

    },

    methods: {

    }

}
</script>

<style>
    /* @import 'https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css';  */

    .daterangepicker table thead {
        color: #17141f !important;
        background-color: white;
    }

</style>