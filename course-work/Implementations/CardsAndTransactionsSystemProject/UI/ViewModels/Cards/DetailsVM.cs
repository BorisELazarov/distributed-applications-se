using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Cards
{
    public class DetailsVM
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [Length(22, 22)]
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
        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
