using System.Web;
using Fly01.Core.Helpers.Reports;
using Microsoft.Reporting.WebForms;

namespace Fly01.Financeiro.Models.Reports
{
    public class ReportFluxo : IReportInfo
    {
        public static ReportFluxo Instance
        {
            get
            {
                return new ReportFluxo();
            }
        }

        public string DataTableName
        {
            get
            {
                return "CashFlow";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Fluxo de Caixa";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportFluxo.rdlc");
            }
        }
    }
}