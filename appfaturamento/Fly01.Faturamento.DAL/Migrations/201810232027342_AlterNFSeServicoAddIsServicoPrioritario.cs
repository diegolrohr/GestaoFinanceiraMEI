namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFSeServicoAddIsServicoPrioritario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFSeServico", "IsServicoPrioritario", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NFSeServico", "IsServicoPrioritario");
        }
    }
}
