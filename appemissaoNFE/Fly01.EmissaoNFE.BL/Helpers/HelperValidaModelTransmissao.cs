﻿using Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao;
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
            var nItem = 1;
            foreach (var item in _entity.Item)
            {
                ValidaIdentificador.ExecutarValidaIdentificador(item, _entitiesToValidade, _entity);
                ValidaEmitente.ExecutarValidaEmitente(item, _entitiesToValidade, _entity);
                ValidaDestinatario.ExecutarValidaDestinatario(item, _entitiesToValidade, _entity);
                ValidaTransporte.ExecutarValidaTransporte(item, _entitiesToValidade, _entity);

                var nItemDetalhe = 1;
                foreach (var detalhe in item.Detalhes)
                {
                    ValidaDetalheProduto.ExecutarValidaDetalheProduto(detalhe, _entitiesToValidade, _entity, nItemDetalhe);
                    ValidaDetalheImposto.ExecutarValidaDetalheImposto(detalhe, _entitiesToValidade, _entity, nItemDetalhe, item);
                    nItemDetalhe++;
                }

                nItem++;
            }
        }
    }
}
