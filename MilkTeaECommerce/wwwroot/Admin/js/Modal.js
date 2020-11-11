$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    var a = document.getElementsByClassName("modal-tag");

    $('.modal-tag').on("click", function () {
        console.log('a');
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);

        $.get(decodedUrl).done(function (data) {
            $.get(decodedUrl).done(function (data) {
                PlaceHolderElement.html(data);
                PlaceHolderElement.find('.modal').modal('show');
            })
        })

        PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
            event.preventDefault();
            var form = $(this).parents('.modal').find('form');
            var actionUrl = form.attr('action');
            var sendData = form.serialize();
            $.post(actionUrl, sendData).done(function (data) {
                PlaceHolderElement.find('.modal').modal('hide');
            })
        })

    })
})


    //$(function modal() {
    //    var PlaceHolderElement = $('#PlaceHolderHere');

    //    var a = document.getElementsByClassName("modal-tag");
    //    a = document.addEventListener('click', function () {

    //    })

    //    $('button[data-toggle="ajax-modal"]').click(function (event) {

    //        var url = $(this).data('url');
    //        var decodedUrl = decodeURIComponent(url);

    //        $.get(decodedUrl).done(function (data) {
    //            $.get(decodedUrl).done(function (data) {
    //                PlaceHolderElement.html(data);
    //                PlaceHolderElement.find('.modal').modal('show');
    //            })
    //        })

    //        PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
    //            event.preventDefault();
    //            var form = $(this).parents('.modal').find('form');
    //            var actionUrl = form.attr('action');
    //            var sendData = form.serialize();
    //            $.post(actionUrl, sendData).done(function (data) {
    //                PlaceHolderElement.find('.modal').modal('hide');
    //            })
    //        })

    //    })