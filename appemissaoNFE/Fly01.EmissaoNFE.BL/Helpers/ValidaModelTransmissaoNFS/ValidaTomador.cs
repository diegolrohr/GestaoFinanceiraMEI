﻿using System;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaTomador
    {
        private static string msgError;
        internal static void ExecutaValidaTomador(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            ValidarInscricaoMunicipal(entity);
            ValidarCpfCnpj(entity, entitiesBLToValidateNFS);
            ValidarRazaoSocial(entity);
            ValidarLogradouro(entity);
            ValidarBairro(entity);
            ValidarCodigoMunicipioIBGE(entity, entitiesBLToValidateNFS);
            ValidarCidade(entity);
            ValidarUF(entity, entitiesBLToValidateNFS);
            ValidarCEP(entity, entitiesBLToValidateNFS);
            ValidarInscricaoEstadual(entity, entitiesBLToValidateNFS);
        }

        private static void ValidarInscricaoEstadual(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if (!EmpresaBL.ValidaIE(entity.Tomador.UF, entity.Tomador.InscricaoEstadual, out msgError))
            {
                switch (msgError)
                {
                    case "1":
                        entity.Fail(true, new Error("IE Tomador - Digito verificador inválido (para este estado).", "InscricaoEstadual"));
                        break;
                    case "2":
                        entity.Fail(true, new Error("IE Tomador - Quantidade de dígitos inválido (para este estado).", "InscricaoEstadual"));
                        break;
                    case "3":
                        entity.Fail(true, new Error("IE Tomador - Inscrição Estadual inválida (para este estado).", "InscricaoEstadual"));
                        break;
                    case "4":
                        entity.Fail(true, new Error("UF do Tomador inválida.", "UF"));
                        break;
                    case "5":
                        entity.Fail(true, new Error("UF do Tomador é um dado obrigatório.", "UF"));
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ValidarCEP(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Tomador.CEP), new Error("CEP do tomador é um dado obrigatório.", "CEP"));

            entity.Fail(entity.Tomador.CEP != null && !entitiesBLToValidateNFS._empresaBL.ValidaCEP(entity.Tomador.CEP),
                    new Error("CEP do tomador inválido.", "CEP"));
        }

        private static void ValidarUF(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entitiesBLToValidateNFS._estadoBL.All.Any(x => x.Sigla.ToUpper() == entity.Tomador.UF.ToUpper()), new Error("UF do tomador é um dado obrigatório.", "UF"));
        }

        private static void ValidarCidade(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Tomador.Cidade), new Error("Cidade é obrigatório.", "Cidade"));
        }

        private static void ValidarCodigoMunicipioIBGE(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge == entity.Tomador.CodigoMunicipioIBGE.ToString()),
                                new Error("Código de município do tomador inválido.", "CodigoMunicipioIBGE"));
        }

        private static void ValidarBairro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Tomador.Bairro), new Error("Bairro do tomador é um dado obrigatório.", "Bairro"));
        }

        private static void ValidarLogradouro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Tomador.Logradouro), new Error("Logradouro do tomador é um dado obrigatório.", "Logradouro"));
        }

        private static void ValidarRazaoSocial(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Tomador.RazaoSocial), new Error("Razão social do tomador é obrigatório.", "RazaoSocial"));
        }

        private static void ValidarCpfCnpj(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entity.Tomador.CpfCnpj == null,new Error("Informe o CPF ou CNPJ do tomador.", "CpfCnpj"));

            if (entity.Tomador.CpfCnpj.Length == 11)
                entity.Fail(entity.Tomador.CpfCnpj != null && (!entitiesBLToValidateNFS._empresaBL.ValidaCPF(entity.Tomador.CpfCnpj)),
                            new Error("CPF do emitente inválido.", "CpfCnpj"));
            else if (entity.Tomador.CpfCnpj.Length == 14)
                entity.Fail((!entitiesBLToValidateNFS._empresaBL.ValidaCNPJ(entity.Tomador.CpfCnpj)),
                            new Error("CNPJ do emitente inválido.", "CpfCnpj"));
            else if (entity.Tomador.CpfCnpj != null && entity.Tomador.CpfCnpj.Length < 11 || entity.Tomador.CpfCnpj.Length > 14)
                entity.Fail((true),
                            new Error("CPF ou CNPJ do Prestador é invalido. Informe 11 ou 14 digítos.", "CpfCnpj"));
        }

        private static void ValidarInscricaoMunicipal(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Tomador.InscricaoMunicipal.ToString()), new Error("Inscrição municipal é um dado obrigatório.", "InscricaoMunicipal"));
        }
    }
}
