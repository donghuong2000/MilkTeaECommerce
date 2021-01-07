

$(document).ready(function () {
    $('#dataTable').DataTable({
        "ajax": {
            "url":'/admin/shipper/getall'
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
                    
                    if (data.lockout==1){
                        return `                               
<div class="dropdown">
  <button class="btn btn btn-info dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    Pending
  </button>
  <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
    <a class="dropdown-item" style="cursor:pointer" onclick=ChangeShipper("${data.id}",3)>Block</a>
    <a class="dropdown-item" style="cursor:pointer" onclick=ChangeShipper("${data.id}",2)>Approved</a>
  </div>
</div>
                        `
                    }
                    if (data.lockout == 2){
                        return `
                             <div class="dropdown">
  <button class="btn btn btn-success dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    Approved
  </button>
  <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
    <a class="dropdown-item" style="cursor:pointer" onclick=ChangeShipper("${data.id}",3)>Block</a>
    <a class="dropdown-item" style="cursor:pointer" onclick=ChangeShipper("${data.id}",2)>Approved</a>
  </div>
</div>  
                            `
                    }
                    if (data.lockout == 3) {
                        return `
                             <div class="dropdown">
  <button class="btn btn btn-danger dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    Block
  </button>
  <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
     <a class="dropdown-item" style="cursor:pointer" onclick=ChangeShipper("${data.id}",3)>Block</a>
    <a class="dropdown-item"style="cursor:pointer" onclick=ChangeShipper("${data.id}",2)>Approved</a>
  </div>
</div>    
                            `
                    }
                }

            },

        ]

    });
}); 


function ChangeShipper(id, status) {
    $.ajax({
        method: 'POST',
        url:'/Admin/Shipper/changeStatus',
        data: { id: id, status: status },
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


