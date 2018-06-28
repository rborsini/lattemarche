function TabellaAllevatori() {

    var url = apiUrl + 'Allevatori';


    var table = $('#allevatori-table').dataTable({
            lengthMenu: [[10, 15, 20, -1], [10, 15, 20, "Tutte"]],
            processing: true,
            pageLength: 10,
            retrieve: true,
            bPaginate: true,
            columns: [
                { "data": "Id" },
                { "data": "RagioneSociale" },
                { "data": "IndirizzoAllevamento" },
                { "data": "Comune" },
                { "data": "Provincia" },
                {
                    "data": null,
                    "render": function (data, type, row) {
                        return '<a class="edit" href="' + webUrl + 'utenti/edit?id=' + row.Id + '" >Dettagli</a>';
                    }
                }
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




//function TabellaAllevatori() {

//    $('#allevatori-table').DataTable({
//        processing: true,
//        serverSide: true,
//        paging: true,
//        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
//        pageLength: 10,
//        ajax: {
//            type: "GET",
//            url: apiUrl + 'Allevatori'
//        },
//        columns: [
//            { "data": "Id" },
//            { "data": "RagioneSociale" },
//            { "data": "IndirizzoAllevamento" },
//            { "data": "Comune" },
//            { "data": "Provincia" },
//            {
//                "data": null,
//                "render": function (data, type, row) {
//                    return '<a class="edit" href="' + webUrl + 'utenti/edit?id=' + row.Id + '" >Dettagli</a>';
//                }
//            }
//        ]

//    });

//}