namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalinutilizadaAddData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalInutilizada", "Data", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotaFiscalInutilizada", "Data");
        }
    }
}
