using System.Web;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Helpers.Reports;

namespace Fly01.Estoque.Models.Reports
{
    public class ReportListaContagem : IReportInfo
    {
        public static ReportListaContagem Instance
        {
            get
            {
                return new ReportListaContagem();
            }
        }

        public string DataTableName
        {
            get
            {
                return "InventarioListaContagem";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Inventário - Lista de Contagem";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportListaContagem.rdlc"); ;
            }
        }
    }
}