using System;
using Fly01.Core;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Newtonsoft.Json;
using Fly01.Core.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.Core.ViewModels;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class BoletoController<TEntity> : BaseController<TEntity> where TEntity : DomainBaseVM
    {
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

        private List<CnabVM> GetCnab(Guid? idArquivo)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"arquivoRemessaId eq {idArquivo}");
            queryString.AddParam("$expand", "contaReceber , contaReceber($expand=pessoa)");

            var boletos = RestHelper.ExecuteGetRequest<ResultBase<CnabVM>>("cnab", queryString);

            return boletos.Data;
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
                var listaBancosBoletos = new List<Boleto2Net.IBanco>();
                var listaArquivosGerados = new List<string>();

                foreach (var item in boletosCnab)
                {
                    var boleto = GeraBoleto(GetBoletoBancario(item.ContaReceberId, item.ContaBancariaCedenteId)).Boleto;

                    boletos.Add(boleto);
                    boletos.Banco = boleto.Banco;
                    qtdBoletos += 1;
                    valorBoletos += (double)boleto.ValorTitulo;
                    listaBancosBoletos.Add(boleto.Banco);
                    //get bancoId
                }

                listaBancosBoletos.ForEach(banco =>
                {
                    var arquivoRemessa = new Boleto2Net.ArquivoRemessa(banco, Boleto2Net.TipoArquivo.CNAB240, 1); // tem que avaliar os dados passados(tipoArquivo, NumeroArquivo)
                    var nomeArquivo = $"{banco.Nome}-{boletos.Banco.Cedente.CPFCNPJ}-{DateTime.Now.ToString("yyyyMMddHHmmss")}";

                    Session[nomeArquivo] = arquivoRemessa.GerarArquivoRemessa(boletos);

                    if (Session[nomeArquivo] != null)
                        SalvaArquivoRemessa(ids, banco.Codigo, nomeArquivo, qtdBoletos, valorBoletos);

                    listaArquivosGerados.Add(nomeArquivo);
                });


                //corrigir retorno do método
                return Json(new { FileGuid = listaArquivosGerados });
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.ToString());
            }
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
                Status = StatusCnab.BoletoGerado.ToString(),
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

        private void SalvaArquivoRemessa(List<Guid> ids, int codigoBanco, string nomeArquivo, int qtdBoletos, double valorBoletos)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"codigo eq {codigoBanco}");

            var arquivoRemessa = new ArquivoRemessaVM()
            {
                Descricao = $"{nomeArquivo}.REM",
                TotalBoletos = qtdBoletos,
                StatusArquivoRemessa = StatusArquivoRemessa.AguardandoRetorno.ToString(),
                ValorTotal = valorBoletos,
                BancoId = RestHelper.ExecuteGetRequest<BancoVM>("banco", queryString).Id
            };

            var result = RestHelper.ExecutePostRequest<ArquivoRemessaVM>("arquivoremessa", JsonConvert.SerializeObject(arquivoRemessa, JsonSerializerSetting.Default));
            var status = ((int)StatusCnab.AguardandoRetorno).ToString();

            ids.ForEach(x =>
            {
                var resource = $"cnab/{x}";
                RestHelper.ExecutePutRequest(resource, JsonConvert.SerializeObject(new
                {
                    arquivoRemessaId = result.Id,
                    status = status
                }));
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

        private List<CnabVM> GetContasReceber(Guid? idArquivo, int pageNo)
        {
            var queryString = new Dictionary<string, string>
            {
                { "arquivoRemessaId", idArquivo.ToString()},
                { "pageNo", pageNo.ToString() },
                { "pageSize", "10"}
            };

            var boletos = RestHelper.ExecuteGetRequest<PagedResult<CnabVM>>("cnab", queryString);

            return boletos.Data.Where(x => x.ArquivoRemessaId == idArquivo).ToList();
        }

        public JsonResult LoadGridBoletos()
        {
            var Id = Guid.Parse(Request.UrlReferrer.Segments.Last());

            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
            var pageNo = param.Start > 0 ? (param.Start / 10) + 1 : 1;

            var response = GetCnab(Id);
            return Json(new
            {
                recordsTotal = response.Count,
                recordsFiltered = response.Count,
                data = response.Select(item => new
                {
                    nossoNumero = item.NossoNumero,
                    pessoa_nome = item.ContaReceber?.Pessoa?.Nome,
                    valorBoleto = item.ValorBoleto.ToString("C", AppDefaults.CultureInfoDefault),
                    dataEmissao = item.DataEmissao.ToString("dd/MM/yyyy"),
                    dataVencimento = item.DataVencimento.ToString("dd/MM/yyyy"),
                    status = item.Status,
                    statusCssClass = EnumHelper.GetCSS(typeof(StatusCnab), item.Status),
                    statusDescription = EnumHelper.GetDescription(typeof(StatusCnab), item.Status),
                    statusTooltip = EnumHelper.GetTooltipHint(typeof(StatusCnab), item.Status),
                })

            }, JsonRequestBehavior.AllowGet);
        }
    }
}