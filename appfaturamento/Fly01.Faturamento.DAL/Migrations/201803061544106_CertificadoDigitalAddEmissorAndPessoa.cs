namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CertificadoDigitalAddEmissorAndPessoa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CertificadoDigital", "Emissor", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.CertificadoDigital", "Pessoa", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CertificadoDigital", "Pessoa");
            DropColumn("dbo.CertificadoDigital", "Emissor");
        }
    }
}
