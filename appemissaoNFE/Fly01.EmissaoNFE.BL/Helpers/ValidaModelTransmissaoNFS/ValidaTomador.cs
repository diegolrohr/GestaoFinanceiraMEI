using System;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaTomador
    {
        private static string msgError;
        internal static void ExecutaValidaTomador(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if (entity.ItemTransmissaoNFSVM.Tomador == null)
            {
                entity.Fail(true, new Error("A entidade tomador não pode ser nula"));
            }
            else
            {
                ValidarCpfCnpj(entity, entitiesBLToValidateNFS);
                ValidarRazaoSocial(entity);
                ValidarLogradouro(entity);
                ValidarNumeroEndereco(entity);
                ValidarBairro(entity);
                ValidarCodigoMunicipioIBGE(entity, entitiesBLToValidateNFS);
                ValidarCodigoMunicipioSIAFI(entity, entitiesBLToValidateNFS);
                ValidarCidade(entity);
                ValidarUF(entity, entitiesBLToValidateNFS);
                ValidarCEP(entity, entitiesBLToValidateNFS);
                ValidarInscricaoEstadual(entity, entitiesBLToValidateNFS);
            }

        }

        private static void ValidarInscricaoEstadual(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if (!string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Tomador.InscricaoEstadual) &&
                !EmpresaBL.ValidaIE(entity.ItemTransmissaoNFSVM.Tomador.UF, entity.ItemTransmissaoNFSVM.Tomador.InscricaoEstadual, out msgError))
            {
                switch (msgError)
                {
                    case "1":
                        entity.Fail(true, new Error("IE tomador - Digito verificador inválido (para este estado).", "InscricaoEstadual"));
                        break;
                    case "2":
                        entity.Fail(true, new Error("IE tomador - Quantidade de dígitos inválido (para este estado).", "InscricaoEstadual"));
                        break;
                    case "3":
                        entity.Fail(true, new Error("IE tomador - Inscrição Estadual inválida (para este estado).", "InscricaoEstadual"));
                        break;
                    case "4":
                        entity.Fail(true, new Error("UF do tomador inválida.", "UF"));
                        break;
                    case "5":
                        entity.Fail(true, new Error("UF do tomador é um dado obrigatório.", "UF"));
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ValidarCodigoMunicipioSIAFI(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if (string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Tomador.CodigoMunicipioSIAFI))
            {
                entity.ItemTransmissaoNFSVM.Tomador.CodigoMunicipioSIAFI = entitiesBLToValidateNFS._siafiBL.RetornaCodigoSiafiIbge(entity.ItemTransmissaoNFSVM.Tomador.CodigoMunicipioIBGE ?? "");
            }
        }

        private static void ValidarCEP(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Tomador.CEP), new Error("CEP do tomador é um dado obrigatório.", "CEP"));

            entity.Fail(entity.ItemTransmissaoNFSVM.Tomador.CEP != null && !entitiesBLToValidateNFS._empresaBL.ValidaCEP(entity.ItemTransmissaoNFSVM.Tomador.CEP),
                    new Error("CEP do tomador inválido.", "CEP"));
        }

        private static void ValidarUF(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._estadoBL.All.Any(x => x.Sigla.ToUpper() == entity.ItemTransmissaoNFSVM.Tomador.UF.ToUpper()), new Error("UF do tomador é um dado obrigatório.", "UF"));
        }

        private static void ValidarCidade(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Tomador.Cidade), new Error("Cidade do tomador é obrigatório.", "Cidade"));
        }

        private static void ValidarCodigoMunicipioIBGE(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge == entity.ItemTransmissaoNFSVM.Tomador.CodigoMunicipioIBGE.ToString()),
                                new Error("Código IBGE do município do tomador inválido.", "CodigoMunicipioIBGE"));
        }

        private static void ValidarBairro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Tomador.Bairro), new Error("Bairro do tomador é um dado obrigatório.", "Bairro"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Tomador.Bairro?.Length > 50, new Error("Bairro do tomador, não pode ter mais que 50 caracteres.", "Bairro"));
        }

        private static void ValidarLogradouro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Tomador.Logradouro), new Error("Logradouro do tomador é um dado obrigatório.", "Logradouro"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Tomador.Logradouro?.Length > 100, new Error("Logradouro do tomador, não pode ter mais que 100 caracteres.", "Logradouro"));
        }

        private static void ValidarNumeroEndereco(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Tomador.NumeroEndereco), new Error("Número do endereço do tomador é um dado obrigatório.", "NumeroEndereco"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Tomador.NumeroEndereco?.Length > 9, new Error("Número do endereço do tomador, não pode ter mais que 9 caracteres.", "NumeroEndereco"));
        }

        private static void ValidarRazaoSocial(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Tomador.RazaoSocial), new Error("Razão social do tomador é obrigatório.", "RazaoSocial"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Tomador.RazaoSocial?.Length > 120, new Error("Razão social do tomador, não pode ter mais de 120 caracteres.", "RazaoSocial"));
        }

        private static void ValidarCpfCnpj(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj == null,new Error("Informe o CPF ou CNPJ do tomador.", "CpfCnpj"));

            if (entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj?.Length == 11)
                entity.Fail(entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj != null && (!entitiesBLToValidateNFS._empresaBL.ValidaCPF(entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj)),
                            new Error("CPF do tomador inválido.", "CpfCnpj"));
            else if (entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj?.Length == 14)
                entity.Fail((!entitiesBLToValidateNFS._empresaBL.ValidaCNPJ(entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj)),
                            new Error("CNPJ do tomador inválido.", "CpfCnpj"));
            else if (entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj != null && entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj?.Length < 11 || entity.ItemTransmissaoNFSVM.Tomador.CpfCnpj?.Length > 14)
                entity.Fail((true),
                            new Error("CPF ou CNPJ do tomador é invalido. Informe 11 ou 14 digítos.", "CpfCnpj"));
        }
    }
}
