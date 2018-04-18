namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterSerieENotaFiscal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "SerieNotaFiscalId", c => c.Guid());
            AddColumn("dbo.NotaFiscal", "NumNotaFiscal", c => c.Int());
            AlterColumn("dbo.SerieNotaFiscal", "NumNotaFiscal", c => c.Int(nullable: false));
            CreateIndex("dbo.NotaFiscal", "SerieNotaFiscalId");
            AddForeignKey("dbo.NotaFiscal", "SerieNotaFiscalId", "dbo.SerieNotaFiscal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscal", "SerieNotaFiscalId", "dbo.SerieNotaFiscal");
            DropIndex("dbo.NotaFiscal", new[] { "SerieNotaFiscalId" });
            AlterColumn("dbo.SerieNotaFiscal", "NumNotaFiscal", c => c.Double(nullable: false));
            DropColumn("dbo.NotaFiscal", "NumNotaFiscal");
            DropColumn("dbo.NotaFiscal", "SerieNotaFiscalId");
        }
    }
}
