using System;
using System.Collections.Generic;
using Fly01.Financeiro.Reports;
using Fly01.Core.Config;
using Microsoft.Reporting.WebForms;
using Fly01.Core.Reports;
using Fly01.Financeiro.Entities.ViewModel;

namespace Fly01.Financeiro.Models.Reports
{
    public class WebReportViewer<T>
    {
        public WebReportViewer(IReportInfo iReportInfo)
        {
            ReportViewerHelper<T, EmpresaVM, EstadoVM, CidadeVM>.reportInfo = iReportInfo;
            GetDataTable();
        }

        private void GetDataTable()
        {
            var dataSetReports = new DataSetReports();

            var tableName = ReportViewerHelper<T, EmpresaVM, EstadoVM, CidadeVM>.reportInfo.DataTableName;

            if (!dataSetReports.Tables.Contains(tableName))
                throw new ArgumentException(string.Format("A tabela {0} não existe no DataSetReports.", tableName));

            var dataTable = dataSetReports.Tables[tableName];
            dataTable.Clear();

            ReportViewerHelper<T, EmpresaVM, EstadoVM, CidadeVM>.dataTable = dataTable;
        }

        public byte[] Print(List<T> data,
                            string platformUrl,
                            string reportFilter = "",
                            ReportParameter[] customParameters = null)
        {
            
            var report = ReportViewerHelper<T, EmpresaVM, EstadoVM, CidadeVM>
                            .GetReport(data,
                                       SessionManager.Current.UserData.TokenData.Username,
                                       reportFilter,
                                       platformUrl,
                                       customParameters);

            return ReportViewerHelper<T, EmpresaVM, EstadoVM, CidadeVM>.PrepareReportToPrint(report);
        }

        public byte[] Print(T data,
                            string platformUrl,
                            string reportFilter = "",
                            ReportParameter[] customParameters = null)
        {
            var report = ReportViewerHelper<T, EmpresaVM, EstadoVM, CidadeVM>
                            .GetReport(data,
                                       SessionManager.Current.UserData.TokenData.Username,
                                       reportFilter,
                                       platformUrl,
                                       customParameters);

            return ReportViewerHelper<T, EmpresaVM, EstadoVM, CidadeVM>.PrepareReportToPrint(report);
        }
    }
}