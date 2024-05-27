using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UI.ViewModels.Users
{
    public class FilterVM
    {

        [DisplayName("Username: ")]
        public string Username { get; set; }
        [DisplayName("First name: ")]
        public string FirstName { get; set; }
        [DisplayName("Last name: ")]
        public string LastName { get; set; }
        public int Age { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfRegistration { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool IsMale { get; set; }
    }
}
