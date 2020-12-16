$(document).ready(function () {

    // total - products
    $.ajax({
        method:'GET',
        url: '/seller/home/TotalProducts',
        success: function (data) {
            console.log(data);
            $('#total-products').text(data);
        }
    });
    $.ajax({
        method: 'GET',
        url: '/seller/home/Earnings',
        success: function (data) {
            console.log(data);
        }
    });
});