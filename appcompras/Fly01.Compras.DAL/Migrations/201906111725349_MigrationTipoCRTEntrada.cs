namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTipoCRTEntrada : DbMigration
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
