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
    public class CardsRepository:BaseRepository<CardDTO>, IRepository<CardDTO>
    {
        private CardsServices _services = new CardsServices();

        public List<CardDTO> GetAll(Expression<Func<CardDTO, bool>> filter = null, Expression<Func<CardDTO, object>> orderBy = null, int page = 1, int itemsPerPage = int.MaxValue)
        {
            return GetAllQuery(filter, orderBy, page, itemsPerPage, _services.GetAll().AsQueryable());
        }

        public CardDTO GetFirstOrDefault(Expression<Func<CardDTO, bool>> filter = null)
        {
            return GetFirstOrDefaultQuery(filter, _services.GetAll().AsQueryable());
        }

        public int Count(Expression<Func<CardDTO, bool>> filter = null)
        {
            return GetAll(filter).Count();
        }

        public void Save(CardDTO item)
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
