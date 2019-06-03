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

namespace Fly01.Faturamento.BL
{
    public class MonitorNFSBL : PlataformaBaseBL<MonitorNF>
    {
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected NFSeBL NFSeBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }

        public MonitorNFSBL(AppDataContextBase context, TotalTributacaoBL totalTributacao, NFSeBL nfseBL, CertificadoDigitalBL certificadoDigitalBL)
            : base(context)
        {
            TotalTributacaoBL = totalTributacao;
            NFSeBL = nfseBL;
            CertificadoDigitalBL = certificadoDigitalBL;
        }

        public void AtualizaStatusTSS(string plataformaUrl)
        {
            var notasFiscaisByPlataforma = (from nf in NFSeBL.Everything.Where(x => (x.Status == StatusNotaFiscal.Transmitida || x.Status == StatusNotaFiscal.EmCancelamento))
                                            where string.IsNullOrEmpty(plataformaUrl) || nf.PlataformaId == plataformaUrl && nf.TipoNotaFiscal == TipoNotaFiscal.NFSe
                                            group nf by new { nf.PlataformaId, nf.TipoAmbiente, nf.CertificadoDigitalId } into g
                                            select new
                                            {
                                                plataformaId = g.Key.PlataformaId,
                                                notaInicial = g.Min(x => x.SefazId),
                                                notaFinal = g.Max(x => x.SefazId),
                                                dataInicial = g.Min(x => x.Data),
                                                dataFinal = g.Max(x => x.Data),
                                                tipoAmbiente = g.Key.TipoAmbiente,
                                                certificadoDigitalId = g.Key.CertificadoDigitalId
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


                    if (TotalTributacaoBL.ConfiguracaoTSSOKNFS(dadosPlataforma.plataformaId))
                    {
                        var monitorVM = new MonitorNFSVM()
                        {
                            Homologacao = entidade.Homologacao,
                            Producao = entidade.Producao,
                            EntidadeAmbienteNFS = entidade.EntidadeAmbienteNFS,
                            NotaInicial = dadosPlataforma.notaInicial.ToString(),
                            NotaFinal = dadosPlataforma.notaFinal.ToString(),
                            DataInicial = dadosPlataforma.dataInicial,
                            DataFinal = dadosPlataforma.dataFinal
                        };

                        var responseMonitor = RestHelper.ExecutePostRequest<ListMonitorNFSRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "monitorNFS", JsonConvert.SerializeObject(monitorVM), null, header);
                        if (responseMonitor == null)
                            continue;

                        foreach (var retorno in responseMonitor.Retornos)
                        {
                            //Atualiza Status NF;
                            var nfse = NFSeBL.Everything.Where(x => x.SefazId == retorno.NotaFiscalId).FirstOrDefault();
                            if (nfse != null)
                            {
                                nfse.Mensagem = "";
                                nfse.Recomendacao = null;

                                nfse.Status = ValidaStatus(retorno.Protocolo, nfse.Status, retorno.Recomendacao, retorno?.Status);

                                if (nfse.Status == StatusNotaFiscal.Autorizada || nfse.Status == StatusNotaFiscal.Cancelada)
                                {
                                    nfse.XML = retorno.XML;
                                }

                                nfse.Recomendacao = retorno.Recomendacao;

                                if(retorno.Erros != null)
                                {
                                    foreach (var erro in retorno?.Erros)
                                    {
                                        nfse.Mensagem += string.Format("\n Código: {0} Mensagem: {1} ", erro.Codigo, erro.Mensagem);
                                    };
                                }

                                if (!string.IsNullOrEmpty(retorno.Protocolo))
                                {
                                    nfse.Mensagem += string.Format("\n Protocolo: {0} ", retorno.Protocolo);
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

        public StatusNotaFiscal ValidaStatus(string protocolo, StatusNotaFiscal statusAnterior, string recomendacao, string statusRetorno)
        {
            protocolo = protocolo.Trim();
            StatusNotaFiscal statusNFSe;
            StatusNFSTSS statusNFSTSS = (StatusNFSTSS)Enum.Parse(typeof(StatusNFSTSS), statusRetorno, true);
            var enviando = (
                recomendacao.Contains("Aguardando") || ((statusNFSTSS == StatusNFSTSS.AguardandoRetorno) || (statusNFSTSS == StatusNFSTSS.AguardandoAssinatura) || (statusNFSTSS == StatusNFSTSS.PendenteTransmissao))
            );

            if (string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.Transmitida && enviando)
            {
                statusNFSe = StatusNotaFiscal.Transmitida;
            }
            else if (string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.Transmitida)
            {
                statusNFSe = StatusNotaFiscal.NaoAutorizada;
            }
            else if (!string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.Transmitida && statusNFSTSS == StatusNFSTSS.Autorizada)
            {
                statusNFSe = StatusNotaFiscal.Autorizada;
            }
            else if (string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.EmCancelamento && enviando)
            {
                statusNFSe = StatusNotaFiscal.EmCancelamento;
            }
            else if (string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.EmCancelamento && !enviando)
            {
                statusNFSe = StatusNotaFiscal.FalhaNoCancelamento;
            }
            else if (!string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.EmCancelamento && statusNFSTSS == StatusNFSTSS.CanceladaInutilizada)
            {
                statusNFSe = StatusNotaFiscal.Cancelada;
            }
            else
            {
                statusNFSe = statusAnterior;
            }

            return statusNFSe;
        }

    }
}
