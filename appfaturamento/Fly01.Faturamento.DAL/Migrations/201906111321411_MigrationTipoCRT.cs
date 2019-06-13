namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTipoCRT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "TipoCRT", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "TipoCRT");
        }
    }
}
