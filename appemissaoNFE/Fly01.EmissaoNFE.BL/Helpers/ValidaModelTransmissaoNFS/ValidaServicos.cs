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
                entity.Fail(true, new Error("A entidade serviço não pode ser nula"));
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
            //TODO: verificar obrigatoriedadeentity.Fail(string.IsNullOrEmpty(item.CodigoTributario), new Error("Código tributário municipal do serviço é um dado obrigatório."));
            entity.Fail(item.CodigoTributario?.Length > 20, new Error("Código tributário municipal do serviço ter mais que 20 caracteres."));
        }

        private static void ValidarDiscriminacaoServico(TransmissaoNFSVM entity, Servico item)
        {
            entity.Fail(string.IsNullOrEmpty(item.Descricao), new Error("Descrição do serviço é um dado obrigatório."));
        }

        private static void ValidarCodigoServico(TransmissaoNFSVM entity, Servico item)
        {
            entity.Fail(string.IsNullOrEmpty(item.CodigoIss), new Error("Código Iss do serviço é um dado obrigatório."));
            entity.Fail(item.CodigoIss?.Length > 9, new Error("O código do serviço não pode ter mais que 9 caracteres."));
        }
    }
}
