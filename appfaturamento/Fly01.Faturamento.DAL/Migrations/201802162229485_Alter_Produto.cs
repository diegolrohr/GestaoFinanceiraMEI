namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alter_Produto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "CestId", c => c.Guid());
            CreateIndex("dbo.Produto", "CestId");
            AddForeignKey("dbo.Produto", "CestId", "dbo.Cest", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produto", "CestId", "dbo.Cest");
            DropIndex("dbo.Produto", new[] { "CestId" });
            DropColumn("dbo.Produto", "CestId");
        }
    }
}
