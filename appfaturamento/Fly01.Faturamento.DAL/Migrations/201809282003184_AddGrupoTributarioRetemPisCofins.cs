namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGrupoTributarioRetemPisCofins : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GrupoTributario", "RetemPis", c => c.Boolean(nullable: false));
            AddColumn("dbo.GrupoTributario", "RetemCofins", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GrupoTributario", "RetemCofins");
            DropColumn("dbo.GrupoTributario", "RetemPis");
        }
    }
}
