using Fly01.Core.Reports;
using Microsoft.Reporting.WebForms;
using System.Web;

namespace Fly01.Financeiro.Models.Reports
{
    public class ReportDre : IReportInfo
    {
        public static ReportDre Instance
        {
            get
            {
                return new ReportDre();
            }
        }

        public string DataTableName
        {
            get
            {
                return "MovimentacaoPorCategoria";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Relatório de DRE";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportDRE.rdlc");
            }
        }
    }
}