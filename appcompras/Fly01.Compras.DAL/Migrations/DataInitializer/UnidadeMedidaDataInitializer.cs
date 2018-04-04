using Fly01.Compras.DAL.Migrations.DataInitializer.Contract;
using Fly01.Compras.Domain.Entities;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Fly01.Compras.DAL.Migrations.DataInitializer
{
    public class UnidadeMedidaDataInitializer : IDataInitializer
    {
        public void Initialize(AppDataContext context)
        {
            if (context.UnidadeMedidas.Any()) return;

            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("0FDEE353-90ED-4C58-AE0A-0AC42DDE6E2B"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "SC", Descricao = "SACO" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("FAEACD07-4486-4F5A-97DB-14628E304191"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "G", Descricao = "GRAMA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("5DF3D0B1-8F68-425C-A551-3A3D0800E04B"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "AR", Descricao = "ARROBA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("B858D9FF-1349-4448-85D5-3E4AEAC50AC9"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "FL", Descricao = "FOLHAS" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("1D9CE497-5109-4F0F-A24E-622F67E6E2F7"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "MG", Descricao = "MILIGRAMA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("05987C20-8108-4203-9BC6-6E2BF7FA30C5"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "M", Descricao = "METRO" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("CA94AB2B-0D73-41D2-AB8A-6F088917E79D"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "ML", Descricao = "MILILITRO" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("2047E9BF-E385-40CE-A065-7C6CF151D9CE"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "CX", Descricao = "CAIXA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("AAADC9AC-4218-4DF7-867E-822899655360"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "KG", Descricao = "QUILOGRAMA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("D63CB9DD-9872-4CD1-93E0-893427889879"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "UN", Descricao = "UNIDADE" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("B3CF2E7A-681F-407F-B33E-94CD7098E113"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "TN", Descricao = "TONELADA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("2F99ECBB-EAE0-4B2A-BA46-AC32C8FC1EE3"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "LB", Descricao = "LIBRA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("5BF34548-C7AC-4F53-90C5-B19D93A2322F"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "M2", Descricao = "METRO QUADRADO" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("63DFCC82-9AE4-4FF2-BB46-C2B6E10A939D"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "GL", Descricao = "GALAO" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("AC007328-736F-4CBE-9D1B-CA9FF3EBC391"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "L", Descricao = "LITRO" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("6F0427B1-0B83-4ACF-8849-CE51F13E1F94"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "MM", Descricao = "MILIMETRO" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("213335F3-A967-4436-9C80-D550B14BA659"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "YD", Descricao = "JARDA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("CC1DA5AA-9435-45CF-8378-D88235D1AB5A"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "HR", Descricao = "HORA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("11F91C21-05BB-45F7-A374-E266C59DF94E"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "PC", Descricao = "PECA" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("DF311088-0C0C-4117-9232-E64D30E295F9"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "M3", Descricao = "METRO CUBICO" });
            context.UnidadeMedidas.AddOrUpdate(new UnidadeMedida { Id = Guid.Parse("C2DF1FD5-375D-4F6B-B110-E8EDA1B57F24"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Abreviacao = "MI", Descricao = "MILHEIRO" });

            context.SaveChanges();

        }
    }
}