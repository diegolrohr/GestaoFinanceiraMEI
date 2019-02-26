using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System.Linq;
using Fly01.Core.ValueObjects;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaTransporte
    {
        private static string msgError;

        public static void ExecutarValidaTransporte(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            var modFrete = EnumHelper.GetDataEnumValues(typeof(TipoFrete));

            ValidarTransportadora(item, entitiesBLToValidate, entity);
            ValidarVeiculo(item, entitiesBLToValidate, entity);
            ValidarVolume(item, entity);
        }

        private static void ValidarVolume(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            if (item.Transporte.Volume != null)
            {
                if (item.Transporte.ModalidadeFrete != TipoFrete.SemFrete)
                {
                    entity.Fail(item.Transporte.Volume.Quantidade < 0,
                                    new Error("Quantidade de volumes inválida", "Item.Transporte.Volume.Quantidade"));
                    entity.Fail(!string.IsNullOrEmpty(item.Transporte.Volume.Especie) && ((item.Transporte.Volume.Especie?.Length > 60) || (item.Transporte.Volume.Especie.Replace(" ", "")?.Length == 0)),
                                    new Error("Espécie de volume inválido. No máximo 60 caracteres ou vazio e sem espaços.", "Item.Transporte.Volume.Especie"));
                    entity.Fail(!string.IsNullOrEmpty(item.Transporte.Volume.Marca) && ((item.Transporte.Volume.Marca?.Length > 60) || (item.Transporte.Volume.Marca.Replace(" ", "")?.Length == 0)),
                                    new Error("Marca do volume inválido. No máximo 60 caracteres ou vazio e sem espaços.", "Item.Transporte.Volume.marca"));
                    entity.Fail(!string.IsNullOrEmpty(item.Transporte.Volume.Numeracao) && ((item.Transporte.Volume.Numeracao?.Length > 60) || (item.Transporte.Volume.Numeracao.Replace(" ", "")?.Length == 0)),
                                    new Error("Numeração do volume inválido. No máximo 60 caracteres ou vazio e sem espaços.", "Item.Transporte.Volume.Numeracao"));
                    entity.Fail(item.Transporte.Volume.PesoLiquido < 0,
                                    new Error("Peso líquido inválido", "Item.Transporte.Volume.PesoLiquido"));
                    entity.Fail(item.Transporte.Volume.PesoBruto < 0,
                                    new Error("Peso bruto inválido", "Item.Transporte.Volume.PesoBruto"));
                }
            }
        }

        private static void ValidarVeiculo(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            if (item.Transporte.Veiculo != null)
            {
                entity.Fail(string.IsNullOrEmpty(item.Transporte.Veiculo.Placa) || item.Transporte.Veiculo.Placa?.Length != 7,
                                new Error("Placa do veículo de transporte inválida", "Item.Transporte.Veiculo.Placa"));
                entity.Fail(string.IsNullOrEmpty(item.Transporte.Veiculo.UF) || !entitiesBLToValidate._estadoBL.All.Any(x => x.Sigla == item.Transporte.Veiculo.UF),
                                new Error("Estado do veículo de transporte inválido", "Item.Transporte.Veiculo.UF"));
                entity.Fail(string.IsNullOrEmpty(item.Transporte.Veiculo.RNTC),
                                new Error("Código RNTC é um dado obrigatório", "Item.Transporte.Veiculo.RNTC"));
            }
        }

        private static void ValidarTransportadora(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            if (item.Transporte.Transportadora != null)
            {
                entity.Fail(string.IsNullOrEmpty(item.Transporte.Transportadora.RazaoSocial),
                                new Error("Razão Social da transportadora é um dado obrigatório", "Item.Transporte.Transportadora.RazaoSocial"));
                entity.Fail(string.IsNullOrEmpty(item.Transporte.Transportadora.CNPJ) && string.IsNullOrEmpty(item.Transporte.Transportadora.CPF),
                                new Error("Informe CPF ou CNPJ da transportadora", "Item.Transporte.Transportadora.CNPJ"));
                entity.Fail(!string.IsNullOrEmpty(item.Transporte.Transportadora.CNPJ) && !entitiesBLToValidate._empresaBL.ValidaCNPJ(item.Transporte.Transportadora.CNPJ),
                                new Error("CNPJ da transportadora é inválido", "Item.Transporte.Transportadora.CNPJ"));
                entity.Fail(!string.IsNullOrEmpty(item.Transporte.Transportadora.CPF) && !entitiesBLToValidate._empresaBL.ValidaCPF(item.Transporte.Transportadora.CPF),
                                new Error("CPF da transportadora é inválido", "Item.Transporte.Transportadora.CPF"));
                entity.Fail(string.IsNullOrEmpty(item.Transporte.Transportadora.Endereco),
                                new Error("Endereço da transportadora é obrigatório", "Item.Transporte.Transportadora.Endereco"));
                entity.Fail(string.IsNullOrEmpty(item.Transporte.Transportadora.Municipio),
                                new Error("Município da transportadora é um dado obrigatório", "Item.Transporte.Transportadora.Municipio"));

                ValidaIETransportadora(item, entity);
            }
        }

        private static void ValidaIETransportadora(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            if (!string.IsNullOrEmpty(item.Transporte.Transportadora.IE))
            {
                if (!InscricaoEstadualHelper.IsValid(item.Transporte.Transportadora.UF, item.Transporte.Transportadora.IE, out msgError))
                {
                    switch (msgError)
                    {
                        case "1":
                            entity.Fail(true, new Error("IE Transportadora - Digito verificador inválido (para este estado)", "Item.Transporte.Transportadora.IE"));
                            break;
                        case "2":
                            entity.Fail(true, new Error("IE Transportadora - Quantidade de dígitos inválido (para este estado)", "Item.Transporte.Transportadora.IE"));
                            break;
                        case "3":
                            entity.Fail(true, new Error("IE Transportadora - Inscrição Estadual inválida (para este estado)", "Item.Transporte.Transportadora.IE"));
                            break;
                        case "4":
                            entity.Fail(true, new Error("UF da transportadora é inválida.", "Item.Transporte.Transportadora.UF"));
                            break;
                        case "5":
                            entity.Fail(true, new Error("UF da transportadora é um campo obrigatório.", "Item.Transporte.Transportadora.UF"));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
