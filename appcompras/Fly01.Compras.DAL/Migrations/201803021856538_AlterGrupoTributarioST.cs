namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterGrupoTributarioST : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GrupoTributario", "CalculaSubstituicaoTributaria", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "AplicaFreteBaseST", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "AplicaDespesaBaseST", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "AplicaIpiBaseST", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GrupoTributario", "AplicaIpiBaseST");
            DropColumn("dbo.GrupoTributario", "AplicaDespesaBaseST");
            DropColumn("dbo.GrupoTributario", "AplicaFreteBaseST");
            DropColumn("dbo.GrupoTributario", "CalculaSubstituicaoTributaria");
        }
    }
}
