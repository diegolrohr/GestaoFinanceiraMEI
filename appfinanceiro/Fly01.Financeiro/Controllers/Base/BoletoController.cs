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
using Fly01.Core.Presentation;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation;
using System.Dynamic;

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

        private List<CnabVM> GetCnab(string filter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", filter);
            queryString.AddParam("$expand", "contaReceber($expand=pessoa),contaBancariaCedente($expand=banco)");

            var boletos = RestHelper.ExecuteGetRequest<ResultBase<CnabVM>>("cnab", queryString);

            return boletos.Data;
        }

        [HttpPost]
        public ActionResult GeraArquivoRemessa(List<Guid> ids)
        {
            try
            {
                var dictBoletos = new List<KeyValuePair<Guid?, Boleto2Net.Boleto>>();
                var listaArquivosGerados = new List<string>();
                var queryString = AppDefaults.GetQueryStringDefault();
                queryString.AddParam("$filter", $"emiteBoleto eq true");

                var listaBancos = RestHelper.ExecuteGetRequest<ResultBase<BancoVM>>(AppDefaults.GetResourceName(typeof(BancoVM)), queryString).Data;

                foreach (var item in GetCnab(ids))
                {
                    dictBoletos.Add(new KeyValuePair<Guid?, Boleto2Net.Boleto>(
                        item.ContaBancariaCedenteId,
                        GeraBoleto(GetBoletoBancario(item.ContaReceberId, item.ContaBancariaCedenteId)).Boleto));
                }

                foreach (var item in dictBoletos.GroupBy(x => x.Key).OrderByDescending(x => x.Key).ToList())
                {
                    var lstBoletos = dictBoletos.Where(x => x.Key == item.Key).Select(x => x.Value).ToList();
                    var banco = lstBoletos.FirstOrDefault().Banco;
                    var codigoBanco = banco.Codigo.ToString("000");
                    var total = (double)lstBoletos.Sum(x => x.ValorTitulo);
                    var boletos = new Boleto2Net.Boletos()
                    {
                        Banco = banco
                    };
                    boletos.AddRange(lstBoletos);

                    var arquivoRemessa = new Boleto2Net.ArquivoRemessa(banco, Boleto2Net.TipoArquivo.CNAB240, 1); // tem que avaliar os dados passados(tipoArquivo, NumeroArquivo)
                    var nomeArquivo = $"{banco.Codigo}-{DateTime.Now.ToString("ddMMyyyyHHmmss")}";
                    Session[nomeArquivo] = arquivoRemessa.GerarArquivoRemessa(boletos);

                    if (Session[nomeArquivo] != null)
                    {
                        var dadosBanco = listaBancos.FirstOrDefault(x => x.Codigo.Contains(codigoBanco));
                        if (dadosBanco != null)
                        {
                            SalvaArquivoRemessa(ids, dadosBanco.Id, nomeArquivo, lstBoletos.Count(), total);
                            listaArquivosGerados.Add(nomeArquivo);
                        }
                    }
                }

                return Json(new { FileGuid = listaArquivosGerados });
            }
            catch (Exception e)
            {
                return JsonResponseStatus.GetFailure($"Ocorreu um erro: {e.Message}");
            }
        }

        [HttpGet]
        public JsonResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId, bool reimprimeBoleto = false)
        {
            try
            {
                var boletoBancario = GetBoletoBancario(contaReceberId, contaBancariaId);
             
                var boletoImpresso = GeraBoleto(boletoBancario);
               
                var html = new StringBuilder();
                html.Append($"<div style=\"margin: 15px;\">{boletoImpresso.MontaHtml()}</div>");

                boletoImpresso.Boleto.NossoNumero = boletoBancario.NossoNumero.ToString();
                SalvaBoleto(boletoImpresso, contaReceberId, contaBancariaId, reimprimeBoleto);

                return Json(new { success = true, message = html.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Ocorreu um erro ao gerar boleto: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        private Boleto2Net.BoletoBancario GeraBoleto(BoletoVM boleto)
        {
            var mensagemBoleto = "";
            var proxy = new Boleto2Net.Boleto2NetProxy();
            var cedente = boleto.Cedente;
            var contaCedente = cedente.ContaBancariaCedente;
            var sacado = boleto.Sacado;
            var carteira = new CarteiraVM(boleto.Cedente.ContaBancariaCedente.CodigoBanco);

            if (!proxy.SetupCobranca(cedente.CNPJ, cedente.RazaoSocial, cedente.Endereco, cedente.EnderecoNumero, cedente.EnderecoComplemento, cedente.EnderecoBairro,
                cedente.EnderecoCidade, cedente.EnderecoUF, cedente.EnderecoCEP, cedente.Observacoes, contaCedente.CodigoBanco, contaCedente.Agencia, contaCedente.DigitoAgencia,
                "1", contaCedente.Conta, contaCedente.DigitoConta, cedente.CodigoCedente, cedente.CodigoDV, "", carteira.CarteiraPadrao, carteira.VariacaoCarteira,
                (int)Boleto2Net.TipoCarteira.CarteiraCobrancaSimples, (int)Boleto2Net.TipoFormaCadastramento.ComRegistro, (int)Boleto2Net.TipoImpressaoBoleto.Empresa, (int)Boleto2Net.TipoDocumento.Tradicional, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.NovoBoleto(ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirSacado(cedente.CNPJ, sacado.Nome, sacado.Endereco, sacado.EnderecoNumero, sacado.EnderecoComplemento, sacado.EnderecoBairro, sacado.EnderecoCidade,
                sacado.EnderecoUF, sacado.EnderecoCEP, sacado.Observacoes, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirBoleto(Boleto2Net.TipoEspecieDocumento.DM.ToString(), boleto.NumeroDocumento, FormataNossoNumero( cedente,boleto.NossoNumero), boleto.DataEmissao,
                DateTime.Now, boleto.DataVencimento, boleto.ValorPrevisto, boleto.NumeroDocumento, "N", ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirMulta(boleto.DataVencimento, boleto.ValorMulta, 2, ref mensagemBoleto)) throw new Exception(mensagemBoleto);
            if (!proxy.DefinirJuros(boleto.DataVencimento.AddDays(1), boleto.ValorJuros, 3, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (boleto.DataDesconto.HasValue)
                if (!proxy.DefinirDesconto(boleto.DataDesconto.Value, boleto.ValorDesconto.Value, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirInstrucoes(boleto.InstrucoesCaixa, "", "", "", "", "", "", "", ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            proxy.FecharBoleto(ref mensagemBoleto);

            var result = new Boleto2Net.BoletoBancario
            {
                Boleto = proxy.boleto,
                OcultarInstrucoes = false,
                MostrarComprovanteEntrega = true,
                MostrarEnderecoCedente = true
            };

            if (result == null)
                throw new Exception("O boleto não pôde ser gerado.");

            return result;
        }

        private void SalvaBoleto(Boleto2Net.BoletoBancario boletoImpresso, Guid contaReceberId, Guid contaBancariaId, bool reimprimeBoleto)
        {
            var cnab = new CnabVM()
            {
                Status = StatusCnab.BoletoGerado.ToString(),
                DataEmissao = boletoImpresso.Boleto.DataEmissao,
                DataVencimento = boletoImpresso.Boleto.DataVencimento,
                NossoNumero = Convert.ToInt32(boletoImpresso.Boleto.NossoNumero),
                DataDesconto = boletoImpresso.Boleto.DataDesconto,
                ValorDesconto = (double)boletoImpresso.Boleto.ValorDesconto,
                ContaBancariaCedenteId = contaBancariaId,
                ContaReceberId = contaReceberId,
                ValorBoleto = (double)boletoImpresso.Boleto.ValorTitulo, 
            };            
            if (!reimprimeBoleto)
                RestHelper.ExecutePostRequest("cnab", JsonConvert.SerializeObject(cnab, JsonSerializerSetting.Default));
            else {
                var cnabToEdit = GetCnab($"contaReceberId eq {contaReceberId}");
                var resourceNamePut = $"cnab/{cnabToEdit.FirstOrDefault().Id}";
                cnab.Id = cnabToEdit.FirstOrDefault().Id;
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(cnab, JsonSerializerSetting.Edit));
            }
        }

        private void SalvaArquivoRemessa(List<Guid> ids, Guid bancoId, string nomeArquivo, int qtdBoletos, double valorBoletos)
        {
            var arquivoRemessa = new ArquivoRemessaVM()
            {
                Descricao = $"{nomeArquivo}.REM",
                TotalBoletos = qtdBoletos,
                StatusArquivoRemessa = StatusArquivoRemessa.AguardandoRetorno.ToString(),
                ValorTotal = valorBoletos,
                BancoId = bancoId
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

            var result = RestHelper.ExecuteGetRequest<BoletoVM>("boleto/imprimeBoleto", queryString);

            if (result == null)
                throw new Exception("Uma ou mais informações obrigatórias não foram preenchidas.");

            return result;
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

            var response = GetCnab($"arquivoRemessaId eq {Id}");
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


        [HttpGet]
        public JsonResult ValidaBoletoJaGeradoParaOutroBanco(Guid contaReceberId, Guid contaBancariaId)
        {
            bool boletoGerado = false; 

            var cnab = GetCnab($"contaReceberId eq {contaReceberId}");
            var bancoId = GetIdBanco($"id eq {contaBancariaId}");

            if (cnab.Count > 0 && cnab.Any(x => x.ContaBancariaCedente.BancoId != bancoId))
                boletoGerado = true;

            return Json(new { success = boletoGerado }, JsonRequestBehavior.AllowGet);
        }

        private Guid? GetIdBanco(string filter)
        {
            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", filter);
           
            return RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>("contaBancaria", queryString).Data.FirstOrDefault().BancoId;
        }

        public string FormataNossoNumero(CedenteVM cedente, int nossoNumero)
        {
            TipoCodigoBanco tipo = (TipoCodigoBanco)Enum.ToObject(typeof(TipoCodigoBanco), cedente.ContaBancariaCedente.CodigoBanco);
            switch (tipo)
            {
                case TipoCodigoBanco.BancoBrasil:
                    return $"{cedente.CodigoCedente}{nossoNumero.ToString().PadLeft(10, '0')}";
            }
            return nossoNumero.ToString();
        }
    }
}