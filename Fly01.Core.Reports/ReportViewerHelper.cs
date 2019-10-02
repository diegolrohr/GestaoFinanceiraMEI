using Fly01.Core.Rest;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;

namespace Fly01.Core.Reports
{
    public static class ReportViewerHelper<T>
    {
        #region Private Methods

        public static IReportInfo reportInfo { get; set; }

        public static DataTable dataTable { get; set; }

        private static ReportConfig GetReportConfig(string reportTitle, string userName, string platformUrl)
        {
            if (string.IsNullOrEmpty(platformUrl))
                throw new ArgumentException("GetReportConfig: platformUrl argument is required.");

            var headerDefault = new StringBuilder();

            return new ReportConfig
            {
                Header = headerDefault.ToString(),
                Footer = string.Format("<h6>{0} emitido dia {1:dd/MM/yyyy} por {2}</h6>", reportTitle, DateTime.Now, userName),
                LogoUrl = ""//empresaVM.ReportLogo != null ? empresaVM.ReportLogo.Replace("data:image/png;base64,", "") : "", 
            };
        }

        private static ReportViewer BuildReport(IReportInfo reportInfo,
                                                object reportData,
                                                string userName,
                                                string dataSourceName = "ds",
                                                string reportFilter = "",
                                                ReportParameter[] customParameters = null,
                                                string platformUrl = "")
        {
            var report = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                ZoomMode = ZoomMode.FullPage,
                ShowPrintButton = true,//somente IE que tem ActiveX, demais browser chamada manual
                ShowRefreshButton = false,
                AsyncRendering = false,
                PageCountMode = PageCountMode.Actual,
                Width = Unit.Percentage(100),
                Height = Unit.Percentage(100),
                KeepSessionAlive = false
            };

            report.LocalReport.EnableExternalImages = true;
            report.LocalReport.DataSources.Clear();
            report.LocalReport.DisplayName = reportInfo.DisplayName;
            report.LocalReport.ReportPath = reportInfo.ReportPath;
            report.LocalReport.DataSources.Add(new ReportDataSource(dataSourceName, reportData));

            var reportConfigs = GetReportConfig(report.LocalReport.DisplayName, userName, platformUrl);
            var rdlcParameters = report.LocalReport.GetParameters();

            ReportParameter[] allReportParameters;
            var currentPosition = 0;

            var defaultParameters = new[]
                {
                    new ReportParameter("ReportLogo", reportConfigs.LogoUrl),
                    new ReportParameter("ReportHeader", reportConfigs.Header),
                    new ReportParameter("ReportTitle", report.LocalReport.DisplayName),
                    new ReportParameter("ReportFooter", reportConfigs.Footer),
                    new ReportParameter("ReportFilter", reportFilter)
                };

            allReportParameters = new ReportParameter[(defaultParameters?.Length ?? 0) +
                                                      (reportInfo.Parameters?.Length ?? 0) +
                                                      (customParameters?.Length ?? 0)];

            Array.Copy(defaultParameters, allReportParameters, defaultParameters.Length);
            currentPosition = defaultParameters.Length;

            if (customParameters != null)
            {
                Array.Copy(customParameters, 0, allReportParameters, currentPosition, customParameters.Length);
                currentPosition = +customParameters.Length;
            }

            if (reportInfo.Parameters != null)
            {
                Array.Copy(reportInfo.Parameters, 0, allReportParameters, currentPosition, reportInfo.Parameters.Length);
                currentPosition = +reportInfo.Parameters.Length;
            }

            //report.LocalReport.SetParameters(allReportParameters);
            foreach (var item in allReportParameters)
            {
                //var p = defaultParameters.First(x => x.Name == item.Name);
                //de todos os parâmetros seta somente os que estão no report.
                bool existe = report.LocalReport.GetParameters().Any(x => x.Name == item.Name);
                if (existe) report.LocalReport.SetParameters(item);
            }

            report.LocalReport.Refresh();

            return report;
        }

        private static IEnumerable<ReportFieldValueVM> GetPropertInfos(object source, string parentName = null, string lastParentName = null)
        {
            if (source == null) yield break;

            var t = source.GetType();
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prp in props)
            {
                if (prp.PropertyType.Module.ScopeName != "CommonLanguageRuntimeLibrary")
                {
                    foreach (var info in GetPropertInfos(prp.GetValue(source), prp.Name, parentName))
                        yield return info;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(parentName))
                        yield return new ReportFieldValueVM { Field = prp.Name, Value = prp.GetValue(source) };
                    else
                    {
                        var prefix = parentName.Substring(parentName.Length - 2) == "VM" ? parentName.Substring(0, parentName.Length - 2) : parentName;
                        //resolve até segundo nível
                        //tinha classes com propiedades de nomes diferentes que utilizavam o mesmo complexType 
                        //ao invés de ComplexExample(sem distinção) fica: nomeComplexExample, nome2ComplexExample
                        if (!string.IsNullOrWhiteSpace(lastParentName))
                            prefix = string.Concat(lastParentName, prefix);
                        yield return new ReportFieldValueVM { Field = string.Concat(prefix, prp.Name), Value = prp.GetValue(source) };
                    }
                }
            }
        }

        private static void FillDataTable(T source, DataTable dataTable)
        {
            var reportFieldsValues = GetPropertInfos(source).ToList();

            var dataRow = dataTable.NewRow();
            foreach (var item in reportFieldsValues)
            {
                if (dataTable.Columns.Contains(item.Field))
                    dataRow[item.Field] = item.Value;
            }

            dataTable.Rows.Add(dataRow);
            dataTable.AcceptChanges();
        }

        private static void FillDataTable(IEnumerable<T> source, DataTable dataTable)
        {
            foreach (var item in source)
            {
                FillDataTable(item, dataTable);
            }
        }

        #endregion

        #region Public Methods

        public static ReportViewer GetReport(T data,
                                             string userName,
                                             string reportFilter,
                                             string platformUrl,
                                             ReportParameter[] customParameters = null)
        {
            FillDataTable(data, dataTable);

            return BuildReport(reportInfo,
                               dataTable,
                               userName,
                               reportFilter: reportFilter,
                               customParameters: customParameters,
                               platformUrl: platformUrl);
        }

        public static ReportViewer GetReport(List<T> data,
                                             string userName,
                                             string reportFilter,
                                             string platformUrl,
                                             ReportParameter[] customParameters = null)
        {
            FillDataTable(data, dataTable);

            return BuildReport(reportInfo,
                               dataTable,
                               userName,
                               reportFilter: reportFilter,
                               customParameters: customParameters,
                               platformUrl: platformUrl);
        }

        public static byte[] PrepareReportToPrint(ReportViewer report)
        {
            //definindo tipo que o relatório será renderizado
            const string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //configurações da página ex: margin, top, left (retirando margens)...
            const string deviceInfo = "<DeviceInfo>" +
                                      "<OutputFormat>PDF</OutputFormat>" +
                                      "<MarginLeft>0</MarginLeft>" +
                                      "<MarginTop>0</MarginTop>" +
                                      "<MarginRight>0</MarginRight>" +
                                      "<MarginBottom>0</MarginBottom>" +
                                      "<Orientation>Landscape</Orientation>" +
                                      "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;

            //Renderizando o relatório o bytes
            var reportBytes = report.LocalReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
            );

            return reportBytes;
        }

        public static void DisableUnwantedExportFormat(ReportViewer reportViewerId, params string[] strFormatName)
        {
            foreach (var extension in reportViewerId.LocalReport.ListRenderingExtensions())
            {
                if (!strFormatName.Contains(extension.Name)) continue;

                var info = extension.GetType()
                    .GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                if (info != null) info.SetValue(extension, false);
            }
        }
        #endregion
    }

}