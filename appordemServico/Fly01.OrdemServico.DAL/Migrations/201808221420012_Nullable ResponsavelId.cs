namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableResponsavelId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.OrdemServico", new[] { "ResponsavelId" });
            AlterColumn("dbo.OrdemServico", "ResponsavelId", c => c.Guid());
            CreateIndex("dbo.OrdemServico", "ResponsavelId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrdemServico", new[] { "ResponsavelId" });
            AlterColumn("dbo.OrdemServico", "ResponsavelId", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrdemServico", "ResponsavelId");
        }
    }
}
