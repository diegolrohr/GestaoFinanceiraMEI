namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFeImportacaoProduto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NFeImportacaoProduto", "Quantidade", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NFeImportacaoProduto", "Quantidade", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
