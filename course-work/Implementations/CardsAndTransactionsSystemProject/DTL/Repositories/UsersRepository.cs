using DAL.Entities;
using DTL.DTO;
using DTL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Repositories
{
    public class UsersRepository : BaseRepository<UserDTO>, IRepository<UserDTO>
    {
        private UsersServices _services=new UsersServices();

        public List<UserDTO> GetAll(Expression<Func<UserDTO, bool>> filter = null, Expression<Func<UserDTO, object>> orderBy = null, int page = 1, int itemsPerPage = int.MaxValue)
        {
            return GetAllQuery(filter, orderBy, page, itemsPerPage, _services.GetAll().AsQueryable());
        }

        public UserDTO GetFirstOrDefault(Expression<Func<UserDTO, bool>> filter = null)
        {
            return GetFirstOrDefaultQuery(filter, _services.GetAll().AsQueryable());
        }

        public int Count(Expression<Func<UserDTO, bool>> filter = null)
        {
            return GetAll(filter).Count();
        }

        public void Save(UserDTO item)
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
