using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaIdentificador
    {
        public static void ExecutarValidaIdentificador(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            if (item.Identificador == null)
                entity.Fail(true, new Error("Os dados de identificação são obrigatórios.", "Item.Identificador"));
            else
            {
                ValidarCodigoUF(item, entitiesBLToValidate, entity);
                ValidarCodigoNotaFiscal(item, entity);
                ValidarDescricaoNatureza(item, entity);
                ValidarSerie(item, entity);
                ValidarSerieEmissaoNormal(item, entity);
                ValidarNumeroDocumento(item, entity);
                ValidarTipoNota(item, entity);
                ValidarTipoDestinoOperacao(item, entity);
                ValidarCodigoMunicipio(item, entitiesBLToValidate, entity);
                ValidarImpressaoDANFE(item, entity);
                ValidarTipoModalidadeEmissao(item, entity);
                ValidarAmbienteTransmissao(item, entity);
                ValidarFinalidadeEmissao(item, entity);
                ValidarInformacaoConsumidorFinal(item, entity);
                ValidarChaveNotaFiscalReferenciada(item, entity);
                ValidarSerieEmissaoNormalRural(item, entity);

                ValidarNFReferenciada(item, entity);
            }
        }

        private static void ValidarNFReferenciada(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            if (item.Identificador.NFReferenciada != null)
            {
                entity.Fail(!String.IsNullOrEmpty(item.Identificador.NFReferenciada.ChaveNFeReferenciada) && item.Identificador.NFReferenciada.ChaveNFeReferenciada.Length != 44,
                    new Error("Tamanho da chave da nota fiscal referenciada deve conter 44 caracteres.", "Item.Identificador.NFReferenciada.ChaveNFeReferenciada"));
                entity.Fail(String.IsNullOrEmpty(item.Identificador.NFReferenciada.ChaveNFeReferenciada),
                    new Error("Informe a chave da nota fiscal referenciada.", "Item.Identificador.NFReferenciada.ChaveNFeReferenciada"));
                entity.Fail(item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Normal || item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Ajuste,
                    new Error("A chave da nota fiscal referenciada não deve ser informada para esse tipo de nota.", "Item.Identificador.NFReferenciada"));
            }
        }

        private static void ValidarSerieEmissaoNormalRural(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Identificador.Serie < 920 && item.Identificador.FormaEmissao == TipoModalidade.Normal && item.Emitente.Cpf != null,
                                new Error("Série inválida para a modalidade 1 (Emissão Produtor Rural). A série deve estar entre 920-969.", "Item.Identificador.Serie"));
        }

        private static void ValidarChaveNotaFiscalReferenciada(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(((item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Devolucao || item.Identificador.FinalidadeEmissaoNFe == TipoCompraVenda.Complementar) && item.Identificador.NFReferenciada == null),
                                new Error("Finalidade de devolução/complementar é necessário informar a chave da nota fiscal referenciada.", "Item.Identificador.NFReferenciada"));
        }

        private static void ValidarInformacaoConsumidorFinal(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Identificador.ConsumidorFinal != 0 && item.Identificador.ConsumidorFinal != 1,
                                new Error("Informação de consumidor final inválida.", "Item.Identificador.ConsumidorFinal"));
        }

        private static void ValidarFinalidadeEmissao(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(((int)item.Identificador.FinalidadeEmissaoNFe < 1 || (int)item.Identificador.FinalidadeEmissaoNFe > 4),
                                new Error("Finalidade da emissão inválida.", "Item.Identificador.FinalidadeEmissaoNFe"));
        }

        private static void ValidarAmbienteTransmissao(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(((int)item.Identificador.Ambiente < 1 || (int)item.Identificador.Ambiente > 2),
                                new Error("Ambiente inválido para transmissão de notas.", "Item.Identificador.Ambiente"));
        }

        private static void ValidarTipoModalidadeEmissao(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(((int)item.Identificador.FormaEmissao < 1 || (int)item.Identificador.FormaEmissao > 7) && (int)item.Identificador.FormaEmissao != 9,
                                new Error("Tipo de modalidade de emissão inválido.", "Item.Identificador.FormaEmissao"));
        }

        private static void ValidarImpressaoDANFE(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail((item.Identificador.ImpressaoDANFE < 0 || (int)item.Identificador.ImpressaoDANFE > 5),
                                new Error("Tipo de impressão da DANFE inválido.", "Item.Identificador.ImpressaoDANFE"));
        }

        private static void ValidarCodigoMunicipio(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(!entitiesBLToValidate._cidadeBL.All.Any(e => e.CodigoIbge == item.Identificador.CodigoMunicipio),
                                new Error("O código do município é inválido.", "Item.Identificador.CodigoMunicipio"));
        }

        private static void ValidarTipoDestinoOperacao(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(((int)item.Identificador.DestinoOperacao < 1 || (int)item.Identificador.DestinoOperacao > 3),
                                new Error("O tipo destino operação da nota é inválido.", "Item.Identificador.DestinoOperacao"));
        }

        private static void ValidarTipoNota(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail((item.Identificador.TipoDocumentoFiscal < 0 || (int)item.Identificador.TipoDocumentoFiscal > 1),
                                new Error("O tipo da nota é inválido.", "Item.Identificador.TipoDocumentoFiscal"));
        }

        private static void ValidarNumeroDocumento(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Identificador.NumeroDocumentoFiscal.ToString()),
                                new Error("O número do documento é obrigatório.", "Item.Identificador.NumeroDocumentoFiscal"));
        }

        private static void ValidarSerieEmissaoNormal(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Identificador.Serie > 889 && item.Identificador.FormaEmissao == TipoModalidade.Normal && item.Emitente.Cnpj != null,
                                new Error("Série inválida para a modalidade 1 (Emissão Normal).", "Item.Identificador.Serie"));
        }

        private static void ValidarSerie(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Identificador.Serie.ToString()),
                                new Error("Série é um dado obrigatório.", "Item.Identificador.Serie"));
        }

        private static void ValidarDescricaoNatureza(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Identificador.NaturezaOperacao), new Error("A descrição de Natureza da Operação é obrigatória.", "Item.Identificador.NaturezaOperacao"));
        }

        private static void ValidarCodigoNotaFiscal(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Identificador.CodigoNF.ToString()), new Error("O código da Nota Fiscal é obrigatório.", "Item.Identificador.CodigoNF"));
        }

        private static void ValidarCodigoUF(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(!entitiesBLToValidate._estadoBL.All.Any(e => e.CodigoIbge == item.Identificador.CodigoUF.ToString()), new Error("O código da UF é inválido.", "Item.Identificador.CodigoUF"));
        }
    }
}
