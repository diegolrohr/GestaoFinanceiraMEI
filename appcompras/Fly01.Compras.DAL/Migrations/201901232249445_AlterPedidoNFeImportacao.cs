namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPedidoNFeImportacao : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NFeImportacaoProduto", new[] { "NFeImportacao_Id" });
            RenameColumn(table: "dbo.NFeImportacaoProduto", name: "NFeImportacao_Id", newName: "NFeImportacaoId");
            AddColumn("dbo.Pedido", "NFeImportacaoId", c => c.Guid());
            AddColumn("dbo.NFeImportacaoProduto", "NovoProduto", c => c.Boolean(nullable: false));
            AddColumn("dbo.NFeImportacaoProduto", "PedidoItemId", c => c.Guid());
            AddColumn("dbo.PedidoItem", "NFeImportacaoProdutoId", c => c.Guid());
            AlterColumn("dbo.NFeImportacaoProduto", "NFeImportacaoId", c => c.Guid(nullable: false));
            CreateIndex("dbo.NFeImportacaoProduto", "NFeImportacaoId");
            CreateIndex("dbo.NFeImportacaoProduto", "PedidoItemId");
            CreateIndex("dbo.Pedido", "NFeImportacaoId");
            CreateIndex("dbo.PedidoItem", "NFeImportacaoProdutoId");
            AddForeignKey("dbo.NFeImportacaoProduto", "PedidoItemId", "dbo.PedidoItem", "Id");
            AddForeignKey("dbo.Pedido", "NFeImportacaoId", "dbo.NFeImportacao", "Id");
            AddForeignKey("dbo.PedidoItem", "NFeImportacaoProdutoId", "dbo.NFeImportacaoProduto", "Id");
            DropColumn("dbo.NFeImportacaoProduto", "NFeImportadaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NFeImportacaoProduto", "NFeImportadaId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.PedidoItem", "NFeImportacaoProdutoId", "dbo.NFeImportacaoProduto");
            DropForeignKey("dbo.Pedido", "NFeImportacaoId", "dbo.NFeImportacao");
            DropForeignKey("dbo.NFeImportacaoProduto", "PedidoItemId", "dbo.PedidoItem");
            DropIndex("dbo.PedidoItem", new[] { "NFeImportacaoProdutoId" });
            DropIndex("dbo.Pedido", new[] { "NFeImportacaoId" });
            DropIndex("dbo.NFeImportacaoProduto", new[] { "PedidoItemId" });
            DropIndex("dbo.NFeImportacaoProduto", new[] { "NFeImportacaoId" });
            AlterColumn("dbo.NFeImportacaoProduto", "NFeImportacaoId", c => c.Guid());
            DropColumn("dbo.PedidoItem", "NFeImportacaoProdutoId");
            DropColumn("dbo.NFeImportacaoProduto", "PedidoItemId");
            DropColumn("dbo.NFeImportacaoProduto", "NovoProduto");
            DropColumn("dbo.Pedido", "NFeImportacaoId");
            RenameColumn(table: "dbo.NFeImportacaoProduto", name: "NFeImportacaoId", newName: "NFeImportacao_Id");
            CreateIndex("dbo.NFeImportacaoProduto", "NFeImportacao_Id");
        }
    }
}
