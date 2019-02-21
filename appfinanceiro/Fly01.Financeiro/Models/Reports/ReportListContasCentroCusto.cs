using System.Web;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Reports;

namespace Fly01.Financeiro.Models.Reports
{
    public class ReportListContasCentroCusto : IReportInfo
    {
        public static ReportListContasCentroCusto Instance
        {
            get
            {
                return new ReportListContasCentroCusto();
            }
        }

        public string DataTableName
        {
            get
            {
                return "ContasCentroCusto";
            }
        }

        public string DisplayName
        {
            get
            {
                return "";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportListContasCentroCusto.rdlc");
            }
        }
    }
}