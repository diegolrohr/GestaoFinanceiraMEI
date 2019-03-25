using Fly01.Faturamento.DAL.Migrations.DataInitializer.Contract;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.DAL.Migrations.DataInitializer
{
    public class NewCestDataInitializer : IDataInitializer
    {
        public void Initialize(AppDataContext context)
        {
            var idCest = Guid.Parse("6CE39B6F-464C-4455-AF2B-FB5686533BBD");
            if (!context.Cests.Any(x => x.Id == idCest))
            {
                context.Cests.Add(new Cest() { Id = idCest, Codigo = "68091100", Descricao = "Produtos de limpeza e conservação doméstica", Segmento = "28. Venda de mercadorias pelo sistema porta a porta", Item = "63.0", Anexo = "", NcmId = Guid.Parse("71200C8E-63CF-4A2E-98FE-9CBEB47C0E36"), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true });
                context.SaveChanges();
            }        
        }
    }
}