using System;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaPrestador
    {
        internal static void ExecutaValidaPrestador(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            if (entity.ItemTransmissaoNFSVM.Prestador == null)
            {
                entity.Fail(true, new Error("A entidade prestador não pode ser nula"));
            }
            else
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
        }

        private static void ValidarBairro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestador.Bairro), new Error("Bairro do prestador é um dado obrigatório.", "Bairro"));
        }

        private static void ValidarCEP(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestador.CEP), new Error("CEP do prestador é um dado obrigatório.", "CEP"));

            entity.Fail(entity.ItemTransmissaoNFSVM.Prestador.CEP != null && !entitiesBLToValidateNFS._empresaBL.ValidaCEP(entity.ItemTransmissaoNFSVM.Prestador.CEP),
                    new Error("CEP do prestador inválido.", "CEP"));
        }

        private static void ValidarLogradouro(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestador.Logradouro), new Error("Logradouro do prestador é um dado obrigatório.", "Logradouro"));
        }

        private static void ValidarUF(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._estadoBL.All.Any(x => x.Sigla.ToUpper() == entity.ItemTransmissaoNFSVM.Prestador.UF.ToUpper()), new Error("UF do prestador é um dado obrigatório.", "UF"));
        }

        private static void ValidarCidade(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestador.Cidade), new Error("Cidade é obrigatório.", "Cidade"));
        }

        private static void ValidarCodigoMunicipioIBGE(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(!entitiesBLToValidateNFS._cidadeBL.All.Any(e => e.CodigoIbge.ToString() == entity.ItemTransmissaoNFSVM.Prestador.CodigoMunicipioIBGE.ToString()),
                                new Error("Código IBGE de município do prestador inválido.", "CodigoMunicipioIBGE"));
        }

        private static void ValidarNomeFantasia(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestador.NomeFantasia), new Error("Nome Fantasia é obrigatório.", "NomeFantasia"));
        }

        private static void ValidarRazaoSocial(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestador.RazaoSocial), new Error("Razão social do prestador é obrigatório.", "RazaoSocial"));
        }

        private static void ValidarCpfCnpj(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj == null,
                new Error("Informe o CPF ou CNPJ do prestador.", "CpfCnpj"));

            if (entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj.Length == 11)
                entity.Fail(entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj != null && (!entitiesBLToValidateNFS._empresaBL.ValidaCPF(entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj)),
                            new Error("CPF do emitente inválido.", "CpfCnpj"));
            else if (entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj.Length == 14)
                entity.Fail((!entitiesBLToValidateNFS._empresaBL.ValidaCNPJ(entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj)),
                            new Error("CNPJ do emitente inválido.", "CpfCnpj"));
            else if (entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj != null && entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj.Length < 11 || entity.ItemTransmissaoNFSVM.Prestador.CpfCnpj.Length > 14)
                entity.Fail((true), 
                            new Error("CPF ou CNPJ do Prestador é invalido. Informe 11 ou 14 digítos.", "CpfCnpj"));
        }

        private static void ValidarInscricaoMunucipal(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Prestador.InscricaoMunicipalPrestador), 
                new Error("Inscrição municipal é obrigatório.", "InscricaoMunicipal"));
        }
    }
}
