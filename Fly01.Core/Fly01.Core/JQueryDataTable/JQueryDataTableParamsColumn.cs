namespace Fly01.Core.JQueryDataTable
{
    public class JQueryDataTableParamsColumn
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public JQueryDataTableParamsColumnSearch Search { get; set; }
    }
}