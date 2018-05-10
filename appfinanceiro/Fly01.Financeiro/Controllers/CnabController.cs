﻿using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using System.Text;
using Fly01.Core.Presentation.JQueryDataTable;
using System.Linq;
using System.Net.Http;

namespace Fly01.Financeiro.Controllers
{
    public class CnabController : BaseController<CnabVM>
    {
        public override Func<CnabVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                numeroBoleto = x.NumeroBoleto,
                valorBoleto = x.ValorBoleto,
                valorDesconto = x.ValorDesconto,
                status = EnumHelper.SubtitleDataAnotation(typeof(StatusCnab), x.Status).Value,
                dataEmissao = x.DataEmissao,
                dataVencimento = x.DataVencimento
            };
        }

        //public ContentResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId, DateTime dataDesconto, double valorDesconto)
        [HttpGet]
        public HttpResponseMessage ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            var boletoImpresso = GetBoletoBancario(contaReceberId, contaBancariaId);

            var html = new StringBuilder();
            html.Append("<div style=\"page-break-after: always;\">");
            html.Append(boletoImpresso.MontaHtml());
            html.Append("</div>");

            if (!string.IsNullOrEmpty(html.ToString()))
                RestHelper.ExecutePostRequest("cnab", JsonConvert.SerializeObject(boletoImpresso));

            return null;
        }

        private Boleto2Net.BoletoBancario GetBoletoBancario(Guid contaReceberId, Guid contaBancariaId)
        {
            var mensagemBoleto = "";
            var queryString = new Dictionary<string, string>
            {
                { "contaReceberId", contaReceberId.ToString() }
                , { "contaBancariaId", contaBancariaId.ToString() }
                //, { "dataDesconto", DateTime.Now.ToString("yyyy-MM-dd") }
                //, { "valorDesconto", "1" }
            };

            var boleto = RestHelper.ExecuteGetRequest<BoletoVM>("cnab/imprimeBoleto", queryString);
            var proxy = new Boleto2Net.Boleto2NetProxy();
            var cedente = boleto.Cedente;
            var contaCedente = cedente.ContaBancariaCedente;
            var sacado = boleto.Sacado;

            if (!proxy.SetupCobranca(cedente.CNPJ, cedente.RazaoSocial, cedente.Endereco, cedente.EnderecoNumero, cedente.EnderecoComplemento,
                cedente.EnderecoBairro, cedente.EnderecoCidade, cedente.EnderecoUF, cedente.EnderecoCEP, cedente.Observacoes, contaCedente.CodigoBanco,
                contaCedente.Agencia, contaCedente.DigitoAgencia, "", contaCedente.Conta, contaCedente.DigitoConta, cedente.CodigoCedente, "",
                "", "11", "019", (int)Boleto2Net.TipoCarteira.CarteiraCobrancaSimples, (int)Boleto2Net.TipoFormaCadastramento.ComRegistro,
                (int)Boleto2Net.TipoImpressaoBoleto.Empresa, 1, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.NovoBoleto(ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirSacado(sacado.CNPJ, sacado.Nome, sacado.Endereco, sacado.EnderecoNumero, sacado.EnderecoComplemento,
                sacado.EnderecoBairro, sacado.EnderecoCidade, sacado.EnderecoUF, sacado.EnderecoCEP, sacado.Observacoes, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirBoleto(boleto.EspecieMoeda, boleto.NumeroDocumento, boleto.NossoNumero, boleto.DataEmissao, DateTime.Now, boleto.DataVencimento,
                boleto.ValorPrevisto, boleto.NumeroDocumento, "N", ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirMulta(boleto.DataVencimento, boleto.ValorMulta, 2, ref mensagemBoleto)) throw new Exception(mensagemBoleto);
            if (!proxy.DefinirJuros(boleto.DataVencimento.AddDays(1), boleto.ValorJuros, 3, ref mensagemBoleto)) throw new Exception(mensagemBoleto);
            if (!proxy.DefinirDesconto(DateTime.Now, 0, ref mensagemBoleto)) throw new Exception(mensagemBoleto);
            if (!proxy.DefinirInstrucoes(boleto.InstrucoesCaixa, "", "", "", "", "", "", "", ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            proxy.FecharBoleto(ref mensagemBoleto);

            return new Boleto2Net.BoletoBancario
            {
                Boleto = proxy.boleto,
                OcultarInstrucoes = false,
                MostrarComprovanteEntrega = true,
                MostrarEnderecoCedente = true
            };
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "Cnab"),
                    WithParams = Url.Action("Edit", "Cnab"),
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados para emissão de boleto",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var configCnab = new FormUI
            {
                Id = "fly01frmBoleto",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            configCnab.Elements.Add(new InputHiddenUI { Id = "id" });
            configCnab.Elements.Add(new AutocompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m6 l6",
                Label = "Banco cedente",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBancoEmiteBoleto", "AutoComplete") + "?emiteBoleto=true",
                LabelId = "bancoNome"
            });
            configCnab.Elements.Add(new AutocompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 m6 l6",
                Label = "Cliente",
                Required = true,
                DataUrl = @Url.Action("Cliente", "AutoComplete"),
                LabelId = "pessoaNome",
                DataUrlPost = Url.Action("PostCliente", "Cliente")
            });

            configCnab.Elements.Add(new ButtonUI
            {
                Id = "btnListarContas",
                Class = "col s4 m4",
                Value = "Listar contas",
                DomEvents = new List<DomEventUI>() {
                    new DomEventUI() { DomEvent = "click", Function = "fnShowListCnab" }
                }
            });

            #region CnabItem
            var dtConfig = new DataTableUI
            {
                Id = "dtCnabItem",
                UrlGridLoad = Url.Action("GridLoadContaCnabItem", "CnabItem"),
                UrlFunctions = Url.Action("Functions", "CnabItem", null, Request.Url.Scheme) + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "pessoaId", Required = true, Value = "PessoaId" }
                }
            };
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Nº", Priority = 1, Type = "number" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Priority = 3, Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Priority = 4, Type = "currency" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricaoParcela", DisplayName = "Parcela", Priority = 5 });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Imprimir boleto", Priority = 6, Searchable = false, Orderable = false, RenderFn = "fnImprimirBoleto", Width = "25%" });
            #endregion

            cfg.Content.Add(configCnab);
            cfg.Content.Add(dtConfig);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "Cnab") },
                Header = new HtmlUIHeader()
                {
                    Title = "Boletos",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Gerar boleto", OnClickFn = "fnNovo" },
                        new HtmlUIButton { Id = "new", Label = "GERAR ARQ. REMESSA", OnClickFn = "fnGerarArquivoRemessa" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var configdt = new DataTableUI()
            {
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "multi" }
                }
            };
            
            configdt.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Nº", Priority = 1, Type = "number" });
            configdt.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Pessoa", Priority = 2 });
            configdt.Columns.Add(new DataTableUIColumn { DataField = "valorBoleto", DisplayName = "Valor", Priority = 3 });
            configdt.Columns.Add(new DataTableUIColumn { DataField = "dataEmissao", DisplayName = "Data emissão", Priority = 4, Type = "date" });
            configdt.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Data vencimento", Priority = 5, Type = "date" });
            configdt.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusArquivoRemessa",
                DisplayName = "Status",
                Priority = 7,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusArquivoRemessa)))
            });

            cfg.Content.Add(configdt);
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public JsonResult LoadGridBoletos(Guid IdArquivo)
        {
            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
            var pageNo = param.Start > 0 ? (param.Start / 10) + 1 : 1;

            var response = GetContasReceber(IdArquivo, pageNo);

            return Json(new
            {
                recordsTotal = response.Paging.TotalRecordCount,
                recordsFiltered = response.Paging.TotalRecordCount,
                data = response.Data.Select(item => new
                {
                    numero = item.NumeroBoleto,
                    pessoa_nome = item.ContaReceber.Pessoa.Nome,
                    valorBoleto = item.ValorBoleto,
                    dataEmissao = item.DataEmissao.ToString("dd/MM/yyyy"),
                    dataVencimento = item.DataVencimento.ToString("dd/MM/yyyy"),
                    statusArquivoRemessa = item.Status
                })

            },JsonRequestBehavior.AllowGet);
        }

        private PagedResult<CnabVM> GetContasReceber(Guid idArquivo, int pageNo)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "arquivoRemessaId", idArquivo.ToString()},
                { "pageNo", pageNo.ToString() },
                { "pageSize", "10"}
            };
            
            return RestHelper.ExecuteGetRequest<PagedResult<CnabVM>>("canb/contasReceberarquivo", queryString);
        }
    }
}