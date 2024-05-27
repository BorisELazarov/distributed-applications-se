
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Repositories
{
    public interface IRepository<T>
    {
        public List<T> GetAll(Expression<Func<T, bool>> filter = null,
            Expression<Func<T, object>> orderBy = null,
            int page = 1,
            int itemsPerPage = Int32.MaxValue);
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null);
        public int Count(Expression<Func<T, bool>> filter = null);
        public void Save(T item);
        public void Delete(int id);
    }
}
