using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Financeiro.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class FluxoCaixaController : PrimitiveBaseController
    {
        public JsonResult LoadSaldos(string dataFinal)
        {
            try
            {
                if (string.IsNullOrEmpty(dataFinal))
                    dataFinal = DateTime.Now.ToString("yyyy-MM-dd");

                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataFinal", dataFinal },
                };

                var response = RestHelper.ExecuteGetRequest<ResponseFluxoCaixaSaldoVM>("fluxocaixa/saldos", queryString);
               
                var responseToView = new
                {
                    TotalPagamentos = response.Value.TotalPagamentos.ToString("C", AppDefaults.CultureInfoDefault),
                    TotalRecebimentos = response.Value.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault),
                    SaldoAtual = response.Value.SaldoAtual.ToString("C", AppDefaults.CultureInfoDefault),
                    SaldoProjetado = response.Value.SaldoProjetado.ToString("C", AppDefaults.CultureInfoDefault)
                };

                return Json(responseToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        private List<FluxoCaixaProjecaoVM> GetProjecao(DateTime dataInicial, DateTime dataFinal, int groupType)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                { "dataFinal", dataFinal.ToString("yyyy-MM-dd") },
                { "groupType", groupType.ToString() }
            };
            var response = RestHelper.ExecuteGetRequest<ResponseFluxoCaixaProjecaoVM>("fluxocaixa/projecao", queryString);
            if (response == null)
                return new List<FluxoCaixaProjecaoVM>();

            return response.Values;
        }

        private PagedResult<FluxoCaixaProjecaoVM> GetProjecaoDetalhe(DateTime dataInicial, DateTime dataFinal, int groupType, int pageNo, int length)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                { "dataFinal", dataFinal.ToString("yyyy-MM-dd") },
                { "groupType", groupType.ToString() },
                { "pageNo", pageNo.ToString() },
                { "pageSize", length.ToString()}
            };
            var response = RestHelper.ExecuteGetRequest<PagedResult<FluxoCaixaProjecaoVM>>("fluxocaixa/projecaodetalhe", queryString);

            return response;
        }

        public JsonResult LoadChart(DateTime dataInicial, DateTime dataFinal, int groupType)
        {
            try
            {
                var response = GetProjecao(dataInicial, dataFinal, groupType);

                var dataChartToView = new
                {
                    success = true,
                    currency = true,
                    labels = response.Select(x => x.Label).ToArray(),
                    datasets = new object[] {
                        new {
                                type = "line",
                                label = "Saldo",
                                backgroundColor = "rgb(44, 55, 57)",
                                borderColor = "rgb(44, 55, 57)",
                                data = response.Select(x => Math.Round(x.SaldoFinal, 2)).ToArray(),
                                fill = false
                            },
                        new {
                                label = "Recebimentos",
                                fill = false,
                                backgroundColor = "rgb(0, 178, 121)",
                                borderColor = "rgb(0, 178, 121)",
                                data = response.Select(x => Math.Round(x.TotalRecebimentos, 2)).ToArray()
                            },
                        new {
                                label = "Pagamentos",
                                fill = false,
                                backgroundColor = "rgb(239, 100, 97)",
                                borderColor = "rgb(239, 100, 97)",
                                data = response.Select(x => Math.Round(x.TotalPagamentos * -1, 2)).ToArray()
                        }
                    }
                };

                return Json(dataChartToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        public JsonResult LoadGridFluxoCaixa(DateTime dataInicial, DateTime dataFinal, int groupType, int length)
        {
            try
            {
                var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
                var pageNo = param.Start > 0 ? (param.Start / length) + 1 : 1;
                var fileType = (Request.QueryString.AllKeys.Contains("fileType")) ? Request.QueryString.Get("fileType") : "";

                var response = GetProjecaoDetalhe(dataInicial, dataFinal, groupType, pageNo, length);
                if (!string.IsNullOrWhiteSpace(fileType))
                {
                    if (response.Data.Count.Equals(0))
                        throw new Exception("Não existem registros para exportar");
                    DataTable dataTable = GridToDataTable(response, param);
                    switch (fileType.ToLower())
                    {
                        case "pdf":
                            GridToPDF(dataTable);
                            break;
                        case "doc":
                            GridToDOC(dataTable);
                            break;
                        case "xls":
                            GridToXLS(dataTable);
                            break;
                        case "csv":
                            GridToCSV(dataTable);
                            break;
                    }

                }

                return Json(new
                {
                    recordsTotal = response.Paging.TotalRecordCount,
                    recordsFiltered = response.Paging.TotalRecordCount,
                    data = response.Data.Select(item => new
                    {
                        data = item.Label,
                        totalRecebimentos = item.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault),
                        totalPagamentos = (item.TotalPagamentos * -1).ToString("C", AppDefaults.CultureInfoDefault),
                        saldoFinal = item.SaldoFinal.ToString("C", AppDefaults.CultureInfoDefault)
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        #region ExportGrid 

        protected void GridToDOC(DataTable data)
        {
            GridView gv = new GridView()
            {
                AllowPaging = false,
                DataSource = data
            };
            gv.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.doc");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gv.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        protected void GridToXLS(DataTable data)
        {
            GridView gv = new GridView()
            {
                AllowPaging = false,
                DataSource = data
            };
            gv.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                gv.Rows[i].Attributes.Add("class", "textmode");
            }
            gv.RenderControl(hw);

            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        protected void GridToPDF(DataTable data)
        {
            Font fontDefault = FontFactory.GetFont("Roboto", 8, BaseColor.BLACK);
            int columns = data.Columns.Count;
            PdfPTable table = new PdfPTable(columns);
            int padding = 5;
            float[] widths = new float[columns];
            for (int x = 0; x < columns; x++)
            {
                string cellText = Server.HtmlDecode(data.Columns[x].ColumnName);
                widths[x] = cellText.Length > 4 ? cellText.Length : 4;
                PdfPCell cell = new PdfPCell(new Phrase(new Chunk(cellText, FontFactory.GetFont("Roboto", 8, BaseColor.WHITE))))
                {
                    BorderWidth = 0,
                    BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f37021")),
                    Padding = padding
                };
                if (x != 0)
                {
                    cell.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#eee"));
                    cell.BorderWidthLeft = 1;
                }

                table.AddCell(cell);
            }
            for (int i = 0; i < data.Rows.Count; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    string cellText = Server.HtmlDecode(data.Rows[i].ItemArray[j].ToString());
                    widths[j] = cellText.Length > widths[j] ? cellText.Length : widths[j];
                    PdfPCell cell = new PdfPCell(new Phrase(new Chunk(cellText, FontFactory.GetFont("Roboto", 8, BaseColor.BLACK))))
                    {
                        BorderWidth = 0,
                        Padding = padding
                    };

                    if (i % 2 != 0)
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#eee"));

                    if (i != 0)
                    {
                        cell.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#eee"));
                        cell.BorderWidthLeft = 1;
                    }

                    table.AddCell(cell);
                }
            }
            var totalWidth = widths.Sum();
            for (int j = 0; j < columns; j++)
            {
                widths[j] = (float)((columns > 3 ? PageSize.A4.Height : PageSize.A4.Width) * 0.98) * (1 / totalWidth * widths[j]);
            }
            table.SetWidthPercentage(widths, PageSize.A4);
            table.LockedWidth = true;
            Response.ContentType = "application/pdf";
            Document pdfDoc = new Document(data.Columns.Count > 3 ? PageSize.A4.Rotate() : PageSize.A4, 10f, 10f, 10f, 0f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            pdfDoc.Add(table);
            pdfDoc.Close();
            Response.End();
        }
        protected void GridToCSV(DataTable data)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            StringBuilder sb = new StringBuilder();
            foreach (DataColumn v in data.Columns)
                sb.Append(v.ColumnName + ',');

            sb.Append("\r\n");
            foreach (DataRow v1 in data.Rows)
            {
                for (int k = 0; k < data.Columns.Count; k++)
                    sb.Append(v1[k].ToString().Replace(",", ";") + ',');
                sb.Append("\r\n");
            }
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected DataTable GridToDataTable(PagedResult<FluxoCaixaProjecaoVM> responseGrid, JQueryDataTableParams param)
        {
            DataTable dt = new DataTable();
            dt.Clear();

            param.Columns.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.Name))
                    dt.Columns.Add(x.Name);
            });

            var data = responseGrid.Data.Select(item => new
            {
                data = item.Label,
                totalRecebimentos = item.TotalRecebimentos.ToString("C", AppDefaults.CultureInfoDefault),
                totalPagamentos = (item.TotalPagamentos * -1).ToString("C", AppDefaults.CultureInfoDefault),
                saldoFinal = item.SaldoFinal.ToString("C", AppDefaults.CultureInfoDefault)
            }).ToList();
            Type o = data.FirstOrDefault().GetType();
            data.ForEach(x =>
            {
                DataRow dtr = dt.NewRow();
                param.Columns.ForEach(y =>
                {
                    if (!string.IsNullOrWhiteSpace(y.Name))
                        dtr[y.Name] = o.GetProperty(y.Data).GetValue(x, null);
                });
                dt.Rows.Add(dtr);
            });
            dt.Columns.Cast<DataColumn>().ToList().ForEach(column =>
            {
                if (dt.AsEnumerable().All(dr => dr.IsNull(column) || dr[column].ToString().Equals("")))
                    dt.Columns.Remove(column);
            });

            return dt;
        }
        #endregion
    }
}