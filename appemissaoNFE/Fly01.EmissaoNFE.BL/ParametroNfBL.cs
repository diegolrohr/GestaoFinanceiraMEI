using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core;
using Fly01.Core.Api.BL;
using Fly01.Core.Notifications;
using System;

namespace Fly01.EmissaoNFE.BL
{
    public class ParametroNfBL : PlataformaBaseBL<ParametroVM>
    {
        protected EntidadeBL EntidadeBL;
        public ParametroNfBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(ParametroVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            int x = default(int);
            entity.Fail(!Int32.TryParse(entity.TipoAmbiente, out x) || x < 0 || x > 2, AmbienteInvalido);
            entity.Fail(!Int32.TryParse(entity.TipoModalidade, out x) || x < 1 || x > 7, ModalidadeInvalida);
            entity.Fail(!(entity.VersaoNFe == "3.10" || entity.VersaoNFe == "4.0"), VersaoInvalida);
            entity.Fail(!string.IsNullOrEmpty(entity.VersaoNFSe) && !Int32.TryParse(entity.VersaoNFSe.Replace(".", ""), out x), VersaoNFSeInvalida);
            entity.Fail(!string.IsNullOrEmpty(entity.VersaoDPEC) && !Int32.TryParse(entity.VersaoDPEC.Replace(".", ""), out x), VersaoDPECInvalida);

            base.ValidaModel(entity);
        }

        public static Error AmbienteInvalido = new Error("Codigo do ambiente inválido.", "Ambiente");
        public static Error ModalidadeInvalida = new Error("Modalidade inválida", "Modalidade");
        public static Error VersaoInvalida = new Error("Versão de NFe inválida", "VersaoNFe");
        public static Error VersaoNFSeInvalida = new Error("Versão de NFSe inválida", "VersaoNFSe");
        public static Error VersaoDPECInvalida = new Error("Versão de DPEC inválida", "VersaoDPEC");
    }
}
