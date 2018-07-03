namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterParametroTributarioTipoHorario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "HorarioVerao", c => c.Int(nullable: false, defaultValue: 2));
            AddColumn("dbo.ParametroTributario", "TipoHorario", c => c.Int(nullable: false, defaultValue: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "TipoHorario");
            DropColumn("dbo.ParametroTributario", "HorarioVerao");
        }
    }
}
