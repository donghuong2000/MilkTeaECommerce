$(document).ready(function () {


    // chờ xác nhận
    $('#dataTableWait').DataTable({
        "ajax": {
            "url": "/Seller/OrderManager/GetAll?status=unconfirm"
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `<img href="#" src="${data}" height=50 width =50>`
                }
            },
            { "data": "title" },
            { "data": "num" },
            { "data": "customer" },
            { "data": "price" },
            { "data": "deliveryInfo" },
            { "data": "total" },
            {
                "data": {"id":"id","status":"status"},
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a onClick=ChangeStatus("${data.id}","confirmed")  class="btn btn-warning text-white" style="cursor:pointer">
                                    <i class="fas fa-check"></i>
                                </a>
                                <a onClick=ChangeStatus("${data.id}","cancelled") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-window-close"></i>
                                </a>
                            </div>                           
                            
                           `;
                }
            }
        ]

    });
    // đã xác nhận
    $('#dataTableConfirm').DataTable( {
        "ajax": {
            "url": "/Seller/OrderManager/GetAll?status=confirmed"
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `<img href="#" src="${data}" height=50 width =50>`
                }
            },
            { "data": "title" },
            { "data": "num" },
            { "data": "customer" },
            { "data": "price" },
            { "data": "deliveryInfo" },
            { "data": "total" },
            {
                "data": { "id": "id", "status": "status" },
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a onClick=ChangeStatus("${data.id}","cancelled") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-window-close"></i>
                                </a>
                            </div>                           
                            
                           `;
                }
            }
        ]

    });
    // đang vận chuyển
    $('#dataTableDelivery').DataTable({
        "ajax": {
            "url": "/Seller/OrderManager/GetAll?status=delivery"
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `<img href="#" src="${data}" height=50 width =50>`
                }
            },
            { "data": "title" },
            { "data": "num" },
            { "data": "customer" },
            { "data": "price" },
            { "data": "deliveryInfo" },
            { "data": "total" },
            {
                "data": { "id": "id", "status": "status" },
                "render": function (data) {
                    return `
                             <div class="text-center">
                                Đang giao
                            </div>                           
                            
                           `;
                }
            }
        ]

    });
    // đã vận chuyển
    $('#dataTableDelivered').DataTable({
        "ajax": {
            "url": "/Seller/OrderManager/GetAll?status=deliveried"
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `<img href="#" src="${data}" height=50 width =50>`
                }
            },
            { "data": "title" },
            { "data": "num" },
            { "data": "customer" },
            { "data": "price" },
            { "data": "deliveryInfo" },
            { "data": "total" },
            {
                "data": { "id": "id", "status": "status" },
                "render": function (data) {
                    return `
                             <div class="text-center">
                                Đã giao
                            </div>                           
                            
                           `;
                }
            }
        ]

    });
    // đã hủy
    $('#dataTableCancel').DataTable({
        "ajax": {
            "url": "/Seller/OrderManager/GetAll?status=cancelled"
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `<img href="#" src="${data}" height=50 width=50>`
                }
            },
            { "data": "title" },
            { "data": "num" },
            { "data": "customer" },
            { "data": "price" },
            { "data": "deliveryInfo" },
            { "data": "total" },
            {
                "data": { "id": "id", "status": "status" },
                "render": function (data) {
                    return `
                             <div class="text-center">
                                Đã Hủy
                            </div>                           
                            
                           `;
                }
            }
        ]

    });

});


function ChangeStatus(id, status) {
    $.ajax({
        method: 'POST',
        url:'/Seller/OrderManager/ChangeStatus',
        data: { id: id, status: status },
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#dataTableWait').DataTable().ajax.reload();
            } else {
                toastr.error(data.message);
                $('#dataTableWait').DataTable().ajax.reload();
            }
        }
    })
}
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Delete(id,url) {
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        swalWithBootstrapButtons.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                        $('#'+id).DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            data.message,
                            'error'
                        )
                    }
                }

            })

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your record is safe :)',
                'error'
            )
        }
    })
}
$('a[data-toggle="pill"]').on('shown.bs.tab', function (e) {
    var btn =  e.target // newly activated tab
    var tab = btn.getAttribute('href');
    var table = $(tab).find('table');
    table.DataTable().ajax.reload();
    
    
})