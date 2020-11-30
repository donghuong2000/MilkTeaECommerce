$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": '/shipper/deliverydetails/getall'
        },
        "columns": [
            { "data": "headerId" },
            { "data": "address" },
            { "data": "phone" },    
            { "data": "payment" },
            { "data": "price" },  
            {
                "data": "orderDetailId",
                "render": function (data) {
                    return `
                             <div class="text-center" >
                                <a href="/Shipper/DeliveryDetails/Details/${data}" 
                                class="btn btn-success" style="font-size:small">Details</a>
                                <a onClick=Delete("/Shipper/DeliveryDetails/Get/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                   Nhận
                                </a>
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
            { "data": "headerId" },
            { "data": "address" },
            { "data": "phone" },
            { "data": "payment" },
            { "data": "price" },
            { "data": "status" },
            {
                "data": "orderDetailId",
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
        url: '/Shipper/deliverydetails/changestatus/' + idOrder,
        success: function (data) {
            console.log(data);
            
            modal.find('#Id').val(data.id);
            modal.find('#Status').val(data.status);
            modal.find('#Order').val(data.orderDetailId);
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

