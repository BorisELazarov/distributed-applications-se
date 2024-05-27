using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Cards
{
    public class RenewVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        public DateOnly ValidThru { get; set; }
        public int UserId { get; set; }
    }
}
