$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/DeliveryDetail/GetAll"
        },
        "columns": [
            { "data": "orderdetailid" },
            { "data": "deliveryid" },
            { "data": "address" },
            { "data": "note" },
            { "data": "price" },
            { "data": "datestart" },
            { "data": "dateend" },
            {
                "data": "orderdetailid",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="#" data-toggle="modal" data-target="#EditModal" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/Admin/DeliveryDetail/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>                           
                            
                           `;
                }
            }
        ]

    });

    $('.selectlist').select2({
        placeholder: "Select a Delivery",
        minimumInputLength: 1,
        ajax: {
            url: '/Admin/Deliveries/GetforSelect',
            data: function (params) {
                return {
                    q: params.term// search term
                };
            },
            processResults: function (data) {
                return {
                    results: data.items
                }
            },
        }



    });



});

$('#CreateSubmit').click(function () {
    var Orderdetailid = $('#Create_orderdetailid').val();
    var Deliveryid = $('#Create_deliveryid').val();
    var Address = $('#Create_address').val();
    var Note = $('#Create_note').val();
    var Price = $('#Create_price').val();
    var Datestart = $('#Create_datestart').val();
    var Dateend = $('#Create_dateend').val();
    $.ajax({
        method: 'POST',
        url: "/Admin/DeliveryDetail/Create",
        data: { orderdetailid: Orderdetailid, deliveryid: Deliveryid, address: Address, note: Note, price: Price, datestart: Datestart, dateend : Dateend },
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#dataTable').DataTable().ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
})
$('#editSubmit').click(function () {
    var Orderdetailid = $('#update_orderdetailid').val();
    var Deliveryid = $('#update_deliveryid').val();
    var Address = $('#update_address').val();
    var Note = $('#update_note').val();
    var Price = $('#update_price').val();
    var Datestart = $('#update_datestart').val();
    var Dateend = $('#update_dateend').val();
    $.ajax({
        method: 'POST',
        url: "/Admin/DeliveryDetail/Update",
        data: { orderdetailid: Orderdetailid, deliveryid: Deliveryid, address: Address, note: Note, price: Price, datestart: Datestart, dateend: Dateend },
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#dataTable').DataTable().ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
})
$('#EditModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var ad = button.data('whatever') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/Admin/DeliveryDetail/Get/' + ad,
        success: function (data) {
            console.log(data)
            console.log(modal.find('#update_dateend'))
            modal.find('#update_orderdetailid').val(data.data.orderDetailid)
            modal.find('#update_deliveryid').val(data.data.deliveryId)
            modal.find('#update_address').val(data.data.address)
            modal.find('#update_note').val(data.data.note)
            modal.find('#update_price').val(data.data.price)
            modal.find('#update_datestart').val(data.data.dateStart)
            modal.find('#update_dateend').val(data.data.dateEnd)
            
        
            //modal.find().val(data.data[0].id)
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
                        $('#dataTable').DataTable().ajax.reload();
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