$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url":'/admin/shop/getall'
        },
        "columns": [
            {
                "data": "image",
                "render": function (data) {
                    return ` 
                            <img src="${data}" height =50 width=50 class="rounded-circle" />

                            `
                }
            },
            { "data": "name" },
            { "data": "de" },
            { "data": "count" },
            {
                "data": {
                    id: "id", isban:"isban"
                },
                "render": function (data) {
                    
                    if (data.isban){
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

            }
           


        ]

    });
}); 
function LockUnlock(data) {
    $.ajax({
        url: '/admin/shop/lockunlock/',
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



