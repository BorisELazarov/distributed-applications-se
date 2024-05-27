using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Cards
{
    public class ChangeInfoVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [Range(0, double.MaxValue, ErrorMessage = "Please, select amount equal or more than 0!")]
        public decimal Balance { get; set; }
        [Length(22, 22)]
        [Required]
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
        public int UserId { get; set; }
    }
}
