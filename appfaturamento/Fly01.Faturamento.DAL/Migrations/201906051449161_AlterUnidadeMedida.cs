namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUnidadeMedida : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UnidadeMedida", "Abreviacao", c => c.String(nullable: false, maxLength: 6, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UnidadeMedida", "Abreviacao", c => c.String(nullable: false, maxLength: 2, unicode: false));
        }
    }
}
