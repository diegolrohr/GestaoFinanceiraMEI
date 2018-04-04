using System.Collections.Generic;

namespace Fly01.Financeiro.Models.ViewModel
{
    public class SearchBox
    {
        public string PageTitle { get; set; }
        public SearchBoxMenuItem PrimaryMenuItem { get; set; }
        public List<SearchBoxMenuItem> Menu { get; set; }
    }
}