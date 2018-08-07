
using Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao;
using Fly01.EmissaoNFE.Domain.ViewModel;

namespace Fly01.EmissaoNFE.BL.Helpers
{
    public class HelperValidaModelTransmissao
    {
        private readonly TransmissaoVM _entity;
        private readonly EntitiesBLToValidate _entitiesToValidade;

        public HelperValidaModelTransmissao(TransmissaoVM entity, EntitiesBLToValidate entitiesToValidade)
        {
            _entity = entity;
            _entitiesToValidade = entitiesToValidade;
        }

        public void ExecutarHelperValidaModel()
        {
            foreach (var item in _entity.Item)
            {
                ValidaClasseIdentificador.ExecutarValidaIdentificador(item, _entitiesToValidade, _entity);
            }
        }
    }
}
