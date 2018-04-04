namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCertificadoDigitalObrigSenha : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CertificadoDigital", "Senha", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CertificadoDigital", "Senha", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
