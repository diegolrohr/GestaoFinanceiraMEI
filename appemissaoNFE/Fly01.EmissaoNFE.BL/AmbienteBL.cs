using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;

namespace Fly01.EmissaoNFE.BL
{
    public class AmbienteBL : PlataformaBaseBL<ParametroVM>
    {
        protected EntidadeBL EntidadeBL;
        public AmbienteBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(ParametroVM entity)
        {
            EntidadeBL.ValidaModel(entity);
            
            int x = default(int);

            entity.Fail(!Int32.TryParse(entity.TipoAmbiente, out x) || x < 0 || x > 2, AmbienteInvalido);

            base.ValidaModel(entity);
        }

        public static Error AmbienteInvalido = new Error("Codigo do ambiente inválido.", "Ambiente");
    }
}
