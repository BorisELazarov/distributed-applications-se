using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Cards
{
    public class CreateVM
    {
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        public DateOnly ValidThru { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [Range(0, double.MaxValue, ErrorMessage = "Please, select amount equal or more than 0!")]
        public decimal Balance { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
