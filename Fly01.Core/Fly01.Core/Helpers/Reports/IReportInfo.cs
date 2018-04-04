using Microsoft.Reporting.WebForms;

namespace Fly01.Core.Helpers.Reports
{
    public interface IReportInfo
    {
        string DisplayName { get; }
        string ReportPath { get; }
        string DataTableName { get; }
        ReportParameter[] Parameters { get; }
    }
}