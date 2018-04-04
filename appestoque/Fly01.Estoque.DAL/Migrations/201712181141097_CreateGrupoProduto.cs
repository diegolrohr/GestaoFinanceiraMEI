namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateGrupoProduto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GrupoProduto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        AliquotaIpi = c.Double(nullable: false),
                        TipoProduto = c.Int(nullable: false),
                        UnidadeMedidaId = c.Guid(),
                        NcmId = c.Guid(),
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
                .ForeignKey("dbo.NCM", t => t.NcmId)
                .Index(t => t.NcmId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GrupoProduto", "NcmId", "dbo.NCM");
            DropIndex("dbo.GrupoProduto", new[] { "NcmId" });
            DropTable("dbo.GrupoProduto");
        }
    }
}
