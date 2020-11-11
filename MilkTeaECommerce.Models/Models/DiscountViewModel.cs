using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class DiscountViewModel
    {
        [DisplayName("Tên")]
        [Required(ErrorMessage = "Phải nhập tên cho Khuyến mãi")]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        [Required(ErrorMessage = "Phải nhập mô tả cho Khuyến mãi")]
        public string Description { get; set; }

        [DisplayName("Ngày bắt đầu")]
        [Required(ErrorMessage = "Phải chọn ngày bắt đầu")]
        [Compare(otherProperty: "validateDateStart",ErrorMessage ="Ngày không hợp lệ")]
        public DateTime DateStart { get; set; }

        public DateTime validateDateStart
        {
            get
            {
                if (DateStart < DateTime.Now)
                {
                    // không hợp lệ
                    return DateTime.Now;
                }
                return DateStart;
            }
        }
        [DisplayName("Ngày kết thúc")]
        [Required(ErrorMessage = "Phải chọn ngày kết thúc")]
        [Compare(otherProperty:"validateDateExpired", ErrorMessage ="Ngày không hợp lệ")]
        public DateTime DateExpired { get; set; }
        public DateTime validateDateExpired
        {
            get
            {
                if(DateExpired<DateStart)
                {
                    // không hợp lệ
                    return DateStart;
                }    
                return DateExpired;
            }
            set
            {
                this.validateDateExpired = value;
            }
            
        }

        [DisplayName("Số lượng khuyến mãi")]
        [Compare("validateTimesUseLimit",ErrorMessage ="Dữ liệu không hợp lệ")]
        public int TimesUseLimit { get; set; }
        public int validateTimesUseLimit
        {
            get
            {
                if (TimesUseLimit < 0)
                {
                    // không hợp lệ
                    return 0;
                }
                return TimesUseLimit;
            }
        }
        [DisplayName("Phần trăm giảm")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Dữ liệu không hợp lệ")]
        public int PercentDiscount { get; set; }
        [DisplayName("Giảm tối đa")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Dữ liệu không hợp lệ")]
        public float MaxDiscount { get; set; }
        [Required(ErrorMessage = "Phải nhập Code cho Khuyến mãi")]
        public string Code { get; set; }

        public virtual ICollection<CategoryDiscount> CategoryDiscount { get; set; }
        public virtual ICollection<SelectListItem> DeliveryDiscount { get; set; }
        public virtual ICollection<SelectListItem> ProductDiscount { get; set; }
        public virtual ICollection<Category> ShowCategoryDiscount { get; set; }

    }
}
