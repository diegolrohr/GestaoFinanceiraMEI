using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaServicos
    {
        internal static void ExecutaValidaServicos(EntitiesBLToValidateNFS entitiesBLToValidateNFS, TransmissaoNFSVM entity)
        {
            if (entity.ItemTransmissaoNFSVM.Servicos == null)
            {
                entity.Fail(true, new Error("A entidade servico não pode ser nula"));
            }
            else
            {
                foreach (var item in entity.ItemTransmissaoNFSVM.Servicos)
                {
                    ValidarCodigoServico(entity, item);
                    ValidarDiscriminacaoServico(entity, item);
                    ValidarCodigoTributacao(entity, item);
                }
            }
        }

        private static void ValidarCodigoTributacao(TransmissaoNFSVM entity, Servico item)
        {
            entity.Fail(string.IsNullOrEmpty(item.CodigoTributario.ToString()), new Error("Código tributário do serviço é um dado obrigatório."));
            entity.Fail(item.CodigoTributario.ToString().Length > 20, new Error("O código da tributação não pode ter mais que 20 caracteres."));
        }

        private static void ValidarDiscriminacaoServico(TransmissaoNFSVM entity, Servico item)
        {
            entity.Fail(string.IsNullOrEmpty(item.Discriminacao), new Error("Discrimonação é um dado obrigatório."));
        }

        private static void ValidarCodigoServico(TransmissaoNFSVM entity, Servico item)
        {
            entity.Fail(string.IsNullOrEmpty(item.Codigo.ToString()), new Error("Código do serviço é um dado obrigatório."));
            entity.Fail(item.Codigo.ToString().Length > 9, new Error("O código do serviço não pode ter mais que 9 caracteres."));
        }
    }
}
