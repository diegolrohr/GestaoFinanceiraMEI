using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core;
using Fly01.Core.Api.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;

namespace Fly01.EmissaoNFE.BL
{
    public class DanfeBL : PlataformaBaseBL<DanfeVM>
    {
        protected EntidadeBL EntidadeBL;

        public DanfeBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(DanfeVM entity)
        {
            EntidadeBL.ValidaModel(entity);
            
            entity.Fail(entity.DanfeId == null, IdRequerido);
            if (!string.IsNullOrEmpty(entity.Logo))
            {
                var md5 = Base64Helper.CalculaMD5Hash(entity.Logo);
                entity.Fail(entity.MD5 != md5, MD5Invalido);
            }

            base.ValidaModel(entity);
        }

        public static Error IdRequerido = new Error("ID da DANFE é um campo obrigatório.", "DanfeId");
        public static Error MD5Invalido = new Error("MD5 de destino difere da origem", "MD5");
    }
}
