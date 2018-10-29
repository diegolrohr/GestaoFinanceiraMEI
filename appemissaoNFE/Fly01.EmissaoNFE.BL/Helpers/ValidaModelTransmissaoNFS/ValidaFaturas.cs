using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaFaturas
    {
        internal static void ExecutaValidaFaturas(EntitiesBLToValidateNFS entitiesBLToValidateNFS, TransmissaoNFSVM entity)
        {
            if(entity.ItemTransmissaoNFSVM.Faturas != null && entity.ItemTransmissaoNFSVM.Faturas.Any())
            {
                var count = 1;
                foreach (var item in entity.ItemTransmissaoNFSVM.Faturas)
                {
                    ValidarFatura(entity, item, count);
                    count++;
                }
            }
        }

        private static void ValidarFatura(TransmissaoNFSVM entity, Fatura item, int count)
        {
            entity.Fail(item.Numero <= 0 , new Error(string.Format("Número da fatura {0} deve ser superior a zero.", count)));
            entity.Fail(item.Valor <= 0, new Error(string.Format("Valor da fatura {0} deve ser superior a zero.", count)));
        }
    }
}
