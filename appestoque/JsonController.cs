using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using Fly01.Estoque.Models.Utils;
using Fly01.Estoque.Entities.Helpers;
using Fly01.Estoque.Entities.Enums;

namespace Fly01.Estoque.Controllers
{
    public class JsonController : Controller
    {
        private ContentResult GenerateContentResult(object content)
        {
            return Content(JsonConvert.SerializeObject(content, JsonUIConfig.DefaultJsonSerializerSettings), "application/json");
        }

        public ContentResult Sidebar()
        {
            SidebarUI config = new SidebarUI() { Id = "nav-bar", AppName = "Estoque", Parent = "header" };

            #region MenuItems
            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Estoque",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Visão Geral", OnClick = @Url.Action("EmConstrucao", "Json")},
                new LinkUI() { Label = "Ajuste Manual", OnClick = @Url.Action("FormAjustarEstoque", "Json")},
                new LinkUI() { Label = "Posição Atual", OnClick = @Url.Action("PosicaoAtual", "Json")},
                new LinkUI() { Label = "Inventário", OnClick = @Url.Action("EmConstrucao", "Json")},
            }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Cadastros",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label = "Produtos", OnClick = @Url.Action("RegisterProduct", "Json")},
                new LinkUI() { Label = "Grupo de Produto", OnClick = @Url.Action("GroupProduct", "Json")},
                new LinkUI() { Label = "Tipo de Movimento", OnClick = @Url.Action("MovementType", "Json")},
            }
            });

            config.MenuItems.Add(new SidebarMenuUI()
            {
                Label = "Ajuda",
                Items = new List<LinkUI>
            {
                new LinkUI() { Label =  "Assistência Remota", Link = "https://secure.logmeinrescue.com/customer/code.aspx"}
            }
            });

            #endregion

            #region User Menu Items
            config.UserMenuItems.Add(new LinkUI() { Label = "Empresa", OnClick = @Url.Action("FormEmpresa", "Json") + "\",\"01" });
            config.UserMenuItems.Add(new LinkUI() { Class = "divider" });
            config.UserMenuItems.Add(new LinkUI() { Label = "Sair", OnClick = @Url.Action("Logoff", "Account") });
            #endregion

            #region Lista de aplicativos do usuário
            var home = new HomeController();

            foreach (AppsInstaladoslataformaVM item in home.AppsList())
            {
                config.MenuApps.Add(new LinkUI() { Label = item.AppName, Class = item.IconClass, Link = item.UrlTarget, OnClick = item.UrlTarget });
            }
            #endregion

            return GenerateContentResult(config);
        }

        #region DataTables
        /*
        public ContentResult FormEmpresa()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create", "Empresa"),
                    WithParams = Url.Action("Edit", "Empresa"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Dados da Empresa",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "Empresa", null, Request.Url.Scheme) + "?fns="
            };

            FormUI config = new FormUI()
            {
                Action = new FormUIAction()
                {
                    Create = Url.Action("Edit", "Empresa"),
                    Edit = Url.Action("Edit", "Empresa"),
                    Get = Url.Action("Json", "Empresa") + "/",
                    //List = @Url.Action("FormEmpresa", "Json") + "\"/\"01"
                    List = Url.Action("Home", "Json")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "Empresa", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new FormUIElement("hidden", "id"));

            config.Elements.Add(new FormUIElement("cpfcnpj", "cnpj", "s12 l2", "CNPJ da Empresa", "", false, true, maxLength: 18));
            config.Elements.Add(new FormUIElement("text", "name", "s12 l5", "Razão Social", "", false, true, 0, 100));
            config.Elements.Add(new FormUIElement("text", "tradingName", "s12 l5", "Nome Fantasia", "", false, false, 0, 100));

            config.Elements.Add(new FormUIElement("text", "cnae", "s12 m6 l3", "CNAE Principal"));
            config.Elements.Add(new FormUIElement("text", "nire", "s12 m6 l3", "NIRE", maxLength: 25));
            config.Elements.Add(new FormUIElement("date", "nireDate", "s12 m6 l3", "Data do NIRE"));
            config.Elements.Add(new FormUIElement("text", "stateInscription", "s12 m6 l3", "Inscrição Estadual", maxLength: 18));

            config.Elements.Add(new FormUIElement("checkbox", "chkIsento", "s12 m6 l3", "Sim, é isento de Inscrição Estadual?")
            {
                DomEvents = new List<FormUIDomEvent>()
                {
                    new FormUIDomEvent() { DomEvent = "change", Function = "fnChkIsentoInscricaoEstadual" }
                }
            });
            //ver mask int
            config.Elements.Add(new FormUIElement("numbers", "stateInscriptionType", "s12 m6 l3", "Tipo Inscrição Estadual", maxLength: 1));
            config.Elements.Add(new FormUIElement("checkbox", "simplesNacionalBool", "s12 m6 l3", "Sim, é optante pelo Simples Nacional"));
            config.Elements.Add(new FormUIElement("numbers", "municipalInscription", "s12 m6 l3", "Inscrição Municipal", maxLength: 18));

            config.Elements.Add(new FormUIElement("cep", "zipcode", "s12 m6 l3", "CEP", "", false, true, 0, 9));
            config.Elements.Add(new FormUIElement("select", "state", "s12 m6 l3", "Estado", required: true)
            {
                #region Estados
                Options = new List<HtmlUIElementBase>
                {
                    new HtmlUIElementBase() { Label = "ACRE", Value = "AC" },
                    new HtmlUIElementBase() { Label = "ALAGOAS", Value = "AL" },
                    new HtmlUIElementBase() { Label = "AMAPÁ", Value = "AP" },
                    new HtmlUIElementBase() { Label = "AMAZONAS", Value = "AM" },
                    new HtmlUIElementBase() { Label = "BAHIA", Value = "BA" },
                    new HtmlUIElementBase() { Label = "CEARÁ", Value = "CE" },
                    new HtmlUIElementBase() { Label = "DISTRITO FEDERAL", Value = "DF" },
                    new HtmlUIElementBase() { Label = "ESPIRITO SANTO", Value = "ES" },
                    new HtmlUIElementBase() { Label = "GOIÁS", Value = "GO" },
                    new HtmlUIElementBase() { Label = "MARANHÃO", Value = "MA" },
                    new HtmlUIElementBase() { Label = "MATO GROSSO", Value = "MT" },
                    new HtmlUIElementBase() { Label = "MATO GROSSO DO SUL", Value = "MS" },
                    new HtmlUIElementBase() { Label = "MINAS GERAIS", Value = "MG" },
                    new HtmlUIElementBase() { Label = "PARÁ", Value = "PA" },
                    new HtmlUIElementBase() { Label = "PARAÍBA", Value = "PB" },
                    new HtmlUIElementBase() { Label = "PARANÁ", Value = "PR" },
                    new HtmlUIElementBase() { Label = "PERNAMBUCO", Value = "PE" },
                    new HtmlUIElementBase() { Label = "PIAUÍ", Value = "PI"},
                    new HtmlUIElementBase() { Label = "RIO DE JANEIRO", Value = "RJ" },
                    new HtmlUIElementBase() { Label = "RIO GRANDE DO NORTE", Value = "RN" },
                    new HtmlUIElementBase() { Label = "RIO GRANDE DO SUL", Value = "RS" },
                    new HtmlUIElementBase() { Label = "RONDÔNIA", Value = "RO"},
                    new HtmlUIElementBase() { Label = "RORAIMA", Value = "RR" },
                    new HtmlUIElementBase() { Label = "SANTA CATARINA", Value = "SC"},
                    new HtmlUIElementBase() { Label = "SÃO PAULO", Value = "SP"},
                    new HtmlUIElementBase() { Label = "SERGIPE", Value = "SE"},
                    new HtmlUIElementBase() { Label = "TOCANTINS", Value = "TO"},
                    new HtmlUIElementBase() { Label = "EXTERIOR", Value = "EX"}
                },
                #endregion
                DomEvents = new List<FormUIDomEvent>()
                {
                    new FormUIDomEvent() { DomEvent = "change", Function = "fnChangeEstado" }
                }
            });
            config.Elements.Add(new FormUIElement("autocomplete", "city", "s12 m6 l6", "Cidade (Escolha o estado antes)", "", false, true, 0, 35)
            {
                DataUrl = Url.Action("City", "AutoComplete"),
                LabelId = "cityDescricao",
                PreFilter = "state"
            });

            config.Elements.Add(new FormUIElement("text", "neighborhood", "s12 m6 l4", "Bairro", "", false, false, 0, 30));
            config.Elements.Add(new FormUIElement("text", "address", "s12 l8", "Endereço (endereço, o número )", "", false, false, 0, 50));

            config.Elements.Add(new FormUIElement("tel", "phone", "s12 m6 l4", "Telefone Comercial", "", false, false, 0, 15));
            config.Elements.Add(new FormUIElement("email", "email", "s12 m6 l8", "E-mail", "", false, false, 0, 70));

            //v2 alterar imagem
            //config.Elements.Add(new FormUIElement("labelset", "labelConfiguracao", "s12", "Configurações"));

            //cfg.Content.Add(new { type = "form", value = config });
            //config.Elements.Add(new FormUIElement("text", "labelLogo", "s12 m6 l4", "Logotipo para relatórios"));
            //config.Elements.Add(new FormUIElement("button", "btnAlterarLogotipo", "s12 l2", "", "Alterar logotipo")
            //{
            //    DomEvents = new List<FormUIDomEvent>() {
            //        new FormUIDomEvent() { DomEvent = "click", Function = "fnAlterarLogotipo" }
            //    }
            //});

            cfg.Content.Add(new { type = "form", value = config });
            return GenerateContentResult(cfg);
        }
        */
        public ContentResult PosicaoAtual()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "PosicaoAtual") },
                Header = new HtmlUIHeader()
                {
                    Title = "Posição Atual",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "new", Label = "Alterar Estoque", OnClickFn = "fnAlterarEstoque" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "PosicaoAtual", null, Request.Url.Scheme) + "?fns="
            };

            DataTableUI config = new DataTableUI
            {
                UrlGridLoad = Url.Action("GridLoad", "PosicaoAtual"),
                UrlFunctions = Url.Action("Functions", "PosicaoAtual", null, Request.Url.Scheme) + "?fns=",
                Options = new DataTableUIConfig() {PageLength = 30, WithoutRowMenu = true}
            };

            config.Columns.Add(new DataTableUIColumn() { DataField = "productId", DisplayName = "Código", Priority = 6 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "description", DisplayName = "Produto", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn()
            {
                DataField = "measureDescription",
                DisplayName = "Unid. Medida",
                Priority = 7,
                RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.measureDescriptionCssClass + \" left\\\" data-badge-caption=\\\" \\\">\" + full.measureDescription + \"</span>\" }"
            });
            config.Columns.Add(new DataTableUIColumn() { DataField = "cost", DisplayName = "Valor Custo", Priority = 4 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "salePrice", DisplayName = "Valor Venda", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "balanceFull", DisplayName = "Qtd. Estoque", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "totalCost", DisplayName = "Custo Total", Priority = 5 });

            cfg.Content.Add(new { type = "dataTable", value = config });
            return GenerateContentResult(cfg);
        }
        public ContentResult Inventario()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "Inventario") },
                Header = new HtmlUIHeader()
                {
                    Title = "Inventário",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "new", Label = "Novo Inventário", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "Inventario", null, Request.Url.Scheme) + "?fns="
            };
            DataTableUI config = new DataTableUI() { UrlGridLoad = Url.Action("GridLoad", "Inventario"), UrlFunctions = Url.Action("Functions", "Inventario", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnVisualizar", Label = "Visualizar", ShowIf = "row.subtitleCode == 2" });
            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.subtitleCode == 1" });
            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.subtitleCode == 1" });

            config.Columns.Add(new DataTableUIColumn()
            {
                DataField = "subtitleCode",
                DisplayName = "Status",
                Priority = 2,
                RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.subtitleCodeCssClass + \" left\\\" data-badge-caption=\\\" \\\">\" + full.subtitleCode + \"</span>\" }"
            });
            config.Columns.Add(new DataTableUIColumn() { DataField = "date", DisplayName = "Data", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "stocktakingDescription", DisplayName = "Descrição", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "processDate", DisplayName = "Data processamento", Priority = 4 });

            cfg.Content.Add(new { type = "dataTable", value = config });
            return GenerateContentResult(cfg);
        }
        public ContentResult RegisterProduct()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "RegisterProduct") },
                Header = new HtmlUIHeader()
                {
                    Title = "Produtos",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "RegisterProduct", null, Request.Url.Scheme) + "?fns="
            };
            DataTableUI config = new DataTableUI() { UrlGridLoad = Url.Action("GridLoad", "RegisterProduct"), UrlFunctions = Url.Action("Functions", "RegisterProduct", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnExcluir", Label = "Excluir" });

            config.Columns.Add(new DataTableUIColumn() { DataField = "id", DisplayName = "Código", Priority = 4 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "description", DisplayName = "Descrição", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "salePrice1", DisplayName = "Valor de venda", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "replacementCost", DisplayName = "Valor de custo", Priority = 4 });

            cfg.Content.Add(new { type = "dataTable", value = config });
            return GenerateContentResult(cfg);
        }
        public ContentResult GroupProduct()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "GroupProduct") },
                Header = new HtmlUIHeader()
                {
                    Title = "Cadastro de Grupo de Produto",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "GroupProduct", null, Request.Url.Scheme) + "?fns="
            };
            DataTableUI config = new DataTableUI() { UrlGridLoad = Url.Action("GridLoad", "GroupProduct"), UrlFunctions = Url.Action("Functions", "GroupProduct", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnExcluir", Label = "Excluir" });

            config.Columns.Add(new DataTableUIColumn() { DataField = "id", DisplayName = "Código", Priority = 2, Visible = false });
            config.Columns.Add(new DataTableUIColumn() { DataField = "description", DisplayName = "Descrição", Priority = 1 });

            cfg.Content.Add(new { type = "dataTable", value = config });
            return GenerateContentResult(cfg);
        }
        public ContentResult MovementType()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "MovementType") },
                Header = new HtmlUIHeader()
                {
                    Title = "Cadastro de Tipo de Movimento",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "MovementType", null, Request.Url.Scheme) + "?fns="
            };
            DataTableUI config = new DataTableUI() { UrlGridLoad = Url.Action("GridLoad", "MovementType"), UrlFunctions = Url.Action("Functions", "MovementType", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction() { OnClickFn = "fnExcluir", Label = "Excluir" });

            config.Columns.Add(new DataTableUIColumn() { DataField = "id", DisplayName = "Código", Priority = 3, Visible = false });
            config.Columns.Add(new DataTableUIColumn() { DataField = "description", DisplayName = "Descrição", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "type", DisplayName = "Tipo", Priority = 2 });

            cfg.Content.Add(new { type = "dataTable", value = config });
            return GenerateContentResult(cfg);
        }
        #endregion

        #region Forms
        public ContentResult FormAjustarEstoque()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create", "AjustarEstoque"),
                    WithParams = Url.Action("Edit", "AjustarEstoque"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Ajuste Manual",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "AjustarEstoque", null, Request.Url.Scheme) + "?fns="
            };

            FormUI config = new FormUI()
            {
                Action = new FormUIAction()
                {
                    Create = Url.Action("Create", "AjustarEstoque"),
                    Edit = Url.Action("Edit", "AjustarEstoque"),
                    Get = Url.Action("Json", "AjustarEstoque") + "/"
                },
                UrlFunctions = Url.Action("Functions", "AjustarEstoque", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new FormUIElement("hidden", "id"));

            config.Elements.Add(new FormUIElement("select", "transactionTPType", "s12 m6", "Finalidade", "", false, true)
            {
                Options = new List<HtmlUIElementBase>
                {
                    new HtmlUIElementBase() { Label = "Entrada", Value = "EN" },
                    new HtmlUIElementBase() { Label = "Saída", Value = "SA"}
                }
            });

            config.Elements.Add(new FormUIElement("autocomplete", "transactionTP", "s12 m6", "Tipo de Movimento", "", false, true)
            {
                DataUrl = @Url.Action("MovementType", "AutoComplete"),
                LabelId = "type",
            });
            config.Elements.Add(new FormUIElement("autocomplete", "productId", "s12 m6", "Produto", "", false, true)
            {
                DataUrl = @Url.Action("Product", "AutoComplete"),
                LabelId = "description",
            });
            config.Elements.Add(new FormUIElement("numbers", "Quantity", "s12 m6", "Quantidade", "", false, true));
            config.Elements.Add(new FormUIElement("textarea", "note", "s12", "Observação"));


            cfg.Content.Add(new { type = "form", value = config });
            return GenerateContentResult(cfg);
        }
        public ContentResult FormInventario()
        {
            ContentUI config = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create", "Inventario"),
                    WithParams = Url.Action("Edit", "Inventario"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Inventário",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton() { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "Inventario", null, Request.Url.Scheme) + "?fns="
            };

            var dataAtual = DateTime.Now;

            config.Content.Add(new
            {
                type = "form",
                value = new FormUI()
                {
                    UrlFunctions = Url.Action("Functions", "Inventario", null, Request.Url.Scheme) + "?fns=",
                    Class = "col s12",
                    Elements = new List<FormUIElement>()
                    {
                        new FormUIElement("text", "stocktakingDescription", "s12", "Descrição", "", false, true)
                    }
                }
            });

            config.Content.Add(new
            {
                type = "dataTable",
                value = new DataTableUI()
                {
                    Class = "col s12",
                    UrlGridLoad = Url.Action("GridLoad", "RegisterProduct"),
                    Columns = new List<DataTableUIColumn>()
                    {
                        new DataTableUIColumn() { DataField = "id", DisplayName = "Id", Priority = 7, Orderable = false, Visible = false },
                        new DataTableUIColumn() { DataField = "productCode", DisplayName = "Código", Priority = 6, Orderable = false },
                        new DataTableUIColumn() { DataField = "productDescription", DisplayName = "Produto", Priority = 2, Orderable = false },
                        new DataTableUIColumn() { DataField = "measureId", DisplayName = "Unid. Medida", Priority = 5, Orderable = false },
                        new DataTableUIColumn() { DataField = "cost", DisplayName = "Custo", Priority = 4, Orderable = false },
                        new DataTableUIColumn() { DataField = "stockQuantity", DisplayName = "Saldo Estoque", Priority = 3, Orderable = false },
                        //new DataTableUIColumn() { DisplayName = "Novo Saldo", Priority = 1, Orderable = false }
                    }
                }
            });
            
            return GenerateContentResult(config);

        }
        public ContentResult FormRegisterProduct()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create", "RegisterProduct"),
                    WithParams = Url.Action("Edit", "RegisterProduct"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Dados do Produto",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar"},
                        new HtmlUIButton() { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "RegisterProduct", null, Request.Url.Scheme) + "?fns="
            };

            FormUI config = new FormUI()
            {
                Action = new FormUIAction()
                {
                    Create = Url.Action("Create", "RegisterProduct"),
                    Edit = Url.Action("Edit", "RegisterProduct"),
                    Get = Url.Action("Json", "RegisterProduct") + "/",
                    List = Url.Action("RegisterProduct", "Json")
                },
                UrlFunctions = Url.Action("Functions", "RegisterProduct", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new FormUIElement("hidden", "isCreate"));

            config.Elements.Add(new FormUIElement("text", "description", "s12 l9", "Descrição", "", false, true, 0, 100));
            config.Elements.Add(new FormUIElement("text", "measure", "s12 l3", "Unidade de Medida", "", false, true));

            //Painel - Dados Basicos
            config.Elements.Add(new FormUIElement("labelset", "productLabel", "s12", "Dados Basicos"));
            config.Elements.Add(new FormUIElement("text", "id", "s12 m6 l3", "Código do Produto", "", false, false, 0, 15));//NOK - falta Hint(Caso o código não seja informado, o sistema irá atribuir um novo código para o produto.)
            config.Elements.Add(new FormUIElement("text", "barCode", "s12 m6 l3", "Código de Barra (EAN)", "", false, false, 0, 15));
            config.Elements.Add(new FormUIElement("text", "groupId", "s12 m6 l3", "Categoria do Produto"));
            config.Elements.Add(new FormUIElement("text", "type", "s12 m6 l3", "Tipo", "", false, true)
            {
                //Options = new List<FormUIElementBase>(SystemValueHelper.GetUIElementBase(SystemValueEnum.TipoFormaPagamento, true, false))
            });

            config.Elements.Add(new FormUIElement("text", "salePrice1", "s6 l3", "Valor de Venda"));
            config.Elements.Add(new FormUIElement("text", "ReplacementCost", "s6 l3", "Valor de Custo"));

            config.Elements.Add(new FormUIElement("text", "applicationPoint", "s12 l3", "Ponto Pedido", "", false, false, 0, 14));
            config.Elements.Add(new FormUIElement("text", "dtLastCalcReplacementCost", "s12 l3", "Último Cálculo Custo"));//NOK - falta Hint (Data do último calculo do custo de reposição gravada pela rotina 'Custo de Reposição'.)

            config.Elements.Add(new FormUIElement("text", "recalcType", "s12 l3", "Tipo Recálculo")
            {
                //Options = new List<FormUIElementBase>(SystemValueHelper.GetUIElementBase(SystemValueEnum.TipoFormaPagamento, true, false))
            });
            config.Elements.Add(new FormUIElement("text", "outOfState", "s6 l3", "Fora Estado")
            {
                //Options = new List<FormUIElementBase>(SystemValueHelper.GetUIElementBase(SystemValueEnum.TipoFormaPagamento, true, false))
            });

            config.Elements.Add(new FormUIElement("text", "ncmLetter", "s6 l3", "Letra NCM", "", false, false, 0, 1));
            config.Elements.Add(new FormUIElement("text", "ncm", "s6 l3", "Código NCM"));
            config.Elements.Add(new FormUIElement("text", "ncmException", "s6 l3", "Ex do NCM"));
            config.Elements.Add(new FormUIElement("text", "netWeight", "s6 l3", "Peso Líquido", "", false, false, 0, 11));
            config.Elements.Add(new FormUIElement("text", "grossWeight", "s6 l3", "Peso Bruto", "", false, false, 0, 11));

            config.Elements.Add(new FormUIElement("textarea", "commercialObservations", "s12", "Observação"));

            //Painel - Dados de Importação
            config.Elements.Add(new FormUIElement("labelset", "productLabel", "s12", "Dados de Importação"));
            config.Elements.Add(new FormUIElement("text", "origin", "s12 m12 l4", "Origem", "", false, true)
            {
                //Options = new List<FormUIElementBase>(SystemValueHelper.GetUIElementBase(SystemValueEnum.TipoFormaPagamento, true, false))
            });
            config.Elements.Add(new FormUIElement("text", "FciCode", "s12 m6 l4", "Código FCI", "", false, true, 0, 36));
            config.Elements.Add(new FormUIElement("text", "FciImportValue", "s12 m6 l4", "Val Cont. Importado (Produção)"));

            config.Elements.Add(new FormUIElement("text", "FciCalculatedSaleValue", "s12 m6 l4", "Valor de Venda Apurado"));
            config.Elements.Add(new FormUIElement("text", "ImportationContent", "s12 m6 l4", "Conteúdo de Importação de MP"));

            cfg.Content.Add(new { type = "form", value = config });
            return GenerateContentResult(cfg);
        }
        public ContentResult FormGroupProduct()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create", "GroupProduct"),
                    WithParams = Url.Action("Edit", "GroupProduct"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Dados do Produto",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar"},
                        new HtmlUIButton() { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "GroupProduct", null, Request.Url.Scheme) + "?fns="
            };

            FormUI config = new FormUI()
            {
                Action = new FormUIAction()
                {
                    Create = Url.Action("Create", "GroupProduct"),
                    Edit = Url.Action("Edit", "GroupProduct"),
                    Get = Url.Action("Json", "GroupProduct") + "/",
                    List = Url.Action("GroupProduct", "Json")
                },
                UrlFunctions = Url.Action("Functions", "GroupProduct", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new FormUIElement("hidden", "id"));
            config.Elements.Add(new FormUIElement("text", "description", "s12 l9", "Descrição", "", false, true, 0, 100));
          

            cfg.Content.Add(new { type = "form", value = config });
            return GenerateContentResult(cfg);
        }
        public ContentResult FormMovementType()
        {
            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create", "MovementType"),
                    WithParams = Url.Action("Edit", "MovementType"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Dados do Tipo de Movimento",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar"},
                        new HtmlUIButton() { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "MovementType", null, Request.Url.Scheme) + "?fns="
            };

            FormUI config = new FormUI()
            {
                Action = new FormUIAction()
                {
                    Create = Url.Action("Create", "MovementType"),
                    Edit = Url.Action("Edit", "MovementType"),
                    Get = Url.Action("Json", "MovementType") + "/",
                    List = Url.Action("MovementType", "Json")
                },
                UrlFunctions = Url.Action("Functions", "MovementType", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new FormUIElement("hidden", "id"));
            config.Elements.Add(new FormUIElement("text", "description", "s12 l9", "Descrição", "", false, true, 0, 100));
            config.Elements.Add(new FormUIElement("text", "type", "s12 l3", "Tipo", "", false, true)
            {
                //Options = new List<FormUIElementBase>(SystemValueHelper.GetUIElementBase(SystemValueEnum.TipoFormaPagamento, true, false))
            });

            cfg.Content.Add(new { type = "form", value = config });
            return GenerateContentResult(cfg);
        }
        
       
        #endregion

        /*
        #region DataTables
        public ContentResult AjustarEstoque()
        {
            DataTableUI config = new DataTableUI(Url.Action("GridLoad", "AjustarEstoque"), "Ajustar Estoque");

            config.Header.Buttons.Add(new HtmlUIButton("new", "Novo", "fnNovo"));

            return GenerateContentResult(config);
        }// NOK - apenas link para o form (fnNovo)
        public ContentResult PosicaoAtual()
        {
            DataTableUI config = new DataTableUI(Url.Action("GridLoad", "PosicaoAtual"), "Posição Atual");

            config.Header.Buttons.Add(new HtmlUIButton("new", "Alterar Estoque", "fnNovo"));

            config.Columns.Add(new DataTableUIColumn("ProductId", "Código", 6));
            config.Columns.Add(new DataTableUIColumn("Description", "Produto", 1));
            config.Columns.Add(new DataTableUIColumn("MeasureDescription", "Unidade Medida", 7));
            config.Columns.Add(new DataTableUIColumn("Cost", "Valor Custo", 4));
            config.Columns.Add(new DataTableUIColumn("SalePrice", "Valor Venda", 3));
            config.Columns.Add(new DataTableUIColumn("BalanceFull", "Qtd. Estoque", 2));
            config.Columns.Add(new DataTableUIColumn("TotalCost", "Custo Total", 5));

            return GenerateContentResult(config);
        } // NOK - button "alterar estoque" não funciona (direcionamento para o Ajustar Estoque) // criei outro form igual -- pode ser retirado
        public ContentResult Inventario()
        {
            DataTableUI config = new DataTableUI(Url.Action("GridLoad", "Inventario"), "Inventário");

            config.Header.Buttons.Add(new HtmlUIButton("new", "Novo Inventário", "fnNovo"));
            
            config.Actions.Add(new DataTableUIAction("fnEditar", "Editar", "row.SubtitleCode == 1"));
            config.Actions.Add(new DataTableUIAction("fnExcluir", "Excluir", "row.SubtitleCode != 2"));
            config.Actions.Add(new DataTableUIAction("fnVisualizar", "visualizar", "row.SubtitleCode == 2"));
            
            config.Columns.Add(new DataTableUIColumn("SubtitleCode", "Status", 2));
            config.Columns.Add(new DataTableUIColumn("Date", "Data", 4));
            config.Columns.Add(new DataTableUIColumn("StocktakingDescription", "Descrição", 1));
            config.Columns.Add(new DataTableUIColumn("ProcessDate", "Data processamento", 5));

            return GenerateContentResult(config);
        }
        public ContentResult RegisterProduct()
        {
            DataTableUI config = new DataTableUI(Url.Action("GridLoad", "RegisterProduct"), "Produtos");

            config.Header.Buttons.Add(new HtmlUIButton("new", "Novo", "fnNovo"));

            config.Actions.Add(new DataTableUIAction("fnEditar", "Editar"));
            config.Actions.Add(new DataTableUIAction("fnExcluir", "Excluir"));

            config.Columns.Add(new DataTableUIColumn("Id", "Código", 4, false,true,false));
            config.Columns.Add(new DataTableUIColumn("Description", "Descrição", 1));
            config.Columns.Add(new DataTableUIColumn("SalePrice1", "Valor de venda", 2));
            config.Columns.Add(new DataTableUIColumn("ReplacementCost", "Valor de custo", 3));

            return GenerateContentResult(config);
        }
        public ContentResult GroupProduct()
        {
            DataTableUI config = new DataTableUI(Url.Action("GridLoad", "GroupProduct"), "Cadastro de Grupo de Produto");

            config.Header.Buttons.Add(new HtmlUIButton("new", "Novo", "fnNovo"));

            config.Actions.Add(new DataTableUIAction("fnEditar", "Editar"));
            config.Actions.Add(new DataTableUIAction("fnExcluir", "Excluir"));

            config.Columns.Add(new DataTableUIColumn("Id", "Código", 2, false, true, false));
            config.Columns.Add(new DataTableUIColumn("Description", "Descrição", 1));

            return GenerateContentResult(config);
        }
        public ContentResult MovementType()
        {
            DataTableUI config = new DataTableUI(Url.Action("GridLoad", "MovementType"), "Cadastro de Tipo de Movimento");

            config.Header.Buttons.Add(new HtmlUIButton("new", "Novo", "fnNovo"));

            config.Actions.Add(new DataTableUIAction("fnEditar", "Editar"));
            config.Actions.Add(new DataTableUIAction("fnExcluir", "Excluir"));

            config.Columns.Add(new DataTableUIColumn("Id", "Código", 3, false, true, false));
            config.Columns.Add(new DataTableUIColumn("Description", "Descrição", 1));
            config.Columns.Add(new DataTableUIColumn("Type", "Tipo", 2));

            return GenerateContentResult(config);
        }
        #endregion

        #region Forms
        public ContentResult FormAjustarEstoque()
        {
            FormUI config = new FormUI("Ajuste Manual")
            {
                Action = new FormUIAction(
                    @Url.Action("Create", "AjustarEstoque"),
                    @Url.Action("Edit", "AjustarEstoque"),
                    @Url.Action("Json", "AjustarEstoque") + "/",
                    @Url.Action("AjustarEstoque", "Json")
                )
            };


            config.Header.Buttons.Add(new HtmlUIButton("save", "Salvar", "fnSalvar", "submit"));

            config.Elements.Add(new FormUIElement("hidden", "Id"));
            config.Elements.Add(new FormUIElement("select", "TransactionTPType", "s12 l6", "Finalidade", "", false, true)
            {
                Options = new List<FormUIElementBase>
                {
                    new FormUIElementBase("Saída", "2"),
                    new FormUIElementBase("Entrada", "3")
                }
            });
            config.Elements.Add(new FormUIElement("autocomplete", "TransactionTP", "s12 l6", "Tipo de Movimento", "", false, true)
            {
                DataUrl = @Url.Action("MovementType", "AutoComplete"),
                LabelId = "TransactionTP",
            });
            config.Elements.Add(new FormUIElement("autocomplete", "ProductId", "s12 l6", "Produto", "", false, true)
            {
                DataUrl = @Url.Action("Product", "AutoComplete"),
                LabelId = "ProductId",
            });
            config.Elements.Add(new FormUIElement("text", "Quantity", "s12 l6", "Quantidade"));
            config.Elements.Add(new FormUIElement("textarea", "Note", "s12", "Observação"));

            return GenerateContentResult(config);
        }
        public ContentResult FormPosicaoAtual()
        {
            FormUI config = new FormUI("Ajuste Manual")
            {
                Action = new FormUIAction(
                    @Url.Action("Create", "PosicaoAtual"),
                    @Url.Action("Edit", "PosicaoAtual"),
                    @Url.Action("Json", "PosicaoAtual") + "/",
                    @Url.Action("PosicaoAtual", "Json")
                )
            };

            config.Header.Buttons.Add(new HtmlUIButton("save", "Salvar", "fnSalvar", "submit"));

            config.Elements.Add(new FormUIElement("hidden", "Id"));
            config.Elements.Add(new FormUIElement("select", "TransactionTPType", "s12 l6", "Finalidade", "", false, true)
            {
                Options = new List<FormUIElementBase>
                {
                    new FormUIElementBase("Saída", "2"),
                    new FormUIElementBase("Entrada", "3")
                }
            });
            config.Elements.Add(new FormUIElement("autocomplete", "TransactionTP", "s12 l6", "Tipo de Movimento", "", false, true)
            {
                DataUrl = @Url.Action("MovementType", "AutoComplete"),
                LabelId = "TransactionTP",
            });
            config.Elements.Add(new FormUIElement("autocomplete", "ProductId", "s12 l6", "Produto", "", false, true)
            {
                DataUrl = @Url.Action("Product", "AutoComplete"),
                LabelId = "ProductId",
            });
            config.Elements.Add(new FormUIElement("text", "Quantity", "s12 l6", "Quantidade"));
            config.Elements.Add(new FormUIElement("textarea", "Note", "s12", "Observação"));

            return GenerateContentResult(config);
        }
        public ContentResult FormInventario()
        {
            FormUI config = new FormUI("Inventário")
            {
                Action = new FormUIAction(
                    @Url.Action("Create", "Inventario"),
                    @Url.Action("Edit", "Inventario"),
                    @Url.Action("Json", "Inventario") + "/",
                    @Url.Action("Inventario", "Json")
                )
            };

            config.Header.Buttons.Add(new HtmlUIButton("cancel", "Cancelar", "fnCancelar", "submit"));
            config.Header.Buttons.Add(new HtmlUIButton("save", "Salvar", "fnSalvar", "submit"));
            config.Header.Buttons.Add(new HtmlUIButton("save", "Imprimir planilha para contagem", "fnImprimirPlanilha", "submit"));
            config.Header.Buttons.Add(new HtmlUIButton("save", "Continuar mais tarde", "fnContinuarMaisTarde", "submit"));
            config.Header.Buttons.Add(new HtmlUIButton("save", "Finalizar Inventário", "fnFinalizarInventario", "submit"));

            config.Elements.Add(new FormUIElement("hidden", "Id"));
            config.Elements.Add(new FormUIElement("hidden", "Warehouse"));
            config.Elements.Add(new FormUIElement("hidden", "ProcessDate"));
            config.Elements.Add(new FormUIElement("hidden", "Date"));
            config.Elements.Add(new FormUIElement("hidden", "SubtitleCode"));
            config.Elements.Add(new FormUIElement("text", "StocktakingDescription", "s6", "Descrição"));

            return GenerateContentResult(config);
        }
        public ContentResult FormRegisterProduct()
        {
            FormUI config = new FormUI("Dados do Produto")
            {
                Action = new FormUIAction(
                    @Url.Action("Create", "RegisterProduct"),
                    @Url.Action("Edit", "RegisterProduct"),
                    @Url.Action("Json", "RegisterProduct") + "/",
                    @Url.Action("RegisterProduct", "Json")
                )
            };

            config.Header.Buttons.Add(new HtmlUIButton("cancel", "Cancelar", "fnCancelar"));
            config.Header.Buttons.Add(new HtmlUIButton("save", "Salvar", "fnSalvar", "submit"));

            config.Elements.Add(new FormUIElement("hidden", "IsCreate"));
            config.Elements.Add(new FormUIElement("text", "Description", "s12 l9", "Descrição","",false,true,0,100));
            config.Elements.Add(new FormUIElement("text", "Measure", "s12 l3", "Unidade de Medida","", false, true));

            //Painel - Dados basicos
            config.Elements.Add(new FormUIElement("text", "Id", "s12 m6 l3", "Código do Produto", "", false, false, 0, 15));//NOK - falta Hint(Caso o código não seja informado, o sistema irá atribuir um novo código para o produto.)
            config.Elements.Add(new FormUIElement("text", "BarCode", "s12 m6 l3", "Código de Barra (EAN)", "", false, false, 0, 15));
            config.Elements.Add(new FormUIElement("text", "GroupId", "s12 m6 l3", "Categoria do Produto"));
            config.Elements.Add(new FormUIElement("select", "Type", "s12 m6 l3", "Tipo", "", false, true)
            {
                Options = new List<FormUIElementBase>
                {                   
                    new FormUIElementBase("Material de Consumo", "MC"),                    
                    new FormUIElementBase("Materia Prima", "MP"),
                    new FormUIElementBase("Produto Acabado", "MA"),
                    new FormUIElementBase("Produto Intermediário", "PI"),
                    new FormUIElementBase("Outros", "O")
                }
            });

            config.Elements.Add(new FormUIElement("text", "SalePrice1", "s6 l3", "Valor de Venda"));//NOK - Diminuir s5 para s3, quebrando a linha depois
            config.Elements.Add(new FormUIElement("text", "ReplacementCost", "s6 l3", "Valor de Custo"));

            config.Elements.Add(new FormUIElement("text", "ApplicationPoint", "s12 l3", "Ponto Pedido", "", false, false, 0, 14));
            config.Elements.Add(new FormUIElement("text", "DtLastCalcReplacementCost", "s12 l3", "Último Cálculo Custo"));//NOK - falta Hint (Data do último calculo do custo de reposição gravada pela rotina 'Custo de Reposição'.)
            config.Elements.Add(new FormUIElement("select", "RecalcType", "s12 l3", "Tipo Recálculo")
            {
                Options = new List<FormUIElementBase>
                {
                    new FormUIElementBase("Ult Prec Compra", "UPC"),
                    new FormUIElementBase("Estrutura", "E"),
                }
            });
            config.Elements.Add(new FormUIElement("select", "OutOfState", "s6 l3", "Fora Estado")
            {
                Options = new List<FormUIElementBase>
                {
                    new FormUIElementBase("Não", "N"),
                    new FormUIElementBase("Sim", "S"),
                }
            });

            config.Elements.Add(new FormUIElement("text", "NcmLetter", "s6 l3", "Letra NCM","", false, false, 0, 1));
            config.Elements.Add(new FormUIElement("text", "Ncm", "s6 l3", "Código NCM"));
            config.Elements.Add(new FormUIElement("text", "NcmException", "s6 l3", "Ex do NCM"));
            config.Elements.Add(new FormUIElement("text", "NetWeight", "s6 l3", "Peso Líquido", "", false, false, 0, 11));
            config.Elements.Add(new FormUIElement("text", "GrossWeight", "s6 l3", "Peso Bruto", "", false, false, 0, 11));

            config.Elements.Add(new FormUIElement("textarea", "CommercialObservations", "s12", "Observação"));


            //Painel - Dados de Importação
            config.Elements.Add(new FormUIElement("select", "Origin", "s12 m12 l4", "Origem", "", false, true)
            {
                Options = new List<FormUIElementBase>
                {
                    new FormUIElementBase("Nacional","N"),
                    new FormUIElementBase("Nacional - Merc/Bem com conteúdo de importação superior a 40%", "1"),
                    new FormUIElementBase("Nacional, Prod. em conformidade com os procedimentos produtivos básicos", "2"),
                    new FormUIElementBase("Nacional - Merc/Bem com conteúdo de importação inferiro ou igual a 40%", "3"),
                    new FormUIElementBase("Nacional - Mercadoria ou bem com conteúdo de importação superiro a 70%", "4"),
                    new FormUIElementBase("Estrangeiro - Importação Direta", "5"),
                    new FormUIElementBase("Estrangeiro - Adquirida Merc. Interno", "6"),
                    new FormUIElementBase("Estrangeiro - Importação direta, sem similar nacional, consta na lista Camex", "7"),
                    new FormUIElementBase("Estrangeiro - Adq. Internamente, sem similar nacional, consta na lista Camex", "8")

                }
            });
            config.Elements.Add(new FormUIElement("text", "FciCode", "s12 m6 l4", "Código FCI", "", false, true, 0, 36));
            config.Elements.Add(new FormUIElement("text", "FciImportValue", "s12 m6 l4", "Val Cont. Importado (Produção)"));

            config.Elements.Add(new FormUIElement("text", "FciCalculatedSaleValue", "s12 m6 l4", "Valor de Venda Apurado"));
            config.Elements.Add(new FormUIElement("text", "ImportationContent", "s12 m6 l4", "Conteúdo de Importação de MP"));

            return GenerateContentResult(config);
        }
        public ContentResult FormGroupProduct()
        {
            FormUI config = new FormUI("Dados do Grupo de Produto")
            {
                Action = new FormUIAction(
                    @Url.Action("Create", "GroupProduct"),
                    @Url.Action("Edit", "GroupProduct"),
                    @Url.Action("Json", "GroupProduct") + "/",
                    @Url.Action("GroupProduct", "Json")
                )
            };

            config.Header.Buttons.Add(new HtmlUIButton("cancel", "Cancelar", "fnCancelar", "submit"));
            config.Header.Buttons.Add(new HtmlUIButton("save", "Salvar", "fnSalvar", "submit"));

            config.Elements.Add(new FormUIElement("hidden", "Id"));
            config.Elements.Add(new FormUIElement("text", "Description", "s12", "Descrição", "", false, true, 0, 100));

            return GenerateContentResult(config);
        }
        public ContentResult FormMovementType()
        {
            FormUI config = new FormUI("Dados do Tipo de Movimento")
            {
                Action = new FormUIAction(
                    @Url.Action("Create", "MovementType"),
                    @Url.Action("Edit", "MovementType"),
                    @Url.Action("Json", "MovementType") + "/",
                    @Url.Action("MovementType", "Json")
                )
            };

            config.Header.Buttons.Add(new HtmlUIButton("cancel", "Cancelar", "fnCancelar", "submit"));
            config.Header.Buttons.Add(new HtmlUIButton("save", "Salvar", "fnSalvar", "submit"));

            config.Elements.Add(new FormUIElement("hidden", "Id"));
            config.Elements.Add(new FormUIElement("text", "Description", "s12 l9", "Descrição", "", false, true, 0, 20));
            config.Elements.Add(new FormUIElement("select", "Type", "s12 l3", "Tipo", "", true, true)
            {
                Options = new List<FormUIElementBase>
                {
                    new FormUIElementBase("Selecione..."),
                    new FormUIElementBase("Entrada", "1"),
                    new FormUIElementBase("Saída", "2"),
                }
            });

            return GenerateContentResult(config);
        }
        #endregion
        */
        #region Dashboards
        public ContentResult EmConstrucao()
        {
            ContentUI config = new ContentUI
            {
                Header = new HtmlUIHeader()
                {
                    Title = "Opção indisponível",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = ""
            };


            config.Content.Add(new
            {
                type = "form",
                value = new FormUI()
                {
                    Elements = new List<FormUIElement>()
                    {
                        new FormUIElement()
                        {
                            Type = "labelset",
                            Class = "col s12",
                            Id = "underconstruction",
                            Name = "underconstruction",
                            Label = "O recurso está em desenvolvimento."
                        }
                    },
                    Class = "col s12",
                }
            });

            return GenerateContentResult(config);

        }
        public ContentResult Home()
        {
            ContentUI config = new ContentUI
            {
                Header = new HtmlUIHeader()
                {
                    Title = "Estamos chegando!",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = ""
            };


            config.Content.Add(new
            {
                type = "form",
                value = new FormUI()
                {
                    Class = "col s12",
                }
            });

            return GenerateContentResult(config);

        }
        #endregion
    }
}