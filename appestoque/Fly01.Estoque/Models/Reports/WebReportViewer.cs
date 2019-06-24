using Fly01.Estoque.Reports;
using Fly01.Core.Config;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using Fly01.Core.Reports;

namespace Fly01.Estoque.Models.Reports
{
    public class WebReportViewer<T>
    {
        public WebReportViewer(IReportInfo iReportInfo)
        {
            ReportViewerHelper<T>.reportInfo = iReportInfo;
            GetDataTable();
        }

        private void GetDataTable()
        {
            var dataSetReports = new DataSetReports();

            var tableName = ReportViewerHelper<T>.reportInfo.DataTableName;

            if (!dataSetReports.Tables.Contains(tableName))
                throw new ArgumentException(string.Format("A tabela {0} não existe no DataSetReports.", tableName));

            var dataTable = dataSetReports.Tables[tableName];
            dataTable.Clear();

            ReportViewerHelper<T>.dataTable = dataTable;
        }

        public byte[] Print(List<T> data, string reportFilter = "", ReportParameter[] customParameters = null)
        {
            var report = ReportViewerHelper<T>.GetReport(data, SessionManager.Current.UserData.TokenData.UserName, reportFilter, SessionManager.Current.UserData.PlatformUrl, customParameters);
            return ReportViewerHelper<T>.PrepareReportToPrint(report);
        }

        public byte[] Print(T data, string reportFilter = "", ReportParameter[] customParameters = null)
        {
            var report = ReportViewerHelper<T>.GetReport(data, SessionManager.Current.UserData.TokenData.UserName, reportFilter, SessionManager.Current.UserData.PlatformUrl, customParameters);
            return ReportViewerHelper<T>.PrepareReportToPrint(report);
        }
    }
}