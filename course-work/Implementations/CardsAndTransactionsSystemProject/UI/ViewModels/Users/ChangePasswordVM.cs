using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Users
{
    public class ChangePasswordVM
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [DisplayName("Old password: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(35)]
        public string OldPassword { get; set; }
        [DisplayName("New password: ")]
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(35)]
        public string NewPassword { get; set; }
    }
}
