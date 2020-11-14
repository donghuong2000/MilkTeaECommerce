$(document).ready(function () {

    $('.js-example-basic-multiple').select2();

    $('input').on('click', function () {
        if ($("#check_category").is(':checked')) {
            console.log('checked');
            document.getElementById("select-category").disabled = false;
        }
        else {
            document.getElementById("select-category").disabled = true;
        }
    });

    $('input').on('click', function () {
        if ($("#check-delivery").is(':checked')) {
            console.log('checked');
            document.getElementById("select-delivery").disabled = false;
        }
        else {
            document.getElementById("select-delivery").disabled = true;
        }
    });
    $('input').on('click', function () {
        if ($("#check-product").is(':checked')) {
            document.getElementById("select-product").disabled = false;
        }
        else {
            document.getElementById("select-product").disabled = true;
        }
    });

   
});