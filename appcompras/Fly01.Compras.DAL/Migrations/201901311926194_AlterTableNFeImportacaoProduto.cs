namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableNFeImportacaoProduto : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NFeImportacaoProduto", "AtualizaValorCompra");
            DropColumn("dbo.NFeImportacaoProduto", "TipoValorVenda");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NFeImportacaoProduto", "TipoValorVenda", c => c.Int(nullable: false));
            AddColumn("dbo.NFeImportacaoProduto", "AtualizaValorCompra", c => c.Boolean(nullable: false));
        }
    }
}
