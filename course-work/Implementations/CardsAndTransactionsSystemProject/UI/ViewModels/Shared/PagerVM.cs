using System.ComponentModel;
using System.Text.Json.Serialization;

namespace UI.ViewModels.Shared
{
    public class PagerVM
    {
        [DefaultValue(1)]
        public int Page { get; set; }
        [DefaultValue(10)]
        public int ItemsPerPage { get; set; }
        [DefaultValue(1)]
        public int PagesCount { get; set; }
    }
}
