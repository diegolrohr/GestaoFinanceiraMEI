using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFs;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaServicos
    {
        internal static void ExecutaValidaServicos(Servico entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            ValidarCodigoServico(entity);
        }

        private static void ValidarCodigoServico(Servico entity)
        {
            //entity.Fail(string.IsNullOrEmpty(entity.Servicos.Bairro), new Error("Bairro do prestador é um dado obrigatório."));
        }
    }
}
