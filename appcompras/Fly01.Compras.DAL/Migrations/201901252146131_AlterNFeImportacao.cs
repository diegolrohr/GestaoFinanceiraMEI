namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFeImportacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFeImportacao", "DataEmissao", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.NFeImportacao", "SomatorioProduto", c => c.Double(nullable: false));
            DropColumn("dbo.NFeImportacao", "Json");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NFeImportacao", "Json", c => c.String(unicode: false));
            DropColumn("dbo.NFeImportacao", "SomatorioProduto");
            DropColumn("dbo.NFeImportacao", "DataEmissao");
        }
    }
}
