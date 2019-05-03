using Fly01.Core.BL;
using System.Linq;
using Fly01.EmissaoNFE.Domain.Entities;
using ResponsavelTecnicoXML = Fly01.EmissaoNFE.Domain.Entities.NFe.ResponsavelTecnico;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Enum;
using System;

namespace Fly01.EmissaoNFE.BL
{
    public class ResponsavelTecnicoBL : DomainBaseBL<ResponsavelTecnico>
    {
        public ResponsavelTecnicoBL(AppDataContextBase context) : base(context) { }

        /// <summary>
        /// As regras de validação ZD01-10 e ZD02-10 (identificação do responsável técnico), ficarão para implementação futura, 
        /// exceto para as UF: AM(13), MS(50), PE(26), PR(41), SC(42) e TO(17), que manterão a data de 07/05/2019(Produção) 25/02/2019(Homologação)
        /// NT 2018.005 v1.30: AL(27)saiu desta lista de estados,as demais prorrogaram para 03/06/2019(Produção)
        /// </summary>
        public void TagResponsavelTecnico(ItemTransmissaoVM nota, TipoAmbiente tipoAmbiente)
        {
            var isUF = ("13,50,26,41,42,17").Contains(nota.Identificador.CodigoUF.ToString());
            var dataHomologacao = new DateTime(2019, 02, 25);
            var dataProducao = new DateTime(2019, 06, 03);

            if (
                isUF &&
                (tipoAmbiente == TipoAmbiente.Homologacao && nota.Identificador.Emissao.Date >= dataHomologacao.Date) ||
                (tipoAmbiente == TipoAmbiente.Producao && nota.Identificador.Emissao.Date >= dataProducao.Date)
            )
            {
                var responsavelTecnico = All.Where(x => x.Id.ToString() == "A39B871C-6913-495C-88F8-1F2668B6AABA").Select(x => new ResponsavelTecnicoXML()
                {
                    CNPJ = x.CNPJ,
                    Contato = x.Contato,
                    Email = x.Email,
                    Fone = x.Fone,
                    CodigoResponsavelTecnico = x.CodigoResponsavelTecnico,
                    IdentificadorCodigoResponsavelTecnico = x.IdentificadorCodigoResponsavelTecnico
                }).FirstOrDefault();

                #region Regra especifica Paraná
                if (nota.Identificador.CodigoUF.ToString() == "41")
                {
                    responsavelTecnico = All.Where(x => x.Id.ToString() == "D21F8B07-580B-47C9-BD1E-5DF033748D7C").Select(x => new ResponsavelTecnicoXML()
                    {
                        CNPJ = x.CNPJ,
                        Contato = x.Contato,
                        Email = x.Email,
                        Fone = x.Fone,
                        CodigoResponsavelTecnico = x.CodigoResponsavelTecnico,
                        IdentificadorCodigoResponsavelTecnico = x.IdentificadorCodigoResponsavelTecnico
                    }).FirstOrDefault();
                }
                #endregion

                CalculaSHA1ResponsavelTecnico(responsavelTecnico, nota.NotaId);
                nota.ResponsavelTecnico = responsavelTecnico;
            }
            else
            {
                nota.ResponsavelTecnico = null;
            }
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
        private void CalculaSHA1ResponsavelTecnico(ResponsavelTecnicoXML responsavelTecnico, string notaId)
        {
            if (!string.IsNullOrEmpty(responsavelTecnico.CodigoResponsavelTecnico) && !string.IsNullOrEmpty(responsavelTecnico.IdentificadorCodigoResponsavelTecnico) && !string.IsNullOrEmpty(notaId))
            {
                var CSRTChave = string.Concat(responsavelTecnico.CodigoResponsavelTecnico.ToUpper(), notaId.Replace("NFe", ""));
                var SHA1 = SHA1Helper.CalculateSHA1(CSRTChave);
                responsavelTecnico.HashCSRT = Base64Helper.CodificaBase64(SHA1);
            }
        }
    }
}
