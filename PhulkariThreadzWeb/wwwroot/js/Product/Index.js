var dataTable;

$(document).ready(function () {

    BindSubCategory();
    $("#ddlSubCategory").select2();
    loadDataTable();

});

function BindSubCategory() {


    var location = window.location.origin;
    
   
    $.getJSON("" + location + "/Admin/ProductImage/GetGroupableCategoryList", function (result) {
        $("#ddlSubCategory").html(""); // makes select null before filling process
        var data = result.data;
        
        if (data.length > 0) {
            $("#ddlSubCategory").append("<option value=''>-All-</option>")
        }
        else {
            $("#ddlSubCategory").append("<option value='' >-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {

            $("#ddlSubCategory").append(
                "<optgroup label=" + data[i].category.name + ">" +
                "<option value = " + data[i].subCategoryId + " > " + data[i].subCategoryName + "</option>"
            )

        }


    });

}

function loadDataTable() {

    var searchKey = $('#txtSearchKey').val();
    var subCatId = $('#ddlSubCategory').val();
   
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll?searchkey=" + searchKey + "&subCatId=" + subCatId +""
            
        },
        "columns": [

            {
                "data": "",
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                "width": "5%", "sorting": false
            },
            { "data": "productName", "width": "25%", "sorting": false },
            { "data": "productDescription", "width": "30%", "sorting": false },
            {
                "data": "price",
                "render": function (data, type, row, meta) {
                    return "₹ " + data;
                },
                "width": "15%", "sorting": false
            },
            { "data": "subCategory.subCategoryName", "width": "30%", "sorting": false },
            {
                "data": "productId",
                "render": function (data, type, row, meta) {
                    return '<a id="btnEdit" href="/Admin/Product/Upsert?id=' + data + '"><i class="bi bi-pencil-square"></i></a>' +
                        '<a id="btnDelete" onclick="Delete(`/Admin/Product/Delete/' + data + '`)"  style="cursor:pointer" ><i class="bi bi-trash"></i></a>' +
                        '<a id="btnAddImage" href="/Admin/ProductImage/Index?productId=' + data + '&productName=' + row.productName+'" style="cursor:pointer" ><i class="bi bi-images"></i></a>';
                }, "width": "15%","sorting": false, className:"test"

            }, 
            {
                "data": "subCategoryId",
                 visible: false                  
            }

        ],
        "fnDrawCallback": function (oSettings) {
            $('#tblData tbody tr td.test #btnEdit').each(function () {

                var sTitle = "<span style='color:green;'>Edit</span>";
                this.setAttribute('rel', 'tooltip');
                this.setAttribute('title', sTitle);

                $(this).tooltip({
                    html: true
                });
            });
            $('#tblData tbody tr td.test #btnDelete').each(function () {

                var sTitle = "<span style='color:red;'>Delete</span>";
                this.setAttribute('rel', 'tooltip');
                this.setAttribute('title', sTitle);

                $(this).tooltip({
                    html: true
                });
            });
            $('#tblData tbody tr td.test #btnAddImage').each(function () {

                var sTitle = "Add Image";
                this.setAttribute('rel', 'tooltip');
                this.setAttribute('title', sTitle);

                $(this).tooltip({
                    html: true
                });
            });

        },    
        "paginate": true,
        "pageLength": 5,
        "lengthChange": false,
        "searching": false,
        "bInfo": false,
        "destroy": true


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

