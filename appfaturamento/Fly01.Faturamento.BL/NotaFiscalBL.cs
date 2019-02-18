using Fly01.Core;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Faturamento.BL
{
    public class NotaFiscalBL : PlataformaBaseBL<NotaFiscal>
    {
        protected NFeBL NFeBL { get; set; }
        protected NFSeBL NFSeBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected SerieNotaFiscalBL SerieNotaFiscalBL { get; set; }
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }

        public NotaFiscalBL(AppDataContext context, NFeBL nfeBL, NFSeBL nfseBL, CertificadoDigitalBL certificadoDigitalBL, TotalTributacaoBL totalTributacaoBL, SerieNotaFiscalBL serieNotaFiscalBL, NotaFiscalInutilizadaBL notaFiscalInutilizadaBL) : base(context)
        {
            NFeBL = nfeBL;
            NFSeBL = nfseBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            TotalTributacaoBL = totalTributacaoBL;
            SerieNotaFiscalBL = serieNotaFiscalBL;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
        }

        public IQueryable<NotaFiscal> Everything => repository.All.Where(x => x.Ativo);

        public override void Insert(NotaFiscal entity)
        {
            entity.Fail(true, new Error("Não é possível inserir, somente em NFe ou NFSe"));
        }

        public override void Update(NotaFiscal entity)
        {
            entity.Fail(true, new Error("Não é possível atualizar, somente em NFe ou NFSe"));
        }

        public override void Delete(NotaFiscal entity)
        {
            entity.Fail(true, new Error("Não é possível deletar, somente em NFe ou NFSe"));
        }

        public TotalPedidoNotaFiscal CalculaTotalNotaFiscal(Guid notaFiscalId)
        {
            if (All.Where(x => x.Id == notaFiscalId).FirstOrDefault().TipoNotaFiscal == TipoNotaFiscal.NFe)
            {
                return NFeBL.CalculaTotalNFe(notaFiscalId);
            }
            else
            {
                return NFSeBL.CalculaTotalNFSe(notaFiscalId);
            }
        }

        public object NotaFiscalXML(Guid id)
        {
            try
            {
                var notaFiscal = All.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
                if (!string.IsNullOrEmpty(notaFiscal.XML))
                {
                    return new { xml = notaFiscal.XML, numNotaFiscal = notaFiscal.NumNotaFiscal, tipoNotaFiscal = notaFiscal.TipoNotaFiscal.ToString() };
                }
                else
                {
                    if (notaFiscal.TipoNotaFiscal == TipoNotaFiscal.NFe)
                    {
                        return ObterXMLDanfeNFe(id, notaFiscal);
                    }
                    else
                    {
                        throw new BusinessException("XML vazio/inválido, verifique o status da nota.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Não foi possível realizar o download do XML. " + ex.Message);
            }
        }

        private object ObterXMLDanfeNFe(Guid id, NotaFiscal notaFiscal)
        {
            if (!TotalTributacaoBL.ConfiguracaoTSSOK())
            {
                throw new BusinessException("Configuração inválida para comunicação com TSS, verifique os dados da empresa, seu certificado digital e parâmetros tributários");
            }
            else
            {
                var header = new Dictionary<string, string>()
                        {
                            { "AppUser", AppUser },
                            { "PlataformaUrl", PlataformaUrl }
                        };

                var entidade = CertificadoDigitalBL.GetEntidade();

                var danfe = new DanfeVM()
                {
                    Homologacao = entidade.Homologacao,
                    Producao = entidade.Producao,
                    EntidadeAmbiente = entidade.EntidadeAmbiente,
                    DanfeId = notaFiscal?.SefazId?.ToString()
                };

                var response = RestHelper.ExecutePostRequest<XMLVM>(AppDefaults.UrlEmissaoNfeApi, "danfeXML", JsonConvert.SerializeObject(danfe), null, header);
                if (string.IsNullOrEmpty(response.XML))
                {
                    throw new BusinessException("XML retornado é vazio/inválido");
                }
                else
                {
                    var NFe = NFeBL.All.Where(x => x.Id == id).FirstOrDefault();
                    NFe.XML = response.XML;
                    NFeBL.Update(NFe);

                    return new { xml = response.XML, numNotaFiscal = notaFiscal.NumNotaFiscal };
                }
            }
        }

        public object NotaFiscalPDF(Guid id)
        {
            try
            {
                var notaFiscal = All.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
                if (!string.IsNullOrEmpty(notaFiscal.PDF))
                {
                    return new { pdf = notaFiscal.PDF, numNotaFiscal = notaFiscal.NumNotaFiscal };
                }
                else
                {
                    if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                    {
                        throw new BusinessException("Configuração inválida para comunicação com TSS, verifique os dados da empresa, seu certificado digital e parâmetros tributários");
                    }
                    else
                    {
                        var header = new Dictionary<string, string>()
                        {
                            { "AppUser", AppUser },
                            { "PlataformaUrl", PlataformaUrl }
                        };

                        var entidade = CertificadoDigitalBL.GetEntidade();

                        var danfe = new DanfeVM()
                        {
                            Homologacao = entidade.Homologacao,
                            Producao = entidade.Producao,
                            EntidadeAmbiente = entidade.EntidadeAmbiente,
                            DanfeId = notaFiscal.SefazId.ToString()
                        };

                        var response = RestHelper.ExecutePostRequest<PDFVM>(AppDefaults.UrlEmissaoNfeApi, "danfePDF", JsonConvert.SerializeObject(danfe), null, header);
                        if (string.IsNullOrEmpty(response.PDF))
                        {
                            throw new BusinessException("PDF retornado é vazio/inválido");
                        }
                        else
                        {
                            if (notaFiscal.TipoNotaFiscal == TipoNotaFiscal.NFe)
                            {
                                var NFe = NFeBL.All.Where(x => x.Id == id).FirstOrDefault();
                                NFe.PDF = response.PDF;
                                NFeBL.Update(NFe);
                            }
                            else
                            {
                                var NFSe = NFSeBL.All.Where(x => x.Id == id).FirstOrDefault();
                                NFSe.PDF = response.PDF;
                                NFSeBL.Update(NFSe);
                            }
                            return new { pdf = response.PDF, numNotaFiscal = notaFiscal.NumNotaFiscal };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Não foi possível realizar o download do PDF. " + ex.Message);
            }
        }

        public void NotaFiscalCancelar(Guid id)
        {
            var notaFiscal = All.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();

            if (notaFiscal.Status != StatusNotaFiscal.Autorizada && notaFiscal.Status != StatusNotaFiscal.FalhaNoCancelamento)
            {
                throw new BusinessException("Somente nota fiscal com status Autorizada ou Falha no Cancelamento, pode ser cancelada");
            }
            else
            {
                try
                {
                    if (notaFiscal.TipoNotaFiscal == TipoNotaFiscal.NFe)
                    {
                        CancelarNFe(id, notaFiscal);
                    }
                    else
                    {
                        CancelarNFSe(id);
                    }
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Erro ao cancelar a nota fiscal: " + ex.Message);
                }
            }
        }

        private void CancelarNFSe(Guid id)
        {
            if (!TotalTributacaoBL.ConfiguracaoTSSOKNFS(PlataformaUrl))
            {
                throw new BusinessException("Configuração inválida para comunicação com TSS, verifique os dados da empresa, seu certificado digital e parâmetros tributários");
            }

            var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", PlataformaUrl }
                    };

            var entidade = CertificadoDigitalBL.GetEntidade();

            var empresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            var notaFiscal = NFSeBL.All.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();

            var cancelar = new CancelarNFSVM()
            {
                Homologacao = entidade.Homologacao,
                Producao = entidade.Producao,
                EntidadeAmbiente = entidade.EntidadeAmbiente,
                CodigoIBGE = empresa.Cidade?.CodigoIbge ?? "",
                IdNotaFiscal = notaFiscal.SefazId,
                XMLUnicoTSSString = notaFiscal.XMLUnicoTSS
            };

            RestHelper.ExecutePostRequest<List<CancelarNFSRetornoVM>>(AppDefaults.UrlEmissaoNfeApi, "cancelarNFS", JsonConvert.SerializeObject(cancelar), null, header);

            var NFSe = NFSeBL.All.Where(x => x.Id == id).FirstOrDefault();
            NFSe.Mensagem = null;
            NFSe.Recomendacao = null;
            NFSe.Status = StatusNotaFiscal.EmCancelamento;
            NFSeBL.Update(NFSe);
        }

        private void CancelarNFe(Guid id, NotaFiscal notaFiscal)
        {
            if (!TotalTributacaoBL.ConfiguracaoTSSOK())
            {
                throw new BusinessException("Configuração inválida para comunicação com TSS, verifique os dados da empresa, seu certificado digital e parâmetros tributários");
            }

            var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", PlataformaUrl }
                    };

            var entidade = CertificadoDigitalBL.GetEntidade();

            var cancelar = new CancelarFaixaVM()
            {
                Homologacao = entidade.Homologacao,
                Producao = entidade.Producao,
                EntidadeAmbiente = entidade.EntidadeAmbiente,
                NotaInicial = notaFiscal.SefazId.ToString(),
                NotaFinal = notaFiscal.SefazId.ToString()
            };

            RestHelper.ExecutePostRequest<List<CancelarFaixaRetornoVM>>(AppDefaults.UrlEmissaoNfeApi, "CancelarFaixa", JsonConvert.SerializeObject(cancelar), null, header);

            var NFe = NFeBL.All.Where(x => x.Id == id).FirstOrDefault();
            NFe.Mensagem = null;
            NFe.Recomendacao = null;
            NFe.Status = StatusNotaFiscal.EmCancelamento;
            NFeBL.Update(NFe);
        }

        public void NotaFiscalInutilizar(NotaFiscalInutilizada entity)
        {
            if (!AllIncluding(x => x.SerieNotaFiscal).AsNoTracking().Any(x => x.SerieNotaFiscal.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal &&
                (x.Status == StatusNotaFiscal.Transmitida || x.Status == StatusNotaFiscal.Autorizada || x.Status == StatusNotaFiscal.Cancelada || x.Status == StatusNotaFiscal.CanceladaForaPrazo || x.Status == StatusNotaFiscal.EmCancelamento || x.Status == StatusNotaFiscal.FalhaNoCancelamento)))
            {
                var notaFiscal = AllIncluding(x => x.SerieNotaFiscal).Where(x => x.SerieNotaFiscal.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal &&
                (!(x.Status == StatusNotaFiscal.Transmitida || x.Status == StatusNotaFiscal.Autorizada || x.Status == StatusNotaFiscal.Cancelada || x.Status == StatusNotaFiscal.CanceladaForaPrazo || x.Status == StatusNotaFiscal.EmCancelamento || x.Status == StatusNotaFiscal.FalhaNoCancelamento))).FirstOrDefault();

                if (notaFiscal != null)
                {
                    notaFiscal.SerieNotaFiscalId = null;
                    notaFiscal.NumNotaFiscal = null;
                    //limpa para ser forçado a escolher um novo número
                    Update(notaFiscal);
                }

                //se existe ó próximo número e vai ser inutilizado, seta a nova 
                var serieNotaFiscal = SerieNotaFiscalBL.All.Where(x => x.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal).FirstOrDefault();
                if (serieNotaFiscal != null)
                {
                    var sugestaoProximoNumNota = serieNotaFiscal.NumNotaFiscal;
                    do
                    {
                        sugestaoProximoNumNota++;
                    }//enquanto sugestão possa estar na lista de inutilizadas
                    while (NotaFiscalInutilizadaBL.All.AsNoTracking().Any(x => x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() && x.NumNotaFiscal == sugestaoProximoNumNota));

                    serieNotaFiscal.NumNotaFiscal = sugestaoProximoNumNota;
                    SerieNotaFiscalBL.Update(serieNotaFiscal);
                }

                if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                {
                    throw new BusinessException("Configuração inválida para comunicação com TSS, verifique os dados da empresa, seu certificado digital e parâmetros tributários");
                }
                else
                {
                    try
                    {
                        var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", PlataformaUrl }
                    };

                        var entidade = CertificadoDigitalBL.GetEntidade();

                        var inutilizarNF = new InutilizarNFVM()
                        {
                            Homologacao = entidade.Homologacao,
                            Producao = entidade.Producao,
                            EntidadeAmbiente = entidade.EntidadeAmbiente,
                            Serie = int.Parse(entity.Serie),
                            Numero = entity.NumNotaFiscal,
                            EmpresaCnpj = TotalTributacaoBL.empresa.CNPJ,
                            ModeloDocumentoFiscal = 55,
                            EmpresaCodigoUF = TotalTributacaoBL.empresa.Cidade != null ? (TotalTributacaoBL.empresa.Cidade.Estado != null ? int.Parse(TotalTributacaoBL.empresa.Cidade.Estado.CodigoIbge) : 0) : 0
                        };

                        var response = RestHelper.ExecutePostRequest<InutilizarNFRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "InutilizarNF", JsonConvert.SerializeObject(inutilizarNF), null, header);

                        entity.SefazChaveAcesso = response.SefazChaveAcesso;
                        entity.Status = StatusNotaFiscal.InutilizacaoSolicitada;
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessException("Erro ao inutilizar a nota fiscal: " + ex.Message);
                    }
                }
            }
            else
            {
                throw new BusinessException("Nota fiscal existente não pode ser inutilizada");
            }
        }
    }
}