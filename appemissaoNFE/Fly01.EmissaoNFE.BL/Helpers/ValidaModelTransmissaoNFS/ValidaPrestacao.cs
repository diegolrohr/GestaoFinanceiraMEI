using System.Linq;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaPrestacao
    {
        internal static void ExecutaValidaPrestacao(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if (entity.ItemTransmissaoNFSVM.Prestacao == null)
            {
                entity.Fail(true, new Error("A entidade prestação não pode ser nula"));
            }
            else
            {
                ValidarLogradouro(entity);
                ValidarCodigoMunicipalIBGE(entity, entitiesBLToValidateNFS);
                ValidarMunicipio(entity);
                ValidarBairro(entity);
                ValidarCEP(entity, entitiesBLToValidateNFS);
                ValidarUF(entity, entitiesBLToValidateNFS);
            }

        }

        private static void ValidarUF(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._estadoBL.All.Any(x => x.Sigla.ToUpper() == entity.ItemTransmissaoNFSVM.Prestacao.UF.ToUpper()), new Error("UF do local de prestação do serviço é um dado obrigatório.", "UF"));
        }

        private static void ValidarCEP(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestacao.CEP), new Error("CEP do local de prestação do serviço é um dado obrigatório.", "CEP"));

            entity.Fail(entity.ItemTransmissaoNFSVM.Prestacao.CEP != null && !entitiesBLToValidateNFS._empresaBL.ValidaCEP(entity.ItemTransmissaoNFSVM.Prestacao.CEP),
                    new Error("CEP do local de prestação do serviço inválido.", "CEP"));
        }

        private static void ValidarBairro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestacao.Bairro), new Error("Bairro do local de prestação do serviço é um dado obrigatório.", "Bairro"));
        }

        private static void ValidarMunicipio(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestacao.Municipio), new Error("Municipio do local de prestação do serviço é um dado obrigatório.", "Municipio"));
        }

        private static void ValidarCodigoMunicipalIBGE(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if(entity.EntidadeAmbienteNFS == TipoAmbiente.Homologacao && entity.ItemTransmissaoNFSVM?.Prestador.CodigoMunicipioIBGE == "3541000")
            {
                //configuração específica para este município
                entity.ItemTransmissaoNFSVM.Prestacao.CodigoMunicipioIBGE = "999";
                entity.ItemTransmissaoNFSVM.Prestacao.Municipio = "Homologação";
            }
            else
            {
                entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge.ToUpper() == entity.ItemTransmissaoNFSVM.Prestacao.CodigoMunicipioIBGE.ToUpper()),
                    new Error("Código IBGE do município do local de prestação do serviço é inválido.", "CodigoMunicipioIBGE"));
            }
        }

        private static void ValidarLogradouro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestacao.Logradouro), new Error("Logradouro do local de prestação do serviço é um dado obrigatório.", "Logradouro"));

        }
    }
}
