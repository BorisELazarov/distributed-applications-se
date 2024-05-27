using DAL.Context;
using DAL.DAO;
using DAL.Entities;
using DTL.DTO;
using DTL.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Repositories
{
    public class BaseRepository<T> where T : BaseDTO
    {
        protected List<T> GetAllQuery(Expression<Func<T, bool>> filter = null,
            Expression<Func<T, object>> orderBy = null,
            int page = 1,
            int itemsPerPage = Int32.MaxValue,
            IQueryable<T> query = null)
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return query
                .Skip(itemsPerPage * (page - 1))
                .Take(itemsPerPage)
                .ToList();
        }
        protected T GetFirstOrDefaultQuery(Expression<Func<T, bool>> filter = null, IQueryable<T> query=null)
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.FirstOrDefault();
        }
    }
}
