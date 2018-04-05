using System.Web;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Reports;

namespace Fly01.Estoque.Models.Reports
{
    public class ReportStock : IReportInfo
    {
        public static ReportStock Instance
        {
            get
            {
                return new ReportStock();
            }
        }

        public string DataTableName
        {
            get
            {
                return "Estoque";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Posição de Estoque";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportStock.rdlc"); ;
            }
        }
    }
}