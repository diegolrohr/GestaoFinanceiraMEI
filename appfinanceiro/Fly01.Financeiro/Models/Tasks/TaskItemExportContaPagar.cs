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
    public class TaskItemExportContaPagar : TaskExportItemBase
    {
        public TaskItemExportContaPagar(Guid id)
            : base(id) { }

        public override void Process(string queryStringFilter, string accessToken)
        {
            Dictionary<string, string> queryStringRequest = ProcessQueryString(queryStringFilter);

            int page = Convert.ToInt32(queryStringRequest.FirstOrDefault(x => x.Key == "page").Value);
            string resource = AppDefaults.GetResourceName(typeof(ContaPagarVM));

            List<ContaPagarVM> recordsToExport = new List<ContaPagarVM>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {
                queryStringRequest.AddParam("page", page.ToString());
                ResultBase<ContaPagarVM> response = RestHelper.ExecuteGetRequestWithoutSession<ResultBase<ContaPagarVM>>(resource, queryStringRequest, accessToken);
                recordsToExport.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;

                TotalRecords = response.Total;
                TotalRecordsProcess = recordsToExport.Count();
            }

            DataTable dataTableContaPagar = new DataTable();
            dataTableContaPagar.Columns.Add(new DataColumn("Status"));
            dataTableContaPagar.Columns.Add(new DataColumn("Número"));
            dataTableContaPagar.Columns.Add(new DataColumn("Emissão"));
            dataTableContaPagar.Columns.Add(new DataColumn("Vencimento"));
            dataTableContaPagar.Columns.Add(new DataColumn("Descrição"));
            dataTableContaPagar.Columns.Add(new DataColumn("Valor"));
            dataTableContaPagar.Columns.Add(new DataColumn("Forma Pagamento"));
            dataTableContaPagar.Columns.Add(new DataColumn("Parcelas"));
            dataTableContaPagar.Columns.Add(new DataColumn("Categoria"));
            dataTableContaPagar.Columns.Add(new DataColumn("Recebedor"));

            if (recordsToExport.Count == 0)
            {
                TotalRecords = -1;
                TotalRecordsProcess = -1;
            }

            var valorTotal = 0.0;

            foreach (var item in recordsToExport)
            {
                DataRow dr = dataTableContaPagar.NewRow();
                dr["Status"] = EnumHelper.SubtitleDataAnotation("StatusContaBancaria", item.StatusContaBancaria).Value;
                dr["Número"] = item.Id.ToString();
                dr["Emissão"] = item.DataEmissao.ToString("dd/MM/yyyy");
                dr["Vencimento"] = item.DataVencimento.ToString("dd/MM/yyyy");
                dr["Descrição"] = item.Descricao;
                dr["Valor"] = item.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault);
                dr["Forma Pagamento"] = item.FormaPagamento;
                dr["Parcelas"] = item.DescricaoParcela.Replace("/", " de ");
                dr["Categoria"] = item.Categoria.Descricao;
                dr["Recebedor"] = item.Pessoa;

                dataTableContaPagar.Rows.Add(dr);
                valorTotal += item.ValorPrevisto;
            }

            DataRow drTotal = dataTableContaPagar.NewRow();
            drTotal["Status"] = "";
            drTotal["Número"] = "";
            drTotal["Emissão"] = "";
            drTotal["Vencimento"] = "";
            drTotal["Descrição"] = "TOTAL";
            drTotal["Valor"] = valorTotal.ToString("C", AppDefaults.CultureInfoDefault);
            drTotal["Forma Pagamento"] = "";
            drTotal["Parcelas"] = "";
            drTotal["Categoria"] = "";
            drTotal["Recebedor"] = "";

            dataTableContaPagar.Rows.Add(drTotal);

            Stream = CSVUtility.GetCSV(dataTableContaPagar);
        }
    }
}