var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable()
{

    var searchKey = $('#txtSearchKey').val();

    dataTable = $('#tblData').dataTable({

        "ajax": {
            "url": "/Admin/SubCategory/GetAll?searchkey=" + searchKey + ""
        },
        "bPaginate": false,
        "searching": false,
        "bInfo": false,
        "destroy": true,
        
        "columns": [
            {
                "data": "",
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                "width": "5%"
            },
            { "data": "subCategoryName", "width": "35%" },
            { "data": "category.name", "width": "35%", "sorting": false },
            {
                "data": "subCategoryId",
                "render": function (data, type, row, meta) {
                    return '<a href="/Admin/SubCategory/Edit?id=' + data + '" title="Edit" "><i class="bi bi-pencil-square"></i></a>&nbsp;<a onclick="Delete(`/Admin/SubCategory/Delete/' + data + '`)" title="Delete" style="cursor:pointer" ><i class="bi bi-trash"></i></a>';
                }, "width": "5%", "sorting": false 

            },
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
                        $('#tblData').DataTable().ajax.reload();
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