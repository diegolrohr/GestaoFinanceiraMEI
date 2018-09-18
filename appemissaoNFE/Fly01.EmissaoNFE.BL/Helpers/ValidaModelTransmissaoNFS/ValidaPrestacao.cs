using System;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaPrestacao
    {
        internal static void ExecutaValidaPrestacao(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            ValidarLogradouro(entity);
            ValidarCodigoMunicipalIBGE(entity, entitiesBLToValidateNFS);
            ValidarMunicipio(entity);
            ValidarBairro(entity);
            ValidarCEP(entity, entitiesBLToValidateNFS);
            ValidarUF(entity, entitiesBLToValidateNFS);
        }

        private static void ValidarUF(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entitiesBLToValidateNFS._estadoBL.All.Any(x => x.Sigla.ToUpper() == entity.Prestacao.UF.ToUpper()), new Error("UF do local de prestação do serviço é um dado obrigatório.", "UF"));
        }

        private static void ValidarCEP(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestacao.CEP), new Error("CEP do local de prestação do serviço é um dado obrigatório.", "CEP"));

            entity.Fail(entity.Prestacao.CEP != null && !entitiesBLToValidateNFS._empresaBL.ValidaCEP(entity.Prestacao.CEP),
                    new Error("CEP do emitente inválido.", "CEP"));
        }

        private static void ValidarBairro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestacao.Bairro), new Error("Bairro do local de prestação do serviço é um dado obrigatório.", "Bairro"));
        }

        private static void ValidarMunicipio(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestacao.Municipio), new Error("Municipio do local de prestação do serviço é um dado obrigatório.", "Municipio"));
        }

        private static void ValidarCodigoMunicipalIBGE(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge.ToUpper() == entity.Prestacao.CodigoMunicipioIBGE.ToUpper()),
                    new Error("Código de município do local de prestação do serviço é inválido.", "CodigoMunicipioIBGE"));
        }

        private static void ValidarLogradouro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestacao.Logradouro), new Error("Logradouro do local de prestação do serviço é um dado obrigatório.", "Logradouro"));

        }
    }
}
