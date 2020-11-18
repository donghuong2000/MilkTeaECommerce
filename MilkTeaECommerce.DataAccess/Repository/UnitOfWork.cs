using MilkTeaECommerce.Data;
using MilkTeaECommerce.DataAccess.Repository.IRepository;
using MilkTeaECommerce.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Delivery = new DeliveryRepository(_db);
        }

        public IDeliveryRepository Delivery { get; private set; }
       
        public void Dispose()
        {
            _db.Dispose();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
