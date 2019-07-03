using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
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
        protected FcpStBL FcpStBL;
        protected PisBL PisBL;
        protected CofinsBL CofinsBL;
        protected InssBL InssBL;
        protected ImpostoRendaBL ImpostoRendaBL;
        protected CsllBL CsllBL;
        protected IssBL IssBL;

        public TributacaoBL(AppDataContextBase context, TabelaIcmsBL tabelaIcmsBL, NcmBL ncmBL, IcmsBL icmsBL, DifalBL difalBL, SubstituicaoTributariaBL substituicaoTributariaBL,
            IpiBL ipiBL, FcpBL fcpBL, FcpStBL fcpStBL, PisBL pisBL, CofinsBL cofinsBL, InssBL inssBL, ImpostoRendaBL impostoRendaBL, CsllBL csllBL, IssBL issBL) : base(context)
        {
            TabelaIcmsBL = tabelaIcmsBL;
            IcmsBL = icmsBL;
            DifalBL = difalBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            IpiBL = ipiBL;
            NcmBL = ncmBL;
            FcpBL = fcpBL;
            FcpStBL = fcpStBL;
            PisBL = pisBL;
            CofinsBL = cofinsBL;
            InssBL = inssBL;
            ImpostoRendaBL = impostoRendaBL;
            CsllBL = csllBL;
            IssBL = issBL;
        }
        
        public TributacaoRetornoVM GeraImpostos(Tributacao entity)
        {
            var retorno = new TributacaoRetornoVM();
            
            if (entity.Ipi != null)
                retorno.Ipi = IpiBL.Ipi(entity);

            if(entity.SubstituicaoTributaria != null)
            {
                retorno.SubstituicaoTributaria = SubstituicaoTributariaBL.CalcularSubstituicaoTributaria(entity, TabelaIcmsBL);

                if (entity.FcpSt != null)
                    retorno.FcpSt = FcpStBL.FcpSt(entity);
            }
            
            if(entity.Icms != null)
            {
                if (entity.Icms.Difal)
                    retorno.Difal = DifalBL.Difal(entity, TabelaIcmsBL);

                retorno.Icms = IcmsBL.Icms(entity, TabelaIcmsBL);

                if (entity.Fcp != null)
                    retorno.Fcp = FcpBL.Fcp(entity);
            }

            if (entity.Pis != null)
                retorno.Pis = PisBL.Pis(entity);

            if (entity.Cofins != null)
                retorno.Cofins = CofinsBL.Cofins(entity);

            if (entity.Inss != null)
                retorno.Inss = InssBL.Inss(entity);

            if (entity.ImpostoRenda != null)
                retorno.ImpostoRenda = ImpostoRendaBL.ImpostoRenda(entity);

            if (entity.Csll != null)
                retorno.Csll = CsllBL.Csll(entity);

            if (entity.Iss != null)
                retorno.Iss = IssBL.Iss(entity);

            return retorno;
        }
        
        public override void ValidaModel(Tributacao entity)
        {
            #region Validações Entity
            entity.Fail(entity.ValorBase < 0, ValorBaseInvalido);
            entity.Fail(!string.IsNullOrEmpty(entity.ValorDespesa.ToString()) && entity.ValorDespesa < 0, ValorDespesaNegativo);
            entity.Fail(!string.IsNullOrEmpty(entity.ValorFrete.ToString()) && entity.ValorFrete < 0, ValorFreteNegativo);
            #endregion

            #region Validações Entity.Icms
            if(entity.Icms != null)
            {
                entity.Fail(!entity.Icms.Aliquota.HasValue, AliquotaRequerida);
                entity.Fail((entity.Icms.IpiNaBase || (entity.SubstituicaoTributaria != null && entity.SubstituicaoTributaria.IpiNaBase)) && entity.Ipi == null, IpiRequerido);
                entity.Fail(string.IsNullOrEmpty(entity.Icms.EstadoOrigem) || !TabelaIcmsBL.All.Where(x => x.SiglaOrigem == entity.Icms.EstadoOrigem).Any(), EstadoOrigemInvalido);
                entity.Fail(string.IsNullOrEmpty(entity.Icms.EstadoDestino) || !TabelaIcmsBL.All.Where(x => x.SiglaDestino == entity.Icms.EstadoDestino).Any(), EstadoDestinoInvalido);
                entity.Fail(entity.SubstituicaoTributaria != null && entity.Icms.Difal, DifalComIcmsST);
            }
            #endregion

            #region Validações Entity.SubstituicaoTributaria
            if(entity.SubstituicaoTributaria != null)
            {
                entity.Fail(!TabelaIcmsBL.All.Any(x => x.SiglaOrigem == entity.SubstituicaoTributaria.EstadoOrigem), EstadoOrigemSTInvalido);
                entity.Fail(!TabelaIcmsBL.All.Any(x => x.SiglaDestino == entity.SubstituicaoTributaria.EstadoDestino), EstadoDestinoSTInvalido);
                entity.Fail(entity.Icms != null && entity.SubstituicaoTributaria.EstadoOrigem != entity.Icms.EstadoOrigem, EstadoOrigemSTDifereDeIcms);
                entity.Fail(entity.Icms != null && entity.SubstituicaoTributaria.EstadoDestino != entity.Icms.EstadoDestino, EstadoDestinoSTDifereDeIcms);
                entity.Fail(entity.SubstituicaoTributaria.Mva <= 0, MvaInvalido);
            }
            #endregion

            #region Validações Entity.Ipi
            entity.Fail(entity.Ipi != null && !entity.Ipi.AliquotaPeloNcm && entity.Ipi.Aliquota <= 0, AliquotaIpiInvalida);
            entity.Fail(entity.Ipi != null && entity.Ipi.AliquotaPeloNcm && string.IsNullOrEmpty(entity.Ipi.Ncm), NcmInvalido);
            entity.Fail(entity.Ipi != null && entity.Ipi.AliquotaPeloNcm && entity.Ipi.Ncm.Length != 8, NcmCodigoInvalido);
            entity.Fail(entity.Ipi != null && entity.Ipi.AliquotaPeloNcm && !string.IsNullOrEmpty(entity.Ipi.Ncm) && entity.Ipi.Ncm.Length == 8 && !NcmBL.All.Where(x => x.Codigo == entity.Ipi.Ncm).Any(), NcmNaoEncontrado);
            #endregion

            #region Validações Entity.Fcp
            entity.Fail(entity.Fcp != null && entity.Icms == null, FcpSemIcms);
            entity.Fail(entity.Fcp != null && entity.Fcp.Aliquota < 0, AliquotaFcpInvalida);
            #endregion
            
            #region Validações Entity.Fcp ST 
            entity.Fail(entity.FcpSt != null && entity.SubstituicaoTributaria == null, FcpStSemIcms);
            entity.Fail(entity.FcpSt != null && entity.FcpSt.Aliquota < 0, AliquotaFcpStInvalida);
            #endregion

            #region Validações Entity.Pis
            entity.Fail(entity.Pis != null && entity.Pis.Aliquota < 0, new Error("Alíquota de PIS inválida.", "Pis.Aliquota"));
            #endregion

            #region Validações Entity.Cofins
            entity.Fail(entity.Cofins != null && entity.Cofins.Aliquota < 0, new Error("Alíquota de COFINS inválida.", "Cofins.Aliquota"));
            #endregion

            #region Validações Entity.Inss
            entity.Fail(entity.Inss != null && entity.Inss.Aliquota < 0, new Error("Alíquota de INSS inválida.", "Inss.Aliquota"));
            #endregion

            #region Validações Entity.ImpostoRenda
            entity.Fail(entity.ImpostoRenda != null && entity.ImpostoRenda.Aliquota < 0, new Error("Alíquota do Imposto de Renda inválida.", "ImpostoRenda.Aliquota"));
            #endregion

            #region Validações Entity.Csll
            entity.Fail(entity.Csll != null && entity.Csll.Aliquota < 0, new Error("Alíquota do CSLL inválida.", "Csll.Aliquota"));
            #endregion

            base.ValidaModel(entity);
        }

        #region ErrorMessages Entity
        public static Error ValorBaseInvalido = new Error("Valor base deve ser maior ou igual a zero.", "ValorBase");
        public static Error ValorDespesaNegativo = new Error("Valor de despesas deve ser maior ou igual a zero.", "ValorDespesa");
        public static Error ValorFreteNegativo = new Error("Valor de frete deve ser maior ou igual a zero.", "ValorFrete");
        #endregion

        #region ErrorMessages Entity.Icms
        public static Error AliquotaRequerida = new Error("Alíquota do ICMS é obrigatória.", "Icms.Aliquota");
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
        public static Error AliquotaFcpInvalida = new Error("Alíquota de FCP inválida.", "Fcp.Aliquota");
        public static Error FcpStSemIcms = new Error("Não é possível calcular FCP sem Substituição Tributária.", "FcpSt.Aliquota");
        public static Error AliquotaFcpStInvalida = new Error("Alíquota de FCP ST inválida.", "FcpSt.Aliquota");
        #endregion
    }
}
