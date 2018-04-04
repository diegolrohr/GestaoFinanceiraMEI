using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Fly01.Core.JQueryDataTable
{
    [NotMapped]
    public class JQueryDataTableParams
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public JQueryDataTableParamsColumnSearch Search { get; set; }

        public List<JQueryDataTableParamsOrder> Order { get; set; }

        public List<JQueryDataTableParamsColumn> Columns { get; set; }

        public static JQueryDataTableParams CreateFromQueryString(NameValueCollection queryStringNV)
        {
            JQueryDataTableParams jDataTable = new JQueryDataTableParams();

            if (queryStringNV.AllKeys.Contains("draw"))
                jDataTable.Draw = int.Parse(queryStringNV.Get("draw"));

            if (queryStringNV.AllKeys.Contains("start"))
                jDataTable.Start = int.Parse(queryStringNV.Get("start"));

            if (queryStringNV.AllKeys.Contains("length"))
                jDataTable.Length = int.Parse(queryStringNV.Get("length"));

            if(queryStringNV.AllKeys.Contains("columns[0][data]"))
            {
                jDataTable.Columns = new List<JQueryDataTableParamsColumn>();
                int i = 0;
                JQueryDataTableParamsColumn column;
                JQueryDataTableParamsColumnSearch columnSearch;
                for (; ; )
                {
                    column = new JQueryDataTableParamsColumn();
                    if (queryStringNV.AllKeys.Contains("columns[" + i + "][data]"))
                        column.Data = queryStringNV["columns[" + i + "][data]"];
                    else
                        break;
                    if (queryStringNV.AllKeys.Contains("columns[" + i + "][name]"))
                        column.Name = queryStringNV["columns[" + i + "][name]"];
                    if (queryStringNV.AllKeys.Contains("columns[" + i + "][searchable]"))
                        column.Searchable = bool.Parse(queryStringNV["columns[" + i + "][searchable]"]);
                    if (queryStringNV.AllKeys.Contains("columns[" + i + "][orderable]"))
                        column.Orderable = bool.Parse(queryStringNV["columns[" + i + "][orderable]"]);
                    if (queryStringNV.AllKeys.Contains("columns[" + i + "][search][value]"))
                    {
                        columnSearch = new JQueryDataTableParamsColumnSearch();
                        columnSearch.Value = queryStringNV["columns[" + i + "][search][value]"].ToString();
                        columnSearch.Regex = bool.Parse(queryStringNV["columns[" + i + "][search][regex]"]);
                        column.Search = columnSearch;
                    }
                    jDataTable.Columns.Add(column);
                    i++;
                }
            }
            if (queryStringNV.AllKeys.Contains("order[0][column]"))
            {
                jDataTable.Order = new List<JQueryDataTableParamsOrder>();
                int i = 0;
                JQueryDataTableParamsOrder order;
                for (; ; )
                {
                    order = new JQueryDataTableParamsOrder();
                    if (queryStringNV.AllKeys.Contains("order[" + i + "][column]"))
                        order.Column = int.Parse(queryStringNV["order[" + i + "][column]"]);
                    else
                        break;
                    if (queryStringNV.AllKeys.Contains("order[" + i + "][dir]"))
                        order.Dir = queryStringNV["order[" + i + "][dir]"];
                    jDataTable.Order.Add(order);
                    i++;
                }
            }

            if (queryStringNV.AllKeys.Contains("search[value]"))
            {
                jDataTable.Search = new JQueryDataTableParamsColumnSearch();
                jDataTable.Search.Value = queryStringNV["search[value]"];
                jDataTable.Search.Regex = bool.Parse(queryStringNV["search[regex]"]);
            }


            return jDataTable;
        }
    }
}