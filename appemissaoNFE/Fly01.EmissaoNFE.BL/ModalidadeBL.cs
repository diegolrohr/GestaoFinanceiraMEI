using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;

namespace Fly01.EmissaoNFE.BL
{
    public class ModalidadeBL : PlataformaBaseBL<ParametroVM>
    {
        protected EntidadeBL EntidadeBL;
        public ModalidadeBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(ParametroVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            int x = default(int);

            entity.Fail(!Int32.TryParse(entity.TipoModalidade, out x) || x < 1 || x > 7, ModalidadeInvalida);

            base.ValidaModel(entity);
        }

        public static Error ModalidadeInvalida = new Error("Modalidade inválida", "Modalidade");
    }
}
