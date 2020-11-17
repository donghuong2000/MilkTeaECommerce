$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url":'/admin/user/getall'
        },
        "columns": [
            { "data": "username" },
            { "data": "email" },
            { "data": "sdt" },
            {
                "data": {
                    id: "id", lockout:"lockout"
                },
                "render": function (data) {
                    
                    if (!data.lockout){
                        return `
                            <div class="text-center">
                                <a onClick=LockUnlock("${data.id}") class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-lock-open"></i>
                                </a>
                            </div>                           
                        `
                    }
                    else {
                        return `
                             <div class="text-center">
                                <a onClick=LockUnlock("${data.id}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-lock"></i>
                                </a>
                            </div>      
                            `
                    }
                }

            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="#"class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onClick=Delete("/Admin/user/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>                           
                            
                           `
                }
            }


        ]

    });
}); 
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
                            'Your file has been deleted.',
                            'success'
                        );
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
function LockUnlock(data) {
    $.ajax({
        url: '/admin/user/lockunlock/',
        data: { id: data },
        success: function (result) {
            if (result.success) {
                toastr.success(result.message);
                $('#dataTable').DataTable().ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })

}



