using DTL.DTO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Transactions
{
    public class DetailsVM
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [Range(22, 22)]
        public string IBAN { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Sum { get; set; }
        [Required]
        public DateOnly DateOfTransaction { get; set; }
        [Required]
        public int CardId { get; set; }
    }
}