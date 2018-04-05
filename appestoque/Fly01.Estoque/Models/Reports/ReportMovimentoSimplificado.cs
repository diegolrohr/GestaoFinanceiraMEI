using System.Web;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Reports;

namespace Fly01.Estoque.Models.Reports
{
    public class ReportMovimentoSimplificado : IReportInfo
    {
        public static ReportMovimentoSimplificado Instance
        {
            get
            {
                return new ReportMovimentoSimplificado();
            }
        }

        public string DataTableName
        {
            get
            {
                return "MovimentoSimplificado";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Movimento de Estoque Simplificado";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportMovimentoSimplificado.rdlc"); ;
            }
        }
    }
}