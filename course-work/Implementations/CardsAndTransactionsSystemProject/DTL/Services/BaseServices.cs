using DAL.DAO;
using DAL.Entities;
using DTL.DTO;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Services
{
    public abstract class BaseServices<T, K> where T : BaseDTO where K : BaseEntity
    {
        protected BaseDAO<K> _dao =new BaseDAO<K>();
        public void Delete(int id)
        {
            _dao.Delete(_dao.GetById(id));
        }
    }
}
