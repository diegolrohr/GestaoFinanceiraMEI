namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alter_ParametroTributario_AliquotaFCP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "AliquotaFCP", c => c.Double(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "AliquotaFCP");
        }
    }
}
