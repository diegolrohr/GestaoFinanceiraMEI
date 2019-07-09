using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using System.Collections.Generic;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Newtonsoft.Json;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Data.Entity;

namespace Fly01.Faturamento.BL
{
    public class MonitorNFBL : PlataformaBaseBL<MonitorNF>
    {
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected NFeBL NFeBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }
        protected NotaFiscalCartaCorrecaoBL NotaFiscalCartaCorrecaoBL { get; set; }

        public MonitorNFBL(AppDataContextBase context, TotalTributacaoBL totalTributacao, NFeBL nFeBL,
            CertificadoDigitalBL certificadoDigitalBL, NotaFiscalInutilizadaBL notaFiscalInutilizadaBL, NotaFiscalCartaCorrecaoBL notaFiscalCartaCorrecaoBL)
            : base(context)
        {
            TotalTributacaoBL = totalTributacao;
            NFeBL = nFeBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
            NotaFiscalCartaCorrecaoBL = notaFiscalCartaCorrecaoBL;
        }

        public void AtualizaStatusTSS(string plataformaUrl)
        {
            var notasFiscaisByPlataforma = (from nf in NFeBL.Everything.AsNoTracking().Where(x => (x.Status == StatusNotaFiscal.Transmitida || x.Status == StatusNotaFiscal.EmCancelamento))
                                            where (string.IsNullOrEmpty(plataformaUrl) || nf.PlataformaId == plataformaUrl) && nf.TipoNotaFiscal == TipoNotaFiscal.NFe
                                            group nf by new { nf.PlataformaId, nf.TipoAmbiente, nf.CertificadoDigitalId } into g
                                            select new
                                            {
                                                plataformaId = g.Key.PlataformaId,
                                                tipoAmbiente = g.Key.TipoAmbiente,
                                                certificadoDigitalId = g.Key.CertificadoDigitalId,
                                                notaInicial = g.Min(x => x.SefazId.Substring(22, 12)),
                                                notaFinal = g.Max(x => x.SefazId.Substring(22, 12))
                                            });

            var header = new Dictionary<string, string>()
            {
                { "AppUser", AppUser },
                { "PlataformaUrl", string.IsNullOrEmpty(plataformaUrl) ? PlataformaUrl : plataformaUrl }
            };

            foreach (var dadosPlataforma in notasFiscaisByPlataforma)
            {
                try
                {
                    var entidade = CertificadoDigitalBL.GetEntidadeFromCertificado(dadosPlataforma.plataformaId, dadosPlataforma.tipoAmbiente, dadosPlataforma.certificadoDigitalId);

                    if (entidade == null)
                        continue;


                    if (TotalTributacaoBL.ConfiguracaoTSSOK(dadosPlataforma.plataformaId))
                    {
                        var sefazIdInicial = NFeBL.Everything.AsNoTracking().Where(x => x.SefazId.Substring(22, 12) == dadosPlataforma.notaInicial && x.PlataformaId == dadosPlataforma.plataformaId).FirstOrDefault()?.SefazId;
                        var sefazIdFinal = NFeBL.Everything.AsNoTracking().Where(x => x.SefazId.Substring(22, 12) == dadosPlataforma.notaFinal && x.PlataformaId == dadosPlataforma.plataformaId).FirstOrDefault()?.SefazId;
                        var monitorVM = new MonitorVM()
                        {
                            Homologacao = entidade.Homologacao,
                            Producao = entidade.Producao,
                            EntidadeAmbiente = entidade.EntidadeAmbiente,
                            NotaInicial = sefazIdInicial,
                            NotaFinal = sefazIdFinal,
                        };

                        var responseMonitor = RestHelper.ExecutePostRequest<ListMonitorRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "monitor", JsonConvert.SerializeObject(monitorVM), null, header);
                        if (responseMonitor == null)
                            continue;

                        foreach (var itemNF in responseMonitor.Retornos)
                        {
                            //Atualiza Status NF;
                            var nfe = NFeBL.Everything.Where(x => x.SefazId == itemNF.NotaId).FirstOrDefault();
                            if (nfe != null)
                            {
                                nfe.Mensagem = null;
                                nfe.Recomendacao = null;
                                nfe.XML = null;

                                nfe.XML = itemNF?.XML;
                                nfe.PDF = itemNF?.PDF;
                                nfe.Status = itemNF.Status;
                                nfe.Mensagem = itemNF?.Mensagem;
                                nfe.Recomendacao = itemNF?.Recomendacao;
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
            var notasFiscaisInutilizadasByPlataforma = (from nf in NotaFiscalInutilizadaBL.Everything.AsNoTracking().Where(x => (x.Status == StatusNotaFiscal.InutilizacaoSolicitada || x.Status == StatusNotaFiscal.Transmitida))
                                                        where string.IsNullOrEmpty(plataformaUrl) || nf.PlataformaId == plataformaUrl
                                                        group nf by new { nf.PlataformaId, nf.TipoAmbiente, nf.CertificadoDigitalId } into g
                                                        select new
                                                        {
                                                            plataformaId = g.Key.PlataformaId,
                                                            tipoAmbiente = g.Key.TipoAmbiente,
                                                            certificadoDigitalId = g.Key.CertificadoDigitalId,
                                                            notaInicial = g.Min(x => x.SefazChaveAcesso.Substring(22, 12)),
                                                            notaFinal = g.Max(x => x.SefazChaveAcesso.Substring(22, 12))
                                                        }).ToList();

            var header = new Dictionary<string, string>()
            {
                { "AppUser", AppUser },
                { "PlataformaUrl", string.IsNullOrEmpty(plataformaUrl) ? PlataformaUrl : plataformaUrl }
            };

            foreach (var dadosPlataforma in notasFiscaisInutilizadasByPlataforma)
            {
                try
                {
                    var entidade = CertificadoDigitalBL.GetEntidadeFromCertificado(dadosPlataforma.plataformaId, dadosPlataforma.tipoAmbiente, dadosPlataforma.certificadoDigitalId);

                    if (entidade == null)
                        continue;


                    if (TotalTributacaoBL.ConfiguracaoTSSOK(dadosPlataforma.plataformaId))
                    {
                        var sefazIdInicial = NotaFiscalInutilizadaBL.Everything.AsNoTracking().Where(x => x.SefazChaveAcesso.Substring(22, 12) == dadosPlataforma.notaInicial && x.PlataformaId == dadosPlataforma.plataformaId).FirstOrDefault()?.SefazChaveAcesso;
                        var sefazIdFinal = NotaFiscalInutilizadaBL.Everything.AsNoTracking().Where(x => x.SefazChaveAcesso.Substring(22, 12) == dadosPlataforma.notaFinal && x.PlataformaId == dadosPlataforma.plataformaId).FirstOrDefault()?.SefazChaveAcesso;
                        var monitorVM = new MonitorVM()
                        {
                            Homologacao = entidade.Homologacao,
                            Producao = entidade.Producao,
                            EntidadeAmbiente = entidade.EntidadeAmbiente,
                            NotaInicial = sefazIdInicial,
                            NotaFinal = sefazIdFinal,
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
            var groupPlataformas = (from nf in NotaFiscalCartaCorrecaoBL.Everything.AsNoTracking().Where(x => (x.Status == StatusCartaCorrecao.Transmitida))
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
                    if (TotalTributacaoBL.ConfiguracaoTSSOK(dadosPlataforma.plataformaId))
                    {
                        var cartasCorrecoesByPlataforma = new List<NotaFiscalCartaCorrecao>();
                        cartasCorrecoesByPlataforma = NotaFiscalCartaCorrecaoBL.Everything.AsNoTracking().Where(x => x.PlataformaId == dadosPlataforma.plataformaId && (x.Status == StatusCartaCorrecao.Transmitida)).ToList();

                        foreach (var cartaCorrecao in cartasCorrecoesByPlataforma)
                        {
                            var notaFiscal = NFeBL.Everything.Where(x => x.Id == cartaCorrecao.NotaFiscalId).AsNoTracking().FirstOrDefault();
                            var entidade = new EntidadeVM();
                            if (notaFiscal != null)
                            {
                                entidade = CertificadoDigitalBL.GetEntidadeFromCertificado(dadosPlataforma.plataformaId, notaFiscal.TipoAmbiente, notaFiscal.CertificadoDigitalId);
                            }
                            else
                            {
                                entidade = CertificadoDigitalBL.GetEntidade(dadosPlataforma.plataformaId);
                            }

                            if (entidade == null)
                                continue;

                            var monitorEventoVM = new MonitorEventoVM()
                            {
                                Homologacao = entidade.Homologacao,
                                Producao = entidade.Producao,
                                EntidadeAmbiente = entidade.EntidadeAmbiente,
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

                            if (responseMonitor.Status == StatusCartaCorrecao.RegistradoENaoVinculado || responseMonitor.Status == StatusCartaCorrecao.RegistradoEVinculado)
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
