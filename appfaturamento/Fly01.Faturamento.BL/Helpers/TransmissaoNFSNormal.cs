using System;
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
            // falta Obter serviço e valores
            return ObterTransmissaoApartirDoItem(itemTransmissaoNFS);
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
            this.ParametrosTributarios = TransmissaoNFSBLs.TotalTributacaoBL.GetParametrosTributarios();
            if (this.ParametrosTributarios == null)
            {
                throw new BusinessException("Acesse o menu Configurações > Parâmetros Tributários e salve as configurações para a transmissão");
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
                Valores = ObterValores(),
                InformacoesComplementares = ObterInformacoesComplementares()
            };
        }

        private InformacoesComplementares ObterInformacoesComplementares()
        {
            return new InformacoesComplementares()
            {
                Descricao = NFSe.MensagemPadraoNota?? "",
                Observacao =  ""
            };
        }

        private Valores ObterValores()
        {
            //TODO preencher só as aliquotas. Para preencher esses dados, é necessario incluir os novos impostos nos parametros tributarios
            throw new NotImplementedException();
        }

        private Tomador ObterTomador()
        {
            return new Tomador()
            {
                InscricaoMunicipal = Cliente.InscricaoMunicipal?? "",
                CpfCnpj = Cliente.CPFCNPJ?? "",
                RazaoSocial = Cliente.Nome?? "",
                Logradouro = Cliente.Endereco?? "",
                NumeroEndereco = Cliente.Numero?? "",
                Bairro = Cliente.Bairro?? "",
                CodigoMunicipioIBGE = Cliente.CidadeCodigoIbge?? "",
                Cidade = Cliente.Cidade?.Nome??"",
                UF = Cliente.Cidade?.Estado?.Sigla?? "",
                CEP = Cliente.CEP?? "",
                Email = Cliente.Email?? "",
                Telefone = Cliente.Telefone?? "",
                InscricaoEstadual = Cliente.InscricaoEstadual?? "",
                SituacaoEspecial = Cliente.SituacaoEspecialNFS

            };
        }

        private Prestacao ObterPrestacao()
        {
            return new Prestacao()
            {
                Logradouro = Cliente.Endereco?? "",
                NumeroEndereco = Cliente.Numero?? "",
                CodigoMunicipioIBGE = Cliente.CidadeCodigoIbge?? "",
                Municipio = Cliente.Cidade?.Nome?? "",
                Bairro = Cliente.Bairro?? "",
                UF = Empresa.Cidade?.Estado?.Sigla?? "", 
                CEP = Empresa.CEP?? ""
            };
        }

        private Prestador ObterPrestador()
        {
            return new Prestador()
            {
                InscricaoMunicipalPrestador = Empresa.InscricaoMunicipal?? "",
                CpfCnpj = Empresa.CNPJ?? "",
                RazaoSocial = Empresa.RazaoSocial?? "",
                NomeFantasia= Empresa.NomeFantasia?? "",
                CodigoMunicipioIBGE = Empresa.Cidade?.CodigoIbge?? "",
                Cidade = Empresa.Cidade?.Nome?? "",
                UF = Empresa.Cidade?.Estado?.Sigla?? "",
                Telefone = Empresa.Telefone?? "",
                TipoIcentivoCultural = ParametrosTributarios.IncentivoCultura ? TipoSimNao.Sim : TipoSimNao.Nao,
                Logradouro = Empresa.Endereco?? "",
                NumeroEndereco = Empresa.Numero?? "",
                Bairro = Empresa.Bairro?? "",
                CEP = Empresa.CEP?? ""
            };
        }

        private Atividade ObterAtividade()
        {
            return new Atividade()
            {
                CodigoCNAE = Empresa.CNAE,
                AliquotaICMS = ParametrosTributarios.AliquotaISS,//TODO Tem que verificar IIS
            };
        }

        private Identificacao ObterIdentificacao()
        {
            return new Identificacao()
            {
                CodigoIBGEPrestador = Empresa.Cidade?.CodigoIbge ?? "",
                DataHoraEmissao = DateTime.Now,
                SerieRPS = TransmissaoNFSBLs.SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == NFSe.SerieNotaFiscalId).FirstOrDefault().ToString(),//TODO: pode ser minusculo
                NumeroRPS = NFSe.NumNotaFiscal.Value,
            };
        }
    }
}
