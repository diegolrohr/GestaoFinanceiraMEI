namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateEstado : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sigla = c.String(nullable: false, maxLength: 2, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        UtcId = c.String(nullable: false, maxLength: 35, unicode: false),
                        CodigoIbge = c.String(maxLength: 2, unicode: false),
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
            DropTable("dbo.Estado");
        }
    }
}
