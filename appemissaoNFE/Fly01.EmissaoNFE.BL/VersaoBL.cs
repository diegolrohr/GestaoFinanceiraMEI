using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;

namespace Fly01.EmissaoNFE.BL
{
    public class VersaoBL : PlataformaBaseBL<ParametroVM>
    {
        protected EntidadeBL EntidadeBL;
        public VersaoBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(ParametroVM entity)
        {
            EntidadeBL.ValidaModel(entity);
            
            entity.Fail(!(entity.VersaoNFe == "3.10" || entity.VersaoNFe == "4.0"), VersaoInvalida);

            base.ValidaModel(entity);
        }

        public static Error VersaoInvalida = new Error("Versão de NFe inválida", "VersaoNFe");
    }
}