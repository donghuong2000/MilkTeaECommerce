using MilkTeaECommerce.Data;
using MilkTeaECommerce.DataAccess.Repository.IRepository;
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
           
        }


       
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
