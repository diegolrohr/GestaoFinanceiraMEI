namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeraOrdemVenda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemServico", "GeraOrdemVenda", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemServico", "GeraOrdemVenda");
        }
    }
}
