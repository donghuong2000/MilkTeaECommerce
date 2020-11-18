using MilkTeaECommerce.DataAccess.Repository.IRepository;
using MilkTeaECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MilkTeaECommerce.Data;
using System.Linq;

namespace MilkTeaECommerce.DataAccess.Repository
{
    public class DeliveryRepository:Repository<Delivery>,IDeliveryRepository
    {
        private readonly ApplicationDbContext _db;
        public DeliveryRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(Delivery delivery)
        {
            var obj = _db.Deliveries.FirstOrDefault(x=>x.Id == delivery.Id);
            if(obj != null)
            {
                obj.Name = delivery.Name;
            }

        }
    }
}
