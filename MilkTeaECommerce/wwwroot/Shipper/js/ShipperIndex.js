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
});

$('#Detail').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var idOrder = button.data('id') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: 'GET',
        url: '/Shipper/DeliveryDetails/Details/' + idOrder,
        success: function (data) {
            console.log(data.idDiscount);
            modal.find('#Id').val(data.id);
            modal.find('#Name').val(data.name);
            modal.find('#Description').val(data.des);
            modal.find('#DateStart').val(data.dateStart);
            modal.find('#DateExpired').val(data.dateEnd);
            modal.find('#TimesUsed').val(data.timeUsed);
            modal.find('#TimesUseLimit').val(data.timeuselimit);
            modal.find('#PercentDiscount').val(data.per);
            modal.find('#MaxDiscount').val(data.max);
            modal.find('#Code').val(data.code);
            modal.find('#CategoryDiscount').val(data.cate);
            modal.find('#DeliveryDiscount').val(data.deli);
            modal.find('#ProductDiscount').val(data.prod);
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

