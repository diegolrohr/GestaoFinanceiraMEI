using Fly01.Estoque.Controllers.Base;
using Fly01.Estoque.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Fly01.Core.Helpers;
using Fly01.Core.Rest;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Estoque.Controllers
{
    public class InventarioController : BaseController<InventarioVM>
    {
        public InventarioController()
        {
            ExpandProperties = "";
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Cadastro de Inventário",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" },
                        new HtmlUIButton { Id = "finish", Label = "Finalizar Inventário", OnClickFn = "fnFinalizaInventario" },
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            #region Form fly01frmInventario

            var formConfigInventario = new FormUI
            {
                Id = "fly01frmInventario",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = Url.Action("List")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            formConfigInventario.Elements.Add(new InputHiddenUI { Id = "id" });
            formConfigInventario.Elements.Add(new InputHiddenUI { Id = "inventarioStatus" });
            formConfigInventario.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12", Label = "Descrição do Inventário", Required = true, MaxLength = 40 });

            #endregion Form fly01frmInventario

            #region Form fly01frmInventarioItem

            string nomeCtrl = @"InventarioItem";

            var formConfigInventarioItem = new FormUI
            {
                Id = "fly01frmInventarioItem",
                ReadyFn = "fnFormReadyItem",
                UrlFunctions = Url.Action("Functions", nomeCtrl, null, Request.Url.Scheme) + "?fns="
            };

            formConfigInventarioItem.Elements.Add(new LabelsetUI { Id = "inventarioItemLabelSet", Class = "col s12", Label = "Produtos" });

            formConfigInventarioItem.Elements.Add(new InputHiddenUI { Id = "saldoProduto", Value = "0" });

            formConfigInventarioItem.Elements.Add(new AutocompleteUI
            {
                Id = "produtoCodigoId",
                Class = "col s5",
                Label = "Codigo Produto",
                Required = true,
                DataUrl = @Url.Action("ProdutoCodigo", "AutoComplete"),
                LabelId = "produtoCodigo",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeProdutoCod" } }
            });

            formConfigInventarioItem.Elements.Add(new AutocompleteUI
            {
                Id = "produtoId",
                Class = "col s5",
                Label = "Produto",
                Required = true,
                DataUrl = @Url.Action("ProdutoDescricao", "AutoComplete"),
                LabelId = "produtoDescricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeProdutoDesc" } }
            });

            formConfigInventarioItem.Elements.Add(new ButtonUI
            {
                Id = "btnAdicionar",
                Class = "col s5 m2",
                Value = "Adicionar",
                DomEvents = new List<DomEventUI>() {
                    new DomEventUI() { DomEvent = "click", Function = "fnAdicionaProduto" }
                }
            });

            #endregion Form fly01frmInventarioItem

            #region DataTable dtInventarioItem

            var dtConfig = new DataTableUI
            {
                Id = "dtInventarioItem",
                UrlGridLoad = Url.Action("GridLoadInventarioItem", "InventarioItem"),
                UrlFunctions = Url.Action("Functions", "InventarioItem", null, Request.Url.Scheme) + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter {Id = "id", Required = true }
                }
            };

            dtConfig.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });

            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "codigoProduto", DisplayName = "Código", Priority = 0, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricaoProduto", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "unidadeMedida", DisplayName = "Un. Medida", Priority = 2, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "custoProduto", DisplayName = "Custo", Priority = 3, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "saldoEstoque", DisplayName = "Saldo Estoque", Priority = 4, Searchable = false, Orderable = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Novo Saldo", Priority = 5, Searchable = false, Orderable = false, RenderFn = "fnRenderSaldoInventariado", Width = "25%" });

            #endregion DataTable dtInventarioItem

            cfg.Content.Add(formConfigInventario);

            cfg.Content.Add(formConfigInventarioItem);

            cfg.Content.Add(dtConfig);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override Func<InventarioVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                descricao = x.Descricao,
                dataUltimaInteracao = x.DataUltimaInteracao.ToString("dd/MM/yyyy"),
                inventarioStatus = x.InventarioStatus,
                inventarioStatusDescription = EnumHelper.SubtitleDataAnotation(typeof(InventarioStatus), x.InventarioStatus).Description,
                inventarioStatusCssClass = EnumHelper.SubtitleDataAnotation(typeof(InventarioStatus), x.InventarioStatus).CssClass,
                inventarioStatusValue = EnumHelper.SubtitleDataAnotation(typeof(InventarioStatus), x.InventarioStatus).Value,
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Inventários",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "Inventario", null, Request.Url?.Scheme) + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions", "Inventario", null, Request.Url?.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "(row.inventarioStatus == 'Aberto')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "(row.inventarioStatus == 'Aberto')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Visualizar", ShowIf = "(row.inventarioStatus == 'Finalizado')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnFinalizarRow", Label = "Finalizar", ShowIf = "(row.inventarioStatus == 'Aberto')" });

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "inventarioStatus",
                DisplayName = "Status do Inventário",
                Priority = 0,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(InventarioStatus))),
                RenderFn = "function(data, type, row, meta) { return createElem(\"span\", {\"class\":\"new badge \" + row.inventarioStatusCssClass + \" left\", \"data-badge-caption\": \" \" }, row.inventarioStatusValue).outerHTML; }"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataUltimaInteracao", DisplayName = "Ultima Interação", Priority = 1, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2, Type = "string" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        [HttpPost]
        public override JsonResult Create(InventarioVM entityVM)
        {
            try
            {
                var result = RestHelper.ExecutePostRequest<InventarioVM>(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));

                var response = new JsonResult();

                response.Data = new { success = true, message = AppDefaults.CreateSuccessMessage, id = result.Id };

                return response;
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private string GetOrderDir(string dir)
        {
            dir = dir.Trim();
            if (dir.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
                return string.Empty;
            return string.Format(" {0}", dir.Trim());
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);

            try
            {
                var gridParams = new Dictionary<string, string>();
                gridParams.AddParam("$count", "true");

                if (param.Length > 0)
                    gridParams.AddParam("$top", param.Length.ToString());

                if (param.Order != null)
                {
                    var paramOrder = param.Order.FirstOrDefault();

                    if (paramOrder != null)
                        gridParams.AddParam("$orderby", string.Format("{0} {1}", param.Columns[paramOrder.Column].Data.Replace("_", "/"), GetOrderDir(paramOrder.Dir)));
                }

                if (param.Columns != null)
                {
                    foreach (JQueryDataTableParamsColumn item in param.Columns)
                    {
                        if ((item.Searchable) && (!string.IsNullOrWhiteSpace(item.Search.Value)) && (!string.IsNullOrWhiteSpace(item.Data)))
                        {
                            PropertyInfo propertyInfo = null;
                            var valueFilter = string.Empty;
                            var filterValid = false;
                            var filterField = item.Data;
                            bool isDatafilter = false;

                            propertyInfo = typeof(InventarioVM).GetProperties().FirstOrDefault(x => x.Name.Equals(
                                (filterField.IndexOf("_", StringComparison.Ordinal) > -1)
                                ? filterField.Substring(0, filterField.IndexOf("_", StringComparison.Ordinal))
                                : filterField, StringComparison.InvariantCultureIgnoreCase));

                            if (propertyInfo != null)
                            {
                                bool mustCompareAsEqual = false;

                                item.Search.Value = ODataStringFormater.GetString(item.Search.Value);

                                if (propertyInfo.PropertyType == typeof(string))
                                {
                                    var specialType = propertyInfo.GetCustomAttributes(typeof(APIEnumAttribute));
                                    mustCompareAsEqual = specialType.Any();

                                    if (mustCompareAsEqual)
                                        valueFilter = string.Format("{0}'{1}'", ((APIEnumAttribute[])(specialType))[0].Get(AppDefaults.APIEnumResourceName), item.Search.Value);
                                    else
                                        valueFilter = item.Search.Value;

                                    filterValid = true;
                                }

                                if ((propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)) && item.Search.Value.IsDate())
                                {
                                    valueFilter = item.Search.Value;
                                    filterValid = isDatafilter = true;
                                    mustCompareAsEqual = true;
                                }

                                if ((propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(double?)))
                                {
                                    var doubleValue = Convert.ToDouble(item.Search.Value);

                                    if (!doubleValue.Equals(default(double)))
                                    {
                                        valueFilter = doubleValue.ToString().Replace(",", ".");
                                        filterValid = true;
                                    }

                                    mustCompareAsEqual = true;
                                }

                                if (propertyInfo.PropertyType.BaseType == typeof(DomainBaseVM))
                                {
                                    item.Data = item.Data.Replace("_", "/");
                                    valueFilter = item.Search.Value;

                                    filterValid = true;
                                }

                                if (filterValid)
                                {
                                    if (isDatafilter)
                                        gridParams.AddParamAndUpdate("$filter", string.Format("{0} lt {1}T23:59:59.99Z and {0} gt {1}T00:00:00.00Z", item.Data, valueFilter));
                                    else
                                        gridParams.AddParamAndUpdate("$filter", Extensions.ReturnFilteredValue(item.Data, valueFilter, mustCompareAsEqual));

                                }
                            }
                        }
                    }
                }

                if (param.Start > 0)
                    gridParams.AddParam("$skip", param.Start.ToString());

                if (filters != null && filters.Any())
                {
                    filters.Remove("action");
                    filters.Remove("controller");
                    if (filters.Any())
                    {// se ainda tem algo
                        gridParams.AddParamAndUpdate("$filter", Extensions.ReturnProcessFilter(filters));
                    }
                }

                //GetQueryStringDefaultGridLoad().ForEach(x => gridParams.AddParamAndUpdate(x.Key, x.Value));
                foreach (var item in GetQueryStringDefaultGridLoad())
                {
                    gridParams.AddParamAndUpdate(item.Key, item.Value);
                }

                var responseGrid = RestHelper.ExecuteGetRequest<ResultBase<InventarioVM>>(ResourceName, gridParams);
                return Json(new
                {
                    recordsTotal = responseGrid.Total,
                    recordsFiltered = responseGrid.Total,
                    data = responseGrid.Data.Select(GetDisplayData())
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    queryStringFilter = string.Empty,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new { },
                    success = false,
                    message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message)
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}