namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateInventarioItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventarioItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SaldoAtual = c.Double(nullable: false),
                        ProdutoId = c.Guid(nullable: false),
                        InventarioId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.Inventario", t => t.InventarioId)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .Index(t => t.ProdutoId)
                .Index(t => t.InventarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InventarioItem", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.InventarioItem", "InventarioId", "dbo.Inventario");
            DropIndex("dbo.InventarioItem", new[] { "InventarioId" });
            DropIndex("dbo.InventarioItem", new[] { "ProdutoId" });
            DropTable("dbo.InventarioItem");
        }
    }
}
