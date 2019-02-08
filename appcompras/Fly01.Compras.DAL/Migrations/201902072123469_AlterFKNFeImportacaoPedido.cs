namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFKNFeImportacaoPedido : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pedido", "NFeImportacaoId", "dbo.NFeImportacao");
            DropForeignKey("dbo.PedidoItem", "NFeImportacaoProdutoId", "dbo.NFeImportacaoProduto");
            DropIndex("dbo.Pedido", new[] { "NFeImportacaoId" });
            DropIndex("dbo.PedidoItem", new[] { "NFeImportacaoProdutoId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.PedidoItem", "NFeImportacaoProdutoId");
            CreateIndex("dbo.Pedido", "NFeImportacaoId");
            AddForeignKey("dbo.PedidoItem", "NFeImportacaoProdutoId", "dbo.NFeImportacaoProduto", "Id");
            AddForeignKey("dbo.Pedido", "NFeImportacaoId", "dbo.NFeImportacao", "Id");
        }
    }
}
