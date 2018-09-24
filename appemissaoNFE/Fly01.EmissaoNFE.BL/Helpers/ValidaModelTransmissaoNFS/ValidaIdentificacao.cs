using System;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaIdentificacao
    {
        internal static void ExecutaValidaIdentificacao(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            ValidarDataHoraEmissao(entity);
            ValidarSerieRPC(entity);
            ValidarNumeroRPC(entity);
            ValidarTipoTributacao(entity);
            ValidarCodigoIBGEPrestador(entity, entitiesBLToValidateNFS);
            ValidarCompetenciaRPS(entity);
        }

        private static void ValidarCodigoIBGEPrestador(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge.ToUpper() == entity.Identificacao.CodigoIBGEPrestador.ToUpper()),
                    new Error("Código de município do prestador, informado na identificação é inválido.", "CodigoIBGEPrestador"));
        }

        private static void ValidarCompetenciaRPS(TransmissaoNFSVM entity)
        {
            entity.Fail(entity.Identificacao.CodigoIBGEPrestador == "3205309" && string.IsNullOrEmpty(entity.Identificacao.CompetenciaRPSString),
                    new Error("Informe a data de competência para o município Vitória(3205309) - ES.", "CompetenciaRPSString"));
        }

        private static void ValidarTipoTributacao(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Identificacao.TipoTributacao.ToString()), new Error("Tipo tributação é um dado o brigatório.", "TipoTributacao"));
        }

        private static void ValidarNumeroRPC(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Identificacao.NumeroRPS.ToString()), new Error("Numero RPC é um dado obrigatório.", "NumeroRPS"));
        }

        private static void ValidarSerieRPC(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Identificacao.SerieRPS), new Error("Série RPC é um dado obrigatório.", "SerieRPS"));
            entity.Fail(entity.Identificacao.SerieRPS.Length > 3, new Error("Série RPC não pode ter mais que 3 caracteres.", "SerieRPS"));            
        }

        private static void ValidarDataHoraEmissao(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Identificacao.DataHoraEmissaoString), new Error("Data de emissão é um dado obrigatório.", "DataHoraEmissao"));
        }
    }
}
 