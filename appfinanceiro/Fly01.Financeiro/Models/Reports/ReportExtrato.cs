using System.Web;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Reports;

namespace Fly01.Financeiro.Models.Reports
{
    public class ReportExtrato : IReportInfo
    {
        public static ReportExtrato Instance
        {
            get
            {
                return new ReportExtrato();
            }
        }

        public string DataTableName
        {
            get
            {
                return "Extrato";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Extrato Bancário";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportExtrato.rdlc");
            }
        }
    }
}