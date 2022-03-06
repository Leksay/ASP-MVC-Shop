var dataTable;

$(document).ready(function () {
    loadCompanyTable();
});

function loadCompanyTable() {
    dataTable = $('#companiesTbl').DataTable(
        {
            "ajax": {
                "url":"/Admin/Company/GetAll"
            },
            "columns": [
                { "data": "name", "width":"15%"},
                { "data": "streetAddress", "width":"15%"},
                { "data": "city", "width":"15%"},
                { "data": "state", "width":"15%"},
                { "data": "postalCode", "width":"10%"},
                { "data": "phoneNumber", "width": "15%" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="btn-group" role="group"">
                                <a href="Company/Upsert?id=${data}" type="button" class="btn btn-primary btn-sm" >
                                    <i class="bi bi-pencil-square p-1"></i> Edit
                                </a>
                            </div>
                            <div class="btn-group" role="group" style="width:100px">
                                <a type="button" onClick = "Delete('/Admin/Company/Delete/${data}')" class="btn btn-outline-danger btn-sm">
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

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}