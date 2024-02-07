$(document).ready(function () {
    BindAddresses();
});


function BindCountries() {

    var location = window.location.origin;

    $.getJSON("" + location + "/Identity/Account/Manage/AccountAddress?handler=CountryList", function (result) {
        $("#ddlCountry").html(""); // makes select null before filling process
        var data = result;

        if (data.length > 0) {
            $("#ddlCountry").append("<option value='0'>-Select Country-</option>")
        }
        else {
            $("#ddlCountry").append("<option value='0'>-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {
            $("#ddlCountry").append("<option value=" + data[i].value + " >" + data[i].name + "</option>")
        }

    })
}

function BindEditCountries(x) {

    var location = window.location.origin;

    $.getJSON("" + location + "/Identity/Account/Manage/AccountAddress?handler=CountryList", function (result) {
        $("#ddlCountry" + x + "").html(""); // makes select null before filling process
        var data = result;

        if (data.length > 0) {
            $("#ddlCountry" + x + "").append("<option value='0'>-Select Country-</option>")
        }
        else {
            $("#ddlCountry" + x + "").append("<option value='0'>-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {
            $("#ddlCountry" + x + "").append("<option value=" + data[i].value + " >" + data[i].name + "</option>")
        }

    })
}


$('#ddlCountry').change(function () {

    if ($(this).val() == "India") {

        $('#dvIndianStates').show();
        $('#dvOtherStates').hide();
        BindIndianStates();
    }
    else {
        $('#dvIndianStates').hide();
        $('#dvOtherStates').show();
        $("#ddlState").html("");
    }

});


function BindIndianStates() {

    var location = window.location.origin;

    $.getJSON("" + location + "/Identity/Account/Manage/AccountAddress?handler=StateList", function (result) {
        $("#ddlState").html("");
        var data = result;

        if (data.length > 0) {
            $("#ddlState").append("<option value='0'>Choose a State</option>")
        }
        else {
            $("#ddlState").append("<option value='0'>-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {
            $("#ddlState").append("<option value=" + data[i].value + " >" + data[i].name + "</option>")
        }

    })
}
function BindEditIndianStates(x) {

    var location = window.location.origin;

    $.getJSON("" + location + "/Identity/Account/Manage/AccountAddress?handler=StateList", function (result) {
        $("#ddlState" + x + "").html("");
        var data = result;

        if (data.length > 0) {
            $("#ddlState" + x + "").append("<option value='0'>Choose a State</option>")
        }
        else {
            $("#ddlState" + x + "").append("<option value='0'>-No Data-</option>")
        }
        for (var i = 0; i < data.length; i++) {
            $("#ddlState" + x + "").append("<option value=" + data[i].value + " >" + data[i].name + "</option>")
        }

    })
}

function isValid() {

    var IsValid = true;

    if ($('#txtAddress1').val() == "") {
        toastr.error("Please enter Flat, House no., Building, Company, Apartment");
        var IsValid = false;
        return IsValid;
    }
    if ($('#txtAddress2').val() == "") {
        toastr.error("Please enter Area, Street, Sector, Village");
        var IsValid = false;
        return IsValid;
    }
    if ($('#ddlCountry').val() == "0") {
        toastr.error("Please select a country");
        var IsValid = false;
        return IsValid;
    }
    if ($('#ddlCountry').val() == "India") {
        if ($('#ddlState').val() == "0") {
            toastr.error("Please select a state");
            var IsValid = false;
            return IsValid;
        }
    }
    if ($('#ddlCountry').val() != "India" && $('#ddlCountry').val() != "0") {
        if ($('#txtState').val() == "") {
            toastr.error("Please enter State / Province / Region");
            var IsValid = false;
            return IsValid;
        }
    }
    if ($('#txtCity').val() == "") {
        toastr.error("Please enter City");
        var IsValid = false;
        return IsValid;
    }
    if ($('#txtPostalCode').val() == "") {
        toastr.error("Please enter Postal Code");
        var IsValid = false;
        return IsValid;
    }

    return IsValid;
}

function isValid(i) {

    var IsValid = true;

    if ($('#txtAddress1' + i + '').val() == "") {
        toastr.error("Please enter Flat, House no., Building, Company, Apartment");
        var IsValid = false;
        return IsValid;
    }
    if ($('#txtAddress2' + i + '').val() == "") {
        toastr.error("Please enter Area, Street, Sector, Village");
        var IsValid = false;
        return IsValid;
    }
    if ($('#ddlCountry' + i + '').val() == "0") {
        toastr.error("Please select a country");
        var IsValid = false;
        return IsValid;
    }
    if ($('#ddlCountry' + i + '').val() == "India") {
        if ($('#ddlState' + i + '').val() == "0") {
            toastr.error("Please select a state");
            var IsValid = false;
            return IsValid;
        }
    }
    if ($('#ddlCountry' + i + '').val() != "India" && $('#ddlCountry' + i + '').val() != "0") {
        if ($('#txtState' + i + '').val() == "") {
            toastr.error("Please enter State / Province / Region");
            var IsValid = false;
            return IsValid;
        }
    }
    if ($('#txtCity' + i + '').val() == "") {
        toastr.error("Please enter City");
        var IsValid = false;
        return IsValid;
    }
    if ($('#txtPostalCode' + i + '').val() == "") {
        toastr.error("Please enter Postal Code");
        var IsValid = false;
        return IsValid;
    }

    return IsValid;

}

function BindAddresses() {

    var location = window.location.origin;

    $.getJSON("" + location + "/Identity/Account/Manage/AccountAddress?handler=AddressList", function (result) {

        var data = result;
        var html = '<div class="row">';
        $('#AddressInfo').html('');
        console.log(data);
        for (var i = 0; i < data.length; i++) {

            if (data[i].isDefault) {
                html +=
                    '<div class="col-sm-14 mt-1">' +
                    '<div class="card">' +
                    '<div class="card-body">' +
                    '<h3>Adress ' + parseInt(i + 1) + ' (default)</h3>' +
                    '<p> ' +
                    '<b> Flat / House no. / Building / Company / Apartment : </b> ' + data[i].address1 + ' <br>' +
                    '<b>Area / Street / Sector / Village : </b>  ' + data[i].address2 + ' <br>' +
                    '<b>City : </b>  ' + data[i].city + ' <br>' +
                    '<b>State : </b> ' + data[i].state + ' <br>' +
                    '<b>Country : </b>' + data[i].country + ' <br>' +
                    '<b>Pin Code : </b>' + data[i].pinCode + ' <br>' +
                    '</p>' +
                    '<div class="mt-2 clearfix">' +
                    '<a href="#" onclick="OpenEdit(' + i + ', ' + "'" + data[i].country + "'" + ', ' + "'" + data[i].state + "'" + ')"><i class="icon-pencil"></i>Edit</a>' +
                    '\xa0 \xa0<a href="#" onclick="Delete(' + i + ', ' + "'" + data[i].userAddress_Id + "'" + ')"><i class="bi bi-trash-fill"></i>Delete</a>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div > ' +
                    '<div class="col-sm-14">' +
                    ' <div class="card mt-3" style="display:none" id="EditAddress' + i + '">' +
                    '<div class="card-body">' +
                    '<h3>Edit Address</h3>' +
                    ' <label class="text">Flat, House no., Building, Company, Apartment</label>' +
                    ' <div class="form-group">' +
                    '<textarea type="text"  id="txtAddress1' + i + '" class="form-control" style="width:100%" rows="2">' + data[i].address1 + '</textarea>' +
                    ' </div>' +


                    '<label class="text">Area, Street, Sector, Village</label>' +
                    '<div class="form-group">' +
                    '<textarea type="text" id="txtAddress2' + i + '" class="form-control" rows="3">' + data[i].address2 + '</textarea>' +
                    '</div>' +

                    '<div class="row">' +
                    '<div class="col-md-9">' +
                    '<label class="text">Country:</label>' +
                    '<div class="form-group select-wrapper">' +
                    '<select id="ddlCountry' + i + '" class="form-control">' +
                    '</select>' +
                    '</div>' +
                    '</div>' +
                    '<div class="col-md-9" id="dvOtherStates' + i + '">' +
                    '<label class="text">State / Province / Region</label>' +
                    '<div class="form-group ">' +
                    '    <input type="text" id="txtState' + i + '" class="form-control">' +
                    '</div>' +
                    '</div>' +
                    ' <div class="col-md-9" style="display:none" id="dvIndianStates' + i + '">' +
                    ' <label class="text">State:</label>' +
                    '<div class="form-group select-wrapper">' +
                    '<select id="ddlState' + i + '" class="form-control">' +
                    ' </select>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +

                    '<div class="row mt-2">' +
                    '<div class="col-md-9">' +
                    '<label class="text">City:</label>' +
                    '<div class="form-group">' +
                    '<input type="text" id="txtCity' + i + '" value=' + data[i].city + ' class="form-control">' +
                    '</div>' +
                    '</div>' +
                    '<div class="col-md-9">' +
                    '<label class="text">Postal code:</label>' +
                    '<div class="form-group">' +
                    '<input type="text" id="txtPostalCode' + i + '" value=' + data[i].pinCode + ' class="form-control">' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="clearfix mt-2">' +
                    '<input id="EditcbIsDefault' + i + '" checked="checked" style="opacity:0" name="checkbox1" type="checkbox">' +
                    '<label id="lblIsDefault" onclick="EditcheckboxToggle(' + i + ')">Set address as default</label>' +
                    '</div>' +
                    '<div class="mt-2">' +
                    '<button type="reset" id="btnCancel" class="btn btn--alt" onclick="CloseEdit(' + i + ')" >Cancel</button>' +
                    '<button type="button" id="btnUpdate" onclick="UpdateAddress(' + i + ', ' + data[i].userAddress_Id + ' )" class="btn ml-1">Update Address</button>' +
                    '</div>' +
                    '</div>' +
                    ' </div>' +
                    '</div > ';
                EditcheckboxToggle(i);
            }
            else {
                html +=
                    '<div class="col-sm-14 mt-1">' +
                    '<div class="card">' +
                    '<div class="card-body">' +
                    '<h3>Adress ' + parseInt(i + 1) + ' </h3>' +
                    '<p> ' +
                    '<b> Flat / House no. / Building / Company / Apartment : </b> ' + data[i].address1 + ' <br>' +
                    '<b>Area / Street / Sector / Village :</b>  ' + data[i].address2 + ' <br>' +
                    '<b>City :</b>  ' + data[i].city + ' <br>' +
                    '<b>State :</b> ' + data[i].state + ' <br>' +
                    '<b>Country :</b>' + data[i].country + ' <br>' +
                    '<b>Pin Code :</b>' + data[i].pinCode + ' <br>' +
                    '</p>' +
                    '<div class="mt-2 clearfix">' +
                    '<a href="#" onclick="OpenEdit(' + i + ', ' + "'" + data[i].country + "'" + ', ' + "'" + data[i].state + "'" + ')"><i class="icon-pencil"></i>Edit</a>' +
                    '\xa0 \xa0<a href="#" onclick="Delete(' + i + ', ' + "'" + data[i].userAddress_Id + "'" + ')"><i class="bi bi-trash-fill"></i>Delete</a>' +

                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div > ' +
                    '<div class="col-sm-14">' +
                    ' <div class="card mt-3" style="display:none" id="EditAddress' + i + '">' +
                    '<div class="card-body">' +
                    '<h3>Edit Address</h3>' +
                    ' <label class="text">Flat, House no., Building, Company, Apartment</label>' +
                    ' <div class="form-group">' +
                    '<textarea type="text"  id="txtAddress1' + i + '" class="form-control" style="width:100%" rows="2">' + data[i].address1 + '</textarea>' +
                    ' </div>' +


                    '<label class="text">Area, Street, Sector, Village</label>' +
                    '<div class="form-group">' +
                    '<textarea type="text" id="txtAddress2' + i + '" class="form-control" rows="3">' + data[i].address2 + '</textarea>' +
                    '</div>' +

                    '<div class="row">' +
                    '<div class="col-md-9">' +
                    '<label class="text">Country:</label>' +
                    '<div class="form-group select-wrapper">' +
                    '<select id="ddlCountry' + i + '" class="form-control">' +
                    '</select>' +
                    '</div>' +
                    '</div>' +
                    '<div class="col-md-9" id="dvOtherStates' + i + '">' +
                    '<label class="text">State / Province / Region</label>' +
                    '<div class="form-group ">' +
                    '    <input type="text" id="txtState' + i + '" class="form-control">' +
                    '</div>' +
                    '</div>' +
                    ' <div class="col-md-9" style="display:none" id="dvIndianStates' + i + '">' +
                    ' <label class="text">State:</label>' +
                    '<div class="form-group select-wrapper">' +
                    '<select id="ddlState' + i + '" class="form-control">' +
                    ' </select>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +

                    '<div class="row mt-2">' +
                    '<div class="col-md-9">' +
                    '<label class="text">City:</label>' +
                    '<div class="form-group">' +
                    '<input type="text" id="txtCity' + i + '" value=' + data[i].city + ' class="form-control">' +
                    '</div>' +
                    '</div>' +
                    '<div class="col-md-9">' +
                    '<label class="text">Postal code:</label>' +
                    '<div class="form-group">' +
                    '<input type="text" id="txtPostalCode' + i + '" value=' + data[i].pinCode + ' class="form-control">' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="clearfix mt-2">' +
                    '<input id="EditcbIsDefault' + i + '" style="opacity:0" name="checkbox1" type="checkbox">' +
                    '<label id="lblIsDefault" onclick="EditcheckboxToggle(' + i + ')">Set address as default</label>' +
                    '</div>' +
                    '<div class="mt-2">' +
                    '<button type="reset" id="btnCancel" class="btn btn--alt" onclick="CloseEdit(' + i + ')" >Cancel</button>' +
                    '<button type="button" id="btnUpdate" onclick="UpdateAddress(' + i + ', ' + data[i].userAddress_Id + ' )" class="btn ml-1">Update Address</button>' +
                    '</div>' +
                    '</div>' +
                    ' </div>' +
                    '</div > ';
            }

        }

        html += ' </div>';

        $('#AddressInfo').append(html);

    });


}


function OpenEdit(i, countryValue, stateValue) {
    $('#EditAddress' + i + '').show();
    BindEditCountries(i);
    setTimeout(function () {

        $('#ddlCountry' + i + '').val(countryValue).change();

    }, 200);

    $('#ddlCountry' + i + '').change(function () {

        if ($(this).val() == "India") {

            $('#dvIndianStates' + i + '').show();
            $('#dvOtherStates' + i + '').hide();
            BindEditIndianStates(i);
            setTimeout(function () {

                $('#ddlState' + i + '').val(stateValue);

            }, 200);
        }
        else {
            $('#dvIndianStates' + i + '').hide();
            $('#dvOtherStates' + i + '').show();
            $('#ddlState' + i + '').html("");
            setTimeout(function () {

                $('#txtState' + i + '').val(stateValue);

            }, 200);
        }

    });

}
function Delete(i, AddressId) {

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


            var data = {

                "UserAddressId": AddressId,
                "UserId": $('#hfUserId').val()
            }

            $.ajax({
                type: "DELETE",

                url: "/Identity/Account/Manage/AccountAddress?handler=AddressDelete",

                data: data,

                headers: {
                    RequestVerificationToken:
                        $('input:hidden[name="__RequestVerificationToken"]').val()
                },
                success: function (data) {

                    if (data == "Success") {
                        toastr.success("Address deleted successfully!");
                        BindAddresses();
                        $('#btnCancel').click();
                    }
                    else {
                        toastr.success("Address not deleted!");
                    }
                },
                error: function () {

                    toastr.error("There was error deleting data!");

                }

            });


        }
    })

}

function CloseEdit(i) {
    $('#EditAddress' + i + '').hide();
}

$('#btnSave').on('click', function () {

    if (isValid()) {

        var state = $('#ddlCountry').val() == "India" ? $('#ddlState').val() : $('#txtState').val();
        var isDefault = $('#cbIsDefault').prop('checked') ? true : false;
        var address1 = $('#txtAddress1').val();
        var address2 = $('#txtAddress2').val();
        var country = $('#ddlCountry').val();
        var city = $('#txtCity').val();
        var pincode = $('#txtPostalCode').val();
        var userId = $('#hfUserId').val();

        var data = {
            "Address1": address1,
            "Address2": address2,
            "Country": country,
            "State": state,
            "City": city,
            "PinCode": pincode,
            "IsDefault": isDefault,
            "Id": userId
        }

        $.ajax({
            type: "POST",

            url: "/Identity/Account/Manage/AccountAddress?handler=AddressSave",

            data: data,

            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {

                if (data == "Success") {
                    toastr.success("Address saved successfully!");
                    BindAddresses();
                    $('#btnCancel').click();
                }
                else {
                    toastr.success("Address not saved!");
                }
            },
            error: function () {

                alert("There was error saving data!");

            }

        });




    }

});

function UpdateAddress(i, userAddressId) {

    if (isValid(i)) {

        var state = $('#ddlCountry' + i + '').val() == "India" ? $('#ddlState' + i + '').val() : $('#txtState' + i + '').val();
        var isDefault = $('#EditcbIsDefault' + i + '').prop('checked') ? true : false;
        var address1 = $('#txtAddress1' + i + '').val();
        var address2 = $('#txtAddress2' + i + '').val();
        var country = $('#ddlCountry' + i + '').val();
        var city = $('#txtCity' + i + '').val();
        var pincode = $('#txtPostalCode' + i + '').val();
        var userId = $('#hfUserId').val();

        var data = {
            "Address1": address1,
            "Address2": address2,
            "Country": country,
            "State": state,
            "City": city,
            "PinCode": pincode,
            "IsDefault": isDefault,
            "Id": userId,
            "UserAddress_Id": userAddressId
        }

        $.ajax({
            type: "POST",

            url: "/Identity/Account/Manage/AccountAddress?handler=AddressUpdate",

            data: data,

            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {

                if (data == "Success") {
                    toastr.success("Address updated successfully!");
                    BindAddresses();
                }
                else {
                    toastr.success("Address not updated!");
                }
            },
            error: function () {

                alert("There was error updating data!");

            }

        });




    }
}

function EditcheckboxToggle(i) {

    if ($('#EditcbIsDefault' + i + '').prop('checked')) {
        $('#EditcbIsDefault' + i + '').prop('checked', false);
    }
    else {
        $('#EditcbIsDefault' + i + '').prop('checked', true);
    }

}
function checkboxToggle() {

    if ($('#cbIsDefault').prop('checked')) {
        $('#cbIsDefault').prop('checked', false);
    }
    else {
        $('#cbIsDefault').prop('checked', true);
    }

}