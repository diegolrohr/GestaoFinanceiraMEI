using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public abstract class TransmissaoNFe
    {
        protected NFe NFe { get; set; }
        protected TransmissaoBLs TransmissaoBLs { get; set; }
        protected ParametroTributario ParametrosTributarios { get; set; }

        public TransmissaoNFe(NFe nfe, TransmissaoBLs transmissaoBLs)
        {
            this.NFe = nfe;
            this.TransmissaoBLs = transmissaoBLs;
            ValidaParametrosTributarios();
        }

        public abstract TransmissaoVM ObterTransmissaoVM();

        public void ValidaParametrosTributarios()
        {
            var parametros = TransmissaoBLs.TotalTributacaoBL.GetParametrosTributarios();
            if (parametros == null)
            {
                throw new BusinessException("Acesse o menu Configurações > Parâmetros Tributários e salve as configurações para a transmissão");
            }

            if (parametros.TipoVersaoNFe != TipoVersaoNFe.v4)
            {
                throw new BusinessException("Permitido somente NF-e versão 4.00. Acesse o menu Configurações > Parâmetros Tributários e altere as configurações");
            }
        }
    }
}