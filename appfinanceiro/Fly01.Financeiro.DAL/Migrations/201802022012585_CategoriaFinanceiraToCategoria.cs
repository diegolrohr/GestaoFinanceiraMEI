namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CategoriaFinanceiraToCategoria : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CategoriaFinanceira", newName: "Categoria");
            RenameColumn(table: "dbo.ContaFinanceira", name: "CategoriaFinanceiraId", newName: "CategoriaId");
            RenameIndex(table: "dbo.ContaFinanceira", name: "IX_CategoriaFinanceiraId", newName: "IX_CategoriaId");
            AddColumn("dbo.Movimentacao", "CategoriaId", c => c.Guid());
            DropColumn("dbo.Categoria", "Classe");
            DropColumn("dbo.Categoria", "Codigo");
            DropColumn("dbo.Movimentacao", "CategoriaFinanceiraId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movimentacao", "CategoriaFinanceiraId", c => c.Guid());
            AddColumn("dbo.Categoria", "Codigo", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Categoria", "Classe", c => c.Int(nullable: false));
            DropColumn("dbo.Movimentacao", "CategoriaId");
            RenameIndex(table: "dbo.ContaFinanceira", name: "IX_CategoriaId", newName: "IX_CategoriaFinanceiraId");
            RenameColumn(table: "dbo.ContaFinanceira", name: "CategoriaId", newName: "CategoriaFinanceiraId");
            RenameTable(name: "dbo.Categoria", newName: "CategoriaFinanceira");
        }
    }
}
