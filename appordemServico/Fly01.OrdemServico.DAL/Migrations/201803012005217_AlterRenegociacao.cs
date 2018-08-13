namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterRenegociacao : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ContaFinanceiraRenegociacao", name: "CategoriaFinanceiraId", newName: "CategoriaId");
            RenameIndex(table: "dbo.ContaFinanceiraRenegociacao", name: "IX_CategoriaFinanceiraId", newName: "IX_CategoriaId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ContaFinanceiraRenegociacao", name: "IX_CategoriaId", newName: "IX_CategoriaFinanceiraId");
            RenameColumn(table: "dbo.ContaFinanceiraRenegociacao", name: "CategoriaId", newName: "CategoriaFinanceiraId");
        }
    }
}
