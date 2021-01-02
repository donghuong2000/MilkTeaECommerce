$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
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

