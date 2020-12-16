$(document).ready(function () {

    // total - products
    $.ajax({
        method:'GET',
        url: '/seller/home/TotalProducts',
        success: function (data) {
            console.log(data);
        }
    });
});