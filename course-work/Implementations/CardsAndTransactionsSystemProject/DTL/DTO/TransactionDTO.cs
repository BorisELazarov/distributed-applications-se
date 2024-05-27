using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DTL.DTO
{
    [Serializable]
    public class TransactionDTO:BaseDTO
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [Length(22, 22)]
        public string IBAN { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Sum { get; set; }
        [Required]
        public DateOnly DateOfTransaction { get; set; }
        [Required]
        public int CardId { get; set; }
    }
}
