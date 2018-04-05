using System.Web;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Reports;

namespace Fly01.Financeiro.Models.Reports
{
    public class ReportListContas : IReportInfo
    {
        public static ReportListContas Instance
        {
            get
            {
                return new ReportListContas();
            }
        }

        public string DataTableName
        {
            get
            {
                return "ListContas";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Lista de Contas a Pagar";
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