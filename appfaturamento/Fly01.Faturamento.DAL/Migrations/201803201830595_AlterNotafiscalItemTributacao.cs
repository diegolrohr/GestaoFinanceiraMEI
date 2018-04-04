namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotafiscalItemTributacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaICMS", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaIPI", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaST", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaCOFINS", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalItemTributacao", "CalculaPIS", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaPIS");
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaCOFINS");
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaST");
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaIPI");
            DropColumn("dbo.NotaFiscalItemTributacao", "CalculaICMS");
        }
    }
}
