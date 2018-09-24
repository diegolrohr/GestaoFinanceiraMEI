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
            ValidarAliquota(entity);
        }
        
        //TODO revisar com Wilson
        private static void ValidarAliquota(TransmissaoNFSVM entity)
        {
            entity.Fail(entity.Atividade.AliquotaICMS <= 0, new Error("Alíquota ICMS deve ser superior a zero.", "AliquotaICMS"));
        }

        private static void ValidarCodigoAtividade(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Atividade.CodigoCNAE.ToString()), new Error("Código atividade é um dado obrigatório.", "CodigoCNAE"));
        }
    }
}
