namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CertificadoDigitalRemoveChaveAdicionaVersao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CertificadoDigital", "Versao", c => c.String(maxLength: 30, unicode: false));
            DropColumn("dbo.CertificadoDigital", "Chave");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CertificadoDigital", "Chave", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.CertificadoDigital", "Versao");
        }
    }
}
