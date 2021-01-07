using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.Models
{
    public enum ShipperRequest
    {
        None, // không phải
        Pending,// đang  duyệt
        Approved,// đã duyệt
        Block //chặn
    }
}
