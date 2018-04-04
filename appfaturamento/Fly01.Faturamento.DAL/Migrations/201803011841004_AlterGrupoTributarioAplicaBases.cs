namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterGrupoTributarioAplicaBases : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GrupoTributario", "AplicaFreteBaseST", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "AplicaDespesaBaseST", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "AplicaIpiBaseST", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GrupoTributario", "AplicaIpiBaseST");
            DropColumn("dbo.GrupoTributario", "AplicaDespesaBaseST");
            DropColumn("dbo.GrupoTributario", "AplicaFreteBaseST");
        }
    }
}
