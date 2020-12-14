$(document).ready(function () {
    loadCheck();
    
})
function loadCheck() {
    var items = paypalm.minicartk.cart.items();
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
