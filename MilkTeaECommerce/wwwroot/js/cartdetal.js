$(document).ready(function () {
    loadCheck();
    
})
function loadCheck() {
    var items = paypalm.minicartk.cart.items();
    var total_bill = paypalm.minicartk.cart.total();
    var html = "Giỏ hàng đang trống";
    if (items.length > 0) {
        for (var i = 0; i < items.length; i++) {
            html +=
                `<tr class="">
                            <td class="invert">${i + 1}</td>
                            <td class="invert-image">
                                <a href="/Product/detail/${items[i].get('add')}">
                                    <img src="${items[i].get('shipping')}" alt=" " class="img-responsive">
                                </a>
                            </td>
                            <td class="invert">
                                <div class="quantity">
                                    <div class="quantity-select">
                                        <div onclick=Minus(${i}) class="entry value-minus">&nbsp;</div>
                                        <div class="entry value">
                                            <span>${items[i].get('quantity')}</span>
                                        </div>
                                        <div  onclick=Pluss(${i}) class="entry value-plus active">&nbsp;</div>
                                    </div>
                                </div>
                            </td>
                            <td class="invert">${items[i].get('item_name')}</td>
                            <td class="invert">${items[i]._total}</td>
                            <td class="invert">
                                <div class="rem">
                                    <div onclick=Remove(${i}) class="close1"> </div>
                                </div>
                            </td>
             </tr>
             `
           
        }
    }
    $('#tolal-bill').text('Tổng tiền: ' + total_bill + ' VND');
    $('#bodytable').html(html);
    
}


function Remove(i) {
    paypalm.minicartk.cart.remove(i);
    loadCheck();
}

function Minus(i) {
    var product = paypalm.minicartk.cart.items()[i];
    var currQuantity = product.get('quantity')
    product.set('quantity', currQuantity - 1);
    paypalm.minicartk.view.hide();
    loadCheck();
   
}
function Pluss(i) {
    var product = paypalm.minicartk.cart.items()[i];
    var currQuantity = product.get('quantity');
    product.set('quantity', currQuantity + 1);
    paypalm.minicartk.view.hide();
    loadCheck();
   
}
$('#submit-bill').click(function () {
    Swal.showLoading()
    var de = $('#delivery-method').val();
    var pa = $('#payment-method').val();
    var discode = $('#discoint').val();

    var product = [];
    var itemsCart = paypalm.minicartk.cart.items();
    for (var i = 0; i < itemsCart.length; i++) {
        product.push(itemsCart[i]._data);
    }
    // validate here


    $.ajax({
        method: 'post',
        url: '/CheckOut/Summary',
        data: { items: JSON.stringify(product), delivery: de, payment: pa, discountCode: discode },
        success: function (data) {
            
                window.location.href = data.url;
        }

    })


})

$('#btn-discount').click(function () {

    var dis = $('#discoint').val();
    if (dis != "") {
        $.ajax({
            url: '/checkout/CheckDiscount?discount=' + dis,
            success: function (data) {
                console.log(data)
                if (data.success) {
                    var html =
                        `
                        <h4><strong>Tên mã giảm giá :</strong>${data.obj.name} </h4>
                        <h4><strong>Mô tả :</strong>${data.obj.de}</h4>
                        <h4><strong>Phần trăm giảm :</strong>${data.obj.pt}</h4>
                        <h4><strong>Giảm tối đa :</strong>${data.obj.max}</h4>
                        <h4><strong>Số lần còn lại :</strong>${data.obj.time}</h4>
                        <h4><strong>Giảm giá cho danh mục :</strong>${data.obj.category}</h4>
                        <h4><strong>Giảm giá cho vận chuyển :</strong>${data.obj.delivery}</h4>
                        <h4><strong>Giảm giá cho sản phẩm :</strong>${data.obj.product}</h4>
                        <h4><strong>Ngày hết hạn :</strong>${data.obj.a}</h4>
                    `
                    $('#info-discount').html(html);
                }
                else {
                    $('#info-discount').html(data.message);
                }
               
            }
        })
    }

})
