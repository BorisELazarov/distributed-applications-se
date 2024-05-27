using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using UI.Enums;

namespace UI.ViewModels.Users
{
    public class CreateVM
    {
        [DisplayName("Username: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(25)]
        public string Username { get; set; }
        [DisplayName("Password: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(35)]
        public string Password { get; set; }
        [DisplayName("FirstName: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [DisplayName("LastName: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(20)]
        public string LastName { get; set; }
        [DisplayName("BirthDate: ")]
        [Required(ErrorMessage = "*This field is required!")]
        public DateOnly BirthDate { get; set; }
        [DisplayName("Gender: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [DefaultValue(1)]
        public Gender Gender { get; set; }
        public bool IsMale { get; set; }
    }
}
