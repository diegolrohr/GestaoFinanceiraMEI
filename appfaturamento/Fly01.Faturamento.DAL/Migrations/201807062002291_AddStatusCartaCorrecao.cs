namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusCartaCorrecao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalCartaCorrecao", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.NotaFiscalCartaCorrecao", "Numero", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscalCartaCorrecao", "Numero");
            DropColumn("dbo.NotaFiscalCartaCorrecao", "Status");
        }
    }
}
