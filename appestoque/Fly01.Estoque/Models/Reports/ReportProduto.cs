using System.Web;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Reports;

namespace Fly01.Estoque.Models.Reports
{
    public class ReportProduto : IReportInfo
    {
        public static ReportProduto Instance
        {
            get
            {
                return new ReportProduto();
            }
        }

        public string DataTableName
        {
            get
            {
                return "Produto";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Produtos";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportProduto.rdlc"); ;
            }
        }
    }
}