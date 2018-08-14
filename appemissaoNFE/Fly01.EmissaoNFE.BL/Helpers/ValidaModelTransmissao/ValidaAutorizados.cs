using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaAutorizados
    {
        public static void ExecutarValidaAutorizados(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity)
        {
            #region Validação da classe Autorizados
            if (item.Emitente.Endereco.UF == "BA" && item.Autorizados != null && item.Autorizados.Count > 0)
            {
                entity.Fail(item.Autorizados.Count > 10, new Error("O número máximo de autorizados é 10", "item.Autorizados"));
                var contAutorizados = 1;
                foreach (var autorizado in item.Autorizados)
                {
                    entity.Fail(string.IsNullOrEmpty(autorizado.CNPJ) && string.IsNullOrEmpty(autorizado.CPF), new Error("Informe CNPJ ou CPF do autorizado " + contAutorizados, "item.Autorizados[" + contAutorizados + "]"));
                    entity.Fail(!string.IsNullOrEmpty(autorizado.CNPJ) && !entitiesBLToValidate._empresaBL.ValidaCNPJ(autorizado.CNPJ), new Error("CNPJ inválido. Autorizado " + contAutorizados, "item.Autorizados[" + contAutorizados + "]"));
                    entity.Fail(!string.IsNullOrEmpty(autorizado.CPF) && !entitiesBLToValidate._empresaBL.ValidaCPF(autorizado.CPF), new Error("CPF inválido. Autorizado " + contAutorizados, "item.Autorizados[" + contAutorizados + "]"));

                    contAutorizados++;
                }
            }
            #endregion
        }
    }
}
