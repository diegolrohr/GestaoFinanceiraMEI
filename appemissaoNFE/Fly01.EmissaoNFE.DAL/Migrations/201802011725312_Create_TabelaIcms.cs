namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Create_TabelaIcms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TabelaIcms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EstadoOrigemId = c.Guid(nullable: false),
                        EstadoDestinoId = c.Guid(nullable: false),
                        SiglaOrigem = c.String(maxLength: 200, unicode: false),
                        SiglaDestino = c.String(maxLength: 200, unicode: false),
                        IcmsAliquota = c.Double(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estado", t => t.EstadoDestinoId)
                .ForeignKey("dbo.Estado", t => t.EstadoOrigemId)
                .Index(t => t.EstadoOrigemId)
                .Index(t => t.EstadoDestinoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TabelaIcms", "EstadoOrigemId", "dbo.Estado");
            DropForeignKey("dbo.TabelaIcms", "EstadoDestinoId", "dbo.Estado");
            DropIndex("dbo.TabelaIcms", new[] { "EstadoDestinoId" });
            DropIndex("dbo.TabelaIcms", new[] { "EstadoOrigemId" });
            DropTable("dbo.TabelaIcms");
        }
    }
}
