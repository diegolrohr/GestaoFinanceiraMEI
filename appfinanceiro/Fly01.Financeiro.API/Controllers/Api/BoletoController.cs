using Fly01.Core.API;
using Fly01.Financeiro.BL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Boleto2Net;
using Fly01.Core.ViewModels.Presentation;
using Fly01.Core.ServiceBus;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/boleto")]
    public class BoletoController : ApiBaseController
    {
        [HttpGet]
        [Route("imprimeBoleto")]
        public async Task<IHttpActionResult> ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var boleto = unitOfWork.CnabBL.ImprimeBoleto(contaReceberId, contaBancariaId);

                if (boleto == null) Ok();

                unitOfWork.CnabBL.SalvaBoleto(boleto, contaReceberId, contaBancariaId, false);
                await unitOfWork.Save();

                return Ok($"<div style=\"margin: 15px;\">{boleto.MontaHtmlEmbedded(true, true)}</div>");
            }
        }

        [HttpGet]
        [Route("getListaBoletos")]
        public IHttpActionResult GetListaBoletos(string listIdCnab)
        {
            var dictBoletos = new List<KeyValuePair<Guid?, BoletoBancario>>();
            var dadosArquivoRemessa = new List<DadosArquivoRemessaVM>();

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var cnabBL = unitOfWork.CnabBL;
                var plataformaId = "";

                listIdCnab.Split(',').Select(x => Guid.Parse(x)).ToList().ForEach(id =>
                {
                    var dadosCnab = cnabBL.GetCnab(id);
                    var boletoVM = cnabBL.GetDadosBoleto(dadosCnab.ContaReceberId.Value, dadosCnab.ContaBancariaCedenteId.Value);
                    var boletoBancario = cnabBL.GeraBoleto(boletoVM);

                    dictBoletos.Add(new KeyValuePair<Guid?, BoletoBancario>(dadosCnab.ContaBancariaCedenteId, boletoBancario));

                    if (string.IsNullOrWhiteSpace(plataformaId))
                        plataformaId = dadosCnab.PlataformaId;

                });

                dictBoletos.GroupBy(x => x.Key).OrderByDescending(x => x.Key).ToList().ForEach(item =>
                {
                    var lstBoletoBancario = dictBoletos.Where(x => x.Key == item.Key).Select(x => x.Value);
                    var banco = lstBoletoBancario.FirstOrDefault().Boleto.Banco;
                    var boletos = new Boletos() { Banco = banco };

                    boletos.AddRange(lstBoletoBancario.Select(x => x.Boleto));

                    RpcClient rpc = new RpcClient();
                    var numeroArquivoRemessa = int.Parse(rpc.Call($"plataformaid={plataformaId}"));

                    var arquivoRemessa = new ArquivoRemessa(banco, BoletoBL.GetTipoCnab(banco.Codigo), numeroArquivoRemessa); 
                    dadosArquivoRemessa.Add(new DadosArquivoRemessaVM
                    {
                        ContaBancariaCedenteId = item.Key.Value,
                        CodigoBanco = banco.Codigo,
                        TotalBoletosGerados = boletos.Count(),
                        ValorTotalArquivoRemessa = (double)lstBoletoBancario.Sum(x => x.Boleto.ValorTitulo),
                        ConteudoArquivoRemessa = arquivoRemessa.GerarArquivoRemessa(boletos)
                    });
                });
            }

            return Ok(dadosArquivoRemessa);
        }
    }
}