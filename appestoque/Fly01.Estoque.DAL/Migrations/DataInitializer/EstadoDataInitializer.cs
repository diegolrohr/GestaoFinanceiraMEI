using Fly01.Core.Entities.Domains.Commons;
using Fly01.Estoque.DAL.Migrations.DataInitializer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Estoque.DAL.Migrations.DataInitializer
{
    public class EstadoDataInitializer : IDataInitializer
    {
        public void Initialize(AppDataContext context)
        {
            var listOfStates = new List<Estado>()
            {
                new Estado() { Id = Guid.Parse("F5FCBC8C-FEF6-4912-BDF7-207CC38A1165"), CodigoIbge = "12", Sigla = "AC", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Acre", UtcId = "SA Pacific Standard Time" },
                new Estado() { Id = Guid.Parse("D3736560-3965-4386-A861-27D855079100"), CodigoIbge = "53", Sigla = "DF", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Distrito Federal", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("556A4ECB-EE84-43F7-A77D-2A83172C86E5"), CodigoIbge = "52", Sigla = "GO", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Goiás", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("83048E48-C250-4B64-A9C3-2AECB9849EF8"), CodigoIbge = "31", Sigla = "MG", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Minas Gerais", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("274C5939-23B6-45B4-8D8B-2AEF839D3413"), CodigoIbge = "14", Sigla = "RR", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Roraima", UtcId = "Atlantic Standard Time" },
                new Estado() { Id = Guid.Parse("8AD5F994-2DA3-49CC-B8B4-2B609BE7D0E0"), CodigoIbge = "21", Sigla = "MA", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Maranhão", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("5820FE67-FDB7-407D-B5CB-3108DC612A95"), CodigoIbge = "23", Sigla = "CE", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Ceará", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("16546247-B363-4FCD-B957-315DB92ED63C"), CodigoIbge = "22", Sigla = "PI", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Piauí", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("FB184864-8DD7-47F0-ABF7-34476B6C27B1"), CodigoIbge = "11", Sigla = "RO", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Rondônia", UtcId = "Atlantic Standard Time" },
                new Estado() { Id = Guid.Parse("2C3C50A5-89B9-452A-B46A-38CD86896700"), CodigoIbge = "33", Sigla = "RJ", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Rio de Janeiro", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("3891825C-A021-47FD-80B9-41F6F0AA65AF"), CodigoIbge = "41", Sigla = "PR", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Paraná", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("92B220E9-C472-481F-A943-444AC5DC00B8"), CodigoIbge = "51", Sigla = "MT", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Mato Grosso", UtcId = "Central Brazilian Standard Time" },
                new Estado() { Id = Guid.Parse("EC896B88-084F-4B18-8F07-736CFE6E337D"), CodigoIbge = "35", Sigla = "SP", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "São Paulo", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("4DC9F47E-E560-4CB6-9D0C-862DBECAF89B"), CodigoIbge = "25", Sigla = "PB", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Paraíba", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("52E3277B-E0E2-49EC-8976-A6D84F13741C"), CodigoIbge = "15", Sigla = "PA", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Pará", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("FDA8BC5C-5FDB-4451-841C-A746705E620C"), CodigoIbge = "43", Sigla = "RS", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Rio Grande do Sul", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("424DF1B9-38A8-485B-A241-AB9ED231D54C"), CodigoIbge = "26", Sigla = "PE", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Pernambuco", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("F32443A4-F172-42E9-84FC-B05E1F12FC31"), CodigoIbge = "42", Sigla = "SC", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Santa Catarina", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("A21B30FE-2F2C-4670-8A45-B24C56A82E9B"), CodigoIbge = "27", Sigla = "AL", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Alagoas", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("573BE76F-428C-4CDE-9400-C7B402F16FC9"), CodigoIbge = "50", Sigla = "MS", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Mato Grosso do Sul", UtcId = "Central Brazilian Standard Time" },
                new Estado() { Id = Guid.Parse("EDE334EB-BDF3-4CB3-8819-CBC6A9265F5A"), CodigoIbge = "24", Sigla = "RN", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Rio Grande do Norte", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("BF3AC31F-A12A-4F43-AC4C-E835444661E8"), CodigoIbge = "16", Sigla = "AP", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Amapá", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("81F4C793-C448-4BAA-ACC8-EF1F8B634785"), CodigoIbge = "29", Sigla = "BA", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Bahia", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("37B29AA5-7BF0-4304-AB8B-F09A9E40686A"), CodigoIbge = "32", Sigla = "ES", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Espírito Santo", UtcId = "E. South America Standard Time" },
                new Estado() { Id = Guid.Parse("DEF11262-9538-43D4-8FF1-F57F3593AD36"), CodigoIbge = "13", Sigla = "AM", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Amazonas", UtcId = "Atlantic Standard Time" },
                new Estado() { Id = Guid.Parse("82146413-7CF2-41D8-8EAA-FCDCAD37D863"), CodigoIbge = "28", Sigla = "SE", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Sergipe", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("8B3ABE67-33C1-4FDA-B719-FEC4FF740C23"), CodigoIbge = "17", Sigla = "TO", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Tocantins", UtcId = "Bahia Standard Time" },
                new Estado() { Id = Guid.Parse("DD4D2CA7-A30A-4660-88D3-9D132916832E"), CodigoIbge = "99", Sigla = "EX", DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, Nome = "Exterior", UtcId = "Bahia Standard Time" }
            };

            if (!context.Estados.Any())
            {
                context.Estados.AddRange(listOfStates);
                context.SaveChanges();
            }
        }
    }
}