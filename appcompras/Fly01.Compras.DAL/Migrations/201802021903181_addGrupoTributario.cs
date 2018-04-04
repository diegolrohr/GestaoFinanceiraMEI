namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addGrupoTributario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cfop",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GrupoTributario",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        CfopId = c.Guid(nullable: false),
                        CalculaIcms = c.Boolean(nullable: false),
                        TipoTributacaoICMS = c.Int(nullable: false),
                        CalculaIcmsDifal = c.Boolean(nullable: false),
                        AplicaIpiBaseIcms = c.Boolean(nullable: false),
                        AplicaFreteBaseIcms = c.Boolean(nullable: false),
                        AplicaDespesaBaseIcms = c.Boolean(nullable: false),
                        CalculaIpi = c.Boolean(nullable: false),
                        TipoTributacaoIPI = c.Int(nullable: false),
                        AplicaFreteBaseIpi = c.Boolean(nullable: false),
                        AplicaDespesaBaseIpi = c.Boolean(nullable: false),
                        CalculaPis = c.Boolean(nullable: false),
                        TipoTributacaoPIS = c.Int(nullable: false),
                        AplicaFreteBasePis = c.Boolean(nullable: false),
                        AplicaDespesaBasePis = c.Boolean(nullable: false),
                        CalculaCofins = c.Boolean(nullable: false),
                        TipoTributacaoCOFINS = c.Int(nullable: false),
                        AplicaFreteBaseCofins = c.Boolean(nullable: false),
                        AplicaDespesaBaseCofins = c.Boolean(nullable: false),
                        CalculaIss = c.Boolean(nullable: false),
                        TipoTributacaoISS = c.Int(nullable: false),
                        TipoPagamentoImpostoISS = c.Int(nullable: false),
                        TipoCFPS = c.Int(nullable: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cfop", t => t.CfopId)
                .Index(t => t.CfopId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GrupoTributario", "CfopId", "dbo.Cfop");
            DropIndex("dbo.GrupoTributario", new[] { "CfopId" });
            DropTable("dbo.GrupoTributario");
            DropTable("dbo.Cfop");
        }
    }
}
