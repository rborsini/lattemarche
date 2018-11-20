<template>
    <div>
        <table class="table table-hover table-striped table-bordered">
            <div class="d-none">
                <slot class="toolbox" name="toolbox"></slot>
            </div>
            <thead>
                <tr>
                    <slot name="thead"></slot>
                </tr>
            </thead>
            <tbody></tbody>
            <tfoot>
                <tr>
                    <slot name="tfoot"></slot>
                </tr>
            </tfoot>
        </table>        
    </div>
</template>

<script>

    var table = null;

    export default {

        props: ['options', 'rows'],

        mounted: function () {

        },

        watch: {

            options: function (options) {
                this.init(options);
            },

            rows: function (rows) {
                table.clear();
                table.rows.add(rows);
                table.draw();
            }

        },

        methods: {

            init: function (options) {

                var vm = this;

                // https://datatables.net/reference/option/dom
                var dom = '';

                if ($('.toolbox')[0])
                    dom = '<"top row"<"col-6 float-left"f><"col-6 float-right toolbox-div">>t<"row"<"col-6"l><"col-6"p>>';
                else
                    dom = '<"row"<"col-6"f><"col-6">>t<"row"<"col-6"l><"col-6"p>>';

                var defaultOptions = {
                    dom: dom,
                    initComplete: function () {

                        if ($('.toolbox')[0])
                            $('.toolbox-div')[0].append($('.toolbox')[0]);
                    },
                    rowGroup: options.rowGroup,
                    columns: options.columns,
                    serverSide: false,
                    paging: true,
                    lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                    pageLength: 100,
                    language: {
                        "sEmptyTable": "Nessun dato presente nella tabella",
                        "sInfo": "Vista da _START_ a _END_ di _TOTAL_ righe",
                        "sInfoEmpty": "Vista da 0 a 0 di 0 righe",
                        "sInfoFiltered": "(filtrati da _MAX_ righe totali)",
                        "sInfoPostFix": "",
                        "sInfoThousands": ",",
                        "sLengthMenu": "Visualizza _MENU_ righe per pagina",
                        "sLoadingRecords": "Caricamento...",
                        "sProcessing": "Elaborazione...",
                        "sSearch": "Cerca:",
                        "sZeroRecords": "La ricerca non ha portato alcun risultato.",
                        "oPaginate": {
                            "sFirst": "Inizio",
                            "sPrevious": "Precedente",
                            "sNext": "Successivo",
                            "sLast": "Fine"
                        },
                        "oAria": {
                            "sSortAscending": ": attiva per ordinare la colonna in ordine crescente",
                            "sSortDescending": ": attiva per ordinare la colonna in ordine decrescente"
                        }
                    }

                };

                // merge delle opzioni
                var fullOptions = Object.assign(defaultOptions, options);

                table = $(this.$el.children[0]).DataTable(fullOptions);

                table.on('draw.dt', function () {
                    vm.$emit('data-loaded');
                });

            }

        }

    }



</script>
