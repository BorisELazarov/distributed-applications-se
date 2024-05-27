using DTL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Services
{
    public interface IServices<T> where T : BaseDTO
    {
        public IEnumerable<T> GetAll();
        public T GetById(int id);
        public void Insert(T dto);
        public void Update(T dto);
        public void Delete(int id);
    }
}
