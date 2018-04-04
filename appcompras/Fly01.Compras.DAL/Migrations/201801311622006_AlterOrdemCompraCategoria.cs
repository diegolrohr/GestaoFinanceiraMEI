namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOrdemCompraCategoria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemCompra", "CategoriaId", c => c.Guid());
            CreateIndex("dbo.OrdemCompra", "CategoriaId");
            AddForeignKey("dbo.OrdemCompra", "CategoriaId", "dbo.Categoria", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdemCompra", "CategoriaId", "dbo.Categoria");
            DropIndex("dbo.OrdemCompra", new[] { "CategoriaId" });
            DropColumn("dbo.OrdemCompra", "CategoriaId");
        }
    }
}
