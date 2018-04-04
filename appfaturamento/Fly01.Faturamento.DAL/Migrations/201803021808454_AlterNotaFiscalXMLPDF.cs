namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalXMLPDF : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "XML", c => c.String(unicode: false));
            AddColumn("dbo.NotaFiscal", "PDF", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscal", "PDF");
            DropColumn("dbo.NotaFiscal", "XML");
        }
    }
}
