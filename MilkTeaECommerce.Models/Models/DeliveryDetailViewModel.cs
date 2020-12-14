using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class DeliveryDetailViewModel
    {
        [Display(Name ="Mã đơn hàng")]
        public string OrderDetailId { get; set; }
        
        public string DeliveryId { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [Display(Name = "Phí ship")]
        public float? Price { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        public DateTime? DateStart { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public DateTime? DateEnd { get; set; }

    }
}
