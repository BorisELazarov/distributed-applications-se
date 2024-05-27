using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User:BaseEntity
    {
        [Required]
        [MaxLength(25)]
        public string Username { get; set; }
        [Required]
        [MaxLength(35)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
        //[Required]
        //[Range(0,150)]
        //public int Age { get; set; }
        //[Required]
        //[DataType(DataType.Date)]
        //public DateOnly DateOfRegistration { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsMale { get; set; }
        //public virtual ICollection<Card> Cards { get; set; }
    }
}
