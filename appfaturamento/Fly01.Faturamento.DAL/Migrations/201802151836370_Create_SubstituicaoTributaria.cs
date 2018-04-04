namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Create_SubstituicaoTributaria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubstituicaoTributaria",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NcmId = c.Guid(nullable: false),
                        EstadoOrigemId = c.Guid(nullable: false),
                        EstadoDestinoId = c.Guid(nullable: false),
                        Mva = c.Double(nullable: false),
                        TipoSubstituicaoTributaria = c.Int(nullable: false),
                        CestId = c.Guid(),
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
                .ForeignKey("dbo.Cest", t => t.CestId)
                .ForeignKey("dbo.Estado", t => t.EstadoDestinoId)
                .ForeignKey("dbo.Estado", t => t.EstadoOrigemId)
                .ForeignKey("dbo.NCM", t => t.NcmId)
                .Index(t => t.NcmId)
                .Index(t => t.EstadoOrigemId)
                .Index(t => t.EstadoDestinoId)
                .Index(t => t.CestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubstituicaoTributaria", "NcmId", "dbo.NCM");
            DropForeignKey("dbo.SubstituicaoTributaria", "EstadoOrigemId", "dbo.Estado");
            DropForeignKey("dbo.SubstituicaoTributaria", "EstadoDestinoId", "dbo.Estado");
            DropForeignKey("dbo.SubstituicaoTributaria", "CestId", "dbo.Cest");
            DropIndex("dbo.SubstituicaoTributaria", new[] { "CestId" });
            DropIndex("dbo.SubstituicaoTributaria", new[] { "EstadoDestinoId" });
            DropIndex("dbo.SubstituicaoTributaria", new[] { "EstadoOrigemId" });
            DropIndex("dbo.SubstituicaoTributaria", new[] { "NcmId" });
            DropTable("dbo.SubstituicaoTributaria");
        }
    }
}
