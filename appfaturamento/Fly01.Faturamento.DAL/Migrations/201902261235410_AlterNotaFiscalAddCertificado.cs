namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalAddCertificado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "CertificadoDigitalId", c => c.Guid());
            AddColumn("dbo.NotaFiscal", "TipoAmbiente", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.NotaFiscal", "CertificadoDigitalId");
            AddForeignKey("dbo.NotaFiscal", "CertificadoDigitalId", "dbo.CertificadoDigital", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscal", "CertificadoDigitalId", "dbo.CertificadoDigital");
            DropIndex("dbo.NotaFiscal", new[] { "CertificadoDigitalId" });
            DropColumn("dbo.NotaFiscal", "TipoAmbiente");
            DropColumn("dbo.NotaFiscal", "CertificadoDigitalId");
        }
    }
}
