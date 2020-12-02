



$('#submitratingmodal').click(function () {
    var productid = $('#ratingproductid').val()
    var ratinguserid = $('#ratinguserid').val()
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

$('#RatingModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var ad = button.data('whatever') // Extract info from data-* attributes
    var modal = $(this)
    $.ajax({
        method: "GET",
        url: '/user/Get/' + ad,
        success: function (data) {
            console.log(data)
            modal.find('#ratingproductid').val(data.data.productid)
            modal.find('#ratinguserid').val(data.data.user)
            modal.find('#ratingimage').attr('src',data.data.productimage)
            modal.find('#ratingname').html(data.data.productname)
            modal.find('#ratingcategory').val(data.data.productcategory)
        }
    })



})


