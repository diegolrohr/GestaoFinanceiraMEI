using Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.EmissaoNFE.BL.Helpers
{
    public class HelperValidaModelTransmissaoNFS
    {
        private readonly TransmissaoNFSVM _entity;
        private readonly EntitiesBLToValidateNFS _entitiesBLToValidateNFS;

        public HelperValidaModelTransmissaoNFS(TransmissaoNFSVM entity , EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            _entity = entity;
            _entitiesBLToValidateNFS = entitiesBLToValidateNFS;
        }

        public void ExecutarHelperValidaModelNFS()
        {
            ValidaIdentificacao.ExecutaValidaIdentificacao(_entity, _entitiesBLToValidateNFS);
            ValidaAtividade.ExecutaValidaAtividade(_entity, _entitiesBLToValidateNFS);
            ValidaPrestador.ExecutaValidaPrestador(_entity, _entitiesBLToValidateNFS);
            ValidaPrestacao.ExecutaValidaPrestacao(_entity, _entitiesBLToValidateNFS);
            ValidaTomador.ExecutaValidaTomador(_entity, _entitiesBLToValidateNFS);
            ValidaValores.ExecutaValidaValores(_entity, _entitiesBLToValidateNFS); 
            ValidaInformacoesComplementares.ExecutaValidaInformacoesCompleme(_entity, _entitiesBLToValidateNFS);


            foreach (var item in _entity.Servicos)
            {
                ValidaServicos.ExecutaValidaServicos(item, _entitiesBLToValidateNFS);
            }
        }
    }
}
