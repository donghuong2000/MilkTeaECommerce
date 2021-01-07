


$('#importfile').on('submit', function (e) {

    var form_data = new FormData();
    attachment_data = $("#importfile").find("input")[0].files[0];
    form_data.append("files", attachment_data);
    console.log(attachment_data)
    console.log(form_data)
    
    $.ajax({
        type: "POST",
        data: form_data,
        url: "/admin/home/importExcel",
        contentType: false,
        processData: false,
        headers: { "X-CSRF-Token": $("meta[name='csrf-token']").attr("content") },
        success: function (data) { 
            $('#alert-text').text(data.message);
        },
        error: function (data) {
        
        }
    });
    e.preventDefault();


})