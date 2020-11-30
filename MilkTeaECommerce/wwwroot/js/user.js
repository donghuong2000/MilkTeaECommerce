function Confirm(mail) {
    Swal.showLoading()
    $.ajax({
        type: 'GET',
        url: '/Identity/MailConfirm?email=' + mail,
        success: function (data) {
            Swal.close();
            Swal.fire(
                'Confirm your mail',
                data.message,
                'info'
            )
        }
    })
}
