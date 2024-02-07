var dataTable;

$(document).ready(function () {

    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    // Get the value of "some_key" in eg "https://example.com/?some_key=some_value"
    var ProductId = params.productId;
    var ProductName = params.productName;
    
    if (ProductId != null) {

        $('#dvDropdowns').hide();
        $('#dvAddImages').show();
        $('#hfProductId').val(ProductId);
        BindImages(ProductId, ProductName);
    }
    else {

        $('#dvDropdowns').show();
        $('#dvAddImages').hide();
        BindCategories();
        $("#ddlCategory").select2();
    }


});



$("#ddlCategory").on("change", function () {

    clear();
    $('#dvAddImages').hide();
    BindProducts($(this).val());
    $("#ddlProducts").select2();

})


$("#ddlProducts").on("change", function () {

    clear();
    $('#hfProductId').val($(this).val());
    if ($(this).val() > 0) {

        $('#dvAddImages').show();
        var selectedText = $(this).select2('data')[0]['text'];
        BindImages($(this).val(), selectedText);
        ShowHideMainCheckbox($(this).val());
    }
    else {
        $('#dvAddImages').hide();
    }


})




$('#btnClear').on("click", function () {

    clear();
});


function clear() {
    $('#imgfile').val('');
    $('#result').html('');
    //$('#imgImage').attr('src', 'https://archive.org/download/no-photo-available/no-photo-available.png');
    $('#cbIsMain').prop('checked', false);
    // $('#imgImage').css("border", "4px solid lightgrey")
}


function BindCategories() {


    var location = window.location.origin;
    $("#dvCategoryDdl").hide();
    $("#dvLoader").show();
    $.getJSON("" + location + "/Admin/ProductImage/GetGroupableCategoryList", function (result) {
        $("#ddlCategory").html(""); // makes select null before filling process
        var data = result.data;
        if (data.length > 0) {
            $("#ddlCategory").append("<option value='0'>-Select Sub Category-</option>")
        }
        else {
            $("#ddlCategory").append("<option value='0' >-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {

            $("#ddlCategory").append(
                "<optgroup label=" + data[i].category.name + ">" +
                "<option value = " + data[i].subCategoryId + " > " + data[i].subCategoryName + "</option>"
            )

        }
        $("#dvCategoryDdl").show();
        $("#dvLoader").hide();

    });

}

function BindProducts(SubCatId) {

    var location = window.location.origin;
    $("#dvProductDdl").hide();
    $("#dvLoader").show();
    $.getJSON("" + location + "/Admin/ProductImage/GetProductList?SubCatId=" + SubCatId, function (result) {
        $("#ddlProducts").html(""); // makes select null before filling process
        var data = result.data;

        if (data.length > 0) {
            $("#ddlProducts").append("<option value='0'>-Select Product-</option>")
        }
        else {
            $("#ddlProducts").append("<option value='0' >-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {
            $("#ddlProducts").append("<option value=" + data[i].productId + " >" + data[i].productName + "</option>")
        }
        $("#dvProductDdl").show();
        $("#dvLoader").hide();

    });

}


//function readURL(input) {
//    if (input.files && input.files[0]) {
//        var reader = new FileReader();

//        reader.onload = function (e) {
//            $('#imgImage').attr('src', e.target.result).width(300).height(200);
//        };

//        reader.readAsDataURL(input.files[0]);
//    }
//}

document.querySelector("#imgfile").addEventListener("change", (e) => { //CHANGE EVENT FOR UPLOADING PHOTOS
    if (window.File && window.FileReader && window.FileList && window.Blob) { //CHECK IF FILE API IS SUPPORTED
        const files = e.target.files; //FILE LIST OBJECT CONTAINING UPLOADED FILES
        const output = document.querySelector("#result");
        output.innerHTML = "";
        for (let i = 0; i < files.length; i++) { // LOOP THROUGH THE FILE LIST OBJECT
            if (!files[i].type.match("image")) continue; // ONLY PHOTOS (SKIP CURRENT ITERATION IF NOT A PHOTO)
            const picReader = new FileReader(); // RETRIEVE DATA URI 
            picReader.addEventListener("load", function (event) { // LOAD EVENT FOR DISPLAYING PHOTOS
                const picFile = event.target;
                const div = document.createElement("div");
                div.innerHTML = `<img class="thumbnail" src="${picFile.result}" title="${picFile.name}"/>`;
                output.appendChild(div);
            });
            picReader.readAsDataURL(files[i]); //READ THE IMAGE
        }
    } else {
        alert("Your browser does not support File API");
    }
});




function isMainChange(input) {

    if (input.checked) {
        $('#imgImage').css("border", "4px solid #20c997")
    }
    else {
        $('#imgImage').css("border", "4px solid lightgrey")
    }
}

function BindImages(id, ProductName) {

    $('#txtProductName').html(ProductName);

    dataTable = $('#tblImages').DataTable({
        "ajax": {
            "url": "/Admin/ProductImage/GetImagesByProductId/" + id,
        },
        "columns": [

            {
                "data": "",
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                "width": "10%", "sorting": false
            },
            {
                "data": "imageUrl",
                "render": function (data, type, row, meta) {

                    if (row.isMainImage) {
                        return '<div class="text-center" title="Main Image"><img src="' + data + '" class="zoom"  style="border-radius:3px; border:2px solid #20c997; width:60px; height:60px" /></div>';
                    }
                    else {
                        return '<div class="text-center"><img src="' + data + '" class="zoom"  style="border-radius:3px; border:2px solid Lightgrey; width:60px; height:60px" /></div>';
                    }
                },
                "width": "40%", "sorting": false
            },
            {
                "data": "isMainImage",
                "render": function (data, type, row, meta) {
                    if (data) {
                        return '<div class="text-center">' + '<input type="hidden" value=' + row.productImgId + ' />' +
                            '<input class="toggle" checked="checked" type="checkbox" ></div>';
                    }
                    else {
                        return '<div class="text-center">' + '<input type="hidden" value=' + row.productImgId + ' />' +
                            '<input type="checkbox" class="toggle" /></div>';
                        //return '<label class="switch" for="checkbox">'
                        //    + '<input type="checkbox" class="toggle" id="checkbox" />'
                        //    + '<div class="slider round"></div>'
                        //    + '</label>';
                    }
                }
            },

            {
                "data": "productImgId",
                "render": function (data, type, row, meta) {
                    return '<a id="btn" onclick="Delete(`/Admin/ProductImage/Delete/' + data + '`)" style="cursor:pointer"><i class="bi bi-trash"></i></a>';
                }, "width": "5%", "sorting": false, className: "test"

            },


        ],
        "fnDrawCallback": function (oSettings) {
            $('#tblImages tbody tr td.test #btn').each(function () {

                var sTitle = "Delete";
                this.setAttribute('rel', 'tooltip');
                this.setAttribute('title', sTitle);

                $(this).tooltip({
                    html: true
                });
            });

        },
        "bPaginate": false,
        "searching": false,
        "bInfo": false,
        "destroy": true


    });
}


$(document).on('change', '.toggle', function () {

    $(".toggle").not(this).prop('checked', false);

    var ProductId = $('#hfProductId').val();
    var ProductImageId = $($(this).prev()[0]).val();

    var data = new FormData();
    data.append("ProductId", ProductId);
    data.append("ProductImageId", ProductImageId);

    $.ajax({
        type: "POST",
        url: "/Admin/ProductImage/SaveMainImage",
        contentType: false,
        processData: false,
        data: data,
        success: function (data) {

            $('#tblImages').DataTable().ajax.reload();

        },
        error: function () {

            alert("There was error uploading files!");

        }

    });


});


function saveImage() {

    if (ValidateInput()) {

        var fileInput = $('#imgfile');
        var cbIsMain;
        if ($('#cbIsMain').is(':checked')) {

            cbIsMain = true;

        } else {

            cbIsMain = false;
        }

        var productId = $('#hfProductId').val();
        var filePath = fileInput.val();

        var data = new FormData();


        var ins = $('#imgfile')[0].files.length;
        for (var x = 0; x < ins; x++) {
            data.append("files", $('#imgfile')[0].files[x]);
        }

        data.append("ProductId", productId);

        $.ajax({
            type: "POST",
            url: "/Admin/ProductImage/SaveProductImage",
            contentType: false,
            processData: false,
            data: data,
            success: function (data) {

                $('#tblImages').DataTable().ajax.reload();

            },
            error: function () {

                alert("There was error uploading files!");

            }

        });

    }


}

function ValidateInput() {
    //if (document.getElementById("ddlProducts").value == "0") {
    //    Swal.fire({
    //        icon: 'error',
    //        title: 'Oops...',
    //        text: 'Please select a product!',
    //    })
    //    return false;
    //}

    var fileInput = document.getElementById('imgfile');
    var filePath = fileInput.value;
    // Allowing file type
    var allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif|\.webp)$/i;

    if (!allowedExtensions.exec(filePath)) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Invalid file type!',
        })
        return false;
    }

    return true;

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
                        $('#tblImages').DataTable().ajax.reload();

                    }
                    else {

                    }
                }
            })


        }
    })


}


function ShowHideMainCheckbox(productId) {

    var location = window.location.origin;

    $.getJSON("" + location + "/Admin/ProductImage/CheckMainImage/" + productId, function (result) {
        $("#ddlSubCategory").html(""); // makes select null before filling process
        if (result.success) {

            $('#lblCheckbox').hide();
        }
        else {

            $('#lblCheckbox').show();
        }

    })


}