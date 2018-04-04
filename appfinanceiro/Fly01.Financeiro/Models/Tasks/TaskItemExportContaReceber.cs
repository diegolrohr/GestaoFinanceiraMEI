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
    public class TaskItemExportContaReceber : TaskExportItemBase
    {
        public TaskItemExportContaReceber(Guid id)
            : base(id) { }

        public override void Process(string queryStringFilter, string accessToken)
        {
            Dictionary<string, string> queryStringRequest = ProcessQueryString(queryStringFilter);

            int page = Convert.ToInt32(queryStringRequest.FirstOrDefault(x => x.Key == "page").Value);
            string resource = AppDefaults.GetResourceName(typeof(ContaReceberVM));

            List<ContaReceberVM> recordsToExport = new List<ContaReceberVM>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {                  
                queryStringRequest.AddParam("page", page.ToString());
                ResultBase<ContaReceberVM> response = RestHelper.ExecuteGetRequestWithoutSession<ResultBase<ContaReceberVM>>(resource, queryStringRequest, accessToken);
                recordsToExport.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;

                TotalRecords = response.Total;
                TotalRecordsProcess = recordsToExport.Count();
            }           

            DataTable dataTableContaReceber = new DataTable();
            dataTableContaReceber.Columns.Add(new DataColumn("Status"));
            dataTableContaReceber.Columns.Add(new DataColumn("Número"));
            dataTableContaReceber.Columns.Add(new DataColumn("Emissão"));
            dataTableContaReceber.Columns.Add(new DataColumn("Vencimento"));
            dataTableContaReceber.Columns.Add(new DataColumn("Descrição"));
            dataTableContaReceber.Columns.Add(new DataColumn("Valor"));
            dataTableContaReceber.Columns.Add(new DataColumn("Forma Pagamento"));
            dataTableContaReceber.Columns.Add(new DataColumn("Parcelas"));
            dataTableContaReceber.Columns.Add(new DataColumn("Categoria"));
            dataTableContaReceber.Columns.Add(new DataColumn("Pagador"));

            if (recordsToExport.Count == 0)
            {
                TotalRecords = -1;
                TotalRecordsProcess = -1;
            }

            var valorTotal = 0.0;

            foreach (var item in recordsToExport)
            {   
                DataRow dr = dataTableContaReceber.NewRow();
                dr["Status"] = EnumHelper.SubtitleDataAnotation("StatusContaBancaria", item.StatusContaBancaria).Value;
                dr["Número"] = item.Id.ToString() ;
                dr["Emissão"] = item.DataEmissao.ToString("dd/MM/yyyy");
                dr["Vencimento"] = item.DataVencimento.ToString("dd/MM/yyyy");
                dr["Descrição"] = item.Descricao;
                dr["Valor"] = item.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault);
                dr["Forma Pagamento"] = item.FormaPagamento;
                dr["Parcelas"] = item.DescricaoParcela.Replace("/", " de ");
                dr["Categoria"] = item.Categoria.Descricao;
                dr["Pagador"] = item.Pessoa;

                dataTableContaReceber.Rows.Add(dr);
                valorTotal += item.ValorPrevisto;
            }

            DataRow drTotal = dataTableContaReceber.NewRow();
            drTotal["Status"] = "";
            drTotal["Número"] = "";
            drTotal["Emissão"] = "";
            drTotal["Vencimento"] = "";
            drTotal["Descrição"] = "TOTAL";
            drTotal["Valor"] = valorTotal.ToString("C", AppDefaults.CultureInfoDefault);
            drTotal["Forma Pagamento"] = "";
            drTotal["Parcelas"] = "";
            drTotal["Categoria"] = "";
            drTotal["Pagador"] = "";

            dataTableContaReceber.Rows.Add(drTotal);

            Stream = CSVUtility.GetCSV(dataTableContaReceber);
        }
    }
}