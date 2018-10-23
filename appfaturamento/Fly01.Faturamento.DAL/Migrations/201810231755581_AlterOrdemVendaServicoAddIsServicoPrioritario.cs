namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemVendaServicoAddIsServicoPrioritario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVendaServico", "IsServicoPrioritario", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVendaServico", "IsServicoPrioritario");
        }
    }
}
