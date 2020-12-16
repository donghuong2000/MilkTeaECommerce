using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.Models
{
    public enum OrderDetailStatus
    {
        unconfirm,
        confirmed,
        admit,
        delivery,
        deliveried,
        cancelled,
        evaluated,
    }
}
