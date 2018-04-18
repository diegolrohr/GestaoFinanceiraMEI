namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CertificadoDigitalAddMd5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CertificadoDigital", "Md5", c => c.String(nullable: false, maxLength: 32, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CertificadoDigital", "Md5");
        }
    }
}
