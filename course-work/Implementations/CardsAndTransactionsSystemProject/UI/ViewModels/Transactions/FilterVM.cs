using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Transactions
{
    public class FilterVM
    {
        public int CardId { get; set; }
        [MaxLength(20)]
        public string Title { get; set; }
        public DateOnly DateOfTransaction { get; set; }
        [Range(0,double.MaxValue)]
        public decimal Sum { get; set; }
        public string IBAN { get; set; }
    }
}