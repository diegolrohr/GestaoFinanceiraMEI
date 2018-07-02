﻿using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Faturamento.DAL;
using System;
using System.Linq;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;

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

        public TotalNotaFiscal CalculaTotalNotaFiscal(Guid notaFiscalId)
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
                    return new { xml = notaFiscal.XML, numNotaFiscal = notaFiscal.NumNotaFiscal };
                }
                else
                {
                    if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                    {
                        throw new BusinessException("Configuração inválida para comunicação com TSS");
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

                        var response = RestHelper.ExecutePostRequest<XMLVM>(AppDefaults.UrlEmissaoNfeApi, "danfeXML", JsonConvert.SerializeObject(danfe), null, header);
                        if (string.IsNullOrEmpty(response.XML))
                        {
                            throw new BusinessException("XML vazio/inválido");
                        }
                        else
                        {
                            if (notaFiscal.TipoNotaFiscal == TipoNotaFiscal.NFe)
                            {
                                var NFe = NFeBL.All.Where(x => x.Id == id).FirstOrDefault();
                                NFe.XML = response.XML;
                                NFeBL.Update(NFe);
                            }
                            else
                            {
                                var NFSe = NFSeBL.All.Where(x => x.Id == id).FirstOrDefault();
                                NFSe.XML = response.XML;
                                NFSeBL.Update(NFSe);
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
                var notaFiscal = All.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
                if (!string.IsNullOrEmpty(notaFiscal.PDF))
                {
                    return new { pdf = notaFiscal.PDF, numNotaFiscal = notaFiscal.NumNotaFiscal };
                }
                else
                {
                    if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                    {
                        throw new BusinessException("Configuração inválida para comunicação com TSS");
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
                            throw new BusinessException("PDF vazio/inválido");
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
            else if (!TotalTributacaoBL.ConfiguracaoTSSOK())
            {
                throw new BusinessException("Configuração inválida para comunicação com TSS");
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
                        var NFe = NFeBL.All.Where(x => x.Id == id).FirstOrDefault();
                        NFe.Mensagem = null;
                        NFe.Recomendacao = null;
                        NFe.Status = StatusNotaFiscal.EmCancelamento;
                        NFeBL.Update(NFe);
                    }
                    else
                    {
                        var NFSe = NFSeBL.All.Where(x => x.Id == id).FirstOrDefault();
                        NFSe.Mensagem = null;
                        NFSe.Recomendacao = null;
                        NFSe.Status = StatusNotaFiscal.EmCancelamento;
                        NFSeBL.Update(NFSe);
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
            //TODO: Diego fazer validações aqui, por causa da referência circular
            //se existe nota com esse numero, e status é transmitida ou autorizada ou cancelada, em cancelamento, cancelada fora do prazo
            //se pode inutilizar e tem uma nota com essa serie/numero, da pra limpar pra ser obrigado a escolher outra
            var serieNotaFiscal = SerieNotaFiscalBL.All.Where(x => x.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal).FirstOrDefault();
            if(serieNotaFiscal != null)
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

            if (true)
            {
                
                if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                {
                    throw new BusinessException("Configuração inválida para comunicação com TSS");
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