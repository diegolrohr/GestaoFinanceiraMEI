namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterConciliacaoBancariaItemStatusConciliado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConciliacaoBancariaItem", "StatusConciliado", c => c.Int(nullable: false));
            DropColumn("dbo.ConciliacaoBancariaItem", "Conciliado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConciliacaoBancariaItem", "Conciliado", c => c.Int(nullable: false));
            DropColumn("dbo.ConciliacaoBancariaItem", "StatusConciliado");
        }
    }
}
