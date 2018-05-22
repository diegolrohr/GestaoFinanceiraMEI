namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterParametroTributario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParametroTributario", "Cnpj", c => c.String(maxLength: 16, unicode: false));
            AddColumn("dbo.ParametroTributario", "InscricaoEstadual", c => c.String(maxLength: 18, unicode: false));
            AddColumn("dbo.ParametroTributario", "UF", c => c.String(maxLength: 2, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParametroTributario", "UF");
            DropColumn("dbo.ParametroTributario", "InscricaoEstadual");
            DropColumn("dbo.ParametroTributario", "Cnpj");
        }
    }
}
