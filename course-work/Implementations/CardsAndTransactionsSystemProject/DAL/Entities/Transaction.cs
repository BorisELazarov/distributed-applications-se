﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Transaction: BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [Length(22,22)]
        public string IBAN { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Sum { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfTransaction { get; set; }
        [Required]
        public int CardId { get; set; }
        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
    }
}
