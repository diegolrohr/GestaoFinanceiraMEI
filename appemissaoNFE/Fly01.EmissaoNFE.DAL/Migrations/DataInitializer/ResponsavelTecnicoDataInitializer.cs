using System.Linq;
using System.Data.Entity.Migrations;
using Fly01.EmissaoNFE.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Fly01.EmissaoNFE.DAL.Migrations.DataInitializer
{
    public class ResponsavelTecnicoDataInitializer
    {
        public void Initialize(AppDataContext context)
        {
            var needUpdateResponsavel = true;
            if (!context.ResponsavelTecnico.Any() || needUpdateResponsavel)
            {
                var lista = new List<ResponsavelTecnico>() {
                    new ResponsavelTecnico()
                    {
                    Id = Guid.Parse("A39B871C-6913-495C-88F8-1F2668B6AABA"),
                    UsuarioInclusao = "Seed",
                    DataInclusao = DateTime.Now,
                    CNPJ = "19116002000179",
                    Contato = "Ramon Martins Da Silva",
                    Email = "resp_tecnico_dfe_mpn@totvs.com.br",
                     Fone = "1128593905",
                    },
                    new ResponsavelTecnico()//PR
                    {
                        Id = Guid.Parse("D21F8B07-580B-47C9-BD1E-5DF033748D7C"),
                        UsuarioInclusao = "Seed",
                        DataInclusao = DateTime.Now,
                        CNPJ = "53113791000122",
                        Contato = "Wilson De Godoy Soares Junior",
                        Email = "resp_tecnico_dfe_mpn@totvs.com.br",
                        Fone = "1128593905",
                    }
                };
                context.ResponsavelTecnico.AddOrUpdate(x => x.Id, lista.ToArray());

                context.SaveChanges();
            };
        }
    }
}
