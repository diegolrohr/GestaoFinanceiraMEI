namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterNCM : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NCM", "PlataformaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NCM", "PlataformaId", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
    }
}
