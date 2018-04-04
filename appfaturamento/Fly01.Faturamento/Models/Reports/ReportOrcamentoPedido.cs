using Fly01.Core.Helpers.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace Fly01.Faturamento.Models.Reports
{
    public class ReportOrcamentoPedido : IReportInfo
    {
        public static ReportOrcamentoPedido Instance
        {
            get
            {
                return new ReportOrcamentoPedido();
            }
        }

        public string DataTableName
        {
            get
            {
                return "OrcamentoPedido";
            }
        }

        public string DisplayName
        {
            get
            {
                return "Orçamento / Pedido";
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
                return HttpContext.Current.Server.MapPath("~/Reports/ReportOrcamentoPedido.rdlc");
            }
        }
    }
}