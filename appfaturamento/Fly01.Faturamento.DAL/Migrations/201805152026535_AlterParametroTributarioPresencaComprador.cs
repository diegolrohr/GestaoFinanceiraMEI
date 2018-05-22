namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterParametroTributarioPresencaComprador : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "TipoPresencaComprador", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "TipoPresencaComprador");
        }
    }
}
