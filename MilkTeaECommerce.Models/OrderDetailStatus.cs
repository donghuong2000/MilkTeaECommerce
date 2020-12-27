using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.Models
{
    public enum OrderDetailStatus
    {
        unconfirm, // chưa xác nhận 
        confirmed, //xác nhận (hiển thị cho shipper ở phần nhận đơn)
        received , // hiển thị cho shipper ở phần đã nhận đơn
        delivery, // đang vận chuyển ( hiển thị cho shipper ở phần đã lấy hàng) 
        deliveried, // hoàn thành
        cancelled, //đã hủy
        evaluated,
    }
}
