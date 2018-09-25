using System;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaIdentificacao
    {
        internal static void ExecutaValidaIdentificacao(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if (entity.ItemTransmissaoNFSVM.Identificacao == null)
            {
                entity.Fail(true, new Error("A entidade identificação não pode ser nula"));
            }
            else
            {
                ValidarDataHoraEmissao(entity);
                ValidarSerieRPC(entity);
                ValidarNumeroRPC(entity);
                ValidarTipoTributacao(entity);
                ValidarCompetenciaRPS(entity);
            }
        }
        
        private static void ValidarCompetenciaRPS(TransmissaoNFSVM entity)
        {
            entity.Fail(entity.ItemTransmissaoNFSVM.Identificacao.CodigoIBGEPrestador == "3205309" && string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Identificacao.CompetenciaRPSString),
                    new Error("Informe a data de competência para o município Vitória(3205309) - ES.", "CompetenciaRPSString"));
        }

        private static void ValidarTipoTributacao(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Identificacao.TipoTributacao.ToString()), new Error("Tipo tributação é um dado o brigatório.", "TipoTributacao"));
        }

        private static void ValidarNumeroRPC(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Identificacao.NumeroRPS.ToString()), new Error("Numero RPC é um dado obrigatório.", "NumeroRPS"));
        }

        private static void ValidarSerieRPC(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Identificacao.SerieRPS), new Error("Série RPC é um dado obrigatório.", "SerieRPS"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Identificacao.SerieRPS.Length > 3, new Error("Série RPC não pode ter mais que 3 caracteres.", "SerieRPS"));            
        }

        private static void ValidarDataHoraEmissao(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Identificacao.DataHoraEmissaoString), new Error("Data de emissão é um dado obrigatório.", "DataHoraEmissao"));
        }
    }
}
 