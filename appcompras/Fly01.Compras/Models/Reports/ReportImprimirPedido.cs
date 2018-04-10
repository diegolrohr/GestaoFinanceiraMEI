using Fly01.Core.Reports;
using Microsoft.Reporting.WebForms;
using System.Web;

namespace Fly01.Compras.Models.Reports
{
    public class ReportImprimirPedido : IReportInfo
    {
        public static ReportImprimirPedido Instance
        {
            get
            {
                return new ReportImprimirPedido();
            }
        }

        public string DataTableName
        {
            get
            {
                return "ImprimirPedido";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Pedido de Compra";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportImprimirPedido.rdlc");
            }
        }
    }
}