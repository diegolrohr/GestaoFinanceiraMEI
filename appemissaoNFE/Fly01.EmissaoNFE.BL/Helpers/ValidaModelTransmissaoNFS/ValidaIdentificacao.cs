using System;
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
            entity.Fail(string.IsNullOrEmpty(entity.Identificacao.SerieRPS), new Error("Série RPC é um dado obrigatório."));
            entity.Fail(entity.Identificacao.SerieRPS.Length > 3, new Error("Série RPC não pode ter mais que 3 caracteres."));            
        }

        private static void ValidarDataHoraEmissao(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Identificacao.DataHoraEmissaoString), new Error("Data de emissão é um dado obrigatório."));
        }
    }
}
 