$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Products/GetAll"
        },
        "columns": [
            {
                "data": { id: "id", image: "image" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Product/Detail/${data.id}">
                                    <img src="${data.image}" height = 100 width =70 class="rounded"/>
                                </a>
                            </div>
                            `

                }
            },
            { "data": "name" },
            { "data": "category" },
            { "data": "shop" },
            { "data": "de" },
            { "data": "price" },
            { "data": "quantity" },
            {
                "data": { isConfirm:"isConfirm",id:"id" },
                "render": function (data) {
                    if (data.isConfirm) {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnLock("${data.id}")  class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-lock-open"></i>
                                </a>
                               
                            </div>  

                            `
                    }
                    else {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnLock("${data.id}")  class="btn btn-danger text-white" style="cursor:pointer">
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
                                <a href="#" data-toggle="modal" data-target="#DetailModalProduct" data-whatever="${data}" class="btn btn-dark text-white" style="cursor:pointer"">
                                    Chi Tiết
                                </a>
                                <a onClick=Delete("/Admin/Products/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Xóa
                                </a>
                            </div>                           
                            
                           `;
                }
            }
        ]

    });

   







});

function LockUnLock(id) {
    $.ajax({
        url: "/Admin/Products/LockUnLock/" + id,
        success: function (data) {
            if (data.success) {
                toastr.success(data.message)
                $('#dataTable').DataTable().ajax.reload();
            }
            else {
                toastr.error(data.message)
            }
        }
    })
}



$('#DetailModalProduct').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var ad = button.data('whatever') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/Admin/Products/Get/' + ad,
        success: function (data) {
            console.log(data)
            modal.find('#update_productid_old').val(data.data.id)
            modal.find('#update_deliveriesid_new').val(data.data.id)
            modal.find('#update_productname').val(data.data.name)
            modal.find('#update_productdescription').val(data.data.description)
            modal.find('#update_productimage').val(data.data.imageUrl)
            modal.find('#update_productprice').val(data.data.price)
            modal.find('#update_productstatus').val(data.data.status)
            modal.find('#update_productquantity').val(data.data.quantity)
            modal.find('#update_productcategory').val(data.data.category.name)
            modal.find('#update_productshopid').val(data.data.shopId)
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