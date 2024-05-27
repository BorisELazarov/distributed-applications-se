using DAL.DAO;
using DAL.Entities;
using DTL.DTO;
using DTL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTL.Services
{
    public class TransactionsServices : BaseServices<TransactionDTO, Transaction>, IServices<TransactionDTO>
    {
        private CardsServices _cardsServices = new CardsServices();

        public IEnumerable<TransactionDTO> GetAll()
        {
            List<TransactionDTO> transactions = new List<TransactionDTO>();
            foreach (Transaction transaction in _dao.GetAll())
            {
                TransactionDTO dto = new TransactionDTO();
                dto.Id = transaction.Id;
                dto.Title = transaction.Title;
                dto.Description = transaction.Description;
                dto.IBAN = transaction.IBAN;
                dto.Sum = transaction.Sum;
                dto.DateOfTransaction = transaction.DateOfTransaction;
                dto.CardId = transaction.CardId;
                transactions.Add(dto);
            }
            return transactions.AsEnumerable();
        }

        public TransactionDTO GetById(int id)
        {
            Transaction transaction = new Transaction();
            TransactionDTO dto = new TransactionDTO();
            dto.Id = transaction.Id;
            dto.Title = transaction.Title;
            dto.Description = transaction.Description;
            dto.IBAN = transaction.IBAN;
            dto.Sum = transaction.Sum;
            dto.DateOfTransaction = transaction.DateOfTransaction;
            dto.CardId = transaction.CardId;
            return dto;
        }

        public void Insert(TransactionDTO dto)
        {
            Transaction transaction = new Transaction();
            transaction.Title = dto.Title;
            transaction.Description = dto.Description;
            transaction.IBAN = dto.IBAN;
            transaction.Sum = dto.Sum;
            transaction.DateOfTransaction = dto.DateOfTransaction;
            transaction.CardId = dto.CardId;
            _dao.Insert(transaction);
        }

        public void Update(TransactionDTO dto)
        {
            Transaction transaction = new Transaction();
            transaction.Id = dto.Id;
            transaction.Title = dto.Title;
            transaction.Description = dto.Description;
            transaction.IBAN = dto.IBAN;
            transaction.Sum = dto.Sum;
            transaction.DateOfTransaction = dto.DateOfTransaction;
            transaction.CardId = dto.CardId;
            _dao.Update(transaction);
        }
    }
}
