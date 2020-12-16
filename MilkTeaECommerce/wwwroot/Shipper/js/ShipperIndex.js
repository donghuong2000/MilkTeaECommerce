$(document).ready(function () {
   
    $('#dataTableNull').DataTable({

        "ajax": {
            "url": '/shipper/deliverydetails/getall',
            "data": {
                "status": "confirmed"
            }
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `
                            <img src="${data} />
                            `
                }
            },
            { "data": "title" },
            { "data": "customer" },
            { "data": "address" },
            { "data": "shopName" },
            { "data": "shopAddress" },
            {
                "data": "id",
                "render": function (data) {
       
                    return `
                             <div class="text-center" >
                                <a id="a-detail" data-toggle="modal" data-target="#Detail" data-id="${data}" data-value="#dataTableNull"
                                class="btn btn-success" style="font-size:small">Chi tiết</a>
                                <a onClick=GetOrder("/Shipper/DeliveryDetails/Get/${data}","#dataTableNull") class="btn btn-danger text-white" 
                                 style="cursor:pointer">Nhận</a>
                            </div>  

                           `
                }
            }
        ]
    });
    $('#dataTableConfirm').DataTable({

        "ajax": {
            "url": '/shipper/deliverydetails/getall/',
            "data": {
                "status":"admit"
            }

        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `
                            <img src="${data} />
                            `
                }
            },
            { "data": "title" },
            { "data": "customer" },
            { "data": "address" },
            { "data": "shopName" },
            { "data": "shopAddress" },
            {
                "data": "id",
                "render": function (data) {
                   
                    return `
                             <div class="text-center" >
                                <a id="a-detail" data-toggle="modal" data-target="#Detail" data-id="${data}" data-value="#dataTableConfirm"
                                class="btn btn-success" style="font-size:small">Chi tiết</a>
                                <a onClick=GetOrder("/Shipper/DeliveryDetails/Get/${data}","#dataTableConfirm") class="btn btn-danger text-white" 
                                 style="cursor:pointer">Đã lấy hàng</a>
                            </div>  

                           `
                }
            }
        ]
    });
    $('#dataTableGet').DataTable({

        "ajax": {
            "url": '/shipper/deliverydetails/getall/',
            "data": {
                "status": "delivery"
            }

        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `
                            <img src="${data} />
                            `
                }
            },
            { "data": "title" },
            { "data": "customer" },
            { "data": "address" },
            { "data": "shopName" },
            { "data": "shopAddress" },
            {
                "data": "id",
                "render": function (data) {

                    return `
                             <div class="text-center" >
                                <a id="a-detail" data-toggle="modal" data-target="#Detail" data-id="${data}" data-value="#dataTableGet"
                                class="btn btn-success" style="font-size:small">Chi tiết</a>
                                <a onClick=GetOrder("/Shipper/DeliveryDetails/Get/${data}","#dataTableGet") class="btn btn-danger text-white" 
                                 style="cursor:pointer">Hoàn thành</a>
                            </div>  

                           `
                }
            }
        ]
    });
    $('#dataTableDone').DataTable({

        "ajax": {
            "url": '/shipper/deliverydetails/getall/',
            "data": {
                "status": "deliveried"
            }

        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `
                            <img src="${data} />
                            `
                }
            },
            { "data": "title" },
            { "data": "customer" },
            { "data": "address" },
            { "data": "shopName" },
            { "data": "shopAddress" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a id="a-detail" data-toggle="modal" data-target="#Detail" data-id="${data}" data-value="#dataTableDone"
                                class="btn btn-success" style="font-size:small">Chi tiết</a>
                            </div>  

                           `
                }
            }
        ]
    });
});
$('#Detail').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idOrder = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    var table = $("#a-detail").data('value');

    $.ajax({
        method: 'GET',
        url: '/Shipper/deliverydetails/Details/' + idOrder,
        success: function (data) {

            if (data.status == "confirmed") {
                data.status = "admit";
            }
            else if (data.status == "admit") {
                data.status = "delivery";
            }
            else if (data.status == "delivery") {
                data.status = "deliveried";
            }
            //else {
            //    data.status = "hidden";
            //}
                
            var url='"/Shipper/DeliveryDetails/Get/'+data.id+'"';
            modal.find('#Id').val(data.id);
            //modal.find('#Id').attr('src', "iadasdsa");
            modal.find('#NameP').val(data.title);
            modal.find('#Image').attr('src',data.image);
            modal.find('#Count').val(data.count);
            modal.find('#Price').val(data.price);
            modal.find('#NameC').val(data.customer);
            modal.find('#Address').val(data.address);
            modal.find('#Payment').val(data.payment); 
            modal.find('#NameS').val(data.shopName);
            modal.find('#AddressS').val(data.shopAddress);
            modal.find("#getOrder").attr('onclick', "GetOrder(" + url + ',"' + table + '")');
            modal.find("#getOrder").show();
            modal.find("#getOrder").text(data.status);
            if (data.status == "hidden") {
                modal.find("#getOrder").hide();
            }
        }
    })
})

const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function GetOrder(url, table) {

    if (url == "") {
        var id = $('#Id').val();
        
        url = '/Shipper/DeliveryDetails/get/' + id;

    }
    Swal.fire(
        'Xác nhận đơn'
    ).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {

                    if (data.success) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Đã thay đổi trạng thái đơn',
                            showConfirmButton: false,
                            timer: 1500
                        })
                        $(table).DataTable().ajax.reload();
                        $('#Detail').removeClass('show');
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Lỗi',
                            'Không thể nhận đơn hàng',
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Hủy',
                'Your record is safe :)',
                'error'
            )
        }
    })
}
$('a[data-toggle="pill"]').on('shown.bs.tab', function (e) {
    var btn = e.target // newly activated tab
    var tab = btn.getAttribute('href');
    var table = $(tab).find('table');
    table.DataTable().ajax.reload();
})

