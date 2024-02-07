var dataTable;
$(document).ready(function () {

    BindtblImages();

});


function BindtblImages() {


    dataTable = $('#tblImages').DataTable({
        "ajax": {
            "url": "/Admin/Settings/GetAllImages",
        },
        "columns": [

            {
                "data": "",
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
                "width": "2%", "sorting": false
            },
            {
                "data": "imageURl",
                "render": function (data, type, row, meta) {

                    return '<div style="text-align:center"><img src="' + data + '"  style="border-radius:3px; border:2px solid #20c997; width:200px; height:120px" /></div>';
               
                },
                "width": "30%", "sorting": false
            },
            {
                "data": "displayOrder",
                "render": function (data, type, row, meta) {

                    return '<input type="number" id="txtOrder" class="form-control" value="'+data+'" />';

                },"className":"order"
            },
            //{
            //    "data": "bannerText",
            //    "render": function (data, type, row, meta) {

            //        return '<input type="text" id="txtBannerText" class="form-control" value="' + data + '" />';

            //    }, "className": "text"
            //},
            //{
            //    "data": "bannerSubText",
            //    "render": function (data, type, row, meta) {

            //        return '<input type="text" id="txtBannerSubText" class="form-control" value="' + data + '" />';

            //    }, "className" : "subtext"
            //},
            {
                "data": "link",
                "render": function (data, type, row, meta) {

                    return '<input type="text" id="txtLink" class="form-control" value="' + data + '" />';

                }, "className": "link"
            },
            {
                "data": "bannerImageId",
                "render": function (data, type, row, meta) {
                    
                    return '<a id="btnSave" onclick="SaveDetails(this, `'+data+'`)" style="cursor:pointer"><i class="bi bi-save-fill"></i></a>'+
                        '<a id="btnDel" onclick="Delete(`/Admin/Settings/DeleteImage/' + data + '`)" style="cursor:pointer"><i class="bi bi-trash"></i></a>';
                }, "width": "5%", "sorting": false, className: "test"

            },


        ],
        "fnDrawCallback": function (oSettings) {
            $('#tblImages tbody tr td.test #btnSave').each(function () {

                var sTitle = "Save";
                this.setAttribute('rel', 'tooltip');
                this.setAttribute('title', sTitle);

                $(this).tooltip({
                    html: true
                });
            });

            $('#tblImages tbody tr td.test #btnDel').each(function () {

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
                        toastr.success(data.message);
                    }
                    else {

                    }
                }
            })


        }
    })


}

function SaveDetails(sender, id) {

    var bannerImageId = id;
    var displayOrder = $(sender).parent().parent().find('td.order #txtOrder').val();
    //var bannerText = $(sender).parent().parent().find('td.text #txtBannerText').val();
    //var bannerSubText = $(sender).parent().parent().find('td.subtext #txtBannerSubText').val();
    var link = $(sender).parent().parent().find('td.link #txtLink').val();
    

    var data = new FormData();
    data.append("BannerImageId", bannerImageId);
    data.append("DisplayOrder", displayOrder);
    //data.append("BannerText", bannerText);
    //data.append("BannerSubText", bannerSubText);
    data.append("Link", link);

    $.ajax({
        type: 'POST',
        url: "/Admin/Settings/SaveImageDetail",
        contentType: false,
        processData: false,
        data: data,
        success: function (data) {
            if (data.success) {
                $('#tblImages').DataTable().ajax.reload();
                toastr.success(data.message);
            }
            else {

            }
        }
    })

}


function AddBannerImages() {

   
    if (ValidateInput()) {

        var ImageId = $('#hfImageId').val();
        var data = new FormData();

        var ins = $('#fuAddImage')[0].files.length;
        for (var x = 0; x < ins; x++) {
            data.append("files", $('#fuAddImage')[0].files[x]);
        }


        $.ajax({
            type: "POST",
            url: "/Admin/Settings/AddBannerImages",
            contentType: false,
            processData: false,
            data: data,
            success: function (data) {

                $('#tblImages').DataTable().ajax.reload();
                toastr.success(data.message);
            },
            error: function () {

                alert("There was error uploading files!");

            }

        });

    }
   

}



$('#dvAddImage').click(function (e) {

    $('#fuAddImage').trigger('click');
});


document.querySelector("#fuAddImage").addEventListener("change", (e) => { //CHANGE EVENT FOR UPLOADING PHOTOS
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
                div.innerHTML = `<img class="thumbnailBanner" src="${picFile.result}" title="${picFile.name}"/>`;
                output.appendChild(div);
            });
            picReader.readAsDataURL(files[i]); //READ THE IMAGE
        }
    } else {
        alert("Your browser does not support File API");
    }
});


function ValidateInput() {

    var token = true;

    var fileInput = document.getElementById('fuAddImage');
    const allowedExtensions = /(\.jpg|\.jpeg|\.png|\.gif|\.webp)$/i;

    console.log(fileInput.files);
    for (let i = 0; i < fileInput.files.length; i++) {

        if (!allowedExtensions.exec(fileInput.files[i].name)) {

            token = false
        }

    }
   
    if (!token) {
        
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Invalid file type!',
        })
    }

    return token;

}

$('#btnClear').on("click", function () {

    clear();
});


function clear() {
    $('#fuAddImage').val('');
    $('#result').html('');
}