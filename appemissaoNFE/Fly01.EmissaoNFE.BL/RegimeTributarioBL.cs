using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;

namespace Fly01.EmissaoNFE.BL
{
    public class RegimeTributarioBL : PlataformaBaseBL<ParametroVM>
    {
        protected EntidadeBL EntidadeBL;
        public RegimeTributarioBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(ParametroVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            int x = default(int);

            entity.Fail(!Int32.TryParse(entity.TipoRegimeTributario, out x) || x < 1 || x > 6, TipoRegimeTributarioInvalido);

            base.ValidaModel(entity);
        }
        
        public static Error TipoRegimeTributarioInvalido = new Error("Regime Especial de Tributação inválido", "RegimeTributario");
    }
}
