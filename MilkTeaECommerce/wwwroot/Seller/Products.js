$(document).ready(function () {
    $('#productTableSeller').DataTable({
        "ajax": {
            "url": "/Seller/Product/GetAll"
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return `
                                <img src="${data}" style="width:5rem" />
                            `
                }
            },
            { "data": "name" },
            { "data": "cate" },
            { "data": "de" },
            { "data": "price" },
            { "data": "quantity" },
            { "data": "confirm" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="/Seller/Product/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                <a href="#" data-toggle="modal" data-target="#DetailModalProductSeller" data-whatever="${data}" class="btn btn-dark text-white" style="cursor:pointer"">
                                    Detail
                                </a>
                                <a onClick=Delete("/Seller/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Xóa
                                </a>
                            </div>                           
                            
                           `;
                }
            }
        ]

    });

    $('.selectlist').select2({
        placeholder: "Select a product",
        minimumInputLength: 1,
        ajax: {
            url: '/Seller/Product/GetforSelect',
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


$('#DetailModalProductSeller').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var ad = button.data('whatever') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/Seller/Product/Get/' + ad,
        success: function (data) {
            console.log(data)
            modal.find('#update_productimage').attr('src', data.data.imageUrl)
            modal.find('#update_productid_old').val(data.data.id)
            modal.find('#update_productname').val(data.data.name)
            modal.find('#update_productdescription').val(data.data.description)
            modal.find('#update_productprice').val(data.data.price)
            modal.find('#update_productquantity').val(data.data.quantity)
            modal.find('#update_productcategory').val(data.data.categoryId)
            modal.find('#update_productshopid').val(data.data.shopId)
            modal.find('#update_productconfirm').val(data.data.isConfirm)
            
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