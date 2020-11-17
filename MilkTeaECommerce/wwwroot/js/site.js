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
            setInterval(window.location.reload(),500);
        }

    })



})
$('#SignUpbtn').click(function () {
    var n = $('#name').val();
    var e = $('#email').val();
    var s = $('#sdt').val();
    var u = $('#username').val();
    var p = $('#password1').val();
    var cp = $('#password2').val();
    console.log('aaaaaaaaa');
    if (false) {
        console.log('aaasssaaaaaa');
        $('#alert-message').removeClass("alert-success");
        $('#alert-message').addClass("alert-danger");
        $('#alert-message').removeAttr('hidden');
        $('#alert-message').html('Comfirm password not success')
    }
    else {
        console.log('aaaa111aaaaa');
        $.ajax({
            method: 'POST',
            url: '/Identity/SignUp',
            data: { name: n, email: e, sdt: s, username: u, password: p },
            success: function (data) {
                $('#alert-message').html(data.message);

                console.log(data);
                if (data.success) {
                    $('#alert-message').removeClass("alert-danger");
                    $('#alert-message').addClass("alert-success");
                }
                else {
                    $('#alert-message').removeClass("alert-success");
                    $('#alert-message').addClass("alert-danger");
                }
                $('#alert-message').removeAttr('hidden');
                setInterval(window.location.reload(), 500);
            }

        })
    }





})


