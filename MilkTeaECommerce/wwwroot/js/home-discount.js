$(document).ready(function(){
    console.log('1');

    var product = "";
    $.ajax({
        method: 'GET',
        url: "/discounts/productdiscount",
        success: function (data) {
            $.each(data, function (index, value) {
                product = product +
                    '<div class="col-md-5" style="cursor: pointer;margin: 1rem 2rem;border: 1px solid rgba(0, 0, 0, 0.16);padding: 1rem;border-radius: 6px;"' + 
                    ' data-target="#discount-modal" data-toggle="modal" data-id="'+value.id+'">' +
                    '<div class="col-xs-4 img-deals" style=" margin-top: 1rem;">' +
                    value.code +
                    '</div>' +
                    '<div class="col-xs-8 img-deal1">' +
                    '<h3>' + value.name + '</h3>' +
                    '<a>' + value.dateStart + '</a>' +
                    '</div>' +
                    '</div>';
                $('#product').append(product);
            });
        }
    });
    var delivery = "";
    $.ajax({
        method: 'GET',
        url: "/discounts/deliverydiscount",
        success: function (data) {
            $.each(data, function (index, value) {
                delivery = delivery +
                    '<div class="col-md-5" style="cursor: pointer;margin: 1rem 2rem;border: 1px solid rgba(0, 0, 0, 0.16);padding: 1rem;border-radius: 6px;"' +
                    ' data-target="#discount-modal" data-toggle="modal" data-id="' + value.id + '">' +
                    '<div class="col-xs-4 img-deals" style=" margin-top: 1rem;">' +
                    value.code +
                    '</div>' +
                    '<div class="col-xs-8 img-deal1">' +
                    '<h3>' + value.name + '</h3>' +
                    '<a>' + value.dateStart + '</a>' +
                    '</div>' +
                    '</div>';
                $('#delivery').append(delivery);
            });
        }
    });
    var category = "";
    $.ajax({
        method: 'GET',
        url: "/discounts/catediscount",
        success: function (data) {
            $.each(data, function (index, value) {
                category = category +
                    '<div class="col-md-5" style="cursor: pointer;margin: 1rem 2rem;border: 1px solid rgba(0, 0, 0, 0.16);padding: 1rem;border-radius: 6px;"' +
                    ' data-target="#discount-modal" data-toggle="modal" data-id="' + value.id + '">' +
                    '<div class="col-xs-4 img-deals" style=" margin-top: 1rem;">' +
                    value.code +
                    '</div>' +
                    '<div class="col-xs-8 img-deal1">' +
                    '<h3>' + value.name + '</h3>' +
                    '<a>' + value.dateStart + '</a>' +
                    '</div>' +
                    '</div>';
                $('#category').append(category);
            });
        }
    });
});
$('#discount-modal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idDiscount = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Discounts/getdetails/' + idDiscount,
        success: function (data) {
            console.log(data);
            modal.find('#discount-name').text(data.name);
            modal.find('#discount-code').text(data.code);
            modal.find('#discount-desc').text("Mô tả : "+data.desc);
            modal.find('#discount-start').text("Ngày bắt đầu :" + data.dateStart);
            if (data.dateExpired != "01-01-0001") {
                modal.find('#discount-exp').text(" Ngày kết thúc :" + data.dateExpired);
            }
        }
    })
})
