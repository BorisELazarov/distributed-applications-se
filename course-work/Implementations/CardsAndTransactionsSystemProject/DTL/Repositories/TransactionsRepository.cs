using DAL.Entities;
using DTL.DTO;
using DTL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Repositories
{
    public class TransactionsRepository : BaseRepository<TransactionDTO>, IRepository<TransactionDTO>
    {
        private TransactionsServices _services = new TransactionsServices();

        public List<TransactionDTO> GetAll(Expression<Func<TransactionDTO, bool>> filter = null, Expression<Func<TransactionDTO, object>> orderBy = null, int page = 1, int itemsPerPage = int.MaxValue)
        {
            return GetAllQuery(filter, orderBy, page, itemsPerPage, _services.GetAll().AsQueryable());
        }

        public TransactionDTO GetFirstOrDefault(Expression<Func<TransactionDTO, bool>> filter = null)
        {
            return GetFirstOrDefaultQuery(filter, _services.GetAll().AsQueryable());
        }

        public int Count(Expression<Func<TransactionDTO, bool>> filter = null)
        {
            return GetAll(filter).Count();
        }

        public void Save(TransactionDTO item)
        {
            if (item.Id > 0)
            {
                _services.Update(item);
            }
            else
            {
                _services.Insert(item);
            }
        }
        public void Delete(int id)
        {
            _services.Delete(id);
        }
    }
}
