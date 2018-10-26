namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterServicoAddUnidadeMedida : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servico", "CodigoFiscalPrestacao", c => c.String(maxLength: 5, unicode: false));
            AddColumn("dbo.Servico", "UnidadeMedidaId", c => c.Guid());
            CreateIndex("dbo.Servico", "UnidadeMedidaId");
            AddForeignKey("dbo.Servico", "UnidadeMedidaId", "dbo.UnidadeMedida", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servico", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropIndex("dbo.Servico", new[] { "UnidadeMedidaId" });
            DropColumn("dbo.Servico", "UnidadeMedidaId");
            DropColumn("dbo.Servico", "CodigoFiscalPrestacao");
        }
    }
}
