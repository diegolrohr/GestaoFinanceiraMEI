using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core;
using Fly01.Core.Api.BL;
using Fly01.Core.Notifications;
using System.Dynamic;
using System.Linq;

namespace Fly01.EmissaoNFE.BL
{
    public class TributacaoBL : PlataformaBaseBL<Tributacao>
    {
        protected TabelaIcmsBL TabelaIcmsBL;
        protected IcmsBL IcmsBL;
        protected DifalBL DifalBL;
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL;
        protected IpiBL IpiBL;
        protected NcmBL NcmBL;
        protected FcpBL FcpBL;

        public TributacaoBL(AppDataContextBase context, TabelaIcmsBL tabelaIcmsBL, NcmBL ncmBL, IcmsBL icmsBL, DifalBL difalBL, SubstituicaoTributariaBL substituicaoTributariaBL, IpiBL ipiBL, FcpBL fcpBL) : base(context)
        {
            TabelaIcmsBL = tabelaIcmsBL;
            IcmsBL = icmsBL;
            DifalBL = difalBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            IpiBL = ipiBL;
            NcmBL = ncmBL;
            FcpBL = fcpBL;
        }
        
        public TributacaoRetornoVM GeraImpostos(Tributacao entity)
        {
            var retorno = new TributacaoRetornoVM();
            
            if (entity.Ipi != null)
                retorno.Ipi = IpiBL.Ipi(entity);

            if(entity.SubstituicaoTributaria != null)
                retorno.SubstituicaoTributaria = SubstituicaoTributariaBL.SubstituicaoTributaria(entity, TabelaIcmsBL);
            
            if(entity.Icms != null)
            {
                if (entity.Icms.Difal)
                    retorno.Difal = DifalBL.Difal(entity, TabelaIcmsBL);

                retorno.Icms = IcmsBL.Icms(entity, TabelaIcmsBL);

                if (entity.Fcp != null)
                    retorno.Fcp = FcpBL.Fcp(entity);
            }      

            return retorno;
        }
        
        public override void ValidaModel(Tributacao entity)
        {
            #region Validações Entity
            entity.Fail(entity.ValorBase <= 0, ValorBaseInvalido);
            entity.Fail(!string.IsNullOrEmpty(entity.ValorDespesa.ToString()) && entity.ValorDespesa < 0, ValorDespesaNegativo);
            entity.Fail(!string.IsNullOrEmpty(entity.ValorFrete.ToString()) && entity.ValorFrete < 0, ValorFreteNegativo);
            #endregion

            #region Validações Entity.Icms
            entity.Fail(((entity.Icms != null && entity.Icms.IpiNaBase) || (entity.SubstituicaoTributaria != null && entity.SubstituicaoTributaria.IpiNaBase)) && entity.Ipi == null, IpiRequerido);
            entity.Fail(entity.Icms != null && (string.IsNullOrEmpty(entity.Icms.EstadoOrigem) || !TabelaIcmsBL.All.Where(x => x.SiglaOrigem == entity.Icms.EstadoOrigem).Any()), EstadoOrigemInvalido);
            entity.Fail(entity.Icms != null && (string.IsNullOrEmpty(entity.Icms.EstadoDestino) || !TabelaIcmsBL.All.Where(x => x.SiglaDestino == entity.Icms.EstadoDestino).Any()), EstadoDestinoInvalido);
            entity.Fail(entity.Icms != null && entity.SubstituicaoTributaria != null && entity.Icms.Difal, DifalComIcmsST);
            #endregion

            #region Validações Entity.SubstituicaoTributaria
            entity.Fail(entity.SubstituicaoTributaria != null && (string.IsNullOrEmpty(entity.SubstituicaoTributaria.EstadoOrigem) || !TabelaIcmsBL.All.Where(x => x.SiglaOrigem == entity.SubstituicaoTributaria.EstadoOrigem).Any()), EstadoOrigemSTInvalido);
            entity.Fail(entity.SubstituicaoTributaria != null && (string.IsNullOrEmpty(entity.SubstituicaoTributaria.EstadoDestino) || !TabelaIcmsBL.All.Where(x => x.SiglaDestino == entity.SubstituicaoTributaria.EstadoDestino).Any()), EstadoDestinoSTInvalido);
            entity.Fail(entity.SubstituicaoTributaria != null && entity.Icms != null && !string.IsNullOrEmpty(entity.SubstituicaoTributaria.EstadoOrigem) && !string.IsNullOrEmpty(entity.Icms.EstadoOrigem) && entity.SubstituicaoTributaria.EstadoOrigem != entity.Icms.EstadoOrigem, EstadoOrigemSTDifereDeIcms);
            entity.Fail(entity.SubstituicaoTributaria != null && entity.Icms != null && !string.IsNullOrEmpty(entity.SubstituicaoTributaria.EstadoDestino) && !string.IsNullOrEmpty(entity.Icms.EstadoDestino) && entity.SubstituicaoTributaria.EstadoDestino != entity.Icms.EstadoDestino, EstadoDestinoSTDifereDeIcms);
            entity.Fail(entity.SubstituicaoTributaria != null && entity.SubstituicaoTributaria.Mva <= 0, MvaInvalido);
            #endregion

            #region Validações Entity.Ipi
            entity.Fail(entity.Ipi != null && !entity.Ipi.AliquotaPeloNcm && entity.Ipi.Aliquota <= 0, AliquotaIpiInvalida);
            entity.Fail(entity.Ipi != null && entity.Ipi.AliquotaPeloNcm && string.IsNullOrEmpty(entity.Ipi.Ncm), NcmInvalido);
            entity.Fail(entity.Ipi != null && entity.Ipi.AliquotaPeloNcm && entity.Ipi.Ncm.Length != 8, NcmCodigoInvalido);
            entity.Fail(entity.Ipi != null && entity.Ipi.AliquotaPeloNcm && !string.IsNullOrEmpty(entity.Ipi.Ncm) && entity.Ipi.Ncm.Length == 8 && !NcmBL.All.Where(x => x.Codigo == entity.Ipi.Ncm).Any(), NcmNaoEncontrado);
            #endregion

            #region Validações Entity.Fcp
            entity.Fail(entity.Fcp != null && entity.Icms == null, FcpSemIcms);
            entity.Fail(entity.Fcp != null && entity.Fcp.Aliquota <= 0, AliquotaFcpInvalida);
            #endregion

            base.ValidaModel(entity);
        }

        #region ErrorMessages Entity
        public static Error ValorBaseInvalido = new Error("Valor base deve ser maior que zero.", "ValorBase");
        public static Error ValorDespesaNegativo = new Error("Valor de despesas deve ser maior ou igual a zero.", "ValorDespesa");
        public static Error ValorFreteNegativo = new Error("Valor de frete deve ser maior ou igual a zero.", "ValorFrete");
        #endregion

        #region ErrorMessages Entity.Icms
        public static Error IpiRequerido = new Error("Informe os dados de IPI, pois ICMS e/ou Substituição Tributária solicitam este dado em sua base.", "Ipi");
        public static Error EstadoOrigemInvalido = new Error("Estado de origem inválido.", "Icms.EstadoOrigem");
        public static Error EstadoDestinoInvalido = new Error("Estado de destino inválido.", "Icms.EstadoDestino");
        public static Error DifalComIcmsST = new Error("Não é possível calcular Difal e Substituição Tributária na mesma operação.", "Icms.Difal");
        #endregion

        #region ErrorMessages Entity.SubstituicaoTributaria
        public static Error EstadoOrigemSTInvalido = new Error("Estado de origem inválido.", "SubstituicaoTributaria.EstadoOrigem");
        public static Error EstadoDestinoSTInvalido = new Error("Estado de destino inválido.", "SubstituicaoTributaria.EstadoDestino");
        public static Error EstadoOrigemSTDifereDeIcms = new Error("Estado de origem da Substituição Tributária difere do informado para o ICMS.", "SubstituicaoTributaria.EstadoOrigem");
        public static Error EstadoDestinoSTDifereDeIcms = new Error("Estado de destino da Substituição Tributária difere do informado para o ICMS.", "SubstituicaoTributaria.EstadoDestino");
        public static Error MvaInvalido = new Error("O MVA deve ser maior que zero.", "SubstituicaoTributaria.Mva");
        #endregion

        #region ErrorMessages Entity.Ipi
        public static Error AliquotaIpiInvalida = new Error("Alíquota de IPI deve ser maior que zero.", "Ipi.Aliquota");
        public static Error NcmInvalido = new Error("NCM inválido para busca de alíquota.", "Ipi.Ncm");
        public static Error NcmCodigoInvalido = new Error("O código NCM tem 8 caracteres.", "Ipi.Ncm");
        public static Error NcmNaoEncontrado = new Error("NCM não encontrado.", "Ipi.Ncm");
        #endregion

        #region ErrorMessages Entity.Fcp
        public static Error FcpSemIcms = new Error("Não é possível calcular FCP sem ICMS.", "Fcp.Aliquota");
        public static Error AliquotaFcpInvalida = new Error("Alíquota de FCP deve ser maior que zero.", "Ipi.Aliquota");
        #endregion
    }
}
