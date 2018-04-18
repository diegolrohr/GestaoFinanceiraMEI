namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSubstituicaoTributariaFCP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubstituicaoTributaria", "Fcp", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubstituicaoTributaria", "Fcp");
        }
    }
}
