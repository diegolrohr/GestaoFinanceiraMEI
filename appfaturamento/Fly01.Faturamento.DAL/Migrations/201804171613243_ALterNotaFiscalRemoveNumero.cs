namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ALterNotaFiscalRemoveNumero : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NotaFiscal", "Numero");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NotaFiscal", "Numero", c => c.Int(nullable: false, identity: true));
        }
    }
}
