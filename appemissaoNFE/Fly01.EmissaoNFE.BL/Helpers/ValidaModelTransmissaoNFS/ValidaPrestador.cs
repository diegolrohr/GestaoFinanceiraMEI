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
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.Bairro), new Error("Bairro do prestador é um dado obrigatório."));
        }

        private static void ValidarCEP(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.CEP), new Error("CEP do prestador é um dado obrigatório."));

            entity.Fail(entity.Prestador.CEP != null && !entitiesBLToValidateNFS._empresaBL.ValidaCEP(entity.Prestador.CEP),
                    new Error("CEP do prestador inválido."));
        }

        private static void ValidarLogradouro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.Logradouro), new Error("Logradouro do prestador é um dado obrigatório."));
        }

        private static void ValidarUF(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entitiesBLToValidateNFS._estadoBL.All.Any(x => x.Sigla.ToUpper() == entity.Prestador.UF.ToUpper()), new Error("UF do prestador é um dado obrigatório."));
        }

        private static void ValidarCidade(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.Cidade), new Error("Cidade é obrigatório."));
        }

        private static void ValidarCodigoMunicipioIBGE(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge == entity.Prestador.CodigoMunicipalIBGE.ToString()),
                                new Error("Código de município do prestador inválido."));
        }

        private static void ValidarNomeFantasia(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.NomeFantasia), new Error("Nome Fantasia é obrigatório."));
        }

        private static void ValidarRazaoSocial(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.RazaoSocial), new Error("Razão social do prestador é obrigatório."));
        }

        private static void ValidarCpfCnpj(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entity.Prestador.CpfCnpj == null,
                new Error("Informe o CPF ou CNPJ do prestador."));

            if (entity.Prestador.CpfCnpj.Length == 11)
                entity.Fail(entity.Prestador.CpfCnpj != null && (!entitiesBLToValidateNFS._empresaBL.ValidaCPF(entity.Prestador.CpfCnpj)),
                            new Error("CPF do emitente inválido."));
            else if (entity.Prestador.CpfCnpj.Length == 14)
                entity.Fail((!entitiesBLToValidateNFS._empresaBL.ValidaCNPJ(entity.Prestador.CpfCnpj)),
                            new Error("CNPJ do emitente inválido."));
            else if (entity.Prestador.CpfCnpj != null && entity.Prestador.CpfCnpj.Length < 11 || entity.Prestador.CpfCnpj.Length > 14)
                entity.Fail((true), 
                            new Error("CPF ou CNPJ do Prestador é invalido."));
        }

        private static void ValidarInscricaoMunucipal(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Prestador.InscricaoMunicipal.ToString()), 
                new Error("Inscrição estadual é obrigatório."));
        }
    }
}
