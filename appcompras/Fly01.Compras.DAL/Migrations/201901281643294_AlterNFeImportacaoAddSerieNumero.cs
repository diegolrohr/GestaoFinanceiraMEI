namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFeImportacaoAddSerieNumero : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFeImportacao", "Serie", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AddColumn("dbo.NFeImportacao", "Numero", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NFeImportacao", "Numero");
            DropColumn("dbo.NFeImportacao", "Serie");
        }
    }
}
