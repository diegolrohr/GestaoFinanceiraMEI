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
            ValidarCodigoTributacao(entity);
        }

        private static void ValidarCodigoTributacao(Servico entity)
        {
            entity.Fail(entity.CodigoTributario.ToString().Length > 20, new Error("O código da tributação não pode ter mais que 20 caracteres."));
        }

        private static void ValidarDiscriminacaoServico(Servico entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Discriminacao), new Error("Discrimonação é um dado obrigatório."));
            entity.Fail(entity.Discriminacao.Length > 9, new Error("O código do serviço não pode ter mais que 9 caracteres."));
        }

        private static void ValidarCodigoServico(Servico entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Codigo.ToString()), new Error("Código do serviço é um dado obrigatório."));
        }
    }
}
