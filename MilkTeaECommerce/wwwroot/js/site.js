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
            $('#alert-message').removeAttr('hidden');
            if (data.success) {

                $('#alert-message').removeClass("alert-danger");
                $('#alert-message').addClass("alert-success");
                if (data.message == "Admin") {
                    window.location.href = data.url;
                }
                else
                    setInterval(window.location.reload(), 500);}
            else {
                $('#alert-message').removeClass("alert-success");
                $('#alert-message').addClass("alert-danger");
            }
            
            
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
    console.log(p+" "+cp)
    if (p!=cp || p =="") {
        
        $('#alert-message').removeClass("alert-success");
        $('#alert-message').addClass("alert-danger");
        $('#alert-message').removeAttr('hidden');
        $('#alert-message').html('Comfirm password not success')
    }
    else {
        Swal.fire({
           
            allowOutsideClick: false,
            onBeforeOpen: () => {
                Swal.showLoading()
            },
        });
        $.ajax({
            method: 'POST',
            url: '/Identity/SignUp',
            data: { name: n, email: e, sdt: s, username: u, password: p },
            success: function (data) {
                Swal.close();
                if (data.success) {
                    Swal.fire(
                        'Create Account Success',
                        'Please confirm your email to get more feature',
                        'success'
                    ).then((result) => {
                        setInterval(window.location.reload(), 500);
                    })
                }
                else {
                    Swal.fire(
                        'Create Account False',
                        data.message,
                        'error'
                    )
                }
            }

        })
    }

})
$('.myModal').on('shown.bs.modal', function () {
    $('#alert-message').attr('hidden', 'true');
})


