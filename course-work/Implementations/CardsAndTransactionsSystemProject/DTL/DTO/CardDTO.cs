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
    public class CardDTO:BaseDTO
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [Length(22, 22)]
        public string IBAN { get; set; }
        [Required]
        [Length(16, 16)]
        public string CardNumber { get; set; }
        [Required]
        public DateOnly ValidThru { get; set; }
        [Required]
        [Length(3, 3)]
        public string SecurityCode { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
