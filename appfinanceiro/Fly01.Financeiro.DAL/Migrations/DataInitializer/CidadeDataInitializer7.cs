using System;
using System.Linq;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.Domain.Entities;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.DAL.Migrations.DataInitializer
{
    public class CidadeDataInitializer7
    {
        public void Initialize(AppDataContext context, List<Estado> ufs)
        {
            var id = Guid.Parse("5F0DE045-BFEE-4B07-9B36-EA660C49C5A8");//adicionado depois que já estava em prod
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