using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace UI.ViewModels.Transactions
{
    public class CreateVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [StringLength(22)]
        public string IBAN { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [Range(1, double.MaxValue, ErrorMessage = "Please, select at least 1 dollar!")]
        public decimal Sum { get; set; }
        public DateOnly DateOfTransaction { get; set; }
        public int CardId { get; set; }
    }
}
