using Microsoft.Reporting.WebForms;

namespace Fly01.Financeiro.Models.Reports
{
    public interface IReportInfo
    {
        string DisplayName { get; }
        string ReportPath { get; }
        string DataTableName { get; }
        ReportParameter[] Parameters { get; }
    }
}