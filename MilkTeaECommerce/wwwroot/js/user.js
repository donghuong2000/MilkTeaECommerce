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

function ShopChannel () {
    Swal.showLoading()
    $.ajax({
        type: 'GET',
        url: '/User/ShopChannel',
        success: function (data) {
            console.log(data)
            Swal.close();
            if (data.status) {
                if (data.message == 'OK') {
                    console.log(data.url);
                    window.location.href = data.url;
                }
                Swal.fire(
                    'info',
                    data.message,
                    'info')
            }
            else {
                Swal.fire(
                        'info',
                        data.message,
                        'info')
            }
            
        }
    })
}
