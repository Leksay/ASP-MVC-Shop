var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            {"data":"title", "width": "15%"},
            {"data":"isbn", "width": "15%"},
            {"data":"price", "width": "15%"},
            {"data":"author", "width": "15%"},
            {"data":"category.name", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group"">
                            <a href="Product/Upsert?id=${data}" type="button" class="btn btn-primary btn-sm" >
                                <i class="bi bi-pencil-square p-1"></i> Edit
                            </a>
                        </div>
                        <div class="btn-group" role="group" style="width:100px">
                            <a type="button" class="btn btn-outline-danger btn-sm">
                                <i class="bi bi-trash3 p-1"></i> Delete
                            </a>
                        </div>
                    `
                },
                "width":"15%"
            }
        ]
    });
}