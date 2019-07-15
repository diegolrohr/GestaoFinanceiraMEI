namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTotalOrdemVenda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVenda", "Total", c => c.Double(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "Total");
        }
    }
}
