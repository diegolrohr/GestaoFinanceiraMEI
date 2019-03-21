using Fly01.Core.BL;
using System.Linq;
using Fly01.EmissaoNFE.Domain.Entities;
using ResponsavelTecnicoXML = Fly01.EmissaoNFE.Domain.Entities.NFe.ResponsavelTecnico;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Helpers;

namespace Fly01.EmissaoNFE.BL
{
    public class ResponsavelTecnicoBL : DomainBaseBL<ResponsavelTecnico>
    {
        public ResponsavelTecnicoBL(AppDataContextBase context) : base(context){}

        public ResponsavelTecnicoXML RetornaResponsavel()
        {
            return All.Select(x => new ResponsavelTecnicoXML()
            {
                CNPJ = x.CNPJ,
                Contato = x.Contato,
                Email = x.Email,
                Fone = x.Fone,
                CodigoResponsavelTecnico = x.CodigoResponsavelTecnico,
                IdentificadorCodigoResponsavelTecnico = x.IdentificadorCodigoResponsavelTecnico
            }).FirstOrDefault();              
        }

        /// <summary>
        /// Geração do hashCSRT
        /// Os passos para a geração do “hashCSRT” estão descritos a seguir:
        /// Passo 1: Concatenar o CSRT com a chave de acesso da NF-e/NFC-e que está sendo emitida.
        /// Passo 2: Aplicar o algoritmo SHA-1 sobre o resultado da concatenação do passo 1, resultando em um string de 20 bytes hexadecimais.
        /// Passo 3: Converter o resultado do passo anterior para Base64, resultando em uma string de 28 caracteres
        /// Passo 4: Montar o grupo de identificação da empresa desenvolvedora do software (tag: infRespTec), com a tag “idCSRT” o identificador do CSRT 
        /// utilizado para a geração do hash e a tag “hashCSRT” o resultado do passo 3
        /// </summary>
        /// <param name="entity">Nota Fiscal</param>
        public void CalculaSHA1ResponsavelTecnico(TransmissaoVM entity)
        {
            foreach (var nota in entity.Item.Where(x =>
                x.ResponsavelTecnico != null &&
                !string.IsNullOrEmpty(x.ResponsavelTecnico.IdentificadorCodigoResponsavelTecnico) &&
                !string.IsNullOrEmpty(x.ResponsavelTecnico.CodigoResponsavelTecnico) &&
                !string.IsNullOrEmpty(x.NotaId)
            ))
            {
                var CSRTChave = string.Concat(nota.ResponsavelTecnico?.CodigoResponsavelTecnico.ToUpper(), nota.NotaId.Replace("NFe", ""));
                var SHA1 = SHA1Helper.CalculateSHA1(CSRTChave);
                nota.ResponsavelTecnico.HashCSRT = Base64Helper.CodificaBase64(SHA1);
            }
        }
    }
}
