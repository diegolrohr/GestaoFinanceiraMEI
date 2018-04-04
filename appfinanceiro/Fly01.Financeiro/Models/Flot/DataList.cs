using System.Collections.Generic;

namespace Fly01.Financeiro.Models.Flot
{
    public class DataList
    {
        public List<DataItem> Data { get; set; }
        public DataList()
        {
            Data = new List<DataItem>();
        }
    }
}