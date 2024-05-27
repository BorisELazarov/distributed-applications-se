using System.ComponentModel.DataAnnotations;

namespace UI.ViewModels.Cards
{
    public class FilterVM
    {
        public int UserId { get; set; }
        [MaxLength(20)]
        public string Title { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; }
    }
}
