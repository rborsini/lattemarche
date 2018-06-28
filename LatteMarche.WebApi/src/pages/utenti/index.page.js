function TabellaUtenti() {

    $('#utenti-table').DataTable({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
        pageLength: 10,
        ajax: {
            type: "GET",
            url: apiUrl + 'utenti'
        },
        columns: [
            { "data": "Id" },
            { "data": "Username" },
            { "data": "Nome" },
            { "data": "Cognome" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return '<a class="edit" href="' + webUrl + 'utenti/edit?id=' + row.Id + '" >Dettagli</a>';
                }
            }
        ]

    });

}