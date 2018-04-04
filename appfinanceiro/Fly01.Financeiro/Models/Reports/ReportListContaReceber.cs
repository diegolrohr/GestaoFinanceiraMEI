using System.Web;
using Fly01.Core.Helpers.Reports;
using Microsoft.Reporting.WebForms;

namespace Fly01.Financeiro.Models.Reports
{
    public class ReportListContaReceber : IReportInfo
    {
        public static ReportListContaReceber Instance
        {
            get
            {
                return new ReportListContaReceber();
            }
        }

        public string DataTableName
        {
            get
            {
                return "ListContaPagarReceber";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Lista de Contas a Receber";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportListContas.rdlc");
            }
        }
    }
}