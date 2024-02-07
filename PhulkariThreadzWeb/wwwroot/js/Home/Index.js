$(document).ready(function () {

    BindCollections();

    if ($('#hfIsUserLoggeIn').val() == 1) {

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


});

function BindCollections() {

    var location = window.location.origin;

    $.getJSON("" + location + "/Customer/Home/GetAllCategories", function (result) {
        $("#dvCollections").html("");
        var data = result.data;

        var html = '<div class="title-tabs">';

        if (data.length > 0) {

            for (i = 0; i < data.length; i++) {

                html += '<h2 class="h3-style" style="cursor:pointer">' +
                    '<a id="showCatProducts" onclick="showCategoryProducts(' + data[i].id + ', this)" data-grid-tab-title><span class="title-tabs-text theme-font">' + data[i].name + '</span></a>' +
                    '</h2>';
            }
        }
        else {

        }
        html += '</div>' +
            '<div class="ps__scrollbar-x-rail" style="left: 0px; bottom: 0px;"><div class="ps__scrollbar-x" tabindex="0" style="left: 0px; width: 0px;"></div></div>' +
            '<div class="ps__scrollbar-y-rail" style="top: 0px; right: 0px;"><div class="ps__scrollbar-y" tabindex="0" style="top: 0px; height: 0px;"></div></div>';
        $("#dvCollections").html(html);
        $("#showCatProducts").click();
    });


}

function showCategoryProducts(categoryId, sender) {
    $('h2').removeClass("active");
    $(sender).parent().addClass("active");

    var location = window.location.origin;

    $.getJSON("" + location + "/Customer/Home/GetProductsByCategory?Id=" + categoryId + "", function (result) {

        var data = result.data;


        var html = '';

        if (data.length > 0) {

            for (i = 0; i < data.length; i++) {

                html +=
                    '<div class="prd prd--style2 prd-labels--max prd-w-lg">' +
                    '<div class="prd-inside">' +
                    '<div class="prd-img-area">' +
                    '<a href ="/Customer/Shop/ProductDetailView?productId=' + data[i].productId + '" class="prd-img image-hover-scale image-container" style="padding-bottom: 128.48%">' +
                    '<img src="' + data[i].mainImageUrl + '" alt=' + data[i].productName + ' class="js-prd-img lazyload fade-up" />' +
                    '<div class="foxic-loader"></div>' +
                    '<div class="prd-big-squared-labels"></div > ' +
                    '</a> ' +
                    '<div class="prd-circle-labels">' +
                    '<a href="#" class="circle-label-compare circle-label-wishlist--add js-add-wishlist mt-0" title="Add to wishlist"><i class="icon-heart-stroke"></i></a>' +
                    '<a href ="#" class="circle-label-compare circle-label-wishlist--off js-remove-wishlist mt-0" title="Remove from wishlist"><i class="icon-heart-hover"></i></a>' +
                    '<a onclick="OpenModel(`' + data[i].productId + '`)" class="circle-label-qview prd-hide-mobile" ><i class="icon-eye"></i><span>QUICK VIEW</span></a>' +
                    '</div > ' +

                    '<ul class="list-options color-swatch" id="ulOptionImages' + i + '">' +


                    '</ul > ' +
                    '</div > ' +

                    '<div class="prd-info">' +
                    '<div class="prd-info-wrap">' +
                    '<div class="prd-info-top">' +
                    '<div id="#dvPrdRating' + i + '" class="prd-rating">' +

                    '</div>' +
                    '<div class="prd-tag">' +
                    '<a href="#">PhulkariThreadz</a>' +
                    '</div>' +
                    '</div > ' +
                    '<div  class="prd-rating justify-content-center">' +

                    '</div>' +
                    '<div class="prd-tag">' +
                    '<a href ="#">PhulkariThreadz</a></div>' +
                    '<h2 class="prd-title">' +
                    '<a href="product.html">' + data[i].productName + '</a></h2>' +
                    '<div class="prd-description">' + data[i].description + '</div>' +
                    '</div > ' +
                    '<div class="prd-hovers">' +
                    '<div class="prd-circle-labels">' +
                    '<div><a href="#" class="circle-label-compare circle-label-wishlist--add js-add-wishlist mt-0" title="Add To Wishlist"><i class="icon-heart-stroke"></i></a> <a href="#" class="circle-label-compare circle-label-wishlist--off js-remove-wishlist mt-0" title="Remove From Wishlist"><i class="icon-heart-hover"></i></a> </div>' +
                    '<div><a onclick="OpenModel(`' + data[i].productId + '`)" class="circle-label-qview prd-hide-mobile" ><i class="icon-eye"></i><span>QUICK VIEW</span></a ></div>' +
                    '</div > ' +
                    '<div class="prd-price"><div class="price-new">₹ ' + data[i].price + '</div></div>' +
                    '<div class="prd-action">' +
                    '<div class="prd-action-left">' +
                    '<form method="post">' +
                    '<a class="btn js-prd-addtocart" onclick="AddToCart(' + data[i].productId + ')" >Add To Cart</a>' +
                    '</form>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div> ';


            }


        }

        $('#dvProducts').html(html);
        var ulHtml = '';
        var ratingHtml = '';
        if (data.length != null) {

            for (var i = 0; i < data.length; i++) {

                for (var j = 0; j < data[i].productImages.length; j++) {

                    ulHtml += '<li data-image="' + data[i].productImages[j].imageUrl + '" onhover="toggleImage(this);">' +
                        '<a href="#" class="js-color-toggle" data-toggle="tooltip" data-placement="right" title="Color Name">' +
                        '<img data-src="' + data[i].productImages[j].imageUrl + '" class="lazyload fade-up" alt="Color Name">' +
                        '</a>' +
                        '</li>';
                }
                $('#ulOptionImages' + i + '').html(ulHtml);
                ulHtml = '';
            }


            for (var l = 0; l < data.length; l++) {
                for (var k = 0; k < parseInt(data[l].ratingCount); k++) {
                    ratingHtml += '<i class="icon-star-fill fill"></i>';
                }
                //for (var i = 0; i < 5 - parseInt(data[l].ratingCount); i++ )
                //{
                //    ratingHtml += '<i class="icon-star"></i>';
                //}


                $('#dvPrdRating' + l + '').html(ratingHtml);
                ratingHtml = '';
            }




        }
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


function toggleImage(sender) {

    $(sender).addClass("active");

}


function OpenModel(id) {

    console.log(id);
    $(".modal-content").load("/Customer/Shop/QuickViewModalBox?productId=" + parseInt(id), function () {

        $('#myModal').show();
        setTimeout(function () {


        }, 500);

    });


};
function CloseModal() {
    $('#myModal').hide();
}