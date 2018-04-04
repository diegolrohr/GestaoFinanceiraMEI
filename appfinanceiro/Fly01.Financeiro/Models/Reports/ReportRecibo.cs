using System.Web;
using Fly01.Core.Helpers.Reports;
using Microsoft.Reporting.WebForms;

namespace Fly01.Financeiro.Models.Reports
{
    public class ReportRecibo : IReportInfo
    {
        public static ReportRecibo Instance
        {
            get
            {
                return new ReportRecibo();
            }
        }

        public string DataTableName
        {
            get
            {
                return "ReciboContaFinanceira";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Recibo de Pagamento";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportRecibo.rdlc");
            }
        }
    }
}