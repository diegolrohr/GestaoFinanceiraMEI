namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterUnidadeMedida : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UnidadeMedida", "Abreviacao", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AddColumn("dbo.UnidadeMedida", "Descricao", c => c.String(nullable: false, maxLength: 20, unicode: false));
            CreateIndex("dbo.GrupoProduto", "UnidadeMedidaId");
            AddForeignKey("dbo.GrupoProduto", "UnidadeMedidaId", "dbo.UnidadeMedida", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GrupoProduto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropIndex("dbo.GrupoProduto", new[] { "UnidadeMedidaId" });
            DropColumn("dbo.UnidadeMedida", "Descricao");
            DropColumn("dbo.UnidadeMedida", "Abreviacao");
        }
    }
}
