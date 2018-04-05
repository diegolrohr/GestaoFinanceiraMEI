using System.Web;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Reports;

namespace Fly01.Estoque.Models.Reports
{
    public class ReportMovimentoDetalhado : IReportInfo
    {
        public static ReportMovimentoDetalhado Instance
        {
            get
            {
                return new ReportMovimentoDetalhado();
            }
        }

        public string DataTableName
        {
            get
            {
                return "MovimentoDetalhado";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Movimento de Estoque Detalhado";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportMovimentoDetalhado.rdlc"); ;
            }
        }
    }
}