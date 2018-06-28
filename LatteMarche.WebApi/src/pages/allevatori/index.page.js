function TabellaAllevatori() {

    $('#allevatori-table').DataTable({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
        pageLength: 10,
        ajax: {
            type: "GET",
            url: apiUrl + 'Allevatori'
        },
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

}