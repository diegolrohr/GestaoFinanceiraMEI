using System.Collections.Generic;
using System.Reflection;

namespace Fly01.Financeiro.Models.ViewModel
{
    public class ImportCsvVM
    {
        public string Entity { get; set; }
        public string PageTitle { get; set; }
        public object DefaultFields { get; set; }
        public IEnumerable<PropertyInfo> Columns { get; set; }
    }
}