namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_OrdemVenda_ImpostosNaoIncidentes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVenda", "TotalImpostosProdutosNaoAgrega", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "TotalImpostosProdutosNaoAgrega");
        }
    }
}
