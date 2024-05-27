using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.ViewModels.Cards
{
    public class InputVM
    {
        public string Title { get; set; }
        public string IBAN { get; set; }
        public string CardNumber { get; set; }
        public DateOnly ValidThru { get; set; }
        public string SecurityCode { get; set; }
        public decimal Balance { get; set; }
        public int UserId { get; set; }
    }
}
