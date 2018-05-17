using System;
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
using System.IO;
using System.Net.Mime;
using Fly01.Core;

namespace Fly01.Financeiro.Controllers
{
    public class CnabController : BaseController<CnabVM>
    {
        public override Func<CnabVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                contaReceberId = x.ContaReceberId,
                contaBancariaId = x.ContaBancariaCedenteId,
                pessoa_nome = x.ContaReceber.Pessoa.Nome,
                nossoNumero = x.NossoNumero,
                valorBoleto = x.ValorBoleto.ToString("C", AppDefaults.CultureInfoDefault),
                valorDesconto = x.ValorDesconto,
                status = x.Status,
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy"),
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy")
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
                "", contaCedente.Conta, contaCedente.DigitoConta, cedente.CodigoCedente, "", "", "11", "019", (int)Boleto2Net.TipoCarteira.CarteiraCobrancaSimples,
                (int)Boleto2Net.TipoFormaCadastramento.ComRegistro, (int)Boleto2Net.TipoImpressaoBoleto.Empresa, 1, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.NovoBoleto(ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirSacado(sacado.CNPJ, sacado.Nome, sacado.Endereco, sacado.EnderecoNumero, sacado.EnderecoComplemento, sacado.EnderecoBairro, sacado.EnderecoCidade,
                sacado.EnderecoUF, sacado.EnderecoCEP, sacado.Observacoes, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirBoleto(boleto.EspecieMoeda, boleto.NumeroDocumento, boleto.NossoNumero, boleto.DataEmissao, DateTime.Now, boleto.DataVencimento, boleto.ValorPrevisto,
                boleto.NumeroDocumento, "N", ref mensagemBoleto)) throw new Exception(mensagemBoleto);

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

            RestHelper.ExecutePostRequest("cnab", JsonConvert.SerializeObject(cnab));
        }

        private BoletoVM GetBoletoBancario(Guid? contaReceberId, Guid? contaBancariaId)
        {
            var queryString = new Dictionary<string, string>
            {
                { "contaReceberId", contaReceberId.ToString() }
                , { "contaBancariaId", contaBancariaId.ToString() }
            };

            return RestHelper.ExecuteGetRequest<BoletoVM>("cnab/imprimeBoleto", queryString);
        }

        private List<CnabVM> GetCnab(List<Guid> idsBoletos)
        {
            List<CnabVM> listaCnab = new List<CnabVM>();

            foreach (var item in idsBoletos)
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "Id", item.ToString()},
                    { "pageSize", "10"}
                };

                var restResponse = RestHelper.ExecuteGetRequest<CnabVM>("cnab/GetCnab", queryString);

                if (restResponse != null)
                    listaCnab.Add(restResponse);
            }

            return listaCnab;
        }

        [HttpPost]
        public ActionResult GerarArquivoRemessa(List<Guid> ids)
        {
            try
            {
                var boletosCnab = GetCnab(ids);
                var boletos = new Boleto2Net.Boletos();

                foreach (var item in boletosCnab)
                {
                    var boleto = GeraBoleto(GetBoletoBancario(item.ContaReceberId, item.ContaBancariaCedenteId)).Boleto;

                    boletos.Add(boleto);
                    boletos.Banco = boleto.Banco;
                }

                var arquivoRemessa = new Boleto2Net.ArquivoRemessa(boletos.Banco, Boleto2Net.TipoArquivo.CNAB240, 1); // tem que avaliar os dados passados(tipoArquivo, NumeroArquivo)

                var nomeArquivo = Guid.NewGuid().ToString();
                Session[nomeArquivo] = arquivoRemessa.GerarArquivoRemessa(boletos);

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
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Nº Conta", Priority = 2, Type = "number" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1, Width = "30%" });
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

            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "id" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "contaReceberId", Visible = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "contaBancariaId", Visible = false });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "nossoNumero", DisplayName = "Nosso Número", Priority = 1, Type = "number", Width = "10%" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Pessoa", Priority = 2 });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorBoleto", DisplayName = "Valor", Priority = 3 });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataEmissao", DisplayName = "Data emissão", Priority = 4, Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Data vencimento", Priority = 5, Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusArquivoRemessa",
                DisplayName = "Status",
                Priority = 6,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusArquivoRemessa)))
            });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Imprimir boleto", Priority = 7, Searchable = false, Orderable = false, RenderFn = "fnImprimirBoleto" });

            cfg.Content.Add(dtConfig);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }
    }
}