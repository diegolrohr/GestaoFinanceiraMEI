using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissaoNFS
{
    public class ValidaIntermediador
    {
        internal static void ExecutaValidaIntermediador(EntitiesBLToValidateNFS entitiesBLToValidateNFS, TransmissaoNFSVM entity)
        {
            if (entity.ItemTransmissaoNFSVM.Intermediador != null)
            {
                ValidarCpfCnpj(entity, entitiesBLToValidateNFS);
                ValidarRazaoSocial(entity);
                ValidarInscricaoMunicipal(entity);
            }
        }

        private static void ValidarInscricaoMunicipal(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Intermediador.InscricaoMunicipal), new Error("Inscrição municipal do intermediador é obrigatório.", "InscricaoMunicipal"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Intermediador.InscricaoMunicipal?.Length > 20, new Error("Inscrição municipal do intermediador, não pode ter mais de 20 caracteres.", "InscricaoMunicipal"));
        }

        private static void ValidarRazaoSocial(TransmissaoNFSVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.ItemTransmissaoNFSVM.Intermediador.RazaoSocial), new Error("Razão social do intermediador é obrigatório.", "RazaoSocial"));
            entity.Fail(entity.ItemTransmissaoNFSVM.Intermediador.RazaoSocial?.Length > 120, new Error("Razão social do intermediador, não pode ter mais de 120 caracteres.", "RazaoSocial"));
        }

        private static void ValidarCpfCnpj(TransmissaoNFSVM entity, EntitiesBLToValidateNFS entitiesBLToValidateNFS)
        {
            entity.Fail(entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj == null, new Error("Informe o CPF ou CNPJ do intermediador.", "CpfCnpj"));

            if (entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj?.Length == 11)
                entity.Fail(entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj != null && (!entitiesBLToValidateNFS._empresaBL.ValidaCPF(entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj)),
                            new Error("CPF do intermediador inválido.", "CpfCnpj"));
            else if (entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj?.Length == 14)
                entity.Fail((!entitiesBLToValidateNFS._empresaBL.ValidaCNPJ(entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj)),
                            new Error("CNPJ do intermediador inválido.", "CpfCnpj"));
            else if (entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj != null && entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj?.Length < 11 || entity.ItemTransmissaoNFSVM.Intermediador.CpfCnpj?.Length > 14)
                entity.Fail((true),
                            new Error("CPF ou CNPJ do intermediador é invalido. Informe 11 ou 14 digítos.", "CpfCnpj"));
        }
    }
}
