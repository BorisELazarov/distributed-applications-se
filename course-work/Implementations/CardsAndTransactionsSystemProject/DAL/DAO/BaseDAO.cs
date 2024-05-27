using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class BaseDAO<T> where T : BaseEntity
    {
        protected CardsAndTransactionsSystemDbContext _context
            = new CardsAndTransactionsSystemDbContext();

        protected DbSet<T> _items;

        public BaseDAO()
        { 
            _items=_context.Set<T>();
        }

        public void Delete(T entity)
        {
           _items.Remove(entity);
           _context.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return _items.AsEnumerable();
        }

        public T GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public void Insert(T entity)
        {
            _items.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _items.Update(entity);
            _context.SaveChanges();
        }
    }
}
