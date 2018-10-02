namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoçãoValorTotal : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrdemServico", "ValorTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdemServico", "ValorTotal", c => c.Boolean(nullable: false));
        }
    }
}
