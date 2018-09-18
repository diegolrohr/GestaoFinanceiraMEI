using System;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaAtividade
    {
        internal static void ExecutaValidaAtividade(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            ValidarCodigoAtividade(entity);
            ValidarAliquota(entity); // Verificar com o Diego
        }

        private static void ValidarAliquota(TransmissaoNFSVM entity)
        {
            throw new NotImplementedException();
        }

        private static void ValidarCodigoAtividade(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Atividade.Codigo.ToString()), new Error("Código atividade é um dado obrigatório."));
        }
    }
}
