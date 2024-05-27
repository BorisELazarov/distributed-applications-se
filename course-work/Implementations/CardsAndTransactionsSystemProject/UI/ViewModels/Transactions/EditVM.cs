using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Transactions
{
    public class EditVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required(ErrorMessage = "*This field is required!")]
        [MaxLength(200)]
        public string Description { get; set; }
        public string IBAN { get; set; }
        public string Sum {  get; set; }
        public DateOnly DateOfTransaction { get; set; }
        public int CardId { get; set; }
    }
}
