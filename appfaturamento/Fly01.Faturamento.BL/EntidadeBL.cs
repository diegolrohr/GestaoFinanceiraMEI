﻿using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.BL;
using System.Collections.Generic;
using System.Linq;
using EmpresaNfeVM = Fly01.EmissaoNFE.Domain.ViewModel.EmpresaVM;
using EmpresaVM = Fly01.Core.Entities.ViewModels.EmpresaVM;
using Fly01.Core;
using Fly01.Core.Rest;
using Fly01.EmissaoNFE.Domain.ViewModel;

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

        public EntidadeBL(AppDataContext context, EstadoBL estadoBL) : base(context)
        {
            EstadoBL = estadoBL;
        }

        public EntidadeVM RetornaEntidade()
        {
            var empresa = GetDadosEmpresa();

            var estado = EstadoBL.All.FirstOrDefault(x => x.Nome == empresa.EstadoNome);

            var entidade = new EmpresaNfeVM
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

            var empresaNfe = RestHelper.ExecutePostRequest<EmpresaNfeVM>
                                (AppDefaults.UrlEmissaoNfeApi,
                                    "Empresa",
                                    entidade,
                                    null,
                                    GetHeaderDefault());

            return empresaNfe;
        }

        private EmpresaVM GetDadosEmpresa()
        {
            return RestHelper.ExecuteGetRequest<EmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");
        }

        public EntidadeVM GetEntidade()
        {
            var certificado = All.FirstOrDefault();
            
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