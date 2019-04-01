using System;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Compras.DAL.Migrations.DataInitializer.Contract;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Compras.DAL.Migrations.DataInitializer
{
    public class AliquotaSimplesNacionalDataInitializer : IDataInitializer
    {
        public void Initialize(AppDataContext context)
        {
            var needUpdateAliquotas = false;

            if (!context.AliquotasSimplesNacional.Any() || needUpdateAliquotas)
            {
                var lista = new List<AliquotaSimplesNacional>() {
                    #region Anexo I
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo1, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa1, SimplesNacional = 4.00, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 12.74, PisPasep = 2.76, Id = Guid.Parse("1BF1F46D-7B15-4025-8D2F-7DA046832E25"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo1, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa2, SimplesNacional = 7.30, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 12.74, PisPasep = 2.76, Id = Guid.Parse("9315B332-75CA-4801-9960-838C157925D3"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo1, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa3, SimplesNacional = 9.50, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 12.74, PisPasep = 2.76, Id = Guid.Parse("63352187-D11E-4A51-8ED4-55DB2DE51A69"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo1, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa4, SimplesNacional = 10.70, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 12.74, PisPasep = 2.76, Id = Guid.Parse("666562E4-4DBB-47FA-ABBB-842A4FDF60AA"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo1, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa5, SimplesNacional = 14.30, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 12.74, PisPasep = 2.76, Id = Guid.Parse("8EDEE8D5-7AE9-4BFE-9DA5-6352E55F63DF"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo1, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa6, SimplesNacional = 19.00, ImpostoRenda = 13.50, Csll = 10.00, Cofins = 28.27, PisPasep = 6.13, Id = Guid.Parse("5D2F58AC-C1CC-45BB-BD0E-918C93561A2F"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    #endregion
                    #region Anexo II
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo2, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa1, SimplesNacional = 4.00, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 12.74, PisPasep = 2.76, Id = Guid.Parse("1BF1F46D-7B15-4025-8D2F-7DA046832E25"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    #endregion
                };

                context.AliquotasSimplesNacional.AddOrUpdate(x => x.Id, lista.ToArray());

                context.SaveChanges();
            };
        }
    }
}
