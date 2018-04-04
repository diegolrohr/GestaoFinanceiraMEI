using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fly01.Financeiro.Models.Tasks;
using Fly01.Financeiro.Models.Tasks.Base;
using Fly01.Core.Config;

namespace Fly01.Financeiro.Controllers
{
    //Referencia: 
    //http://blog.janjonas.net/2012-01-02/asp_net-mvc_3-async-jquery-progress-indicator-long-running-tasks
    public class TaskExportController : Controller
    {
        private static List<TaskExportItemBase> tasks = new List<TaskExportItemBase>();

        public JsonResult Start(TaskType taskType, string queryStringFilter)
        {
            var taskId = Guid.NewGuid();
            TaskExportItemBase taskItem;
            string taskLabel = String.Empty;

            switch (taskType)
            {
                case TaskType.ExportacaoContaPagar:
                    taskItem = new TaskItemExportContaPagar(taskId);
                    taskLabel = "Exportação Contas Pagar";
                    break;
                case TaskType.ExportacaoContaReceber:
                    taskItem = new TaskItemExportContaReceber(taskId);
                    taskLabel = "Exportação Contas Receber";
                    break;
                case TaskType.ExportacaoExtrato:
                    taskItem = new TaskItemExportExtrato(taskId);
                    taskLabel = "Exportação Extrato";
                    break;
                case TaskType.ExportacaoDRE:
                    taskItem = new TaskItemExportDRE(taskId);
                    taskLabel = "Exportação DRE";
                    break;
                case TaskType.ExportacaoFluxoCaixa:
                    taskItem = new TaskItemExportFluxoCaixa(taskId);
                    taskLabel = "Exportação Fluxo Caixa";
                    break;
                default:
                    throw new ArgumentException("TaskType not Implemented");
            }

            tasks.Add(taskItem);
            string accessToken = SessionManager.Current.UserData.TokenData.AccessToken;
            Task.Factory.StartNew(() =>
            {
                taskItem.Process(queryStringFilter, accessToken);
            });

            return Json(new { id = taskId, label = taskLabel });
        }

        public ActionResult Progress(Guid id)
        {
            TaskExportItemBase taskItem = tasks.FirstOrDefault(x => x.Id == id);
            return Json(new { percent = taskItem != null ? taskItem.GetPercent() : 100 });
        }

        public FileResult DownloadCSV(Guid id)
        {
            TaskExportItemBase taskItem = tasks.FirstOrDefault(x => x.Id == id);
            byte[] fileContents = taskItem.Stream.ToArray();
            var fileName = String.Format("{0}.csv", id);
            tasks.Remove(taskItem);
            return File(fileContents, "text/csv", fileName);
        }
    }
}