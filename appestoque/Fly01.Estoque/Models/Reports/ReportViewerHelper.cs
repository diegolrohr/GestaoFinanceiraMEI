using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
using Fly01.Estoque.Entities.ViewModel;
using Fly01.Estoque.Reports;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json.Linq;
using Fly01.Estoque.Models.Utils;

namespace Fly01.Estoque.Models.Reports
{
    public static class ReportViewerHelper<T>
    {
        #region Private Methods

        private static ReportConfig GetReportConfig(string reportTitle, string userName)
        {
            var resourceById = string.Format("{0}/{1}", AppDefaults.GetResourceName(typeof(BranchVM)), "01");
            var branchVm = RestHelper.ExecuteGetRequest<BranchVM>(resourceById);

            string logoBase64;
            try
            {
                logoBase64 = RestHelper.ExecuteGetRequest<JObject>("logo").Value<string>("fileContent");
            }
            catch (Exception)
            {
                logoBase64 = "";
            }

            var headerDefault = new StringBuilder();
            headerDefault.AppendFormat("{0} | CNPJ: {1}", branchVm.Name, branchVm.CNPJ);
            headerDefault.Append("<br/>");
            headerDefault.AppendFormat("Endereço: {0}", branchVm.Address);
            headerDefault.Append("<br/>");
            headerDefault.AppendFormat("Bairro: {0} | CEP: {1} | Cidade: {2}", branchVm.Neighborhood, branchVm.ZipCode, branchVm.City);
            headerDefault.Append("<br/>");
            headerDefault.AppendFormat("Email: {0} ", branchVm.Email);

            return new ReportConfig
            {
                Header = headerDefault.ToString(),
                Footer = string.Format("<h6>{0} emitido dia {1:dd/MM/yyyy} por {2}</h6>", reportTitle, DateTime.Now, userName),
                LogoUrl = logoBase64,
            };
        }

        private static ReportViewer BuildReport(IReportInfo reportInfo,
                                                object reportData,
                                                string userName,
                                                string dataSourceName = "ds",
                                                string reportFilter = "",
                                                ReportParameter[] parameters = null)
        {
            var report = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                ZoomMode = ZoomMode.FullPage,
                ShowPrintButton = true,
                ShowRefreshButton = false,
                AsyncRendering = false,
                PageCountMode = PageCountMode.Actual,
                Width = Unit.Percentage(100),
                Height = Unit.Percentage(100),
                KeepSessionAlive = false,

            };

            report.LocalReport.EnableExternalImages = true;
            report.LocalReport.DataSources.Clear();
            report.LocalReport.DisplayName = reportInfo.DisplayName;
            report.LocalReport.ReportPath = reportInfo.ReportPath;
            report.LocalReport.DataSources.Add(new ReportDataSource(dataSourceName, reportData));

            var reportConfigs = GetReportConfig(report.LocalReport.DisplayName, userName);
            var rdlcParameters = report.LocalReport.GetParameters();

            if (reportInfo.Parameters == null)
            {
                var defaultParameters = new[]
                            {
                            new ReportParameter("ReportLogo", reportConfigs.LogoUrl),
                            new ReportParameter("ReportHeader", reportConfigs.Header),
                            new ReportParameter("ReportTitle", report.LocalReport.DisplayName),
                            new ReportParameter("ReportFooter", reportConfigs.Footer),
                            new ReportParameter("ReportFilter", reportFilter)
                            };

                foreach (var item in rdlcParameters)
                {
                    var p = defaultParameters.First(x => x.Name == item.Name);
                    if (p != null) report.LocalReport.SetParameters(p);
                }
            }
            else
            {
                foreach (var item in rdlcParameters)
                {
                    var p = reportInfo.Parameters.First(x => x.Name == item.Name);
                    if (p != null) report.LocalReport.SetParameters(p);
                }
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

        private static DataTable GetDataTable(IReportInfo reportInfo)
        {
            var dataSetReports = new DataSetReports();

            var tableName = reportInfo.DataTableName;

            if (!dataSetReports.Tables.Contains(tableName))
                throw new ArgumentException(string.Format("A tabela {0} não existe no DataSetReports.", tableName));

            var dataTable = dataSetReports.Tables[tableName];
            dataTable.Clear();

            return dataTable;
        }

        #endregion

        #region Public Methods

        public static ReportViewer GetReport(IReportInfo reportInfo, T data, string userName, string reportFilter)
        {
            var dataTable = GetDataTable(reportInfo);
            FillDataTable(data, dataTable);

            return BuildReport(reportInfo, dataTable, userName, reportFilter: reportFilter);
        }

        public static ReportViewer GetReport(IReportInfo reportInfo, List<T> data, string userName, string reportFilter)
        {
            var dataTable = GetDataTable(reportInfo);
            FillDataTable(data, dataTable);

            return BuildReport(reportInfo, dataTable, userName, reportFilter: reportFilter);
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
                                      "<MarginTop>0</MarginTop>" +
                                      "<MarginLeft>0</MarginLeft>" +
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