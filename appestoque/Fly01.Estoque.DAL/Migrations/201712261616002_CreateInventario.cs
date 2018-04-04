namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateInventario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventario",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DataUltimaInteracao = c.DateTime(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        InventarioStatus = c.Int(nullable: false),
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
            DropTable("dbo.Inventario");
        }
    }
}
