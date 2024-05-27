using DAL.Entities;
using DTL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Services
{
    public class CardsServices : BaseServices<CardDTO, Card>, IServices<CardDTO>
    {
        private UsersServices _usersServices=new UsersServices();
        public IEnumerable<CardDTO> GetAll()
        {
            List<CardDTO> cards = new List<CardDTO>();
            foreach (Card card in _dao.GetAll())
            {
                CardDTO dto = new CardDTO();
                dto.Id = card.Id;
                dto.Title = card.Title;
                dto.IBAN = card.IBAN;
                dto.CardNumber = card.CardNumber;
                dto.ValidThru = card.ValidThru;
                dto.SecurityCode = card.SecurityCode;
                dto.Balance = card.Balance;
                dto.UserId = card.UserId;
                cards.Add(dto);
            }
            return cards.AsQueryable();
        }

        public CardDTO GetById(int id)
        {
            Card card = _dao.GetById(id);
            CardDTO dto = new CardDTO();
            dto.Id = card.Id;
            dto.Title = card.Title;
            dto.IBAN = card.IBAN;
            dto.CardNumber = card.CardNumber;
            dto.ValidThru = card.ValidThru;
            dto.SecurityCode = card.SecurityCode;
            dto.Balance = card.Balance;
            dto.UserId = card.UserId;
            return dto;
        }
        public void Update(CardDTO dto)
        {
            Card card = new Card();
            card.Id = dto.Id;
            card.Title = dto.Title;
            card.IBAN = dto.IBAN;
            card.CardNumber = dto.CardNumber;
            card.ValidThru = dto.ValidThru;
            card.SecurityCode = dto.SecurityCode;
            card.Balance = dto.Balance;
            card.UserId = dto.UserId;
            _dao.Update(card);
        }

        public void Insert(CardDTO dto)
        {
            Card card=new Card();
            card.Id = dto.Id;
            card.Title = dto.Title;
            card.IBAN = dto.IBAN;
            card.CardNumber = dto.CardNumber;
            card.ValidThru=dto.ValidThru;
            card.SecurityCode = dto.SecurityCode;
            card.Balance = dto.Balance;
            card.UserId = dto.UserId;
            _dao.Insert(card);
        }
    }
}
