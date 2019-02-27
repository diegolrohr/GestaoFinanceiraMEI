namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNfInutilizadaAddCertificado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalInutilizada", "CertificadoDigitalId", c => c.Guid());
            AddColumn("dbo.NotaFiscalInutilizada", "TipoAmbiente", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.NotaFiscalInutilizada", "CertificadoDigitalId");
            AddForeignKey("dbo.NotaFiscalInutilizada", "CertificadoDigitalId", "dbo.CertificadoDigital", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscalInutilizada", "CertificadoDigitalId", "dbo.CertificadoDigital");
            DropIndex("dbo.NotaFiscalInutilizada", new[] { "CertificadoDigitalId" });
            DropColumn("dbo.NotaFiscalInutilizada", "TipoAmbiente");
            DropColumn("dbo.NotaFiscalInutilizada", "CertificadoDigitalId");
        }
    }
}
