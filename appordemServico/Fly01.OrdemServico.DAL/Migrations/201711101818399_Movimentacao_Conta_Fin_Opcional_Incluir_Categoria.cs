namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Movimentacao_Conta_Fin_Opcional_Incluir_Categoria : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Movimentacao", new[] { "ContaFinanceiraId" });
            AddColumn("dbo.Movimentacao", "CategoriaFinanceiraId", c => c.Guid());
            AddColumn("dbo.Movimentacao", "Descricao", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Movimentacao", "ContaFinanceiraId", c => c.Guid());
            CreateIndex("dbo.Movimentacao", "ContaFinanceiraId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Movimentacao", new[] { "ContaFinanceiraId" });
            AlterColumn("dbo.Movimentacao", "ContaFinanceiraId", c => c.Guid(nullable: false));
            DropColumn("dbo.Movimentacao", "Descricao");
            DropColumn("dbo.Movimentacao", "CategoriaFinanceiraId");
            CreateIndex("dbo.Movimentacao", "ContaFinanceiraId");
        }
    }
}
