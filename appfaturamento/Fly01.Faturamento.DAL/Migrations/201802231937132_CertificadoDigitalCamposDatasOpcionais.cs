namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CertificadoDigitalCamposDatasOpcionais : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CertificadoDigital", "DataEmissao", c => c.DateTime());
            AlterColumn("dbo.CertificadoDigital", "DataExpiracao", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CertificadoDigital", "DataExpiracao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CertificadoDigital", "DataEmissao", c => c.DateTime(nullable: false));
        }
    }
}
