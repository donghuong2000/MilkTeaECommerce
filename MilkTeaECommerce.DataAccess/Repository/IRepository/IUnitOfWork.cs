using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        
        void Save();
        IDeliveryRepository Delivery { get; }
    }
}
