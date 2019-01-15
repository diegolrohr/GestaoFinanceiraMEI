namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableKitAddColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KitItem", "Quantidade", c => c.Double(nullable: false));
            AddColumn("dbo.KitItem", "TipoItem", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KitItem", "TipoItem");
            DropColumn("dbo.KitItem", "Quantidade");
        }
    }
}
