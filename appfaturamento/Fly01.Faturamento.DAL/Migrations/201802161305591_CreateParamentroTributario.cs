namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateParamentroTributario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParametroTributario",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SimplesNacional = c.Boolean(nullable: false),
                        IncentivoCultura = c.Boolean(nullable: false),
                        TipoRegimeEspecialTrib = c.Int(nullable: false),
                        AliquotaSimplesNacional = c.Double(nullable: false),
                        AliquotaISS = c.Double(nullable: false),
                        AliquotaPISPASEP = c.Double(nullable: false),
                        AliquotaCOFINS = c.Double(nullable: false),
                        RegistroSimplificadoMT = c.Boolean(nullable: false),
                        MensagemPadraoNota = c.String(maxLength: 200, unicode: false),
                        TipoMensagemNFSE = c.Int(),
                        TipoLayoutNFSE = c.Int(),
                        NovoModeloUnicoXMLTSS = c.Boolean(nullable: false),
                        SIAFI = c.String(maxLength: 200, unicode: false),
                        TipoAmbienteNFS = c.Int(nullable: false),
                        Versao = c.String(maxLength: 200, unicode: false),
                        Usuario = c.String(maxLength: 200, unicode: false),
                        Senha = c.String(maxLength: 200, unicode: false),
                        ChaveAutenticacao = c.String(maxLength: 200, unicode: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ParametroTributario");
        }
    }
}
