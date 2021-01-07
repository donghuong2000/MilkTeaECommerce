using MilkTeaECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.DataAccess.Repository.IRepository
{
    public interface IDeliveryRepository: IRepository<Delivery>
    {
        void Update(Delivery delivery);
    }
}
