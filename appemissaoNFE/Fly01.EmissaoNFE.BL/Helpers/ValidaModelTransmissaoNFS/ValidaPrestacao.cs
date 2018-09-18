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
        }

        private static void ValidarCEP(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestacao.CEP), new Error("CEP do local de prestação do serviço é um dado obrigatório."));

            entity.Fail(entity.Prestacao.CEP != null && !entitiesBLToValidateNFS._empresaBL.ValidaCEP(entity.Prestacao.CEP),
                    new Error("CEP do emitente inválido."));
        }

        private static void ValidarBairro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestacao.Bairro), new Error("Bairro do local de prestação do serviço é um dado obrigatório."));
        }

        private static void ValidarMunicipio(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestacao.Municipio), new Error("Municipio do local de prestação do serviço  é um dado obrigatório."));
        }

        private static void ValidarCodigoMunicipalIBGE(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge == entity.Prestacao.CodigoMunicipioIBGE.ToString()),
                    new Error("Código de município do local de prestação do serviço é inválido."));
        }

        private static void ValidarLogradouro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestacao.Logradouro), new Error("Logradouro do local de prestação do serviço é um dado obrigatório."));

        }
    }
}
