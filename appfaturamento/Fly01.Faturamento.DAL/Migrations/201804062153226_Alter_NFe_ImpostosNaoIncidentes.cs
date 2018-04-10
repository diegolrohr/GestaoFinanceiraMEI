namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alter_NFe_ImpostosNaoIncidentes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFe", "TotalImpostosProdutosNaoAgrega", c => c.Double(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NFe", "TotalImpostosProdutosNaoAgrega");
        }
    }
}
