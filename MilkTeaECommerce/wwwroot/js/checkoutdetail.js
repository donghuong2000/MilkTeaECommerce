$(document).ready(function () {
    $('#checkoutdetail').DataTable({
        "ajax": {
            "url": '/admin/user/getall'
        },
        "columns": [
            { "data": "username" },
            { "data": "email" },
            { "data": "sdt" },
            {
                "data": {
                    id: "id", lockout: "lockout"
                },
                "render": function (data) {

                    if (!data.lockout) {
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