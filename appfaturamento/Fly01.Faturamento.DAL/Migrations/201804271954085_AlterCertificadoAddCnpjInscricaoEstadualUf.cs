namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterCertificadoAddCnpjInscricaoEstadualUf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CertificadoDigital", "Cnpj", c => c.String(maxLength: 16, unicode: false));
            AddColumn("dbo.CertificadoDigital", "InscricaoEstadual", c => c.String(maxLength: 18, unicode: false));
            AddColumn("dbo.CertificadoDigital", "UF", c => c.String(maxLength: 2, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CertificadoDigital", "UF");
            DropColumn("dbo.CertificadoDigital", "InscricaoEstadual");
            DropColumn("dbo.CertificadoDigital", "Cnpj");
        }
    }
}
