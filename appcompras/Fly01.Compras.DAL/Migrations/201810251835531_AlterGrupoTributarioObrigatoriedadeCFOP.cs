namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterGrupoTributarioObrigatoriedadeCFOP : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GrupoTributario", new[] { "CfopId" });
            AlterColumn("dbo.GrupoTributario", "CfopId", c => c.Guid());
            CreateIndex("dbo.GrupoTributario", "CfopId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GrupoTributario", new[] { "CfopId" });
            AlterColumn("dbo.GrupoTributario", "CfopId", c => c.Guid(nullable: false));
            CreateIndex("dbo.GrupoTributario", "CfopId");
        }
    }
}
