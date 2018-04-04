namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterPedido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedido", "OrcamentoOrigemId", c => c.Guid());
            AlterColumn("dbo.OrdemCompraItem", "Quantidade", c => c.Double(nullable: false));
            CreateIndex("dbo.Pedido", "OrcamentoOrigemId");
            AddForeignKey("dbo.Pedido", "OrcamentoOrigemId", "dbo.Orcamento", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pedido", "OrcamentoOrigemId", "dbo.Orcamento");
            DropIndex("dbo.Pedido", new[] { "OrcamentoOrigemId" });
            AlterColumn("dbo.OrdemCompraItem", "Quantidade", c => c.Int(nullable: false));
            DropColumn("dbo.Pedido", "OrcamentoOrigemId");
        }
    }
}
