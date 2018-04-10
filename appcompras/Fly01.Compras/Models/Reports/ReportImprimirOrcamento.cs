using Fly01.Core.Reports;
using Microsoft.Reporting.WebForms;
using System.Web;

namespace Fly01.Compras.Models.Reports
{
    public class ReportImprimirOrcamento : IReportInfo
    {
        public static ReportImprimirOrcamento Instance
        {
            get
            {
                return new ReportImprimirOrcamento();
            }
        }

        public string DataTableName
        {
            get
            {
                return "ImprimirOrcamento";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Orçamento de Compra";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportImprimirOrcamento.rdlc");
            }
        }
    }
}