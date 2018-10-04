namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterParametroTributarioNFS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "VersaoNFSe", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "TipoAmbienteNFS", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.ParametroTributario", "IncentivoCultura", c => c.Boolean(nullable: false));
            AddColumn("dbo.ParametroTributario", "UsuarioWebServer", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "SenhaWebServer", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "ChaveAutenticacao", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "Autorizacao", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "TipoTributacaoNFS", c => c.Int(nullable: false, defaultValue: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "TipoTributacaoNFS");
            DropColumn("dbo.ParametroTributario", "Autorizacao");
            DropColumn("dbo.ParametroTributario", "ChaveAutenticacao");
            DropColumn("dbo.ParametroTributario", "SenhaWebServer");
            DropColumn("dbo.ParametroTributario", "UsuarioWebServer");
            DropColumn("dbo.ParametroTributario", "IncentivoCultura");
            DropColumn("dbo.ParametroTributario", "TipoAmbienteNFS");
            DropColumn("dbo.ParametroTributario", "VersaoNFSe");
        }
    }
}
