using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fly01.Financeiro.Entities.ViewModel;
using Fly01.Financeiro.Models.Tasks.Base;
using Fly01.Financeiro.Models.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Models.Tasks
{
    public class TaskItemExportFluxoCaixa : TaskExportItemBase
    {
        public TaskItemExportFluxoCaixa(Guid id)
            : base(id) { }

        //Mesmo método do DashBoardController.GetLabel
        private string GetLabel(DashboardFluxoCaixaType dashboardFluxoCaixaType, string paymentDate, string paymentMonth)
        {
            string result = String.Empty;
            string monthName = String.Empty;
            //"paymentDate": "20160608"
            //"paymentMonth": "201606"            

            switch (dashboardFluxoCaixaType)
            {
                case DashboardFluxoCaixaType.Mensal:
                    monthName = AppDefaults.CultureInfoDefault.DateTimeFormat.GetAbbreviatedMonthName(String.Concat(paymentMonth, "01").ToDateTime().Month);
                    result = String.Concat(char.ToUpper(monthName[0]), monthName.Substring(1));
                    break;
                case DashboardFluxoCaixaType.PreDefinido:
                    var dt = paymentDate.ToDateTime();
                    monthName = AppDefaults.CultureInfoDefault.DateTimeFormat.GetAbbreviatedMonthName(dt.Month);
                    monthName = String.Concat(char.ToUpper(monthName[0]), monthName.Substring(1));

                    result = String.Format("{0}/{1}", dt.Day, monthName);
                    break;
                default:
                    throw new ArgumentException("DashboardFluxoCaixaType inválido");
            }

            return result;
        }

        public override void Process(string queryStringFilter, string accessToken)
        {
            Dictionary<string, string> queryStringRequest = ProcessQueryString(queryStringFilter);

            string groupValue = queryStringRequest.FirstOrDefault(x => x.Key == "groupBy").Value;
            var fluxoCaixaType = DashboardFluxoCaixaType.Mensal;

            switch (groupValue)
            {
                case "paymentMonth":
                    fluxoCaixaType = DashboardFluxoCaixaType.Mensal;
                    break;
                case "paymentDate":
                    fluxoCaixaType = DashboardFluxoCaixaType.PreDefinido;
                    break;
            }

            int page = Convert.ToInt32(queryStringRequest.FirstOrDefault(x => x.Key == "page").Value);
            string resource = AppDefaults.GetResourceName(typeof(CashFlowVM));

            List<CashFlowVM> recordsToExport = new List<CashFlowVM>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {
                
                queryStringRequest.AddParam("page", page.ToString());
                ResultBase<CashFlowVM> response = RestHelper.ExecuteGetRequestWithoutSession<ResultBase<CashFlowVM>>(resource, queryStringRequest, accessToken);
                recordsToExport.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;

                TotalRecords = response.Total;
                TotalRecordsProcess = recordsToExport.Count();
            }

            DataTable dataTableFluxoCaixa = new DataTable();
            dataTableFluxoCaixa.Columns.Add(new DataColumn("Data"));
            dataTableFluxoCaixa.Columns.Add(new DataColumn("Recebimento"));
            dataTableFluxoCaixa.Columns.Add(new DataColumn("Pagamento"));
            dataTableFluxoCaixa.Columns.Add(new DataColumn("Saldo Final"));

            if (recordsToExport.Count == 0)
            {
                TotalRecords = -1;
                TotalRecordsProcess = -1;
            }

            foreach (var item in recordsToExport)
	        {
                DataRow dr = dataTableFluxoCaixa.NewRow();
                dr["Data"] = GetLabel(fluxoCaixaType, item.PaymentDate, item.PaymentMonth);
                dr["Recebimento"] = item.Receipts.ToString("C", AppDefaults.CultureInfoDefault);
                dr["Pagamento"] = (item.Payments * -1).ToString("C", AppDefaults.CultureInfoDefault);
                dr["Saldo Final"] = item.Balance.ToString("C", AppDefaults.CultureInfoDefault);

                dataTableFluxoCaixa.Rows.Add(dr);
	        }

            Stream = CSVUtility.GetCSV(dataTableFluxoCaixa);
        }
    }
}