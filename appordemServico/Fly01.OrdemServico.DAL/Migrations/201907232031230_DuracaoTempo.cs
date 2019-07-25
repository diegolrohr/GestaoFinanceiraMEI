namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DuracaoTempo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemServico", "Tempo", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemServico", "Tempo");
        }
    }
}
