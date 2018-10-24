namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterParametroAddTipoRegimeEspecialTributacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "TipoRegimeEspecialTributacao", c => c.Int(nullable: false, defaultValue:1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "TipoRegimeEspecialTributacao");
        }
    }
}
