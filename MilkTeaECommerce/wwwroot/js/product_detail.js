$(document).ready(function () {
    $.ajax({
        method: 'GET',
        url: "/Product/Get_avarage_rating_shop_and_product/",
        success: function (data) {
            console.log(data.rating_shop)
            console.log(data.rating_product)
            if (data.rating_shop == 0 && data.rating_product == 0)
            {
                document.getElementById("rating5_shop").checked = false;
                document.getElementById("rating5_product").checked = false;
            }
            else if (data.rating_shop != 0 && data.rating_product == 0)
            {
                document.getElementById("rating" + data.rating_shop + "_shop").checked = true;
                document.getElementById("rating5_product").checked = false;
            }
            else if (data.rating_shop == 0 && data.rating_product != 0)
            {
                document.getElementById("rating5_shop").checked = false;
                document.getElementById("rating" + data.rating_product + "_product").checked = true;
            }
            else
            {
                document.getElementById("rating" + data.rating_shop + "_shop").checked = true
                document.getElementById("rating" + data.rating_product + "_product").checked = true
            }
            $(':radio:not(:checked)').attr('disabled', true);

        }
    });
})


