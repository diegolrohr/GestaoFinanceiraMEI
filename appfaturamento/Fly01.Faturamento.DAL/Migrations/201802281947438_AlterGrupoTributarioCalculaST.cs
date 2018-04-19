namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterGrupoTributarioCalculaST : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GrupoTributario", "CalculaSubstituicaoTributaria", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GrupoTributario", "CalculaSubstituicaoTributaria");
        }
    }
}
