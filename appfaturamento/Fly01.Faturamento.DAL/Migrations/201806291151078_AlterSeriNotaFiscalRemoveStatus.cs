namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSeriNotaFiscalRemoveStatus : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SerieNotaFiscal", "StatusSerieNotaFiscal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SerieNotaFiscal", "StatusSerieNotaFiscal", c => c.Int(nullable: false));
        }
    }
}
