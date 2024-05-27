using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Card:BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [Length(22,22)]
        public string IBAN { get; set; }
        [Required]
        [Length(16, 16)]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly ValidThru { get; set; }
        [Required]
        [Length(3, 3)]
        [DataType(DataType.CreditCard)]
        public string SecurityCode { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        //public ICollection<Transaction> Transactions { get; set; }

    }
}
