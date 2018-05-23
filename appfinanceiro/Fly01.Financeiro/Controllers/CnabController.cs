﻿using System;
using System.Web.Mvc;
using Newtonsoft.Json;
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
using System.Net.Mime;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Controllers
{
    public class CnabController : BaseController<CnabVM>
    {
        public CnabController()
        {
            ExpandProperties = "contaReceber($expand=pessoa($select=nome))";
        }

        public override Func<CnabVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                contaReceberId = x.ContaReceberId,
                contaBancariaId = x.ContaBancariaCedenteId,
                pessoa_nome = x.ContaReceber.Pessoa.Nome,
                nossoNumero = x.NossoNumero,
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                valorBoleto = x.ValorBoleto.ToString("C", AppDefaults.CultureInfoDefault),
                valorDesconto = x.ValorDesconto,
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(StatusCnab), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusCnab), x.Status),
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy")
            };
        }

        [HttpGet]
        public JsonResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            try
            {
                var boletoImpresso = GeraBoleto(GetBoletoBancario(contaReceberId, contaBancariaId));
                if (boletoImpresso == null) throw new Exception("O boleto não pôde ser gerado.");

                var html = new StringBuilder();
                html.Append($"<div style=\"margin: 15px;\">{boletoImpresso.MontaHtml()}</div>");

                SalvaBoleto(boletoImpresso, contaReceberId, contaBancariaId);

                return Json(new { success = true, message = html.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ocorreu um erro ao gerar boleto: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        private Boleto2Net.BoletoBancario GeraBoleto(BoletoVM boleto)
        {
            var mensagemBoleto = "";
            var proxy = new Boleto2Net.Boleto2NetProxy();
            var cedente = boleto.Cedente;
            var contaCedente = cedente.ContaBancariaCedente;
            var sacado = boleto.Sacado;

            if (!proxy.SetupCobranca(cedente.CNPJ, cedente.RazaoSocial, cedente.Endereco, cedente.EnderecoNumero, cedente.EnderecoComplemento, cedente.EnderecoBairro,
                cedente.EnderecoCidade, cedente.EnderecoUF, cedente.EnderecoCEP, cedente.Observacoes, contaCedente.CodigoBanco, contaCedente.Agencia, contaCedente.DigitoAgencia,
                "1", contaCedente.Conta, contaCedente.DigitoConta, cedente.CodigoCedente, "", "", "11", "019", (int)Boleto2Net.TipoCarteira.CarteiraCobrancaSimples,
                (int)Boleto2Net.TipoFormaCadastramento.ComRegistro, (int)Boleto2Net.TipoImpressaoBoleto.Empresa, (int)Boleto2Net.TipoDocumento.Tradicional, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.NovoBoleto(ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirSacado(cedente.CNPJ, sacado.Nome, sacado.Endereco, sacado.EnderecoNumero, sacado.EnderecoComplemento, sacado.EnderecoBairro, sacado.EnderecoCidade,
                sacado.EnderecoUF, sacado.EnderecoCEP, sacado.Observacoes, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirBoleto(Boleto2Net.TipoEspecieDocumento.DM.ToString(), boleto.NumeroDocumento, boleto.NossoNumero, boleto.DataEmissao,
                DateTime.Now, boleto.DataVencimento, boleto.ValorPrevisto, boleto.NumeroDocumento, "N", ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirMulta(boleto.DataVencimento, boleto.ValorMulta, 2, ref mensagemBoleto)) throw new Exception(mensagemBoleto);
            if (!proxy.DefinirJuros(boleto.DataVencimento.AddDays(1), boleto.ValorJuros, 3, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (boleto.DataDesconto.HasValue)
                if (!proxy.DefinirDesconto(boleto.DataDesconto.Value, boleto.ValorDesconto.Value, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

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

        private void SalvaBoleto(Boleto2Net.BoletoBancario boletoImpresso, Guid contaReceberId, Guid contaBancariaId)
        {
            var cnab = new CnabVM()
            {
                Status = StatusCnab.EmAberto.ToString(),
                DataEmissao = boletoImpresso.Boleto.DataEmissao,
                DataVencimento = boletoImpresso.Boleto.DataVencimento,
                NossoNumero = boletoImpresso.Boleto.NossoNumero,
                DataDesconto = boletoImpresso.Boleto.DataDesconto,
                ValorDesconto = (double)boletoImpresso.Boleto.ValorDesconto,
                ContaBancariaCedenteId = contaBancariaId,
                ContaReceberId = contaReceberId,
                ValorBoleto = (double)boletoImpresso.Boleto.ValorTitulo
            };

            RestHelper.ExecutePostRequest("cnab", JsonConvert.SerializeObject(cnab, JsonSerializerSetting.Default));
        }

        private void SalvaArquivoRemessa(List<Guid> ids, string nomeArquivo, int qtdBoletos, double valorBoletos)
        {
            var arquivoRemessa = new ArquivoRemessaVM()
            {
                Descricao = $"{nomeArquivo}.REM",
                TotalBoletos = qtdBoletos,
                StatusArquivoRemessa = StatusArquivoRemessa.Exportado.ToString(),
                ValorTotal = valorBoletos
            };

            var result = RestHelper.ExecutePostRequest<ArquivoRemessaVM>("arquivoremessa", JsonConvert.SerializeObject(arquivoRemessa, JsonSerializerSetting.Default));

            ids.ForEach(x =>
            {
                var resource = $"cnab/{x}";
                RestHelper.ExecutePutRequest(resource, JsonConvert.SerializeObject(new { arquivoRemessaId = result.Id }));
            });
        }

        private BoletoVM GetBoletoBancario(Guid? contaReceberId, Guid? contaBancariaId)
        {
            var queryString = new Dictionary<string, string>
            {
                { "contaReceberId", contaReceberId.ToString() }
                , { "contaBancariaId", contaBancariaId.ToString() }
            };

            return RestHelper.ExecuteGetRequest<BoletoVM>("boleto/imprimeBoleto", queryString);
        }

        private List<CnabVM> GetCnab(List<Guid> idsBoletos)
        {
            var listaCnab = new List<CnabVM>();

            foreach (var item in idsBoletos)
            {
                var queryString = new Dictionary<string, string>
                {
                    { "Id", item.ToString()},
                    { "pageSize", "10"}
                };

                var restResponse = RestHelper.ExecuteGetRequest<CnabVM>("cnab", queryString);

                if (restResponse != null)
                    listaCnab.Add(restResponse);
            }

            return listaCnab;
        }

        [HttpPost]
        public ActionResult GeraArquivoRemessa(List<Guid> ids)
        {
            try
            {
                var boletosCnab = GetCnab(ids);
                var boletos = new Boleto2Net.Boletos();
                var qtdBoletos = 0;
                var valorBoletos = 0.0;

                foreach (var item in boletosCnab)
                {
                    var boleto = GeraBoleto(GetBoletoBancario(item.ContaReceberId, item.ContaBancariaCedenteId)).Boleto;

                    boletos.Add(boleto);
                    boletos.Banco = boleto.Banco;
                    qtdBoletos += 1;
                    valorBoletos += (double)boleto.ValorTitulo;
                }

                var arquivoRemessa = new Boleto2Net.ArquivoRemessa(boletos.Banco, Boleto2Net.TipoArquivo.CNAB240, 1); // tem que avaliar os dados passados(tipoArquivo, NumeroArquivo)

                var nomeArquivo = $"{boletos.Banco.Cedente.CPFCNPJ}-{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                Session[nomeArquivo] = arquivoRemessa.GerarArquivoRemessa(boletos);

                if (Session[nomeArquivo] != null)
                    SalvaArquivoRemessa(ids, nomeArquivo, qtdBoletos, valorBoletos);

                return Json(new { FileGuid = nomeArquivo });
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.ToString());
            }
        }

        [HttpGet]
        public ActionResult Download(string fileName)
        {
            if (Session[fileName] != null)
            {
                var arquivoDownload = File((byte[])Session[fileName], MediaTypeNames.Application.Octet, fileName + ".REM");
                Session[fileName] = null;

                return arquivoDownload;
            }

            return null;
        }

        public override ContentResult Form()
        {
            #region Headers

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
                        new HtmlUIButton { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar" }
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
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            configCnab.Elements.Add(new InputHiddenUI { Id = "id" });
            configCnab.Elements.Add(new AutoCompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m6 l6",
                Label = "Banco cedente",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBancoEmiteBoleto", "AutoComplete") + "?emiteBoleto=true",
                LabelId = "bancoNome"
            });
            configCnab.Elements.Add(new AutoCompleteUI
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
            cfg.Content.Add(configCnab);

            #endregion

            #region CnabItem
            var dtConfig = new DataTableUI
            {
                Id = "dtCnabItem",
                UrlGridLoad = Url.Action("GridLoadContaCnabItem", "CnabItem"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "pessoaId", Required = true, Value = "PessoaId" }
                }
            };
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Nº Conta", Priority = 2, Type = "number" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1, Width = "30%" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Priority = 3, Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Priority = 4, Type = "currency" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricaoParcela", DisplayName = "Parcela", Priority = 5 });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Imprimir boleto", Priority = 6, Searchable = false, Orderable = false, RenderFn = "fnImprimirBoleto", Width = "25%" });
            #endregion

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
                        new HtmlUIButton { Id = "btnGerarBoleto", Label = "GERAR BOLETO", OnClickFn = "fnNovo" },
                        new HtmlUIButton { Id = "btnGerarArqRemessa", Label = "GERAR ARQ. REMESSA", OnClickFn = "fnGerarArquivoRemessa" }
                    }
                },
                Functions = new List<string> { "fnFormReadyCnab" },
                UrlFunctions = Url.Action("Functions") + "?fns=",
            };

            var dtConfig = new DataTableUI()
            {
                Id = "dtBoletos",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnFormReadyCnab" },
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "multi" }
                }
            };
            dtConfig.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusCnab))),
                Priority = 6,
                Width = "12%",
                RenderFn = "fnRenderEnum"
            });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "nossoNumero", DisplayName = "Nº boleto", Priority = 6 });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", Priority = 3, DisplayName = "Cliente" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", Priority = 4, DisplayName = "Data Vencimento", Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorBoleto", Priority = 5, DisplayName = "Valor" });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Imprimir", Priority = 2, Searchable = false, Orderable = false, RenderFn = "fnImprimirBoleto" });

            cfg.Content.Add(dtConfig);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }
    }
}