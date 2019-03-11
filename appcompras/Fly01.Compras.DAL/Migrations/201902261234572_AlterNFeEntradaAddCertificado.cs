namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFeEntradaAddCertificado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalEntrada", "CertificadoDigitalId", c => c.Guid());
            AddColumn("dbo.NotaFiscalEntrada", "TipoAmbiente", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.NotaFiscalEntrada", "CertificadoDigitalId");
            AddForeignKey("dbo.NotaFiscalEntrada", "CertificadoDigitalId", "dbo.CertificadoDigital", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscalEntrada", "CertificadoDigitalId", "dbo.CertificadoDigital");
            DropIndex("dbo.NotaFiscalEntrada", new[] { "CertificadoDigitalId" });
            DropColumn("dbo.NotaFiscalEntrada", "TipoAmbiente");
            DropColumn("dbo.NotaFiscalEntrada", "CertificadoDigitalId");
        }
    }
}
