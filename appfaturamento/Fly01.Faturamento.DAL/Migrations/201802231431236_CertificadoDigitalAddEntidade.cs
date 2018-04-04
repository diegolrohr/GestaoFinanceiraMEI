namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CertificadoDigitalAddEntidade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CertificadoDigital", "Entidade", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.CertificadoDigital", "Certificado", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CertificadoDigital", "Certificado", c => c.String(nullable: false, maxLength: 200, unicode: false));
            DropColumn("dbo.CertificadoDigital", "Entidade");
        }
    }
}
