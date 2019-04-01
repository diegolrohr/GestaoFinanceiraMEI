using System.Linq;
using System.Data.Entity.Migrations;
using Fly01.EmissaoNFE.Domain.Entities;
using System;

namespace Fly01.EmissaoNFE.DAL.Migrations.DataInitializer
{
    public class ResponsavelTecnicoDataInitializer
    {
        public void Initialize(AppDataContext context)
        {
            if (!context.ResponsavelTecnico.Any())
            {
                var responsavel = new ResponsavelTecnico()
                {
                    Id = Guid.Parse("A39B871C-6913-495C-88F8-1F2668B6AABA"),
                    UsuarioInclusao = "Seed",
                    DataInclusao = DateTime.Now,
                    CNPJ = "53113791000122",
                    Contato = "Ramon Martins Da Silva",
                    Email = "resp_tecnico_dfe_mpn@totvs.com.br",
                    Fone = "11966068881",
                };
                context.ResponsavelTecnico.AddOrUpdate(x => x.Id, responsavel);

                context.SaveChanges();
            };
        }
    }
}
