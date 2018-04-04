using System.Data.Entity.Migrations;

namespace Fly01.Compras.DAL.Migrations
{ 
    public partial class CreateCategoria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        CategoriaPaiId = c.Guid(),
                        TipoCarteira = c.Int(nullable: false),
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
                .ForeignKey("dbo.Categoria", t => t.CategoriaPaiId)
                .Index(t => t.CategoriaPaiId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categoria", "CategoriaPaiId", "dbo.Categoria");
            DropIndex("dbo.Categoria", new[] { "CategoriaPaiId" });
            DropTable("dbo.Categoria");
        }
    }
}
