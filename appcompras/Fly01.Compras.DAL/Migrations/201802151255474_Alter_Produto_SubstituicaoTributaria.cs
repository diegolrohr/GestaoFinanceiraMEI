namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alter_Produto_SubstituicaoTributaria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "CestId", c => c.Guid());
            AddColumn("dbo.SubstituicaoTributaria", "CestId", c => c.Guid());
            CreateIndex("dbo.Produto", "CestId");
            CreateIndex("dbo.SubstituicaoTributaria", "CestId");
            AddForeignKey("dbo.Produto", "CestId", "dbo.Cest", "Id");
            AddForeignKey("dbo.SubstituicaoTributaria", "CestId", "dbo.Cest", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubstituicaoTributaria", "CestId", "dbo.Cest");
            DropForeignKey("dbo.Produto", "CestId", "dbo.Cest");
            DropIndex("dbo.SubstituicaoTributaria", new[] { "CestId" });
            DropIndex("dbo.Produto", new[] { "CestId" });
            DropColumn("dbo.SubstituicaoTributaria", "CestId");
            DropColumn("dbo.Produto", "CestId");
        }
    }
}
