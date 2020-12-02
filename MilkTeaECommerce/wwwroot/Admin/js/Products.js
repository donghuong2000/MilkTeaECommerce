$(document).ready(function () {
    $('#productTable').DataTable({
        "ajax": {
            "url": "/Admin/Products/GetAll"
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "description" },
            { "data": "imageUrl" },
            { "data": "price" },
            { "data": "status" },
            { "data": "quantity" },
            { "data": "categoryId" },
            { "data": "shopId" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                <a href="#" data-toggle="modal" data-target="#EditModalProduct" data-whatever="${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    Sửa
                                </a>
                                <a href="#" data-toggle="modal" data-target="#DetailModalProduct" data-whatever="${data}" class="btn btn-dark text-white" style="cursor:pointer"">
                                    Detail
                                </a>
                                <a onClick=Delete("/Admin/Products/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    Xóa
                                </a>
                            </div>                           
                            
                           `;
                }, "width": "40%"
            }
        ]

    });

    $('.selectlist').select2({
        placeholder: "Select a product",
        minimumInputLength: 1,
        ajax: {
            url: '/Admin/Products/GetforSelect',
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

$('#CreateSubmitProduct').click(function () {
    var Id = $('#create_productid').val();
    var Name = $('#create_productname').val();
    var Description = $('#create_productdescription').val();
    var ImageUrl = $('#create_productimage').val();
    var Price = $('#create_productprice').val();
    var Status = $('#create_productstatus').val();
    var Quantity = $('#create_productquantity').val();
    var Category = $("#create_productcategory").find(":selected").val();
    var ShopId = $('#create_productshopid').find(":selected").val();
    $.ajax({
        method: 'POST',
        url: "/Admin/Products/Create",
        data: { id: Id, name: Name, description: Description, imageUrl: ImageUrl, price: Price, status: Status, quantity: Quantity, categoryid: Category, shopId: ShopId },
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
$('#editSubmitProduct').click(function () {
    var oldId = $('#update_productid_old').val();
    var newId = $('#update_productid_new').val();
    var Name = $('#update_productname').val();
    var Description = $('#update_productdescription').val();
    var ImageUrl = $('#update_productimage').val();
    var Price = $('#update_productprice').val();
    var Status = $('#update_productstatus').val();
    var Quantity = $('#update_productquantity').val();
    var Category = $('#update_productcategory').val();
    var ShopId = $('#update_productshopid').val();
    $.ajax({
        method: 'POST',
        url: "/Admin/Products/Update",
        data: { oldid: oldId, name: Name, description: Description, imageUrl: ImageUrl, price: Price, status: Status, quantity: Quantity, category: Category, shopId: ShopId },
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
$('#EditModalProduct').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var ad = button.data('whatever') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/Admin/Products/Get/' + ad,
        success: function (data) {
            console.log(data)
            modal.find('#update_productid_old').val(data.data.id)
            //modal.find('#update_deliveriesid_new').val(data.data.id)
            modal.find('#update_productname').val(data.data.name)
            modal.find('#update_productdescription').val(data.data.description)
            modal.find('#update_productimage').val(data.data.imageUrl)
            modal.find('#update_productprice').val(data.data.price)
            modal.find('#update_productstatus').val(data.data.status)
            modal.find('#update_productquantity').val(data.data.quantity)
            modal.find('#update_productcategory').val(data.data.categoryId)
            modal.find('#update_productshopid').val(data.data.shopId)
            //modal.find().val(data.data[0].id)
        }
    })



})
$('#DetailModalProduct').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var ad = button.data('whatever') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/Admin/Products/Edit/' + ad,
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