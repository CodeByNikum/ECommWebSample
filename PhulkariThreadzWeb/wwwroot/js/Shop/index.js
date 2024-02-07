var removeSubCatArr = [];
var wishListArr = [];
var tempUserCart = [];

$(document).ready(function () {
    toggleSidebarMenu();
    $("#ctCategory").load("/Customer/Shop/CategoryListBox");
    $("#ddlView").change();
    $('.js-horview').click();

    // Get the modal
    var modal = document.getElementById("myModal");
    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            $('#myModal').hide();
        }
    }



});
// When the user clicks the button, open the modal 
function OpenModel(id) {

    $(".modal-content").load("/Customer/Shop/QuickViewModalBox?productId=" + parseInt(id), function () {

        $('#myModal').show();
        setTimeout(function () {


        }, 500);

    });


};
function CloseModal() {
    $('#myModal').hide();
}

function toggleSidebarMenu() {
    var CatOpen = false;
    var ColOpen = false;
    var SizeOpen = false;
    var PriceOpen = false;
    $("#taCategory").click(function () {
        if (!CatOpen) {
            $('#CategoryMenu').addClass('open');
            $('#ctCategory').show();
            CatOpen = true;
        }
        else {
            $('#CategoryMenu').removeClass('open');
            $('#ctCategory').hide();
            CatOpen = false;
        }
    });
    $("#taColors").click(function () {
        if (!ColOpen) {
            $('#ColorsMenu').addClass('open');
            $('#ctColors').show();
            ColOpen = true;
        }
        else {
            $('#ColorsMenu').removeClass('open');
            $('#ctColors').hide();
            ColOpen = false;
        }
    });
    $("#taSize").click(function () {
        if (!SizeOpen) {
            $('#SizeMenu').addClass('open');
            $('#ctSize').show();
            SizeOpen = true;
        }
        else {
            $('#SizeMenu').removeClass('open');
            $('#ctSize').hide();
            SizeOpen = false;
        }
    });
    $("#taPrice").click(function () {
        if (!PriceOpen) {
            $('#PriceMenu').addClass('open');
            $('#ctPrice').show();
            PriceOpen = true;
        }
        else {
            $('#PriceMenu').removeClass('open');
            $('#ctPrice').hide();
            PriceOpen = false;
        }
    });
}
$("#ddlView").change(function () {

    var test = JSON.stringify(removeSubCatArr);
    var data = { subCatList: test, value: parseInt(this.value), sortby: $('#ddlSortBy').val() };

    $("#productBox").load("/Customer/Shop/ProductBox01", $.param(data, true), function () {

        setTimeout(function () {

            $('.js-horview').click();
            if ($("#ddlView option:selected").text() == "All") {
                $('.circle-loader-wrap').hide();

            }
            else {
                $('.circle-loader-wrap').show();
            }


            $('.items-count').html($('#hfProductCount').val() + ' item(s)');


            var selectedItemCount = $('#ctCategory #CategoryList .active.category').length;
            if (selectedItemCount < 0) {
                $('#spnSelectedItem').html('0 items');
            }
            else {
                $('#spnSelectedItem').html(selectedItemCount + ' items');
            }

            var selectedTags = '';
            $('#ctCategory #CategoryList .active.category').each(function (i, e) {

                selectedTags += '<li><a onclick="removeSelection(`' + $(e).attr('id') + '`);">' + $(e).attr('name') + '</a></li>';

            });
            $('.selected-filters').html(selectedTags);


        }, 500);


    });



}).trigger();
function Sorting() {

    $("#ddlView").change();

};
function LoadMore(sender) {
    Value = 5 + $(sender).attr('data-loaded');
    var test = JSON.stringify(removeSubCatArr);
    var data = { subCatList: test, value: Value, sortby: $('#ddlSortBy').val() };

    $("#productBox").load("/Customer/Shop/ProductBox01", $.param(data, true), function () {

        setTimeout(function () {

            $('.js-horview').click();

            if ($("#ddlView option:selected").text() == "All") {
                $('.circle-loader-wrap').hide();

            }
            else {
                if (($('#btnLoadMore').attr('data-loaded')) == $('#hfTotalProducts').val()) {
                    $('.circle-loader-wrap').hide();
                }
                else {
                    $('.circle-loader-wrap').show();
                }
            }


            $('.items-count').html($('#hfProductCount').val() + ' item(s)');

            var selectedItemCount = $('#ctCategory #CategoryList .active.category').length;
            if (selectedItemCount < 0) {
                $('#spnSelectedItem').html('0 items');
            }
            else {
                $('#spnSelectedItem').html(selectedItemCount + ' items');
            }

            var selectedTags = '';
            $('#ctCategory #CategoryList .active.category').each(function (i, e) {

                selectedTags += '<li><a onclick="removeSelection(`' + $(e).attr('id') + '`);">' + $(e).attr('name') + '</a></li>';

            });
            $('.selected-filters').html(selectedTags);



        }, 500);

    });



}
function toggleImage(sender) {
    $(sender).addClass("active");
}
function setActiveClass(sender, type) {

    $(sender).addClass("active");
    $(sender).prev().removeClass("active");
    $(sender).prev().prev().removeClass("active");
    $(sender).next().removeClass("active");
    $(sender).next().next().removeClass("active");

    if (type == 'hor') {
        $('#dvProductGrid').addClass('prd-horgrid');
        $('#dvProductGrid').removeClass('prd-listview');
        $('#dvProductGrid').removeClass('prd-grid');

        $('#dvProductLables').removeClass('prd-w-xl');
        $('#dvProductLables').removeClass('prd-w-sm');
        $('#dvProductLables').addClass('prd-w-xs');
        $('.prd-rating').removeClass('justify-content-center');
        $('.prd-rating').parent().addClass('prd-info-top');
    }
    if (type == 'grid') {
        $('#dvProductGrid').removeClass('prd-horgrid');
        $('#dvProductGrid').removeClass('prd-listview');
        $('#dvProductGrid').addClass('prd-grid');
        $('.prd-rating').addClass('justify-content-center');
        $('.prd-rating').parent().removeClass('prd-info-top');

        $('#dvProductLables').addClass('prd-w-xl');
        $('#dvProductLables').removeClass('prd-w-sm');
        $('#dvProductLables').removeClass('prd-w-xs');
    }
    if (type == 'list') {
        $('#dvProductGrid').removeClass('prd-horgrid');
        $('#dvProductGrid').addClass('prd-listview');
        $('#dvProductGrid').removeClass('prd-grid');

        $('#dvProductLables').removeClass('prd-w-xl');
        $('#dvProductLables').addClass('prd-w-sm');
        $('#dvProductLables').removeClass('prd-w-xs');
        $('.prd-rating').removeClass('justify-content-center');
        $('.prd-rating').parent().addClass('prd-info-top');
    }



}
function toggleCatList(sender) {
    if (!$(sender).prev().hasClass('open')) {
        $(sender).prev().addClass('open');
        $(sender).next().show();
    }
    else {
        $(sender).prev().removeClass('open');
        $(sender).next().hide();
    }
}
function toggleOuter(sender) {

    if ($(sender).parent().hasClass('active')) {
        $(sender).parent().removeClass('active');
        $(sender).next().next().children('li').removeClass('active');

        $(sender).next().next().children().each(function (i, e) {
            removeSubCatArr.push({ ID: $(e).attr('id') });
        });
        $("#ddlView").change();
    }
    else {
        $(sender).parent().addClass('active');
        $(sender).next().next().children('li').addClass('active');

        $(sender).next().next().children().each(function (i, e) {


            for (var i = 0; i < removeSubCatArr.length; i++) {
                if (removeSubCatArr[i].ID == $(e).attr('id')) {
                    removeSubCatArr.splice(i, 1);
                }
            }
        });
        $("#ddlView").change();

    }
}
function toggleInner(sender) {

    if ($(sender).parent().hasClass('active')) {

        $(sender).parent().removeClass('active');

        var removeSubCatId = $(sender).parent().attr('id');
        removeSubCatArr.push({ ID: removeSubCatId });
        $("#ddlView").change();

        if (!$(sender).parent().siblings().hasClass('active')) {
            $(sender).parent().parent().parent().removeClass('active');
        }
    }
    else {
        $(sender).parent().addClass('active');

        var addSubCatId = $(sender).parent().attr('id');
        for (var i = 0; i < removeSubCatArr.length; i++) {
            if (removeSubCatArr[i].ID == addSubCatId) {
                removeSubCatArr.splice(i, 1);
            }
        }
        $("#ddlView").change();
        if ($(sender).parent().prevUntil().siblings().hasClass('active')) {
            $(sender).parent().parent().parent().addClass('active');
        }
    }
}
function clearSelection() {

    $('#ctCategory #CategoryList .category').each(function (i, e) {

        $(e).removeClass('active');
    })

    $('#ctCategory #CategoryList .subcategory').each(function (i, e) {
        $(e).removeClass('active');

        removeSubCatArr.push({ ID: $(e).attr('id').toString() });

    });
    $("#ddlView").change();

}
function removeSelection(id) {


    $('#ctCategory #CategoryList .active.category').each(function (i, e) {

        if ($(e).attr('id') == id) {

            $(e).children('ul').children('li').each(function (f, g) {

                $(g).removeClass('active');
                removeSubCatArr.push({ ID: $(g).attr('id').toString() });

            });
            $(e).removeClass('active');
            return true;
        }
    });

    $("#ddlView").change();
}
function DetailView(productId) {

    $.ajax({
        type: "POST",
        url: "/Customer/Shop/RedirectToDetailedView?productId=" + productId,
        contentType: false,
        processData: false,
        success: function (data) {


        },
        error: function () {

            alert("There was error uploading files!");

        },


    });

}
function AddToCart(productId) {

    if ($('#hfIsUserLoggeIn').val() == 1) {
        $.ajax({
            type: "POST",
            url: "/Customer/Shop/AddToCart?productId=" + productId + "&Count=" + 1,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.message == "Success") {
                    toastr.success("Added in cart");

                    $('.minicart-qty').html(data.cartCount);
                    $('.minicart-total').html(data.totalPrice + " ₹");
                }

            },
            error: function () {

                toastr.error("There was an error, may be you are not logged in!");

            },


        });

    }
    else {

        toastr.error("Please Sign In First");

        setTimeout(function () {
            location.href = '/Identity/Account/Login';

        }, 500)


        //tempUserCart.push({ "ProductId": productId });
        //window.sessionStorage.setItem("tempCartItems", JSON.stringify(tempUserCart));


        //var tempCart = JSON.parse(sessionStorage.getItem("tempCartItems"));//no brackets
        //var i;
        //for (i = 0; i < tempCart.length; i++) {
        //    console.log(tempCart[i]);
        //}


        //$.ajax({
        //    type: "POST",
        //    url: "/Customer/Shop/AddToCart?productId=" + productId + "&Count=" + 1 + "&IsTemp=" + 1,
        //    contentType: false,
        //    processData: false,
        //    success: function (data) {
        //        if (data.message == "Success") {
        //            toastr.success("Added in cart");

        //            $('.minicart-qty').html(data.cartCount);
        //            $('.minicart-total').html(data.totalPrice + " ₹");
        //        }

        //    },
        //    error: function () {

        //        toastr.error("There was an error, please contact to our administrator!");

        //    },


        //});


    }


}
function AddToCartWithCount(productId) {


    if ($('#hfIsUserLoggeIn').val() == 1) {

        var Count = $('.qty-input').val();

        $.ajax({
            type: "POST",
            url: "/Customer/Shop/AddToCart?productId=" + productId + "&Count=" + Count,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.message == "Success") {
                    toastr.success("Added in cart");
                    sessionStorage.setItem("CartCount", data.cartCount);
                    $('.minicart-qty').html(data.cartCount);
                    $('.minicart-total').html(data.totalPrice + " ₹");
                }

            },
            error: function () {

                toastr.error("There was an error, may be you are not logged in!");

            },


        });

    }
    else {

        toastr.error("Please Sign In First");

        setTimeout(function () {
            location.href = '/Identity/Account/Login';

        }, 500)
    }



}
function AddToWishlist(productId) {

    if ($('#hfIsUserLoggeIn').val() == 1) {

        $.ajax({
            type: "POST",
            url: "/Customer/Shop/AddToWishlist?productId=" + productId,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.message == "Success") {
                    toastr.success("Added in wishlist");
                    console.log(data.wishlist);
                }

            },
            error: function () {

                toastr.error("There was an error, may be you are not logged in!");

            },


        });

    }
    else {

        toastr.error("Please Sign In First");

        setTimeout(function () {
            location.href = '/Identity/Account/Login';

        }, 500)
    }


}
function RemoveFromWishlist(productId) {

}



