﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;
using ServicoEmissao = Fly01.EmissaoNFE.Domain.Entities.NFS.Servico;

namespace Fly01.Faturamento.BL.Helpers
{
    public class TransmissaoNFSNormal
    {
        protected TransmissaoNFSBLs TransmissaoNFSBLs { get; set; }
        protected NFSe NFSe { get; set; }
        protected ManagerEmpresaVM Empresa { get; set; }
        protected Pessoa Cliente { get; set; }
        protected ParametroTributario ParametrosTributarios { get; set; }

        public TransmissaoNFSNormal(TransmissaoNFSBLs transmissaoNFSBLs, NFSe entity)
        {
            TransmissaoNFSBLs = transmissaoNFSBLs;
            NFSe = entity;
            ValidaParametrosTributarios();

            Empresa = ApiEmpresaManager.GetEmpresa(TransmissaoNFSBLs.PlataformaUrl);
            if (entity != null)
                Cliente = TransmissaoNFSBLs.TotalTributacaoBL.GetPessoa(entity.ClienteId);
        }

        public TransmissaoNFSVM ObterTransmissaoNFSVM()
        {
            var itemTransmissaoNFS = ObterTransmissaoNFS();
            return ObterTransmissaoApartirDoItem(itemTransmissaoNFS);
        }

        public string SubstringTelefone(string telefone = "")
        {
            if (telefone != null)
            {
                if (telefone.Length == 10)
                {
                    telefone = telefone.Substring((telefone.Length - 8), 8);
                }
                else if (telefone.Length > 10)
                {
                    telefone = telefone.Substring((telefone.Length - 9), 9);
                }
            }

            return telefone;
        }

        /// <summary>
        /// Ordenado por data de inclusão, pois na aglutinação dos serviço, as informações 
        /// de código Iss, Nbs e CodMunicipal, vale do primeiro
        /// </summary>
        /// <returns></returns>
        public IQueryable<NFSeServico> ObterNFSeServicos()
        {
            return TransmissaoNFSBLs.NFSeServicoBL.AllIncluding(
                x => x.GrupoTributario.Cfop,
                x => x.Servico.Nbs,
                x => x.Servico.Iss).AsNoTracking().Where(x => x.NotaFiscalId == NFSe.Id).OrderBy(x => x.DataInclusao);
        }

        private EntidadeVM ObterEntidade() => TransmissaoNFSBLs.CertificadoDigitalBL.GetEntidade();

        private TransmissaoNFSVM ObterTransmissaoApartirDoItem(ItemTransmissaoNFSVM itemTransmissaoNFS)
        {
            var entidade = ObterEntidade();
            return new TransmissaoNFSVM()
            {
                Homologacao = entidade.Homologacao,
                Producao = entidade.Producao,
                EntidadeAmbiente = entidade.EntidadeAmbiente,
                ItemTransmissaoNFSVM = itemTransmissaoNFS
            };
        }

        private void ValidaParametrosTributarios()
        {
            ParametrosTributarios = TransmissaoNFSBLs.TotalTributacaoBL.GetParametrosTributarios();
            if (ParametrosTributarios == null || (ParametrosTributarios != null && !ParametrosTributarios.ParametroValidoNFS))
            {
                throw new BusinessException("Acesse o menu Configurações > Parâmetros Tributários e salve as configurações para a transmissão de NFS");
            }
        }

        private ItemTransmissaoNFSVM ObterTransmissaoNFS()
        {
            return new ItemTransmissaoNFSVM()
            {
                Identificacao = ObterIdentificacao(),
                Atividade = ObterAtividade(),
                Prestador = ObterPrestador(),
                Prestacao = ObterPrestacao(),
                Tomador = ObterTomador(),
                Servicos = ObterServicos(),
                Valores = ObterValores(),
                InformacoesComplementares = ObterInformacoesComplementares()
            };
        }

        private InformacoesComplementares ObterInformacoesComplementares()
        {
            return new InformacoesComplementares()
            {
                Descricao = NFSe.MensagemPadraoNota ?? "",
                Observacao = NFSe.Observacao ?? ""
            };
        }

        private List<ServicoEmissao> ObterServicos()
        {
            var result = new List<ServicoEmissao>();
            foreach (var NFSeServico in ObterNFSeServicos())
            {
                var itemTributacao = new NotaFiscalItemTributacao();
                itemTributacao = TransmissaoNFSBLs.NotaFiscalItemTributacaoBL.All.Where(x => x.NotaFiscalItemId == NFSeServico.Id).FirstOrDefault();
                var somaRetencoes =
                    itemTributacao.PISValorRetencao +
                    itemTributacao.COFINSValorRetencao +
                    itemTributacao.CSLLValorRetencao +
                    itemTributacao.INSSValorRetencao +
                    itemTributacao.ImpostoRendaValorRetencao +
                    NFSeServico.ValorOutrasRetencoes;

                //Codigo Iss especifico, se informado, prioritário a tabela padrão
                var codigoIss = !string.IsNullOrEmpty(NFSeServico.Servico.CodigoIssEspecifico) ?
                        NFSeServico.Servico.CodigoIssEspecifico.Trim() :
                        (NFSeServico.Servico.Iss != null ?
                            (ParametrosTributarios.FormatarCodigoISS ? FormatarCodigoISS(NFSeServico.Servico.Iss.Codigo) : NFSeServico.Servico.Iss.Codigo)
                            : null);

                result.Add(new ServicoEmissao()
                {
                    CodigoIss = codigoIss,
                    CodigoNBS = NFSeServico.Servico.Nbs != null ? NFSeServico.Servico.Nbs.Codigo : null,
                    AliquotaIss = itemTributacao.ISSAliquota,
                    IdCNAE = NFSeServico.Servico.CodigoTributacaoMunicipal ?? "",
                    CNAE = Empresa.CNAE,
                    CodigoTributario = NFSeServico.Servico.CodigoTributacaoMunicipal ?? "",
                    Descricao = NFSeServico.Servico.Descricao.ToUpper(),
                    Quantidade = NFSeServico.Quantidade,
                    ValorUnitario = NFSeServico.Valor,
                    ValorTotal = (NFSeServico.Quantidade * NFSeServico.Valor),
                    BaseCalculo = (NFSeServico.Quantidade * NFSeServico.Valor),
                    ISSRetido = itemTributacao.RetemISS ? TipoSimNao.Sim : TipoSimNao.Nao,//TODO: rever de acordo com a configuração
                    ValorDeducoes = 0.0,//Fixo
                    ValorPIS = itemTributacao.PISValor,
                    ValorCofins = itemTributacao.COFINSValor,
                    ValorINSS = itemTributacao.INSSValor,
                    ValorIR = itemTributacao.ImpostoRendaValor,
                    ValorCSLL = itemTributacao.CSLLValor,
                    ValorISS = itemTributacao.ISSValor,
                    ValorISSRetido = itemTributacao.ISSValorRetencao,
                    ValorOutrasRetencoes = somaRetencoes,
                    DescontoCondicional = 0.00,
                    DescontoIncondicional = NFSeServico.Desconto,
                    CodigoIBGEPrestador = Empresa.Cidade?.CodigoIbge ?? ""
                });
            };

            return result;
        }

        private Tomador ObterTomador()
        {
            return new Tomador()
            {
                InscricaoMunicipal = Cliente.InscricaoMunicipal ?? "",
                CpfCnpj = Cliente.CPFCNPJ ?? "",
                RazaoSocial = Cliente.Nome ?? "",
                Logradouro = Cliente.Endereco ?? "",
                NumeroEndereco = Cliente.Numero ?? "",
                Bairro = Cliente.Bairro ?? "",
                CodigoMunicipioIBGE = Cliente.Cidade?.CodigoIbge ?? "",
                Cidade = Cliente.Cidade?.Nome ?? "",
                UF = Cliente.Estado?.Sigla ?? "",
                CEP = Cliente.CEP ?? "",
                Email = Cliente.Email ?? "",
                Telefone = SubstringTelefone(Cliente.Telefone) ?? "",
                InscricaoEstadual = Cliente.InscricaoEstadual ?? "",
                SituacaoEspecial = Cliente.SituacaoEspecialNFS,
                ConsumidorFinal = Cliente.ConsumidorFinal
            };
        }

        private Prestacao ObterPrestacao()
        {
            return new Prestacao()
            {
                Logradouro = Cliente.Endereco ?? "",
                NumeroEndereco = Cliente.Numero ?? "",
                //CodigoMunicipioIBGE = Cliente.Cidade?.CodigoIbge ?? "",
                //Municipio = Cliente.Cidade?.Nome ?? "",
                CodigoMunicipioIBGE = "999",
                Municipio = "Homologação",
                Bairro = Cliente.Bairro ?? "",
                UF = Cliente.Estado?.Sigla ?? "",
                CEP = Cliente.CEP ?? ""
            };
        }

        private Prestador ObterPrestador()
        {
            return new Prestador()
            {
                InscricaoMunicipalPrestador = Empresa.InscricaoMunicipal ?? "",
                CpfCnpj = Empresa.CNPJ ?? "",
                RazaoSocial = Empresa.RazaoSocial ?? "",
                NomeFantasia = Empresa.NomeFantasia ?? "",
                CodigoMunicipioIBGE = Empresa.Cidade?.CodigoIbge ?? "",
                Cidade = Empresa.Cidade?.Nome ?? "",
                UF = Empresa.Cidade?.Estado?.Sigla ?? "",
                Telefone = SubstringTelefone(Empresa.Telefone) ?? "",
                TipoIcentivoCultural = ParametrosTributarios.IncentivoCultura ? TipoSimNao.Sim : TipoSimNao.Nao,
                Logradouro = Empresa.Endereco ?? "",
                NumeroEndereco = Empresa.Numero ?? "",
                Bairro = Empresa.Bairro ?? "",
                CEP = Empresa.CEP ?? ""
            };
        }

        private Atividade ObterAtividade()
        {
            return new Atividade()
            {
                CodigoCNAE = Empresa.CNAE,
                AliquotaIss = ParametrosTributarios.AliquotaISS,
            };
        }

        private Identificacao ObterIdentificacao()
        {
            return new Identificacao()
            {
                TipoTributacao = ParametrosTributarios.TipoTributacaoNFS,
                CodigoIBGEPrestador = Empresa.Cidade?.CodigoIbge ?? "",
                DataHoraEmissao = DateTime.Now,
                //SerieRPS = TransmissaoNFSBLs.SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == NFSe.SerieNotaFiscalId).FirstOrDefault().Serie.ToUpper(),
                SerieRPS = "8",
                NumeroRPS = NFSe.NumNotaFiscal.Value,
                CompetenciaRPS = DateTime.Now
            };
        }

        private Valores ObterValores()
        {
            var itensTributacoes = new List<NotaFiscalItemTributacao>();
            foreach (var NFSeServico in ObterNFSeServicos())
            {
                itensTributacoes.Add(TransmissaoNFSBLs.NotaFiscalItemTributacaoBL.All.Where(x => x.NotaFiscalItemId == NFSeServico.Id).FirstOrDefault());
            }

            return new Valores()
            {
                //Podem ter impostos distintos em cada serviço
                //mas na aglutinação será juntado em 1 serviço só os impostos
                AliquotasCOFINS = itensTributacoes.Any(x => x.COFINSAliquota > 0) ? itensTributacoes.FirstOrDefault(x => x.COFINSAliquota > 0).COFINSAliquota : 0,
                AliquotasCSLL = itensTributacoes.Any(x => x.CSLLAliquota > 0) ? itensTributacoes.FirstOrDefault(x => x.CSLLAliquota > 0).CSLLAliquota : 0,
                AliquotasINSS = itensTributacoes.Any(x => x.INSSAliquota > 0) ? itensTributacoes.FirstOrDefault(x => x.INSSAliquota > 0).INSSAliquota : 0,
                AliquotasIR = itensTributacoes.Any(x => x.ImpostoRendaAliquota > 0) ? itensTributacoes.FirstOrDefault(x => x.ImpostoRendaAliquota > 0).ImpostoRendaAliquota : 0,
                AliquotasISS = itensTributacoes.Any(x => x.ISSAliquota > 0) ? itensTributacoes.FirstOrDefault(x => x.ISSAliquota > 0).ISSAliquota : 0,
                AliquotasPIS = itensTributacoes.Any(x => x.PISAliquota > 0) ? itensTributacoes.FirstOrDefault(x => x.PISAliquota > 0).PISAliquota : 0,
            };
        }

        public string FormatarCodigoISS(string codigoISS = "")
        {
            if (codigoISS.Length == 3)
            {
                codigoISS = codigoISS.Insert(1, ".");
            }
            else if (codigoISS.Length == 4)
            {
                codigoISS = codigoISS.Insert(2, ".");
            }

            return codigoISS;
        }
    }
}
