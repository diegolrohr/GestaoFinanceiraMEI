namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOrdemVendaAjusteEstoqueAutomatico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVenda", "AjusteEstoqueAutomatico", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "AjusteEstoqueAutomatico");
        }
    }
}
