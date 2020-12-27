$(document).ready(function () {
   // data table hiển thị các đơn hàng đã confirmed để cho shipper nhận
    $('#dataTableNull').DataTable({

        "ajax": {
            "url": '/shipper/home/GetAllConfirmed',//xác nhận (hiển thị cho shipper ở phần nhận đơn)
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
                                <a onClick=GetOrder("/Shipper/home/Get/${data}","#dataTableNull") class="btn btn-danger text-white" 
                                 style="cursor:pointer">Nhận</a>
                            </div>  

                           `
                }
            }
        ]
    });
    $('#dataTableConfirm').DataTable({

        "ajax": {
            "url": '/shipper/home/getall/',
            "data": {
                "status": "received"     // hiển thị cho shipper ở phần đã nhận đơn
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
                                <a onClick=GetOrder("/Shipper/home/Get/${data}","#dataTableConfirm") class="btn btn-danger text-white" 
                                 style="cursor:pointer">Đã lấy hàng</a>
                            </div>  

                           `
                }
            }
        ]
    });
    $('#dataTableGet').DataTable({

        "ajax": {
            "url": '/shipper/home/getall/',
            "data": {
                "status": "delivery"    // đang vận chuyển ( hiển thị cho shipper ở phần đã lấy hàng) 
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
                                <a onClick=GetOrder("/Shipper/home/Get/${data}","#dataTableGet") class="btn btn-danger text-white" 
                                 style="cursor:pointer">Hoàn thành</a>
                                <a onClick=Cancel("/Shipper/home/Cancel/${data}","#dataTableGet") class="btn btn-danger text-white" 
                                 style="cursor:pointer">Đã hủy</a>
                            </div>  

                           `
                }
            }
        ]
    });
    $('#dataTableDone').DataTable({

        "ajax": {
            "url": '/shipper/home/getall/',
            "data": {
                "status": "deliveried"  // hoàn thành
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
    $('#dataTableCancelled').DataTable({

        "ajax": {
            "url": '/shipper/home/getall/',
            "data": {
                "status": "cancelled"   //đã hủy
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
    var text = "";
    $.ajax({
        method: 'GET',
        url: '/Shipper/home/Details/' + idOrder,
        success: function (data) {
            if (data.status == "confirmed") {
                data.status = "received";
                text = "Nhận đơn";
            }
            else if (data.status == "received") {
                data.status = "delivery";
                text = "Đã lấy hàng";
            }
            else if (data.status == "delivery") {
                data.status = "deliveried";
                text = "Hoàn thành";
            }
                
            var url='"/Shipper/home/Get/'+data.id+'"';
            modal.find('#Id').val(data.id);
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
            modal.find("#getOrder").text(text);
            if (data.status == "deliveried" || data.status=="cancelled") {
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
        
        url = '/Shipper/home/get/' + id;

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

function Cancel(url, table) {

    if (url == "") {
        var id = $('#Id').val();

        url = '/Shipper/home/Cancel/' + id;

    }
    Swal.fire(
        'Xác nhận hủy đơn'
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
                            title: 'Đã hủy đơn hàng',
                            showConfirmButton: false,
                            timer: 1500
                        })
                        $(table).DataTable().ajax.reload();
                        $('#Detail').removeClass('show');
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Lỗi',
                            'Không thể hủy đơn hàng',
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

