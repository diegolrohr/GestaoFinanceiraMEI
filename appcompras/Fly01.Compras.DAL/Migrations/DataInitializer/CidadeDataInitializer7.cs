using System;
using System.Linq;
using System.Collections.Generic;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.DAL.Migrations.DataInitializer
{
    public class CidadeDataInitializer7
    {
        public void Initialize(AppDataContext context, List<Estado> ufs)
        {
            //adicionado depois que já estava em prod
            var id = Guid.Parse("5F0DE045-BFEE-4B07-9B36-EA660C49C5A8");
            if (!context.Cidades.Any(x => x.Id == id))
            {
                var listOfCidades = new List<Cidade>()
                {
                    new Cidade() { Id = Guid.Parse("5F0DE045-BFEE-4B07-9B36-EA660C49C5A8"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, CodigoIbge = "9999999", Nome = "Exterior", EstadoId = ufs.Where(u => u.CodigoIbge.Equals("99")).FirstOrDefault().Id }
                };

                context.Cidades.AddRange(listOfCidades);
                context.SaveChanges();
            }
        }
    }
}