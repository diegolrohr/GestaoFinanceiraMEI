using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaResponsavelTecnico
    {
        public static void ExecutarValidaResponsavelTecnico(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            if (item.ResponsavelTecnico != null)
            {
                entity.Fail(string.IsNullOrEmpty(item.ResponsavelTecnico.CNPJ), new Error("Informe CNPJ do responsável técnico", "Item.ResponsavelTecnico.CNPJ"));
                entity.Fail(string.IsNullOrEmpty(item.ResponsavelTecnico.Contato), new Error("Informe o nome de contato do responsável técnico", "Item.ResponsavelTecnico.Contato"));
                entity.Fail(string.IsNullOrEmpty(item.ResponsavelTecnico.Fone), new Error("Informe o telefone de contato do responsável técnico", "Item.ResponsavelTecnico.Fone"));
                entity.Fail(string.IsNullOrEmpty(item.ResponsavelTecnico.Email), new Error("Informe o e-mail de contato do responsável técnico", "Item.ResponsavelTecnico.Email"));

                entity.Fail(!string.IsNullOrEmpty(item.ResponsavelTecnico.CNPJ) && !entitiesBLToValidate._empresaBL.ValidaCNPJ(item.ResponsavelTecnico?.CNPJ), new Error("CNPJ do responsável técnico inválido."));
                entity.Fail(!string.IsNullOrEmpty(item.ResponsavelTecnico.Email) && !entitiesBLToValidate._empresaBL.ValidaEmail(item.ResponsavelTecnico?.Email), new Error("E-mail do responsável técnico inválido."));

                entity.Fail(!string.IsNullOrEmpty(item.ResponsavelTecnico.Email) && (item.ResponsavelTecnico.Email.Length < 6 || item.ResponsavelTecnico.Email.Length > 60), new Error("E-mail do responsável técnico deve possuir entre 6 e 60 caracteres."));
                entity.Fail(!string.IsNullOrEmpty(item.ResponsavelTecnico.Email) && (item.ResponsavelTecnico.Contato.Length < 2 || item.ResponsavelTecnico.Contato.Length > 60), new Error("Nome do contato do responsável técnico deve possuir entre 2 e 60 caracteres."));
                entity.Fail(!string.IsNullOrEmpty(item.ResponsavelTecnico.Fone) && (item.ResponsavelTecnico.Fone.Length < 6 || item.ResponsavelTecnico.Fone.Length > 14), new Error("Telefone de contato do responsável técnico deve possuir entre 6 e 14 caracteres."));
                entity.Fail(!string.IsNullOrEmpty(item.ResponsavelTecnico.IdentificadorCodigoResponsavelTecnico) && (item.ResponsavelTecnico.IdentificadorCodigoResponsavelTecnico.Length != 2), new Error("Identificador do CSRT (Código de segurança do responsável técnico) deve possuir 2 caracteres."));

                entity.Fail(!string.IsNullOrEmpty(item.ResponsavelTecnico.IdentificadorCodigoResponsavelTecnico) && string.IsNullOrEmpty(item.ResponsavelTecnico.CodigoResponsavelTecnico), 
                    new Error("Se informado o identificador do CSRT (Código de segurança do responsável técnico), informe o CSRT do responsável técnico.", "Item.ResponsavelTecnico.IdentificadorCodigoResponsavelTecnico"));
                entity.Fail(!string.IsNullOrEmpty(item.ResponsavelTecnico.CodigoResponsavelTecnico) && string.IsNullOrEmpty(item.ResponsavelTecnico.IdentificadorCodigoResponsavelTecnico),
                    new Error("Se informado o CSRT (Código de segurança do responsável técnico), informe o identificador do CSRT do responsável técnico.", "Item.ResponsavelTecnico.CodigoResponsavelTecnico"));

            }
        }
    }
}
