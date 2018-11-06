using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaPagamentos
    {
        internal static void ExecutaValidaFaturas(EntitiesBLToValidateNFS entitiesBLToValidateNFS, TransmissaoNFSVM entity)
        {
            if(entity.ItemTransmissaoNFSVM.Pagamentos != null)
            {
                if (entity.ItemTransmissaoNFSVM.Pagamentos.ListaPagamentos == null || !entity.ItemTransmissaoNFSVM.Pagamentos.ListaPagamentos.Any())
                {
                    entity.Fail(true, new Error("Os dados da lista de pagamentos são obrigatórios."));
                }
                else
                {
                    var count = 1;
                    foreach (var item in entity.ItemTransmissaoNFSVM.Pagamentos.ListaPagamentos)
                    {
                        ValidarPagamento(entity, item, count);
                        count++;
                    }
                }
            }
        }

        private static void ValidarPagamento(TransmissaoNFSVM entity, Pagamento item, int count)
        {
            entity.Fail(item.NumeroParcela <= 0, new Error(string.Format("Número da parcela do pagamento {0} deve ser superior a zero.", count)));
            entity.Fail(item.NumeroParcela > 999, new Error(string.Format("Número da parcela do pagamento {0} só pode ter mais até 3 números.", count)));
            entity.Fail(item.Valor <= 0, new Error(string.Format("Valor do pagamento {0} deve ser superior a zero.", count)));
            entity.Fail(item.DataVencimento == null, new Error(string.Format("Data de vencimento do pagamento {0} deve ser informada.", count)));
        }
    }
}
