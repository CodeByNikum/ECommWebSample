$(document).ready(function () {

    BindCategory();
    var catId = $("#hfCategoryId").val();
    var subCatId = $('#hfSubCategoryId').val();

    setTimeout(function () {
        if (catId != null) {
            $('#ddlCategory').val(catId)
        }
        BindSubCategory($("#ddlCategory").val())

        setTimeout(function () {
            if (subCatId != null) {
                $("#ddlSubCategory").val(subCatId)
            }
        }, 200);
       
    }, 200);

   
  
});


function BindCategory() {

    var location = window.location.origin;
   

    $.getJSON("" + location + "/Admin/Product/GetCategoryList" , function (result) {
        $("#ddlCategory").html(""); // makes select null before filling process
        var data = result.data;

        if (data.length > 0) {
            $("#ddlCategory").append("<option value='0'>-Select Category-</option>")
        }
        else {
            $("#ddlCategory").append("<option value='0'>-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {
            $("#ddlCategory").append("<option value=" + data[i].id + " >" + data[i].name + "</option>")
        }

    })


}



$("#ddlCategory").on("change", function () {
    BindSubCategory($(this).val());
})


function BindSubCategory(val) {

    var location = window.location.origin;
   

    $.getJSON("" + location +"/Admin/Product/GetDropdownList/"+ val, function (result) {
        $("#ddlSubCategory").html(""); // makes select null before filling process
        var data = result.data;

        if (data.length > 0) {
            $("#ddlSubCategory").append("<option value='0'>-Select Sub Category-</option>")
        }
        else {
            $("#ddlSubCategory").append("<option value='0' >-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {
            $("#ddlSubCategory").append("<option value=" + data[i].subCategoryId +" >" + data[i].subCategoryName + "</option>")
        }

    })
}


function ValidateInput() {
    if (document.getElementById("ddlSubCategory").value == "0") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please select sub category!',
        })
        return false;
    }
    return true;

}


$('#dvAddImage').click(function (e) {

    $('#fuAddImage').trigger('click');
});

const chooseFile = document.getElementById("fuAddImage");
const imgPreview = document.getElementById("img-preview");


chooseFile.addEventListener("change", function () {
    getImgData();
});


function getImgData() {
    $('#img-preview').show();
    const files = chooseFile.files[0];
    if (files) {
        const fileReader = new FileReader();
        fileReader.readAsDataURL(files);
        fileReader.addEventListener("load", function () {
            imgPreview.style.display = "block";
            imgPreview.innerHTML = '<img src="' + this.result + '" class="zoom ml-2" style="border-radius:3px; border:2px solid Lightgrey;" width="100" height="100"  />';
        });
    }
}


