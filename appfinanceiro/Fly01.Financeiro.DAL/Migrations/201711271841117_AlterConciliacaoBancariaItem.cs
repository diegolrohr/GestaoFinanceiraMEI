namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterConciliacaoBancariaItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConciliacaoBancariaItem", "Conciliado", c => c.Int(nullable: false));
            DropColumn("dbo.ConciliacaoBancariaItem", "ConciliadoTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConciliacaoBancariaItem", "ConciliadoTotal", c => c.Boolean(nullable: false));
            DropColumn("dbo.ConciliacaoBancariaItem", "Conciliado");
        }
    }
}
