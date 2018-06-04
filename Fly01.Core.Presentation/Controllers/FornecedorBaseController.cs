﻿using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class FornecedorBaseController<T> : BaseController<T> where T : PessoaVM
    {
        public FornecedorBaseController()
        {
            ExpandProperties = "estado($select=id,nome,sigla),cidade($select=id,nome,estadoId)";
        }

        public override JsonResult Create(T entityVM)
        {
            NormarlizarEntidade(ref entityVM);
            return base.Create(entityVM);
        }

        [HttpPost]
        public override JsonResult Edit(T entityVM)
        {
            NormarlizarEntidade(ref entityVM);
            return base.Edit(entityVM);
        }

        protected void NormarlizarEntidade(ref T entityVM)
        {
            const string regexSomenteDigitos = @"[^\d]";

            entityVM.Fornecedor = true;
            entityVM.CPFCNPJ = Regex.Replace(entityVM.CPFCNPJ ?? "", regexSomenteDigitos, "");
            entityVM.TipoDocumento = GetTipoDocumento(entityVM.CPFCNPJ ?? "");
            entityVM.Celular = Regex.Replace(entityVM.Celular ?? "", regexSomenteDigitos, "");
            entityVM.Telefone = Regex.Replace(entityVM.Telefone ?? "", regexSomenteDigitos, "");
            entityVM.CEP = Regex.Replace(entityVM.CEP ?? "", regexSomenteDigitos, "");
            entityVM.InscricaoEstadual = Regex.Replace(entityVM.InscricaoEstadual ?? "", regexSomenteDigitos, "");

            if (string.IsNullOrEmpty(entityVM.TipoIndicacaoInscricaoEstadual))
                entityVM.TipoIndicacaoInscricaoEstadual = "ContribuinteICMS";
        }

        private string GetTipoDocumento(string documento)
        {
            if (documento.Length <= 11)
                return "F";
            if (documento.Length > 11)
                return "J";

            return null;
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var tempExpand = ExpandProperties;
            ExpandProperties = string.Empty;
            var customFilters = base.GetQueryStringDefaultGridLoad();
            ExpandProperties = tempExpand;

            customFilters.AddParam("$filter", "fornecedor eq true");
            customFilters.AddParam("$select", "id,nome,cpfcnpj,email,telefone,dataInclusao,registroFixo");

            return customFilters;
        }

        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                nome = x.Nome,
                cpfcnpj = FormatterUtils.FormatDocument(x.CPFCNPJ),
                email = x.Email,
                telefone = string.IsNullOrEmpty(x.Telefone) ? "" : Regex.Replace(x.Telefone, x.Telefone.Length == 10 ? @"(\d{2})(\d{4})(\d{4})" : @"(\d{2})(\d{4})(\d{5})", "($1) $2-$3"),
                registroFixo = x.RegistroFixo
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Fornecedores",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" },
                        new HtmlUIButton { Id = "import", Label = "Importar fornecedores", OnClickFn = "fnImportarCadastro" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" });

            config.Columns.Add(new DataTableUIColumn { DataField = "nome", DisplayName = "Fornecedor", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "cpfcnpj", DisplayName = "CPF / CNPJ", Priority = 2, Type = "cpfcnpj" });
            config.Columns.Add(new DataTableUIColumn { DataField = "email", DisplayName = "E-mail", Priority = 3, Type = "email" });
            config.Columns.Add(new DataTableUIColumn { DataField = "telefone", DisplayName = "Telefone", Priority = 4, Type = "tel" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
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
                    Title = "Dados do fornecedor",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = Url.Action("Create"),
                    Edit = Url.Action("Edit"),
                    Get = Url.Action("Json") + "/",
                    List = @Url.Action("List", "Fornecedor")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputCpfcnpjUI { Id = "cpfcnpj", Class = "col s12 l4", Label = "CPF / CNPJ", MaxLength = 18 });
            config.Elements.Add(new InputTextUI { Id = "nome", Class = "col s12 l8", Label = "Razão Social / Nome Completo", Required = true, MaxLength = 100 });

            config.Elements.Add(new InputTextUI { Id = "nomeComercial", Class = "col s12 l6", Label = "Nome Comercial", MaxLength = 100 });
            config.Elements.Add(new InputEmailUI { Id = "email", Class = "col s7 l6", Label = "E-mail", MaxLength = 70 });

            config.Elements.Add(new InputTextUI { Id = "contato", Class = "col s12 l3", Label = "Pessoa de Contato", MaxLength = 45 });
            config.Elements.Add(new InputTelUI { Id = "celular", Class = "col s3 l2", Label = "Celular", MaxLength = 15 });
            config.Elements.Add(new InputTelUI { Id = "telefone", Class = "col s3 l2", Label = "Telefone", MaxLength = 15 });

            config.Elements.Add(new InputCepUI
            {
                Id = "cep",
                Class = "col s4 l2",
                Label = "CEP",
                MaxLength = 9,
                DomEvents = new List<DomEventUI>() { new DomEventUI { DomEvent = "keyup", Function = "fnBuscaCEP" } }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "estadoId",
                Class = "col s6 l3",
                Label = "Estado",
                MaxLength = 35,
                DataUrl = Url.Action("Estado", "AutoComplete"),
                LabelId = "estadoNome",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeEstado" }
                }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "cidadeId",
                Class = "col s6 l3",
                Label = "Cidade (Escolha o estado antes)",
                MaxLength = 35,
                DataUrl = Url.Action("Cidade", "AutoComplete"),
                LabelId = "cidadeNome",
                PreFilter = "estadoId"
            });

            config.Elements.Add(new InputTextUI { Id = "bairro", Class = "col s12 l3", Label = "Bairro", MaxLength = 30 });
            config.Elements.Add(new InputTextUI { Id = "endereco", Class = "col s12 l4", Label = "Endereço", MaxLength = 50 });
            config.Elements.Add(new InputTextUI { Id = "numero", Class = "col s6 l2", Label = "Número", MaxLength = 20 });
            config.Elements.Add(new InputTextUI { Id = "complemento", Class = "col s6 l3", Label = "Complemento", MaxLength = 20 });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoIndicacaoInscricaoEstadual",
                Class = "col s12 l3",
                Label = "Indicação Inscrição Estadual",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoIndicacaoInscricaoEstadual))),
                ConstrainWidth = true
            });
            config.Elements.Add(new InputTextUI { Id = "inscricaoEstadual", Class = "col s6 l3", Label = "Inscrição Estadual", MaxLength = 18 });
            config.Elements.Add(new InputTextUI { Id = "inscricaoMunicipal", Class = "col s6 l3", Label = "Inscrição Municipal", MaxLength = 18 });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 100 });

            config.Elements.Add(new InputCheckboxUI { Id = "cliente", Class = "col s12 l3", Label = "É Cliente" });
            config.Elements.Add(new InputCheckboxUI { Id = "transportadora", Class = "col s12 l3", Label = "É Transportadora" });
            config.Elements.Add(new InputCheckboxUI { Id = "vendedor", Class = "col s12 l3", Label = "É Vendedor" });
            config.Elements.Add(new InputCheckboxUI { Id = "consumidorFinal", Class = "col s12 l3", Label = "É Consumidor Final" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public virtual ActionResult ImportaCadastro()
        {
            return View();
        }

        public ContentResult FormImportacao()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("ImportaCadastro"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Importar fornecedores",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton() { Id = "save", Label = "Importar", OnClickFn = "fnCarregarArquivo", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI()
            {
                Action = new FormUIAction()
                {
                    Create = Url.Action("ImportaCadastro"),
                    Edit = Url.Action("ImportaCadastro"),
                    Get = Url.Action("Json", "Fornecedor") + "/ImportarCadastro",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnImportaCadastroFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputFileUI { Id = "arquivo", Class = "col s12", Label = "Arquivo de importação em lotes (.csv)", Required = true, Accept = ".csv" });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", Readonly = true, MaxLength = 100 });

            cfg.Content.Add(config);

            cfg.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "blue",
                Id = "cardDuvidas",
                Title = "Dúvidas",
                Placeholder = "Se preferir você pode baixar um arquivo modelo de importação",
                Action = new LinkUI
                {
                    Label = "Baixar arquivo modelo"
                }

            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public JsonResult ImportaArquivo(string pConteudo)
        {
            return JsonResponseStatus.GetJson(ImportacaoArquivoHelper.ImportaArquivo("Cadastro de Fornecedores", pConteudo));
        }
    }
}