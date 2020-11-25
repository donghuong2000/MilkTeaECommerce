$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Deliveries/GetAll"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="#" data-toggle="modal" data-target="#EditModal" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/Admin/Deliveries/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>                           
                            
                           `;
                }, "width": "40%"
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
    var Id = $('#Create_deliveriesid').val();
    var Name = $('#Create_deliveriesname').val();
    $.ajax({
        method: 'POST',
        url: "/Admin/Deliveries/Create",
        data: { id: Id, name: Name },
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
    var oldId = $('#update_deliveriesid_old').val();
    var newId = $('#update_deliveriesid_new').val();
    var Name = $('#update_deliveriesname').val();
    $.ajax({
        method: 'POST',
        url: "/Admin/Deliveries/Update",
        data: { newid: newId, newname: Name },
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
        url: '/Admin/Deliveries/Get/' + ad,
        success: function (data) {
            console.log(data)
            modal.find('#update_deliveriesid_old').val(data.data.id)
            modal.find('#update_deliveriesid_new').val(data.data.id)
            modal.find('#update_deliveriesname').val(data.data.name)
        
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