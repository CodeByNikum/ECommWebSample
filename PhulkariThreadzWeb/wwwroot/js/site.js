// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



$(document).ready(function () {

   
    if ($('#hfIsUserLoggeIn').val() == 1) {

        addProduct();


        $('.minicart-link').click(function () {


            $("#dvMiniCart").load("/Customer/Shop/MiniCartView", function () {

                setTimeout(function () {

                    if ($("#dvMiniCart").find('.minicart-prd').length > 0) {

                        $('.minicart-empty').addClass('d-none');
                    }
                    else {
                        $('.minicart-empty').removeClass('d-none');
                    }

                }, 100);
            });
           
        });


        
    }

});


function addProduct() {

    $.ajax({
        type: "GET",
        url: "/Customer/Shop/GetCartCount",
        contentType: false,
        processData: false,
        success: function (data) {

            $('.minicart-qty').html(data.cartCount);
           
            if (data.totalPrice == 0) {
                $('.minicart-total').html('');
            }
            else {
                $('.minicart-total').html(data.totalPrice + " ₹");
            }
            
        },
        error: function () {

            toastr.error("There was an error, may be you are not logged in!");

        },


    });

}



function GetWishlistCount() {

    $.ajax({
        type: "GET",
        url: "/Customer/Shop/GetWishlistCount",
        contentType: false,
        processData: false,
        success: function (data) {
            $('.wishlist-qty').html(data.wishlistCount);
        },
        error: function () {
            toastr.error("There was an error!");
        },
    });

}

function remProduct(id) {

    $.ajax({
        type: "DELETE",
        url: "/Customer/Shop/RemoveFromCart?Id=" + id,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.message == "Success") {
                addProduct();
            }

        },
        error: function () {

            toastr.error("There was an error, may be you are not logged in!");

        },


    });
}

function closeMiniCart() {

    $('#dropdnMinicart').removeClass('is-opened');

}
