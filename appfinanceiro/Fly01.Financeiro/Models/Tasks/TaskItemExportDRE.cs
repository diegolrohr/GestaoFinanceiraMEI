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
    public class TaskItemExportDRE : TaskExportItemBase
    {
        public TaskItemExportDRE(Guid id)
            : base(id) { }

        public override void Process(string queryStringFilter, string accessToken)
        {
            Dictionary<string, string> queryStringRequest = ProcessQueryString(queryStringFilter);

            int page = Convert.ToInt32(queryStringRequest.FirstOrDefault(x => x.Key == "page").Value);
            string resource = AppDefaults.GetResourceName(typeof(DemonstrativoResultadoExercicioVM));

            List<DemonstrativoResultadoExercicioVM> recordsToExport = new List<DemonstrativoResultadoExercicioVM>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {

                queryStringRequest.AddParam("page", page.ToString());
                ResultBase<DemonstrativoResultadoExercicioVM> response = RestHelper.ExecuteGetRequestWithoutSession<ResultBase<DemonstrativoResultadoExercicioVM>>(resource, queryStringRequest, accessToken);
                recordsToExport.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;

                TotalRecords = response.Total;
                TotalRecordsProcess = recordsToExport.Count();
            }

            DataTable dataTableDRE = new DataTable();
            dataTableDRE.Columns.Add(new DataColumn("Grupo"));
            dataTableDRE.Columns.Add(new DataColumn("Descrição"));
            dataTableDRE.Columns.Add(new DataColumn("Previsto"));
            dataTableDRE.Columns.Add(new DataColumn("Pago/Recebido"));
            dataTableDRE.Columns.Add(new DataColumn("Todos"));

            if (recordsToExport.Count == 0)
            {
                TotalRecords = -1;
                TotalRecordsProcess = -1;
            }

            //int idGrupo = 0;
            //foreach (var item in recordsToExport)
            //{
            //    idGrupo++;

            //    DataRow dr = dataTableDRE.NewRow();

            //    string itemDescricao = String.Empty;

            //    if (item.Type == "1")
            //        itemDescricao = String.Concat("(+) ", item.Name);
            //    else if (item.Type == "2")
            //        itemDescricao = String.Concat("(-) ", item.Name);
            //    else if (item.Type == "3")
            //        itemDescricao = String.Concat("(=) ", item.Name);

            //    dr["Grupo"] = idGrupo.ToString();
            //    dr["Descrição"] = itemDescricao;

            //    dr["Previsto"] = item.AmountPending.ToString("C", AppDefaults.CultureInfoDefault);
            //    dr["Pago/Recebido"] = item.AmountRealized.ToString("C", AppDefaults.CultureInfoDefault);
            //    dr["Todos"] = item.TotalAmount.ToString("C", AppDefaults.CultureInfoDefault);
                
            //    dataTableDRE.Rows.Add(dr);

            //    if (item.Type == "1" || item.Type == "2")
            //    {
            //        foreach (var detail in item.Itens)
            //        {
            //            DataRow drDetail = dataTableDRE.NewRow();
            //            drDetail["Grupo"] = idGrupo.ToString();
            //            drDetail["Descrição"] = detail.CategoryDescription;
            //            drDetail["Previsto"] = detail.AmountPending.ToString("C", AppDefaults.CultureInfoDefault);
            //            drDetail["Pago/Recebido"] = detail.AmountRealized.ToString("C", AppDefaults.CultureInfoDefault);
            //            drDetail["Todos"] = detail.TotalAmount.ToString("C", AppDefaults.CultureInfoDefault);

            //            dataTableDRE.Rows.Add(drDetail);
            //        }
            //    }                
            //}

            Stream = CSVUtility.GetCSV(dataTableDRE);
        }
    }
}