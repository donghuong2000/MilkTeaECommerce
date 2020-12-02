$(document).ready(function () {
    $('#categoryTable').DataTable({
        "ajax": {
            "url": "/Admin/Categories/GetAll"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            {
                "data": "id",
                
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="#" data-toggle="modal" data-target="#EditModalCategory" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                <a href="#" data-toggle="modal" data-target="#DetailModalCategory" data-whatever="${data}" class="btn btn-dark text-white" style="cursor:pointer"">
                                    Chi tiết
                                </a>
                                <a onClick=Delete("/Admin/Categories/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Xóa
                                </a>
                            </div>                           
                            
                           `;
                }, "width": "40%"
            }
        ]

    });

    $('.selectlist').select2({
        placeholder: "Select a Category",
        minimumInputLength: 1,
        ajax: {
            url: '/Admin/Categories/GetforSelect',
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

$('#CreateSubmitCategory').click(function () {
    var Id = $('#create_categoryid').val();
    var Name = $('#create_categoryname').val();
    
    $.ajax({
        method: 'POST',
        url: "/Admin/Categories/Create",
        data: { id: Id, name: Name},
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#categoryTable').DataTable().ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
})


$('#EditSubmitCategory').click(function () {
    var oldId = $('#update_categoryid').val();
    var newId = $('#update_categoryid_new').val();
    var Name = $('#update_categoryname').val();
    $.ajax({
        method: 'POST',
        url: "/Admin/Categories/Update",
        data: { oldid: oldId, name: Name },
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#categoryTable').DataTable().ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
})

$('#EditModalCategory').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var ad = button.data('whatever') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/Admin/Categories/Get/' + ad,
        success: function (data) {
            console.log(data)
            modal.find('#update_categoryid').val(data.data.id)
            //modal.find('#update_deliveriesid_new').val(data.data.id)
            modal.find('#update_categoryname').val(data.data.name)
            //modal.find().val(data.data[0].id)
        }
    })



})

$('#DetailModalCategory').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var ad = button.data('whatever') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/Admin/Categories/Edit/' + ad,
        success: function (data) {
            console.log(data)
            modal.find('#detail_categoryid').val(data.data.id)
           // modal.find('#update_deliveriesid_new').val(data.data.id)
            modal.find('#detail_categoryname').val(data.data.name)
            
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
                        $('#categoryTable').DataTable().ajax.reload();
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