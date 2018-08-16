﻿using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using System.Collections.Generic;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Newtonsoft.Json;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using System;

namespace Fly01.Compras.BL
{
    public class MonitorNFBL : PlataformaBaseBL<MonitorNF>
    {
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected NFeEntradaBL NFeEntradaBL { get; set; }
        protected NotaFiscalEntradaBL NotaFiscalEntradaBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }
        protected NotaFiscalCartaCorrecaoEntradaBL NotaFiscalCartaCorrecaoEntradaBL { get; set; }

        public MonitorNFBL(AppDataContextBase context, TotalTributacaoBL totalTributacao, NFeEntradaBL nFeEntradaBL,
            NotaFiscalEntradaBL notaFiscalEntradaBL, CertificadoDigitalBL certificadoDigitalBL, NotaFiscalInutilizadaBL notaFiscalInutilizadaBL, NotaFiscalCartaCorrecaoEntradaBL notaFiscalCartaCorrecaoEntradaBL)
            : base(context)
        {
            TotalTributacaoBL = totalTributacao;
            NFeEntradaBL = nFeEntradaBL;
            NotaFiscalEntradaBL = notaFiscalEntradaBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
            NotaFiscalCartaCorrecaoEntradaBL = notaFiscalCartaCorrecaoEntradaBL;
        }

        public void AtualizaStatusTSS(string plataformaUrl)
        {
            var notasFiscaisByPlataforma = (from nf in NotaFiscalEntradaBL.Everything.Where(x => (x.Status == StatusNotaFiscal.Transmitida || x.Status == StatusNotaFiscal.EmCancelamento))
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
                            var nfe = NFeEntradaBL.Everything.Where(x => x.SefazId == itemNF.NotaId).FirstOrDefault();
                            if (nfe != null)
                            {
                                nfe.Mensagem = null;
                                nfe.Recomendacao = null;

                                nfe.Status = (StatusNotaFiscal)System.Enum.Parse(typeof(StatusNotaFiscal), itemNF.Status.ToString());
                                nfe.Mensagem = itemNF.Mensagem;
                                nfe.Recomendacao = itemNF.Recomendacao;
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

        public void AtualizaStatusTSSInutilizada(string plataformaUrl)
        {
            var notasFiscaisInutilizadasByPlataforma = (from nf in NotaFiscalInutilizadaBL.Everything.Where(x => (x.Status == StatusNotaFiscal.InutilizacaoSolicitada || x.Status == StatusNotaFiscal.Transmitida))
                                                        where string.IsNullOrEmpty(plataformaUrl) || nf.PlataformaId == plataformaUrl
                                                        group nf by nf.PlataformaId into g
                                                        select new { plataformaId = g.Key, notaInicial = g.Min(x => x.SefazChaveAcesso), notaFinal = g.Max(x => x.SefazChaveAcesso) });

            var header = new Dictionary<string, string>()
            {
                { "AppUser", AppUser },
                { "PlataformaUrl", string.IsNullOrEmpty(plataformaUrl) ? PlataformaUrl : plataformaUrl }
            };

            foreach (var dadosPlataforma in notasFiscaisInutilizadasByPlataforma)
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

                        foreach (var itemNF in (from r in responseMonitor.Retornos
                                                join nfi in NotaFiscalInutilizadaBL.Everything on r.NotaId equals nfi.SefazChaveAcesso
                                                select r))
                        {
                            //Atualiza Status NF inutilizada;
                            var nfInutilizada = NotaFiscalInutilizadaBL.Everything.Where(x => x.SefazChaveAcesso == itemNF.NotaId).FirstOrDefault();
                            if (nfInutilizada != null)
                            {
                                nfInutilizada.Mensagem = null;
                                nfInutilizada.Recomendacao = null;

                                nfInutilizada.Status = (StatusNotaFiscal)System.Enum.Parse(typeof(StatusNotaFiscal), itemNF.Status.ToString());
                                nfInutilizada.Mensagem = itemNF.Mensagem;
                                nfInutilizada.Recomendacao = itemNF.Recomendacao;
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

        public void AtualizaStatusTSSCartaCorrecao(string plataformaUrl, Guid idNotaFiscal)
        {
            var groupPlataformas = (from nf in NotaFiscalCartaCorrecaoEntradaBL.Everything.Where(x => (x.Status == StatusCartaCorrecao.Transmitida))
                                    where (string.IsNullOrEmpty(plataformaUrl) || nf.PlataformaId == plataformaUrl)
                                    && (idNotaFiscal == default(Guid) || nf.NotaFiscalId == idNotaFiscal)
                                    group nf by nf.PlataformaId into g
                                    select new { plataformaId = g.Key });

            var header = new Dictionary<string, string>()
            {
                { "AppUser", AppUser },
                { "PlataformaUrl", string.IsNullOrEmpty(plataformaUrl) ? PlataformaUrl : plataformaUrl }
            };


            foreach (var dadosPlataforma in groupPlataformas)
            {
                try
                {
                    var dadosCertificado = CertificadoDigitalBL.GetEntidade(dadosPlataforma.plataformaId);

                    if (dadosCertificado == null)
                        continue;

                    if (TotalTributacaoBL.ConfiguracaoTSSOK(dadosPlataforma.plataformaId))
                    {
                        var cartasCorrecoesByPlataforma = new List<NotaFiscalCartaCorrecaoEntrada>();
                        cartasCorrecoesByPlataforma = NotaFiscalCartaCorrecaoEntradaBL.Everything.Where(x => x.PlataformaId == dadosPlataforma.plataformaId && (x.Status == StatusCartaCorrecao.Transmitida)).ToList();

                        foreach (var cartaCorrecao in cartasCorrecoesByPlataforma)
                        {
                            var monitorEventoVM = new MonitorEventoVM()
                            {
                                Homologacao = dadosCertificado.Homologacao,
                                Producao = dadosCertificado.Producao,
                                EntidadeAmbiente = dadosCertificado.EntidadeAmbiente,
                                IdEvento = cartaCorrecao.IdRetorno,
                                SefazChaveAcesso = cartaCorrecao.NotaFiscal.SefazId
                            };

                            var responseMonitor = RestHelper.ExecutePostRequest<MonitorEventoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "monitorevento", JsonConvert.SerializeObject(monitorEventoVM), null, header);
                            if (responseMonitor == null)
                                continue;                           
                            
                            cartaCorrecao.Mensagem = string.Format("{0} {1}",
                                (responseMonitor.Motivo != null ? responseMonitor.Motivo : ""),
                                (responseMonitor.MotivoEvento != null ? responseMonitor.MotivoEvento : ""));

                            cartaCorrecao.Status = responseMonitor.Status;
                            cartaCorrecao.XML = responseMonitor.XML;
                            cartaCorrecao.IdRetorno = responseMonitor.IdEvento;

                            if(responseMonitor.Status == StatusCartaCorrecao.RegistradoENaoVinculado || responseMonitor.Status == StatusCartaCorrecao.RegistradoEVinculado)
                            {
                                var idRetornoLength = cartaCorrecao.IdRetorno.Length;
                                cartaCorrecao.Numero = int.Parse(responseMonitor.IdEvento.Substring(idRetornoLength - 2, 2));
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
