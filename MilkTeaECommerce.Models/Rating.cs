using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class Rating
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ApplicationUserId { get; set; }
        public string Content { get; set; }
        public float? Rate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Product Product { get; set; }
    }
}
