using DTL.DTO;
using DTL.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using NuGet.Protocol;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class APICardsController : ControllerBase
    {
        private readonly CardsRepository _cardsRepo = new CardsRepository();
        private readonly UsersRepository _usersRepo = new UsersRepository();

        // GET: api/<ApiCardsController>
        [HttpGet("{userId}, {balance}, {order}, {page}, {itemsPerPage}")]
        public ActionResult<List<CardDTO>> GetCards(int userId, decimal balance,
            int order, int page, int itemsPerPage)
        {
            if (_usersRepo.GetFirstOrDefault(x => x.Id == userId) == null)
            {
                return NotFound();
            }
            Expression<Func<CardDTO, bool>> filter = Filter(balance, userId);
            Expression<Func<CardDTO, object>> orderBy = OrderBy(order);
            List<CardDTO> cards = _cardsRepo.GetAll(filter, orderBy, page, itemsPerPage);

            return Ok(cards);
        }

        private Expression<Func<CardDTO, bool>> Filter(decimal balance, int userId)
        {
            Expression<Func<CardDTO, bool>> filter = c =>
            (balance == 0 || c.Balance == balance/100) && c.UserId == userId;
            return filter;
        }

        private Expression<Func<CardDTO, object>> OrderBy(int orderKey)
        {
            Expression<Func<CardDTO, object>> orderBy = c => c.Id;
            switch (orderKey)
            {
                case 1:
                    orderBy = c => c.Title;
                    break;
                case 2:
                    orderBy = c => c.Balance;
                    break;
                default:
                    break;
            }
            return orderBy;
        }

        // GET api/<ApiCardsController>/5
        [HttpGet("{id}")]
        public ActionResult<CardDTO> GetCard(int id)
        {
            CardDTO card = _cardsRepo.GetFirstOrDefault(x => x.Id == id);
            if (card==null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        // POST api/<ApiCardsController>
        [HttpPost]
        public ActionResult<CardDTO> PostCard(CardDTO card)
        {

            if (_usersRepo.GetFirstOrDefault(u => u.Id == card.UserId) == null)
            {
                return NotFound("User not existing");
            }
            Random rnd = new Random();
            do
            {
                card.IBAN = "";
                for (int i = 0; i < 2; i++)
                {
                    card.IBAN += Convert.ToChar(rnd.Next(1, 26) + 65);
                }
                for (int i = 0; i < 2; i++)
                {
                    card.IBAN += rnd.Next(0, 9).ToString();
                }
                for (int i = 0; i < 4; i++)
                {
                    card.IBAN += Convert.ToChar(rnd.Next(1, 26) + 65);
                }
                for (int i = 0; i < 14; i++)
                {
                    card.IBAN += rnd.Next(0, 9).ToString();
                }
            } while (_cardsRepo.GetFirstOrDefault(c => c.IBAN == card.IBAN) != null);
            do
            {
                card.CardNumber = "";
                for (int i = 0; i < 16; i++)
                {
                    card.CardNumber += rnd.Next(0, 9).ToString();
                }
            } while (_cardsRepo.GetFirstOrDefault(c => c.CardNumber == card.CardNumber) != null);
            card.SecurityCode = "";
            for (int i = 0; i < 3; i++)
            {
                card.SecurityCode += rnd.Next(0, 9).ToString();
            }
            _cardsRepo.Save(card);
            card = _cardsRepo.GetFirstOrDefault(x => x.IBAN == card.IBAN);
            return Ok(card);
        }

        [HttpPut]
        public ActionResult ChangeInfo(CardDTO card)
        {
            try
            {
                _cardsRepo.Save(card);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (card==null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpGet("{id}, {validThru}")]
        public ActionResult<CardDTO> Renew(int id, string validThru)
        {
            Random rnd = new Random();
            CardDTO card = _cardsRepo.GetFirstOrDefault(u => u.Id == id);
            card.ValidThru = DateOnly.Parse(validThru);
            do
            {
                card.CardNumber = "";
                for (int i = 0; i < 16; i++)
                {
                    card.CardNumber += rnd.Next(0, 9).ToString();
                }
            } while (_cardsRepo.GetFirstOrDefault(c => c.CardNumber == card.CardNumber) != null);
            card.SecurityCode = "";
            for (int i = 0; i < 3; i++)
            {
                card.SecurityCode += rnd.Next(0, 9).ToString();
            }

            try
            {
                _cardsRepo.Save(card);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (card==null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(card);
        }

        

        // DELETE api/<ApiCardsController>/5
        [HttpDelete("{id}")]
        public ActionResult<int> DeleteCard(int id)
        {
            CardDTO card = _cardsRepo.GetFirstOrDefault(x => x.Id == id);
            if (card == null)
                return NotFound();
            _cardsRepo.Delete(id);
            return Ok(card.UserId);
        }
    }
}
