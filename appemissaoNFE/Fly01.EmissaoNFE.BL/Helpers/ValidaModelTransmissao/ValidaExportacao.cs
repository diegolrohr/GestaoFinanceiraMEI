using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaExportacao
    {
        public static void ExecutarValidaExportacao(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            if (item.Exportacao != null)
            {
                entity.Fail(string.IsNullOrEmpty(item.Exportacao.UFSaidaPais), new Error("Informe a UF de embarque da exportação", "Item.Exportacao.UFSaidaPais"));
                entity.Fail(string.IsNullOrEmpty(item.Exportacao.LocalEmbarque), new Error("Informe o local de embarque da exportação", "Item.Exportação.LocalEmbarque"));
                entity.Fail(!string.IsNullOrEmpty(item.Exportacao.LocalEmbarque) && (item.Exportacao.LocalEmbarque?.Length > 60), new Error("Local de embarque da exportação so pode conter até 60 caracteres."));
                entity.Fail(!string.IsNullOrEmpty(item.Exportacao.LocalDespacho) && (item.Exportacao.LocalDespacho?.Length > 60), new Error("Local de despacho da exportação so pode conter até 60 caracteres."));
            }
        }
    }
}
