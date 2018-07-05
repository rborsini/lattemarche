function TabellaDocumenti() {

    var url = apiUrl + 'documenti';

    var table = $('#documenti-table').dataTable({
        lengthMenu: [[10, 15, 20, -1], [10, 15, 20, "Tutte"]],
        processing: true,
        pageLength: 10,
        retrieve: true,
        bPaginate: true,
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
        columns: [
            { "data": "Id" },
            { "data": "Descrizione" },
            { "data": "DataInserimento" },
            //{
            //    "data": null,
            //    "render": function (data, type, row) {
            //        // TODO
            //        return '<a class="edit" href="' + webUrl + 'normative/edit?id=' + row.Id + '" >Dettagli</a>';
            //    }
            //}
        ]
    });

    // Caricamento dati JSON
    $.getJSON(url, function (result) {

        table.fnClearTable();
        if (result.length > 0) {
            table.fnAddData(result);
        }
        table.fnDraw();
    });

}