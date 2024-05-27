using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewModels.Users
{
    public class LoginVM
    {
        [DisplayName("Username: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(25)]
        public string Username { get; set; }
        [DisplayName("Password: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(35)]
        public string Password { get; set; }
    }
}
