



$('#submit_rating_modal').click(function () {
    var orderDetailId = $('#orderdetailid').val()
    var productId = $('#ratingproductid').val()
    var ratingUserId = $('#ratinguserid').val()
    var ratingContent = $('#ratingcontent').val()
    var ratingRate = 0
    for (var i = 5; i >= 1; i--) 
    {
        if (document.getElementById("star" + i.toString()).checked == true)
        {
            ratingRate = i;
            break;
        }
    }
    $.ajax({
        method: 'POST',
        url: "/Rating/Create",
        data: { orderdetailid: orderDetailId, productid: productId, userid: ratingUserId, content: ratingContent, rate: ratingRate },
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                window.location.reload();
            }
            else {
                toastr.error(data.message);
                window.location.reload();
            }
        }
    })
})

$('#RatingModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var productId = button.data('productid') // Extract info from data-* attributes 
    var orderdetailId = button.data('orderdetailid')
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: "/Rating/Get",
        data: { orderdetailid: orderdetailId, productid: productId},
        success: function (data) {
            console.log(data)
            console.log(orderdetailId)
            modal.find('#orderdetailid').val(data.data.orderdetailid)
            modal.find('#ratingproductid').val(data.data.productid)
            modal.find('#ratinguserid').val(data.data.user)
            modal.find('#ratingimage').attr('src',data.data.productimage)
            modal.find('#ratingname').html(data.data.productname)
            modal.find('#ratingcategory').html(data.data.productcategory)
            modal.find('#ratingcontent').html(data.data.content)
            var rate = "star" + Math.floor(data.data.rating).toString()
            console.log(rate)
            if (rate == "star0")
            {
                document.getElementById('star1').checked = false;
                document.getElementById('star2').checked = false;
                document.getElementById('star3').checked = false;
                document.getElementById('star4').checked = false; // neu get element by id khong get ra duoc thi nhung cau lenh phia sau se khong thuc hien duoc
                modal.find('#ratingrate').html(0);
            }
            else
            {
                document.getElementById(rate).checked = true;
            }
            

        }
    })
})


$('#RatingModalReadOnly').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var productId = button.data('productid') // Extract info from data-* attributes 
    var orderdetailId = button.data('orderdetailid')
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: "/Rating/Get",
        data: { orderdetailid: orderdetailId, productid: productId },
        success: function (data) {
            console.log(data)
            console.log(orderdetailId)
            modal.find('#orderdetailidreadonly').val(data.data.orderdetailid)
            modal.find('#ratingproductidreadonly').val(data.data.productid)
            modal.find('#ratinguseridreadonly').val(data.data.user)
            modal.find('#ratingimagereadonly').attr('src', data.data.productimage)
            modal.find('#ratingnamereadonly').html(data.data.productname)
            modal.find('#ratingcategoryreadonly').html(data.data.productcategory)
            modal.find('#ratingcontentreadonly').html(data.data.content)
            var rate = "star" + Math.floor(data.data.rating).toString()
            console.log(rate)
            if (rate == "star0") {
                modal.find('#ratingratereadonly').html(0);
                document.getElementById("star1readonly").checked = false;
                document.getElementById("star2readonly").checked = false;
                document.getElementById("star3readonly").checked = false;
                document.getElementById("star4readonly").checked = false; // neu get element by id khong get ra duoc thi nhung cau lenh phia sau se khong thuc hien duoc
                $(':radio:not(:checked)').attr('disabled', true);
            }
            else {
                document.getElementsByClassName('rating-star').readOnly = false; 
                modal.find('#ratingratereadonly').html(Math.floor(data.data.rating));
                document.getElementById(rate + "readonly").checked = true;
                $(':radio:not(:checked)').attr('disabled', true);
               
            }


        }
    })
})

