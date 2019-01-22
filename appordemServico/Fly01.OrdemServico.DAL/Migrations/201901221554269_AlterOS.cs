namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemServico", "Duracao", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemServico", "Duracao");
        }
    }
}
