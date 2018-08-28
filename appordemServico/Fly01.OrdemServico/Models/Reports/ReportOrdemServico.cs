using Fly01.Core.Reports;
using Microsoft.Reporting.WebForms;
using System.Web;

namespace Fly01.OrdemServico.Models.Reports
{
    public class ReportOrdemServico : IReportInfo
    {
        public static ReportOrdemServico Instance
        {
            get
            {
                return new ReportOrdemServico();
            }
        }

        public string DataTableName
        {
            get
            {
                return "OrdemServico";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Ordem de Serviço";
            }
        }

        public ReportParameter[] Parameters
        {
            get
            {
                return null;
            }
        }

        public string ReportPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/Reports/ReportOrdemServico.rdlc");
            }
        }
    }
}