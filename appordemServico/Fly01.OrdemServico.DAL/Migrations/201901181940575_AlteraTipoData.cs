namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteraTipoData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemServico", "HoraEntrega", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemServico", "HoraEntrega");
        }
    }
}
