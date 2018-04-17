using System.Linq;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.BL;
using System.Collections.Generic;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Newtonsoft.Json;
using System;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Faturamento.BL
{
    public class MonitorNFBL : PlataformaBaseBL<MonitorNF>
    {
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected NFeBL NFeBL { get; set; }
        protected NFSeBL NFSeBL { get; set; }
        protected NotaFiscalBL NotaFiscalBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }

        public MonitorNFBL(AppDataContextBase context, TotalTributacaoBL totalTributacao, NFeBL nFeBL, NFSeBL nFSeBL, NotaFiscalBL notaFiscalBL, CertificadoDigitalBL certificadoDigitalBL)
            : base(context)
        {
            TotalTributacaoBL = totalTributacao;
            NFeBL = nFeBL;
            NFSeBL = nFSeBL;
            NotaFiscalBL = notaFiscalBL;
            CertificadoDigitalBL = certificadoDigitalBL;
        }

        public void AtualizaStatusTSS(string plataformaUrl)
        {
            var notasFiscaisByPlataforma = (from nf in NotaFiscalBL.AllWithoutPlataformaId.Where(x => (x.Status == StatusNotaFiscal.Transmitida || x.Status == StatusNotaFiscal.EmCancelamento))
                                            where string.IsNullOrEmpty(plataformaUrl) || nf.PlataformaId == plataformaUrl
                                            group nf by nf.PlataformaId into g
                                            select new { plataformaId = g.Key, notaInicial = g.Min(x => x.SefazId), notaFinal = g.Max(x => x.SefazId) });

            var header = new Dictionary<string, string>()
            {
                { "AppUser", AppUser },
                { "PlataformaUrl", string.IsNullOrEmpty(plataformaUrl) ? PlataformaUrl : plataformaUrl }
            };

            foreach (var dadosPlataforma in notasFiscaisByPlataforma)
            {
                try
                {
                    var dadosCertificado = CertificadoDigitalBL.GetEntidade(dadosPlataforma.plataformaId);

                    if (dadosCertificado == null)
                        continue;


                    if (TotalTributacaoBL.ConfiguracaoTSSOK(dadosPlataforma.plataformaId))
                    {
                        var monitorVM = new MonitorVM()
                        {
                            Homologacao = dadosCertificado.Homologacao,
                            Producao = dadosCertificado.Producao,
                            EntidadeAmbiente = dadosCertificado.EntidadeAmbiente,
                            NotaInicial = dadosPlataforma.notaInicial.ToString(),
                            NotaFinal = dadosPlataforma.notaFinal.ToString(),
                        };

                        var responseMonitor = RestHelper.ExecutePostRequest<ListMonitorRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "monitor", JsonConvert.SerializeObject(monitorVM), null, header);
                        if (responseMonitor == null)
                            continue;

                        foreach (var itemNF in responseMonitor.Retornos)
                        {
                            //Atualiza Status NF;
                            var nfe = NFeBL.AllWithoutPlataformaId.Where(x => x.SefazId == itemNF.NotaId).FirstOrDefault();
                            if (nfe != null)
                            {
                                nfe.Mensagem = null;
                                nfe.Recomendacao = null;

                                nfe.Status = (StatusNotaFiscal)Enum.Parse(typeof(StatusNotaFiscal), itemNF.Status.ToString());
                                nfe.Mensagem = itemNF.Mensagem;
                                nfe.Recomendacao = itemNF.Recomendacao;
                            }
                            else
                            {
                                var nfse = NFSeBL.AllWithoutPlataformaId.Where(x => x.SefazId == itemNF.NotaId).FirstOrDefault();
                                if (nfse != null)
                                {
                                    nfse.Mensagem = null;
                                    nfse.Recomendacao = null;

                                    nfse.Status = (StatusNotaFiscal)Enum.Parse(typeof(StatusNotaFiscal), itemNF.Status.ToString());
                                    nfse.Mensagem = itemNF.Mensagem;
                                    nfse.Recomendacao = itemNF.Recomendacao;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
