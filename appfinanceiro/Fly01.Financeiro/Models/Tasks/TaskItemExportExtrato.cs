using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fly01.Financeiro.Entities.ViewModel;
using Fly01.Financeiro.Models.Tasks.Base;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Models.Tasks
{
    public class TaskItemExportExtrato : TaskExportItemBase
    {
        public TaskItemExportExtrato(Guid id)
            : base(id) { }

        public override void Process(string queryStringFilter, string accessToken)
        {
            Dictionary<string, string> queryStringRequest = ProcessQueryString(queryStringFilter);

            int page = Convert.ToInt32(queryStringRequest.FirstOrDefault(x => x.Key == "page").Value);
            string resource = AppDefaults.GetResourceName(typeof(BankStatementVM));

            List<BankStatementVM> recordsToExport = new List<BankStatementVM>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {
                
                queryStringRequest.AddParam("page", page.ToString());
                ResultBase<BankStatementVM> response = RestHelper.ExecuteGetRequestWithoutSession<ResultBase<BankStatementVM>>(resource, queryStringRequest, accessToken);
                recordsToExport.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;

                TotalRecords = response.Total;
                TotalRecordsProcess = recordsToExport.Count();
            }

            DataTable dataTableExtrato = new DataTable();
            dataTableExtrato.Columns.Add(new DataColumn("Data"));
            dataTableExtrato.Columns.Add(new DataColumn("Lançamento"));
            dataTableExtrato.Columns.Add(new DataColumn("Valor"));
            dataTableExtrato.Columns.Add(new DataColumn("Saldo"));

            if (recordsToExport.Count == 0)
            {
                TotalRecords = -1;
                TotalRecordsProcess = -1;
            }

            foreach (var item in recordsToExport)
	        {
                DataRow dr = dataTableExtrato.NewRow();
                dr["Data"] = item.Date.HasValue ? ((DateTime)item.Date).ToString("dd/MM/yyyy") : string.Empty;

                if (item.OperationType == "B")
                    dr["Lançamento"] = "Saldo Final";
                else
                    dr["Lançamento"] = !string.IsNullOrWhiteSpace(item.History) ? item.History : item.CategoryDescription;

                dr["Valor"] = item.Value.ToString("C", AppDefaults.CultureInfoDefault);
                dr["Saldo"] = item.Balance.ToString("C", AppDefaults.CultureInfoDefault);
                
                dataTableExtrato.Rows.Add(dr);
	        }

            Stream = CSVUtility.GetCSV(dataTableExtrato);
        }
    }
}