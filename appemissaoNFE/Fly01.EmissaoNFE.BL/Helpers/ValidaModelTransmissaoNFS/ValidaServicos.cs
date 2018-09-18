using System;
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
            ValidarDiscriminacaoServico(entity);
        }

        private static void ValidarDiscriminacaoServico(Servico entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Discriminacao), new Error("Discrimonação é um dado obrigatório."));
        }

        private static void ValidarCodigoServico(Servico entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Codigo.ToString()), new Error("Código do serviço é um dado obrigatório."));
        }
    }
}
