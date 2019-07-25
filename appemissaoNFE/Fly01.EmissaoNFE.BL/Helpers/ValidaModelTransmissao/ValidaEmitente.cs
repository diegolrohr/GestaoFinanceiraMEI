using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System.Linq;
using Fly01.Core.ValueObjects;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaEmitente
    {
        private static string msgError;

        public static void ExecutarValidaEmitente(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            if (item.Emitente == null)
                entity.Fail(true, new Error("Os dados do emitente são obrigatórios.", "Item.Emitente"));
            else
            {
                ValidarCPF_CNPJNulo(item, entity);
                ValidarCPF(item, entitiesBLToValidate, entity);
                ValidarCNPJ(item, entitiesBLToValidate, entity);
                ValidarNomeEmitenteNulo(item, entity);
                ValidarLogradouroEmitente(item, entity);
                ValidarNumeroEmitente(item, entity);
                ValidarBairroEmitente(item, entity);
                ValidarCodigoMunicipio(item, entitiesBLToValidate, entity);
                ValidarCodigoMunicipioDiferenteDoInformado(item, entity);
                ValidarMunicipio(item, entity);

                ValidarInscricaoEstadual(item, entitiesBLToValidate, entity);
                ValidarCEPNulo(item, entity);
                ValidarCEPInvalido(item, entitiesBLToValidate, entity);
            }
        }

        private static void ValidarCEPInvalido(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(item.Emitente.Endereco.Cep != null && !entitiesBLToValidate._empresaBL.ValidaCEP(item.Emitente.Endereco.Cep),
                                new Error("CEP do emitente inválido.", "item.Emitente.Endereco.Cep"));
        }

        private static void ValidarCEPNulo(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Emitente.Endereco.Cep == null,
                                new Error("CEP do emitente é um dado obrigatório.", "item.Emitente.Endereco.Cep"));
        }

        private static void ValidarInscricaoEstadual(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            if (!string.IsNullOrEmpty(item.Emitente.InscricaoEstadual))
            {
                if (!InscricaoEstadualHelper.IsValid(item.Emitente.Endereco.UF, item.Emitente.InscricaoEstadual, out msgError))
                {
                    switch (msgError)
                    {
                        case "1":
                            entity.Fail(true, new Error("IE Emitente - Digito verificador inválido (para este estado).", "Item.Emitente.InscricaoEstadual"));
                            break;
                        case "2":
                            entity.Fail(true, new Error("IE Emitente - Quantidade de dígitos inválido (para este estado).", "Item.Emitente.InscricaoEstadual"));
                            break;
                        case "3":
                            entity.Fail(true, new Error("IE Emitente - Inscrição Estadual inválida (para este estado).", "Item.Emitente.InscricaoEstadual"));
                            break;
                        case "4":
                            entity.Fail(true, new Error("UF do emitente inválida.", "item.Emitente.Endereco.UF"));
                            break;
                        case "5":
                            entity.Fail(true, new Error("UF do emitente é um dado obrigatório.", "item.Emitente.Endereco.UF"));
                            break;
                        default:
                            break;
                    }
                }
            }
            else
                entity.Fail(true, new Error("Inscrição Estadual do emitente é um dado obrigatório.", "Item.Emitente.InscricaoEstadual"));
        }

        private static void ValidarMunicipio(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Municipio),
                                new Error("Município do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Municipio"));
        }

        private static void ValidarCodigoMunicipioDiferenteDoInformado(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Emitente.Endereco.CodigoMunicipio != item.Identificador.CodigoMunicipio,
                                new Error("Código de município do emitente difere do informado na identificação.", "Item.Emitente.Endereco.CodigoMunicipio"));
        }

        private static void ValidarCodigoMunicipio(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(!entitiesBLToValidate._cidadeBL.All.Any(e => e.CodigoIbge == item.Emitente.Endereco.CodigoMunicipio),
                                new Error("Código de município do emitente inválido.", "Item.Emitente.Endereco.CodigoMunicipio"));
        }

        private static void ValidarBairroEmitente(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Bairro),
                                new Error("Bairro do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Bairro"));
        }

        private static void ValidarNumeroEmitente(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Numero),
                new Error("Número do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Numero"));
        }

        private static void ValidarLogradouroEmitente(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Logradouro),
                                new Error("Logradouro do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Logradouro"));
        }

        private static void ValidarNomeEmitenteNulo(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Emitente.Nome),
                                new Error("Nome do emitente é um dado obrigatório.", "Item.Emitente.Nome"));
            entity.Fail((item.Emitente.Nome.Length > 60), new Error("A razão social da Empresa deve possuir entre 2 a 60 caracteres. Altere a razão social da sua empresa", "Item.Emitente.Nome"));
        }

        private static void ValidarCNPJ(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(item.Emitente.Cnpj != null && (!entitiesBLToValidate._empresaBL.ValidaCNPJ(item.Emitente.Cnpj) || item.Emitente.Cnpj.Length != 14),
                                new Error("CNPJ do emitente inválido.", "Item.Emitente.Cnpj"));
        }

        private static void ValidarCPF(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(item.Emitente.Cpf != null && (!entitiesBLToValidate._empresaBL.ValidaCPF(item.Emitente.Cpf) || item.Emitente.Cpf.Length != 11),
                                new Error("CPF do emitente inválido.", "Item.Emitente.Cpf"));
        }

        private static void ValidarCPF_CNPJNulo(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Emitente.Cnpj == null && item.Emitente.Cpf == null,
                new Error("Informe o CPF ou CNPJ do emitente.", "Item.Emitente.Cnpj"));
        }
    }
}
