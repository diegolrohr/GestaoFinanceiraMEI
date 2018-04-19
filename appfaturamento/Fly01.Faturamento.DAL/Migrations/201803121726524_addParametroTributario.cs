namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addParametroTributario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "NumeroRetornoNF", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "TipoAmbiente", c => c.Int(nullable: false));
            AddColumn("dbo.ParametroTributario", "TipoVersaoNFe", c => c.Int(nullable: false));
            AddColumn("dbo.ParametroTributario", "TipoModalidade", c => c.Int(nullable: false));
            DropColumn("dbo.ParametroTributario", "IncentivoCultura");
            DropColumn("dbo.ParametroTributario", "TipoRegimeEspecialTrib");
            DropColumn("dbo.ParametroTributario", "TipoMensagemNFSE");
            DropColumn("dbo.ParametroTributario", "TipoLayoutNFSE");
            DropColumn("dbo.ParametroTributario", "NovoModeloUnicoXMLTSS");
            DropColumn("dbo.ParametroTributario", "SIAFI");
            DropColumn("dbo.ParametroTributario", "TipoAmbienteNFS");
            DropColumn("dbo.ParametroTributario", "Versao");
            DropColumn("dbo.ParametroTributario", "Usuario");
            DropColumn("dbo.ParametroTributario", "Senha");
            DropColumn("dbo.ParametroTributario", "ChaveAutenticacao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParametroTributario", "ChaveAutenticacao", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "Senha", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "Usuario", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "Versao", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "TipoAmbienteNFS", c => c.Int(nullable: false));
            AddColumn("dbo.ParametroTributario", "SIAFI", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ParametroTributario", "NovoModeloUnicoXMLTSS", c => c.Boolean(nullable: false));
            AddColumn("dbo.ParametroTributario", "TipoLayoutNFSE", c => c.Int());
            AddColumn("dbo.ParametroTributario", "TipoMensagemNFSE", c => c.Int());
            AddColumn("dbo.ParametroTributario", "TipoRegimeEspecialTrib", c => c.Int(nullable: false));
            AddColumn("dbo.ParametroTributario", "IncentivoCultura", c => c.Boolean(nullable: false));
            DropColumn("dbo.ParametroTributario", "TipoModalidade");
            DropColumn("dbo.ParametroTributario", "TipoVersaoNFe");
            DropColumn("dbo.ParametroTributario", "TipoAmbiente");
            DropColumn("dbo.ParametroTributario", "NumeroRetornoNF");
        }
    }
}
