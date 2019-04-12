using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.ValueObjects;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;

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
                ValidarPaisDestinatario(item, entity);
                ValidarInscricaoEstadualDestinatario(item, entity);
                ValidarCEPDestinatario(item, entitiesBLToValidate, entity);
                ValidarIdEstrangeiro(item, entity);
            }
        }

        private static void ValidarInscricaoEstadualDestinatario(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            if (item.Destinatario.EhExportacao())
            {
                entity.Fail(!string.IsNullOrEmpty(item.Destinatario.InscricaoEstadual),
                                new Error("Se UF do destinatário é exterior, inscrição estadual não deve ser informada.", "item.Destinatario.InscricaoEstadual"));
                entity.Fail(item.Destinatario.IndInscricaoEstadual != TipoIndicacaoInscricaoEstadual.NaoContribuinte,
                                new Error("Se UF do destinatário é exterior, indicação da inscrição estadual deve ser não contribuinte.", "item.Destinatario.InscricaoEstadual"));
            }

            if (!string.IsNullOrEmpty(item.Destinatario.InscricaoEstadual) && item.Destinatario.IndInscricaoEstadual == TipoIndicacaoInscricaoEstadual.ContribuinteICMS && !item.Destinatario.EhExportacao())
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
            entity.Fail(item.Destinatario.Endereco.Cep != null && !item.Destinatario.EhExportacao() && !entitiesBLToValidate._empresaBL.ValidaCEP(item.Destinatario.Endereco.Cep),
                                new Error("CEP do destinatário inválido.", "item.Destinatario.Endereco.Cep"));
        }

        private static void ValidarMunicipioDestinatario(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Municipio),
                            new Error("Município do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Municipio"));
            entity.Fail(item.Destinatario.EhExportacao() && ((string.IsNullOrEmpty(item.Destinatario.Endereco.Municipio)) || (!string.IsNullOrEmpty(item.Destinatario.Endereco.Municipio) && item.Destinatario.Endereco.Municipio?.ToUpper() != "EXTERIOR")),
                            new Error("Se UF do destinatário é exterior, município do destinatário também deve ser exterior.", "Item.Destinatario.Endereco.Municipio"));
        }

        private static void ValidarPaisDestinatario(ItemTransmissaoVM item, TransmissaoVM entity)
        {            
            entity.Fail(item.Destinatario.EhExportacao() && ((string.IsNullOrEmpty(item.Destinatario.Endereco.PaisCodigoBacen)) || (string.IsNullOrEmpty(item.Destinatario.Endereco.PaisNome))),
                            new Error("Se UF do destinatário é exterior, informe o codigo bacen e nome do País do destinatário.", "Item.Destinatario.Endereco.PaisCodigoBacen"));
            entity.Fail(item.Destinatario.EhExportacao() && 
                ((!string.IsNullOrEmpty(item.Destinatario.Endereco.PaisCodigoBacen) && item.Destinatario.Endereco.PaisCodigoBacen == "1058")) ||
                ((!string.IsNullOrEmpty(item.Destinatario.Endereco.PaisNome) && item.Destinatario.Endereco.PaisNome.ToUpper() == "Brasil")),
                        new Error("Se UF do destinatário é exterior, o país do destinatário não pode ser Brasil.", "Item.Destinatario.Endereco.PaisCodigoBacen"));
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

        private static void ValidarIdEstrangeiro(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Destinatario.EhExportacao() && string.IsNullOrEmpty(item.Destinatario.IdentificacaoEstrangeiro),
                new Error("Quando UF destinatário for exportação, informe o identificador estrangeiro do destinatário.", "Item.Destinatario.IdentificacaoEstrangeiro"));
            entity.Fail(item.Destinatario.IdentificacaoEstrangeiro != null && item.Destinatario.IdentificacaoEstrangeiro?.Length > 20,
                new Error("Id estrangeiro do destinatário só pode ter até 20 caracteres.", "Item.Destinatario.IdentificacaoEstrangeiro"));
        }

        private static void ValidarCPF_CNPJ(ItemTransmissaoVM item, TransmissaoVM entity)
        {
            entity.Fail(item.Destinatario.EhExportacao() && (!string.IsNullOrEmpty(item.Destinatario.Cnpj) || !string.IsNullOrEmpty(item.Destinatario.Cpf)),
                new Error("Quando UF destinatário for exportação, não informe CPF/CNPJ, informe o identificador do estrangeiro.", "Item.Destinatario.Cnpj"));
            entity.Fail(item.Destinatario.Cnpj == null && item.Destinatario.Cpf == null && !item.Destinatario.EhExportacao(),
                new Error("Informe o CPF/CNPJ do destinatário.", "Item.Destinatario.Cnpj"));
        }
    }
}
