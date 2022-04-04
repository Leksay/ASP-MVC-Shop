var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    } else if (url.includes("pending")) {
        loadDataTable("pending");
    } else if (url.includes("completed")) {
        loadDataTable("completed");
    } else if (url.includes("approved")) {
        loadDataTable("approved");
    } else {
        loadDataTable("all");
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },
        "columns": [
            {"data":"id", "width": "5%"},
            {"data":"name", "width": "15%"},
            {"data":"phoneNumber", "width": "15%"},
            {"data":"applicationUser.email", "width": "25%"},
            {"data":"orderStatus", "width": "15%"},
            {"data":"orderTotal", "width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group"">
                            <a href="Order/Details?orderId=${data}" type="button" class="btn btn-primary btn-sm" >
                                <i class="bi bi-pencil-square p-1"></i>
                            </a>
                        </div>
                    `
                },
                "width":"10%"
            }
        ]
    });
}