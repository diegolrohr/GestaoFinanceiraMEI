using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Compras.DAL;
using System;
using System.Linq;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Compras.BL
{
    public class NotaFiscalEntradaBL : PlataformaBaseBL<NotaFiscalEntrada>
    {
        protected NFeEntradaBL NFeEntradaBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected SerieNotaFiscalBL SerieNotaFiscalBL { get; set; }
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }

        public NotaFiscalEntradaBL(AppDataContext context, NFeEntradaBL nfeEntradaBL, CertificadoDigitalBL certificadoDigitalBL, TotalTributacaoBL totalTributacaoBL, SerieNotaFiscalBL serieNotaFiscalBL, NotaFiscalInutilizadaBL notaFiscalInutilizadaBL) : base(context)
        {
            NFeEntradaBL = nfeEntradaBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            TotalTributacaoBL = totalTributacaoBL;
            SerieNotaFiscalBL = serieNotaFiscalBL;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
        }

        public IQueryable<NotaFiscalEntrada> Everything => repository.All.Where(x => x.Ativo);

        public override void Insert(NotaFiscalEntrada entity)
        {
            entity.Fail(true, new Error("Não é possível inserir, somente em NFe ou NFSe"));
        }

        public override void Update(NotaFiscalEntrada entity)
        {
            entity.Fail(true, new Error("Não é possível atualizar, somente em NFe ou NFSe"));
        }

        public override void Delete(NotaFiscalEntrada entity)
        {
            entity.Fail(true, new Error("Não é possível deletar, somente em NFe ou NFSe"));
        }

        public List<NotaFiscalEntrada> NotasFiscaisPeriodo(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {

                List<NotaFiscalEntrada> notasFiscais = AllIncluding(x => x.SerieNotaFiscal).AsNoTracking().Where(x => x.DataInclusao >= dataInicial && x.DataInclusao <= dataFinal && x.Status == StatusNotaFiscal.Autorizada).ToList();

                return notasFiscais;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Não foi possível buscar as Notas para este período. " + ex.Message);
            }
        }


        public TotalPedidoNotaFiscal CalculaTotalNotaFiscal(Guid notaFiscalId)
        {
            if (All.Where(x => x.Id == notaFiscalId).FirstOrDefault().TipoNotaFiscal == TipoNotaFiscal.NFe)
            {
                return NFeEntradaBL.CalculaTotalNFe(notaFiscalId);
            }
            else
            {
                return null;
            }
        }

        private bool EhNotaFiscalMigradaDoFlyAntigo(NotaFiscalEntrada notaFiscal)
        {
            return (notaFiscal?.OrdemCompraOrigemId == null && notaFiscal?.UsuarioInclusao.ToLower() == "fly01@totvs.com.br");
        }

        private string ObterIdNotaMigradaFlyAntigo(NotaFiscalEntrada notaFiscal)
        {
            return string.Format("{0}{1}", notaFiscal?.SerieNotaFiscal?.Serie?.PadRight(3, ' '), notaFiscal?.NumNotaFiscal?.ToString()?.PadLeft(9, '0'));
        }

        public object NotaFiscalXML(Guid id)
        {
            try
            {
                var notaFiscal = AllIncluding(x => x.SerieNotaFiscal).AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
                if (!string.IsNullOrEmpty(notaFiscal.XML))
                {
                    return new { xml = notaFiscal.XML, numNotaFiscal = notaFiscal.NumNotaFiscal, tipoNotaFiscal = notaFiscal.TipoNotaFiscal.ToString() };
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

                        var entidade = CertificadoDigitalBL.GetEntidadeFromCertificado(string.Empty, notaFiscal.TipoAmbiente, notaFiscal.CertificadoDigitalId);
                        var sefazId = EhNotaFiscalMigradaDoFlyAntigo(notaFiscal) ? ObterIdNotaMigradaFlyAntigo(notaFiscal) : notaFiscal?.SefazId;

                        var danfe = new DanfeVM()
                        {
                            Homologacao = entidade.Homologacao,
                            Producao = entidade.Producao,
                            EntidadeAmbiente = entidade.EntidadeAmbiente,
                            DanfeId = sefazId
                        };

                        var response = RestHelper.ExecutePostRequest<XMLVM>(AppDefaults.UrlEmissaoNfeApi, "danfeXML", JsonConvert.SerializeObject(danfe), null, header);
                        if (string.IsNullOrEmpty(response.XML))
                        {
                            throw new BusinessException("XML vazio/inválido");
                        }
                        else
                        {
                            if (notaFiscal.TipoNotaFiscal == TipoNotaFiscal.NFe)
                            {
                                var NFe = NFeEntradaBL.All.Where(x => x.Id == id).FirstOrDefault();
                                NFe.XML = response.XML;
                                NFeEntradaBL.Update(NFe);
                            }
                            return new { xml = response.XML, numNotaFiscal = notaFiscal.NumNotaFiscal };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Não foi possível realizar o download do XML. " + ex.Message);
            }
        }

        public object NotaFiscalPDF(Guid id)
        {
            try
            {
                var notaFiscal = AllIncluding(x => x.SerieNotaFiscal).AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
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

                        var entidade = CertificadoDigitalBL.GetEntidadeFromCertificado(string.Empty, notaFiscal.TipoAmbiente, notaFiscal.CertificadoDigitalId);
                        var sefazId = EhNotaFiscalMigradaDoFlyAntigo(notaFiscal) ? ObterIdNotaMigradaFlyAntigo(notaFiscal) : notaFiscal?.SefazId;

                        var danfe = new DanfeVM()
                        {
                            Homologacao = entidade.Homologacao,
                            Producao = entidade.Producao,
                            EntidadeAmbiente = entidade.EntidadeAmbiente,
                            DanfeId = sefazId
                        };

                        var response = RestHelper.ExecutePostRequest<PDFVM>(AppDefaults.UrlEmissaoNfeApi, "danfePDF", JsonConvert.SerializeObject(danfe), null, header);
                        if (string.IsNullOrEmpty(response.PDF))
                        {
                            throw new BusinessException("PDF vazio/inválido");
                        }
                        else
                        {
                            if (notaFiscal.TipoNotaFiscal == TipoNotaFiscal.NFe)
                            {
                                var NFe = NFeEntradaBL.All.Where(x => x.Id == id).FirstOrDefault();
                                NFe.PDF = response.PDF;
                                NFeEntradaBL.Update(NFe);
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
            else if (!TotalTributacaoBL.ConfiguracaoTSSOK())
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

                    var entidade = CertificadoDigitalBL.GetEntidadeFromCertificado(string.Empty, notaFiscal.TipoAmbiente, notaFiscal.CertificadoDigitalId);

                    var cancelar = new CancelarFaixaVM()
                    {
                        Homologacao = entidade.Homologacao,
                        Producao = entidade.Producao,
                        EntidadeAmbiente = entidade.EntidadeAmbiente,
                        NotaInicial = notaFiscal.SefazId.ToString(),
                        NotaFinal = notaFiscal.SefazId.ToString()
                    };

                    RestHelper.ExecutePostRequest<List<CancelarFaixaRetornoVM>>(AppDefaults.UrlEmissaoNfeApi, "CancelarFaixa", JsonConvert.SerializeObject(cancelar), null, header);
                    if (notaFiscal.TipoNotaFiscal == TipoNotaFiscal.NFe)
                    {
                        var NFe = NFeEntradaBL.All.Where(x => x.Id == id).FirstOrDefault();
                        NFe.Mensagem = null;
                        NFe.Recomendacao = null;
                        NFe.Status = StatusNotaFiscal.EmCancelamento;
                        NFeEntradaBL.Update(NFe);
                    }
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Erro ao cancelar a nota fiscal: " + ex.Message);
                }
            }
        }

        public void NotaFiscalInutilizar(NotaFiscalInutilizada entity)
        {
            if (!AllIncluding(x => x.SerieNotaFiscal).AsNoTracking().Any(x => x.SerieNotaFiscal.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal &&
                (x.Status == StatusNotaFiscal.Transmitida || x.Status == StatusNotaFiscal.Autorizada || x.Status == StatusNotaFiscal.Cancelada || x.Status == StatusNotaFiscal.CanceladaForaPrazo || x.Status == StatusNotaFiscal.EmCancelamento || x.Status == StatusNotaFiscal.FalhaNoCancelamento)))
            {
                var notaFiscal = AllIncluding(x => x.SerieNotaFiscal, x => x.CertificadoDigital).Where(x => x.SerieNotaFiscal.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal &&
                (!(x.Status == StatusNotaFiscal.Transmitida || x.Status == StatusNotaFiscal.Autorizada || x.Status == StatusNotaFiscal.Cancelada || x.Status == StatusNotaFiscal.CanceladaForaPrazo || x.Status == StatusNotaFiscal.EmCancelamento || x.Status == StatusNotaFiscal.FalhaNoCancelamento))).FirstOrDefault();

                if(notaFiscal != null)
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
                            EmpresaCnpj = TotalTributacaoBL.GetEmpresa()?.CNPJ,
                            ModeloDocumentoFiscal = 55,
                            EmpresaCodigoUF = TotalTributacaoBL.GetEmpresa()?.Cidade != null ? (TotalTributacaoBL.GetEmpresa()?.Cidade?.Estado != null ? int.Parse(TotalTributacaoBL.GetEmpresa()?.Cidade?.Estado?.CodigoIbge) : 0) : 0 
                        };

                        var response = RestHelper.ExecutePostRequest<InutilizarNFRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "InutilizarNF", JsonConvert.SerializeObject(inutilizarNF), null, header);

                        entity.TipoAmbiente = entidade.EntidadeAmbiente;
                        entity.CertificadoDigitalId = CertificadoDigitalBL.CertificadoAtualValido()?.Id;
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