$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": '/shipper/deliverydetails/getall'
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
            //{ "data": "quantity" },
            //{ "data": "price" },
            { "data": "customer" },
            { "data": "address" },
            //{ "data": "payment" },
            { "data": "shopName" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a href="/Shipper/DeliveryDetails/Details/${data}" 
                                class="btn btn-success" style="font-size:small">Details</a>
                                <a href="/Shipper/DeliveryDetails/Details/${data}" 
                                class="btn btn-success" style="font-size:small">Details</a>
                                 <a href="#" data-target="#Detail" data-toggle="modal" data-id="${data}" 
                                class="btn btn-success" style="font-size:small">Nhaanj</a>
                            </div>  

                           `
                }
            }
        ]
    });
    $('#dataTableDetail').DataTable({
        "ajax": {
            "url": '/shipper/deliverydetails/getorder'
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
            //{ "data": "quantity" },
            //{ "data": "price" },
            { "data": "customer" },
            { "data": "address" },
            //{ "data": "payment" },
            { "data": "shopName" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a href="/Shipper/DeliveryDetails/changestatus/${data}" 
                                class="btn btn-success" style="font-size:small">Thay đổi trạng thái</a>
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
    console.log(idOrder)
    $.ajax({
        method: 'GET',
        url: '/Shipper/deliverydetails/Details/' + idOrder,
        success: function (data) {
            console.log(data);
            console.log(modal.find('#Id').val());
            modal.find('#Id').val(data.id);
            modal.find('#NameP').val(data.title);
            modal.find('#Image').val(data.image);
            modal.find('#Count').val(data.count);
            modal.find('#Price').val(data.price);
            modal.find('#NameC').val(data.customer);
            modal.find('#Address').val(data.address);
            modal.find('#Payment').val(data.payment);
            modal.find('#NameS').val(data.shopName);
            //modal.find('#AddressS').val(data.orderDetailId);
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
function Delete(url) {
    Swal.fire(
        'Xác nhận đơn'
    ).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    console.log(data);
                    if (data.success) {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Your work has been saved',
                            showConfirmButton: false,
                            timer: 1500
                        })
                        //swalWithBootstrapButtons.fire(
                        //    'Deleted!',
                        //    'Your file has been deleted.',
                        //    'success'
                        //);
                        $('#dataTable').DataTable().ajax.reload();
                    }
                    else {
                        swalWithBootstrapButtons.fire(
                            'Error',
                            'Can not delete this, maybe it not exit or error from sever',
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

