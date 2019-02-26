using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Linq;
using Fly01.Core.ValueObjects;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaDestinatario
    {
        private static string msgError;

        public static void ExecutarValidaDestinatario(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            if (item.Destinatario == null)
                entity.Fail(true, new Error("Os dados do destinatário são obrigatórios.", "Item.Destinatario"));
            else
            {
                ValidarCPF_CNPJ(item, entity);
                ValidarCPF(item, entitiesBLToValidate, entity);
                ValidarCNPJ(item, entitiesBLToValidate, entity);
                ValidarNomeDestinatario(item, entity);
                ValidarLogradouro(item, entity);
                ValidarNumeroDestinatario(item, entity);
                ValidarBairroDestinatario(item, entity);
                ValidarCodigoMunicipioDestinatario(item, entitiesBLToValidate, entity);
                ValidarMunicipioDestinatario(item, entity);
                ValidarInscricaoEstadualDestinatario(item, entity);
                ValidarCEPDestinatario(item, entitiesBLToValidate, entity);
            }
        }

        private static void ValidarInscricaoEstadualDestinatario(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            if (!string.IsNullOrEmpty(item.Destinatario.InscricaoEstadual) && item.Destinatario.IndInscricaoEstadual == IndInscricaoEstadual.ContribuinteICMS)
            {
                if (!InscricaoEstadualHelper.IsValid(item.Destinatario.Endereco.UF, item.Destinatario.InscricaoEstadual, out msgError))
                {
                    switch (msgError)
                    {
                        case "1":
                            entity.Fail(true, new Error("IE Destinatário - Digito verificador inválido (para este estado).", "Item.Destinatario.InscricaoEstadual"));
                            break;
                        case "2":
                            entity.Fail(true, new Error("IE Destinatário - Quantidade de dígitos inválido (para este estado).", "Item.Destinatario.InscricaoEstadual"));
                            break;
                        case "3":
                            entity.Fail(true, new Error("IE Destinatário - Inscrição Estadual inválida (para este estado).", "Item.Destinatario.InscricaoEstadual"));
                            break;
                        case "4":
                            entity.Fail(true, new Error("UF do destinatário inválida.", "item.Destinatario.Endereco.UF"));
                            break;
                        case "5":
                            entity.Fail(true, new Error("UF do destinatário é um dado obrigatório.", "item.Destinatario.Endereco.UF"));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void ValidarCEPDestinatario(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(item.Destinatario.Endereco.Cep != null && !entitiesBLToValidate._empresaBL.ValidaCEP(item.Destinatario.Endereco.Cep),
                                new Error("CEP do destinatário inválido.", "item.Destinatario.Endereco.Cep"));
        }

        private static void ValidarMunicipioDestinatario(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Municipio),
                                new Error("Município do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Municipio"));
        }

        private static void ValidarCodigoMunicipioDestinatario(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(item.Destinatario.Endereco.CodigoMunicipio != null && !entitiesBLToValidate._cidadeBL.All.Any(e => e.CodigoIbge == item.Destinatario.Endereco.CodigoMunicipio),
                                new Error("Código de município do destinatário inválido.", "Item.Destinatario.Endereco.CodigoMunicipio"));
        }

        private static void ValidarBairroDestinatario(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Bairro),
                                new Error("Bairro do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Bairro"));
        }

        private static void ValidarNumeroDestinatario(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Numero),
                                new Error("Número do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Numero"));
        }

        private static void ValidarLogradouro(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Logradouro),
                                new Error("Logradouro do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Logradouro"));
        }

        private static void ValidarNomeDestinatario(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Destinatario.Nome),
                                new Error("Nome do destinatário é um dado obrigatório.", "Item.Destinatario.Nome"));
        }

        private static void ValidarCNPJ(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(item.Destinatario.Cnpj != null && (!entitiesBLToValidate._empresaBL.ValidaCNPJ(item.Destinatario.Cnpj) || item.Destinatario.Cnpj.Length != 14),
                                new Error("CNPJ do destinatário inválido.", "Item.Destinatario.Cnpj"));
        }

        private static void ValidarCPF(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            entity.Fail(item.Destinatario.Cpf != null && (!entitiesBLToValidate._empresaBL.ValidaCPF(item.Destinatario.Cpf) || item.Destinatario.Cpf.Length != 11),
                new Error("CPF do destinatário inválido.", "Item.Destinatario.Cpf"));
        }

        private static void ValidarCPF_CNPJ(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Destinatario.Cnpj == null && item.Destinatario.Cpf == null,
                new Error("Informe o CPF ou CNPJ do destinatário.", "Item.Destinatario.Cnpj"));
        }
    }
}
