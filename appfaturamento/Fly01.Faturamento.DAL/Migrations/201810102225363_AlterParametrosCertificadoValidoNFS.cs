namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterParametrosCertificadoValidoNFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CertificadoDigital", "CertificadoValidoNFS", c => c.Boolean(nullable: false));
            AddColumn("dbo.ParametroTributario", "FormatarCodigoISS", c => c.Boolean(nullable: false));
            AddColumn("dbo.ParametroTributario", "ParametroValidoNFS", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "ParametroValidoNFS");
            DropColumn("dbo.ParametroTributario", "FormatarCodigoISS");
            DropColumn("dbo.CertificadoDigital", "CertificadoValidoNFS");
        }
    }
}
