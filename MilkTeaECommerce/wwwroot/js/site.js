// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




$('#SignInbtn').click(function () {
    var u = $('#usn').val();
    var p = $('#pwd').val();
    $.ajax({
        method:'POST',
        url: '/Identity/SignIn',
        data: { username: u, password: p },
        success: function (data) {
            $('#alert-message').html(data.message);
           
            console.log(data);
            if (data.success) {
                $('#alert-message').removeClass("alert-danger");
                $('#alert-message').addClass("alert-success");            }
            else {
                $('#alert-message').removeClass("alert-success");
                $('#alert-message').addClass("alert-danger");
            }
            $('#alert-message').removeAttr('hidden');
            setTimeout(window.location.reload(),1000);
        }

    })



})
$('#SignUpbtn').click(function () {
    



})


