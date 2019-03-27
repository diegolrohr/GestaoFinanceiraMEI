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
        public async Task Initialize(AppDataContext context)
        {
            var needUpdateAliquotas = false;

            if (!context.AliquotasSimplesNacional.Any() || needUpdateAliquotas)
            {
                var lista = new List<AliquotaSimplesNacional>() {
                    #region Anexo I
                    new AliquotaSimplesNacional() { TipoEnquadramentoEmpresa = TipoEnquadramentoEmpresa.Anexo1,  Id = Guid.Parse(""), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true },
                    #endregion

                };

                context.AliquotasSimplesNacional.AddOrUpdate(x => x.Id, lista.ToArray());

                await context.SaveChanges();
            };
        }
    }
}
