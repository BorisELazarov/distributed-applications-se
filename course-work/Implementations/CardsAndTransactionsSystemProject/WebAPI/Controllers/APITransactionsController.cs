using DTL.DTO;
using DTL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using NuGet.Common;
using NuGet.ProjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class APITransactionsController : ControllerBase
    {
        private readonly TransactionsRepository _transactionsRepo = new TransactionsRepository();
        private readonly CardsRepository _cardsRepo = new CardsRepository();
        // GET: api/<ApiTransactionsController>
        [HttpGet("{cardId}, {dateOfTransaction}, {sum}, {order}, {page}, {itemsPerPage}")]
        public ActionResult<List<TransactionDTO>> GetTransactions(int cardId,
            string dateOfTransaction, decimal sum,
            int order, int page, int itemsPerPage)
        {
            Expression<Func<TransactionDTO, bool>> filter = Filter(sum,
            DateOnly.Parse(dateOfTransaction), cardId);
            Expression<Func<TransactionDTO, object>> orderBy = OrderBy(order);
            List<TransactionDTO> transactions = _transactionsRepo.GetAll(filter,orderBy,page,itemsPerPage);
            return Ok(transactions);
        }

        private Expression<Func<TransactionDTO, bool>> Filter(decimal sum, DateOnly dateOfTransaction,int cardId)
        {
            Expression<Func<TransactionDTO, bool>> filter = t =>
            (dateOfTransaction == DateOnly.MinValue || t.DateOfTransaction == dateOfTransaction) &&
            (sum == 0 || t.Sum == sum / 100) && t.CardId == cardId;
            return filter;
        }

        private Expression<Func<TransactionDTO, object>> OrderBy(int orderKey)
        {
            Expression<Func<TransactionDTO, object>> orderBy = t => t.Id;
            switch (orderKey)
            {
                case 1:
                    orderBy = c => c.Title;
                    break;
                case 2:
                    orderBy = c => c.DateOfTransaction;
                    break;
                case 3:
                    orderBy = c => c.Sum;
                    break;
                default:
                    break;
            }
            return orderBy;
        }

        // GET api/<ApiTransactionsController>/5
        [HttpGet("{id}")]
        public ActionResult<TransactionDTO> GetTransaction(int id)
        {
            TransactionDTO transaction = _transactionsRepo.GetFirstOrDefault(x => x.Id == id);
            if (transaction == null)
            {
                return NotFound("Incorrect id");
            }
            return Ok(transaction);
        }

        // POST api/<ApiTransactionsController>
        [HttpPost]
        public ActionResult<TransactionDTO> PostTransaction(TransactionDTO transaction)
        {
            CardDTO sender = _cardsRepo.GetFirstOrDefault(c => c.Id == transaction.CardId);
            if (sender == null)
            {
                return NotFound("Card not existing");
            }
            CardDTO receiver = _cardsRepo.GetFirstOrDefault(t => t.IBAN == transaction.IBAN); 
            if (receiver==null)
            {
                return BadRequest("There is no such IBAN");
            }
            //if (sender.UserId != GlobalVars.loggedUser.Id)
            //{
            //    return Unauthorized("Unauthorized operation using another's user card");
            //}
            if (transaction.Sum > sender.Balance)
            {
                return BadRequest("The sum is more than the card's balance");
            }
            sender.Balance -= transaction.Sum;
            _cardsRepo.Save(sender);
            receiver.Balance += transaction.Sum;
            _cardsRepo.Save(receiver);
            _transactionsRepo.Save(transaction);

            return Created();
        }

        // PUT api/<ApiTransactionsController>/5
        [HttpPut]
        public ActionResult<TransactionDTO> PutTransaction(TransactionDTO transaction)
        {
            try
            {
                _transactionsRepo.Save(transaction);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (transaction == null)
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

        // DELETE api/<ApiTransactionsController>/5
        [HttpDelete("{id}")]
        public ActionResult DeleteTransaction(int id)
        {
            TransactionDTO transaction = _transactionsRepo.GetFirstOrDefault(t => t.Id == id);
            if (transaction==null)
                return NotFound();
            CardDTO card= _cardsRepo.GetFirstOrDefault(card=>card.Id==id);
            //if (card.UserId == GlobalVars.loggedUser.Id)
            //{
            //    return Unauthorized("Unauthorized operation using another's user card");
            //}
            _transactionsRepo.Delete(transaction.Id);
            return Ok();
        }
    }
}
