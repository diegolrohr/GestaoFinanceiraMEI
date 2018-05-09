﻿using Fly01.Faturamento.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Reports;

namespace Fly01.Faturamento.BL
{
    public class EntidadeBL : PlataformaBaseBL<CertificadoDigital>
    {
        private Dictionary<string, string> GetHeaderDefault()
        {
            return new Dictionary<string, string>()
            {
                { "PlataformaUrl", PlataformaUrl },
                { "AppUser", AppUser }
            };
        }

        protected EstadoBL EstadoBL;
        private ManagerEmpresaVM empresa;

        public EntidadeBL(AppDataContext context, EstadoBL estadoBL) : base(context)
        {
            EstadoBL = estadoBL;
            empresa = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");
        }

        public EntidadeVM RetornaEntidade()
        {
            var empresa = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");            
            string estadoNome =  empresa.Cidade != null && empresa.Cidade.Estado != null ? empresa.Cidade.Estado.Nome : string.Empty;

            var estado = EstadoBL.All.FirstOrDefault(x => x.Nome == estadoNome);

            var entidade = new EmpresaVM
            {
                Nome = empresa.RazaoSocial,
                NIRE = empresa.Nire,
                Municipio = empresa.Cidade?.Nome,
                CodigoIBGE = empresa.Cidade?.CodigoIbge,
                InscricaoMunicipal = empresa.InscricaoMunicipal,
                InscricaoEstadual = empresa.InscricaoEstadual,
                Fone = empresa.Telefone,
                Fantasia = empresa.NomeFantasia,
                Email = empresa.Email,
                Cnpj = empresa.CNPJ,
                Cep = empresa.CEP,
                Bairro = empresa.Bairro,
                Endereco = empresa.Endereco,
                UF = estado?.Sigla
            };

            var empresaNfe = RestHelper.ExecutePostRequest<EmpresaVM>
                                (AppDefaults.UrlEmissaoNfeApi,
                                    "Empresa",
                                    entidade,
                                    null,
                                    GetHeaderDefault());

            return empresaNfe;
        }

        public EntidadeVM GetEntidade()
        {
            var certificado = All.Where(x => x.Cnpj == empresa.CNPJ).FirstOrDefault();
            
            if (certificado != null && certificado.EntidadeHomologacao != null && certificado.EntidadeProducao != null)
            {
                var retorno = new EntidadeVM
                {
                    Homologacao = certificado.EntidadeHomologacao,
                    Producao = certificado.EntidadeProducao
                };

                return retorno;
            }
            else
            {
                var entidades = RetornaEntidade();
                
                return entidades;
            }
        }
    }
}