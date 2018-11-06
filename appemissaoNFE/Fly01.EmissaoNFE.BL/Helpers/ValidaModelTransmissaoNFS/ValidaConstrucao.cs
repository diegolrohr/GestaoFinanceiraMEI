using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaConstrucao
    {
        internal static void ExecutaValidaConstrucao(EntitiesBLToValidateNFS entitiesBLToValidateNFS, TransmissaoNFSVM entity)
        {
            if (entity.ItemTransmissaoNFSVM.Construcao != null)
            {
                ValidarCodigoObra(entity);
                ValidarART(entity);
                ValidarTipoObra(entity);
            }
        }

        private static void ValidarCodigoObra(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Construcao.CodigoObra), new Error("Código da obra da construção é obrigatório.", "CodigoObra"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Construcao.CodigoObra?.Length > 15, new Error("Código da obra da construção, não pode ter mais de 15 caracteres.", "CodigoObra"));
        }

        private static void ValidarART(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Construcao.ArtObra), new Error("ART (Anotação de Responsabilidade Técnica) da construção é obrigatório.", "ArtObra"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Construcao.ArtObra?.Length > 15, new Error("ART (Anotação de Responsabilidade Técnica) da construção, não pode ter mais de 15 caracteres.", "ArtObra"));
        }

        private static void ValidarTipoObra(TransmissaoNFSVM entity)
        {
            entity.Fail(entity.ItemTransmissaoNFSVM.Construcao.TipoObra?.Length > 1, new Error("Tipo da obra da construção, não pode ter mais de 1 caracteres.", "TipoObra"));
        }
    }
}
