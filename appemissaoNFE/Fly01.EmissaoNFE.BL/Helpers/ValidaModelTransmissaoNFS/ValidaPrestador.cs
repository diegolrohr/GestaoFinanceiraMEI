using System;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaPrestador
    {
        internal static void ExecutaValidaPrestador(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            ValidarInscricaoMunucipal(entity);
            ValidarCpfCnpj(entity, entitiesBLToValidateNFS);
            ValidarRazaoSocial(entity);
            ValidarNomeFantasia(entity);
            ValidarCodigoMunicipioIBGE(entity, entitiesBLToValidateNFS);
            ValidarCidade(entity);
            ValidarUF(entity, entitiesBLToValidateNFS);
            ValidarLogradouro(entity);
            ValidarBairro(entity);
            ValidarCEP(entity,entitiesBLToValidateNFS);

        }

        private static void ValidarBairro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.Bairro), new Error("Bairro do prestador é um dado obrigatório.", "Bairro"));
        }

        private static void ValidarCEP(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.CEP), new Error("CEP do prestador é um dado obrigatório.", "CEP"));

            entity.Fail(entity.Prestador.CEP != null && !entitiesBLToValidateNFS._empresaBL.ValidaCEP(entity.Prestador.CEP),
                    new Error("CEP do prestador inválido.", "CEP"));
        }

        private static void ValidarLogradouro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.Logradouro), new Error("Logradouro do prestador é um dado obrigatório.", "Logradouro"));
        }

        private static void ValidarUF(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entitiesBLToValidateNFS._estadoBL.All.Any(x => x.Sigla.ToUpper() == entity.Prestador.UF.ToUpper()), new Error("UF do prestador é um dado obrigatório.", "UF"));
        }

        private static void ValidarCidade(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.Cidade), new Error("Cidade é obrigatório.", "Cidade"));
        }

        private static void ValidarCodigoMunicipioIBGE(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge.ToUpper() == entity.Prestador.CodigoMunicipioIBGE.ToUpper()),
                                new Error("Código IBGE de município do prestador inválido.", "CodigoMunicipioIBGE"));
        }

        private static void ValidarNomeFantasia(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.NomeFantasia), new Error("Nome Fantasia é obrigatório.", "NomeFantasia"));
        }

        private static void ValidarRazaoSocial(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.RazaoSocial), new Error("Razão social do prestador é obrigatório.", "RazaoSocial"));
        }

        private static void ValidarCpfCnpj(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entity.Prestador.CpfCnpj == null,
                new Error("Informe o CPF ou CNPJ do prestador.", "CpfCnpj"));

            if (entity.Prestador.CpfCnpj.Length == 11)
                entity.Fail(entity.Prestador.CpfCnpj != null && (!entitiesBLToValidateNFS._empresaBL.ValidaCPF(entity.Prestador.CpfCnpj)),
                            new Error("CPF do emitente inválido.", "CpfCnpj"));
            else if (entity.Prestador.CpfCnpj.Length == 14)
                entity.Fail((!entitiesBLToValidateNFS._empresaBL.ValidaCNPJ(entity.Prestador.CpfCnpj)),
                            new Error("CNPJ do emitente inválido.", "CpfCnpj"));
            else if (entity.Prestador.CpfCnpj != null && entity.Prestador.CpfCnpj.Length < 11 || entity.Prestador.CpfCnpj.Length > 14)
                entity.Fail((true), 
                            new Error("CPF ou CNPJ do Prestador é invalido. Informe 11 ou 14 digítos.", "CpfCnpj"));
        }

        private static void ValidarInscricaoMunucipal(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.InscricaoMunicipal.ToString()), 
                new Error("Inscrição municipal é obrigatório.", "InscricaoMunicipal"));
        }
    }
}
