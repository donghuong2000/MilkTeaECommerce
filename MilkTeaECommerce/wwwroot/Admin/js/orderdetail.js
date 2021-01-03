$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
        },
        "columns": [
            { "data": "id" },
            {
                "data": { image: "image", product: "product" },
                "render": function (data) {
                    return `<img src=${data.image} class="rounded" height = 100 width=70 />
                            <label>${data.product}</label>
                            `
                }
            },
            { "data": "quantity" },
            { "data": "price" },
            { "data": "status" },
            { "data": "delivery" },
           
        ]

    })
}
)
