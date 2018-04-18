using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.EmissaoNFE.BL
{
    public class CertificadoBL : PlataformaBaseBL<CertificadoVM>
    {
        protected EntidadeBL EntidadeBL;
        public CertificadoBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(CertificadoVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            entity.Fail(entity.Certificado == null || entity.Certificado.Length == 0, CertificadoInvalido);
            entity.Fail(entity.Senha == null || entity.Senha.Length == 0, SenhaInvalida);
            if(entity.Certificado != null)
            {
                var md5 = Base64Helper.CalculaMD5Hash(entity.Certificado);
                entity.Fail(entity.MD5 != md5, MD5Invalido);
            }
            base.ValidaModel(entity);
        }
        
        public static Error CertificadoInvalido = new Error("Certificado não informado", "Certificado");
        public static Error SenhaInvalida = new Error("Senha não informada", "Senha");
        public static Error MD5Invalido = new Error("MD5 de destino difere da origem", "MD5");
    }
}
