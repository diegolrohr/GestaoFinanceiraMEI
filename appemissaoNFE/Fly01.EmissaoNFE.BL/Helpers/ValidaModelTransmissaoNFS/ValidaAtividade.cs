using System;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaAtividade
    {
        internal static void ExecutaValidaAtividade(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if (entity.ItemTransmissaoNFSVM.Atividade == null)
            {
                entity.Fail(true, new Error("A entidade atividade não pode ser nula"));
            }
            else
            {
                ValidarCodigoAtividade(entity);
                ValidarAliquota(entity);
            }
        }
        
        private static void ValidarAliquota(TransmissaoNFSVM entity)
        {
            entity.Fail(entity.ItemTransmissaoNFSVM.Atividade.AliquotaIss <= 0, new Error("Alíquota Iss da atividade deve ser superior ou igual a zero.", "AliquotaICMS"));
        }

        private static void ValidarCodigoAtividade(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Atividade.CodigoCNAE.ToString()), new Error("Código atividade é um dado obrigatório.", "CodigoCNAE"));
        }
    }
}
