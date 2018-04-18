using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;

namespace Fly01.EmissaoNFE.BL
{
    public class CceBL : PlataformaBaseBL<CceVM>
    {
        protected EntidadeBL EntidadeBL;
        public CceBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(CceVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            int x = default(int);

            entity.Fail(!Int32.TryParse(entity.TipoAmbienteCCE, out x) || x < 0 || x > 2, AmbienteCCEInvalido);
            entity.Fail(!Int32.TryParse(entity.TipoAmbienteEPP, out x) || x < 0 || x > 2, AmbienteEPPInvalido);
            entity.Fail(!Int32.TryParse(entity.TipoFusoHorario, out x) || x < 1 || x > 4, FusoHorarioInvalido);
            
            base.ValidaModel(entity);
        }

        public static Error AmbienteCCEInvalido = new Error("Codigo do ambiente CCE inválido.", "AmbienteCCE");
        public static Error AmbienteEPPInvalido = new Error("Codigo do ambiente EPP inválido.", "AmbienteEPP");
        public static Error FusoHorarioInvalido = new Error("Codigo de fuso horário inválido.", "FusoHorario");
    }
}
