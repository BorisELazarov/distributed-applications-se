using System.ComponentModel.DataAnnotations;
using UI.Enums.Cards;

namespace UI.ViewModels.Cards
{
    public class SortVM
    {
        public int Key { get; set; }
        [Required]
        public Sort Value {  get; set; }
    }
}