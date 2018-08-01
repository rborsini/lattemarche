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
        </table>        
    </div>
</template>

<script>

    var table = null;

    export default {

        props: ['columns', 'rows'],

        mounted: function () {

        },

        watch: {

            columns: function (columns) {
                
                this.init(columns);
            },

            rows: function (rows) {
                table.clear();
                table.rows.add(rows);
                table.draw();
            }

        },

        methods: {

            init: function (columns) {

                var vm = this;

                // https://datatables.net/reference/option/dom
                var dom = '';

                if ($('.toolbox')[0])
                    dom = '<"top row"<"col-6"i><"col-6 float-right toolbox-div">>t<"row"<"col-6"l><"col-6"p>>';
                else
                    dom = '<"row"<"col-6"i><"col-6"f>>t<"row"<"col-6"l><"col-6"p>>';

                table = $(this.$el.children[0]).DataTable({
                    dom: dom,
                    initComplete: function () {

                        if ($('.toolbox')[0])
                            $('.toolbox-div')[0].append($('.toolbox')[0]);
                    },
                    serverSide: false,
                    paging: true,
                    lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                    pageLength: 10,
                    language: {
                        "sEmptyTable": "Nessun dato presente nella tabella",
                        "sInfo": "Vista da _START_ a _END_ di _TOTAL_ righe",
                        "sInfoEmpty": "Vista da 0 a 0 di 0 righe",
                        "sInfoFiltered": "(filtrati da _MAX_ righe totali)",
                        "sInfoPostFix": "",
                        "sInfoThousands": ",",
                        "sLengthMenu": "Visualizza _MENU_ righe",
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
                    },
                    columns: columns

                });

                table.on('draw.dt', function () {
                    vm.$emit('data-loaded');
                });

            }

        }

    }



</script>
