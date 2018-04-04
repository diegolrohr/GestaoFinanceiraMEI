namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPlataformaIdTipoMovimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoMovimento", "PlataformaId", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TipoMovimento", "PlataformaId");
        }
    }
}
