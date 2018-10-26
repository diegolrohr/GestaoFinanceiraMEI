using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using System.Linq;

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
                var count = 1;
                foreach (var item in entity.ItemTransmissaoNFSVM.Servicos)
                {
                    ValidarCodigoServico(entity, item, count);
                    ValidarDiscriminacaoServico(entity, item, count);
                    ValidarCodigoTributacao(entity, item, count);
                    ValidarUnidadeMedida(entity, item, count);
                    ValidarCodigoFiscalPrestacao(entity, item, count);
                    count++;
                }
            }
        }

        private static void ValidarCodigoFiscalPrestacao(TransmissaoNFSVM entity, Servico item, int count)
        {
            entity.Fail(item.CodigoFiscalPrestacao?.Length > 5, new Error(string.Format("Código fiscal de prestação do serviço {0} não pode ter mais que 5 caracteres.", count)));
        }

        private static void ValidarUnidadeMedida(TransmissaoNFSVM entity, Servico item, int count)
        {
            entity.Fail(item.CodigoTributario?.Length > 2, new Error(string.Format("Sigla da unidade de medida do serviço {0} não pode ter mais que 2 caracteres.", count)));
        }

        private static void ValidarCodigoTributacao(TransmissaoNFSVM entity, Servico item, int count)
        {
            entity.Fail(item.CodigoTributario?.Length > 20, new Error(string.Format("Código tributário municipal do serviço {0} não pode ter mais que 20 caracteres.", count)));
        }

        private static void ValidarDiscriminacaoServico(TransmissaoNFSVM entity, Servico item, int count)
        {
            entity.Fail(string.IsNullOrEmpty(item.Descricao), new Error(string.Format("Descrição do serviço {0} é um dado obrigatório.", count)));
        }

        private static void ValidarCodigoServico(TransmissaoNFSVM entity, Servico item, int count)
        {
            entity.Fail(string.IsNullOrEmpty(item.CodigoIss), new Error(string.Format("Código Iss do serviço {0} é um dado obrigatório.", count)));
            entity.Fail(item.CodigoIss?.Length > 9, new Error(string.Format("O código do serviço {0} não pode ter mais que 9 caracteres.", count)));
        }
    }
}
