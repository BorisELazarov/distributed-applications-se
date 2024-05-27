using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTL.DTO
{
    public class UserDTO:BaseDTO
    {
        [Required]
        [MaxLength(25)]
        public string Username { get; set; }
        [Required]
        [MaxLength(35)]
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
        public bool IsMale { get; set; }
    }
}
