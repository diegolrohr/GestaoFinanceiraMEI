namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValorTotaldouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrdemServico", "ValorTotal", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrdemServico", "ValorTotal", c => c.Boolean(nullable: false));
        }
    }
}
