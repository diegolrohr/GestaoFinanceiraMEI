using System;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Compras.DAL.Migrations.DataInitializer.Contract;
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
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo2, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa1, SimplesNacional = 4.50, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 11.51, PisPasep = 2.49, Ipi = 7.50, Id = Guid.Parse("B62A9AD7-36D6-4FF8-BE12-B7E5C21BEAE3"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo2, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa2, SimplesNacional = 7.80, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 11.51, PisPasep = 2.49, Ipi = 7.50, Id = Guid.Parse("E66D4E90-FC78-44F5-A94B-63AF64D39CF3"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo2, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa3, SimplesNacional = 10.00, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 11.51, PisPasep = 2.49, Ipi = 7.50, Id = Guid.Parse("FB65DBDB-52EA-44A2-A0F5-E013AFC66A02"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo2, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa4, SimplesNacional = 11.20, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 11.51, PisPasep = 2.49, Ipi = 7.50, Id = Guid.Parse("13652326-95BC-426C-BC55-877967E754D8"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo2, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa5, SimplesNacional = 14.70, ImpostoRenda = 5.50, Csll = 3.50, Cofins = 11.51, PisPasep = 2.49, Ipi = 7.50, Id = Guid.Parse("D509E0EB-058B-4953-AFEF-CD418767B732"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo2, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa6, SimplesNacional = 30.00, ImpostoRenda = 8.50, Csll = 7.50, Cofins = 20.96, PisPasep = 4.54, Ipi = 35.00, Id = Guid.Parse("DBBF2299-5B89-4CF4-8B6C-7E9040DD8F30"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    #endregion

                    #region Anexo III
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo3, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa1, SimplesNacional = 6.00, ImpostoRenda = 4.00, Csll = 3.50, Cofins = 12.82, PisPasep = 2.78, Iss = 33.50, Id = Guid.Parse("0062EAA2-AFDD-4E92-B5BC-1431B2A21546"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo3, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa2, SimplesNacional = 11.20, ImpostoRenda = 4.00, Csll = 3.50, Cofins = 14.05, PisPasep = 3.05, Iss = 32.00, Id = Guid.Parse("31455560-BC34-45B0-88B9-46F0A9B4F372"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo3, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa3, SimplesNacional = 13.50, ImpostoRenda = 4.00, Csll = 3.50, Cofins = 13.64, PisPasep = 2.96, Iss = 32.50, Id = Guid.Parse("14691A26-4E6B-489B-9FBA-477191900C37"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo3, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa4, SimplesNacional = 16.00, ImpostoRenda = 4.00, Csll = 3.50, Cofins = 13.64, PisPasep = 2.96, Iss = 32.50, Id = Guid.Parse("E4AEA1C4-CD26-45E6-B458-09247DCB5A87"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo3, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa5, SimplesNacional = 21.00, ImpostoRenda = 4.00, Csll = 3.50, Cofins = 12.82, PisPasep = 2.78, Iss = 33.50, Id = Guid.Parse("025051E9-BAB6-48DB-B4B7-64D3C9843684"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo3, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa6, SimplesNacional = 33.00, ImpostoRenda = 35.00, Csll = 15.00, Cofins = 16.03, PisPasep = 3.47, Id = Guid.Parse("6D04DD39-B6F1-4AE7-8338-3DB8BD801A60"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    #endregion

                    #region Anexo IV
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo4, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa1, SimplesNacional = 4.50, ImpostoRenda = 18.80, Csll = 15.20, Cofins = 17.67, PisPasep = 3.83, Iss = 44.50, Id = Guid.Parse("C14971A3-509E-45DA-8E61-BF7FE7A231A0"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo4, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa2, SimplesNacional = 9.00, ImpostoRenda = 19.80, Csll = 15.20, Cofins = 20.55, PisPasep = 4.45, Iss = 40.00, Id = Guid.Parse("F9C617FB-264B-47C8-BAA6-CFD5BA5CE64E"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo4, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa3, SimplesNacional = 10.20, ImpostoRenda = 20.80, Csll = 15.20, Cofins = 19.73, PisPasep = 4.27, Iss = 40.00, Id = Guid.Parse("385A42DB-6DCB-4E01-AFB2-F2142C2E5524"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo4, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa4, SimplesNacional = 14.00, ImpostoRenda = 17.80, Csll = 19.20, Cofins = 18.90, PisPasep = 4.10, Iss = 40.00, Id = Guid.Parse("D0128956-D30F-45A6-B64D-2BF88A2C4A53"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo4, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa5, SimplesNacional = 22.00, ImpostoRenda = 18.80, Csll = 19.20, Cofins = 18.08, PisPasep = 3.92, Iss = 40.00, Id = Guid.Parse("D4DCFA44-E768-405D-A620-3E8A1038C35E"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo4, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa6, SimplesNacional = 33.00, ImpostoRenda = 53.50, Csll = 21.50, Cofins = 20.55, PisPasep = 4.45, Id = Guid.Parse("025739E5-7CD7-43B5-9965-CAA2790CE578"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    #endregion

                    #region Anexo V
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo5, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa1, SimplesNacional = 15.50, ImpostoRenda = 25.00, Csll = 15.00, Cofins = 14.10, PisPasep = 3.05, Iss = 14.00, Id = Guid.Parse("18C21A25-A4A5-45A6-ADBD-1A061AD09059"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo5, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa2, SimplesNacional = 18.00, ImpostoRenda = 23.00, Csll = 15.00, Cofins = 14.10, PisPasep = 3.05, Iss = 17.00, Id = Guid.Parse("3DB80651-E153-418D-AA34-5AECA06C52F1"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo5, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa3, SimplesNacional = 19.50, ImpostoRenda = 24.00, Csll = 15.00, Cofins = 14.92, PisPasep = 3.23, Iss = 19.00, Id = Guid.Parse("083297C6-896D-42A5-A2B6-DAE2C6827DDE"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo5, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa4, SimplesNacional = 20.50, ImpostoRenda = 21.00, Csll = 15.00, Cofins = 15.74, PisPasep = 3.41, Iss = 21.00, Id = Guid.Parse("558A1BA5-E3F0-4822-9C00-B2D408C47643"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo5, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa5, SimplesNacional = 23.00, ImpostoRenda = 23.00, Csll = 12.50, Cofins = 14.10, PisPasep = 3.05, Iss = 23.50, Id = Guid.Parse("72113372-DB54-47A8-A06B-A053AE1BF89F"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo5, TipoFaixaReceitaBruta = TipoFaixaReceitaBruta.Faixa6, SimplesNacional = 30.50, ImpostoRenda = 35.00, Csll = 15.50, Cofins = 16.44, PisPasep = 3.56, Id = Guid.Parse("AC1C32E4-52FD-4621-80F2-62D28EEF0775"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    #endregion
                };

                context.AliquotasSimplesNacional.AddOrUpdate(x => x.Id, lista.ToArray());

                context.SaveChanges();
            };
        }
    }
}
