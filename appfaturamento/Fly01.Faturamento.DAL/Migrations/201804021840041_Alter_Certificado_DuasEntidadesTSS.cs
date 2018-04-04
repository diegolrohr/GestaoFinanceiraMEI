namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_Certificado_DuasEntidadesTSS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CertificadoDigital", "EntidadeHomologacao", c => c.String(maxLength: 6, unicode: false));
            AddColumn("dbo.CertificadoDigital", "EntidadeProducao", c => c.String(maxLength: 6, unicode: false));
            DropColumn("dbo.CertificadoDigital", "Entidade");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CertificadoDigital", "Entidade", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.CertificadoDigital", "EntidadeProducao");
            DropColumn("dbo.CertificadoDigital", "EntidadeHomologacao");
        }
    }
}
