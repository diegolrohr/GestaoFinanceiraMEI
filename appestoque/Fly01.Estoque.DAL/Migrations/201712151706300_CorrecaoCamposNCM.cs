namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CorrecaoCamposNCM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NCM", "Codigo", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AddColumn("dbo.NCM", "Descricao", c => c.String(nullable: false, maxLength: 200, unicode: false));
            DropColumn("dbo.NCM", "Dominio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NCM", "Dominio", c => c.String(nullable: false, maxLength: 200, unicode: false));
            DropColumn("dbo.NCM", "Descricao");
            DropColumn("dbo.NCM", "Codigo");
        }
    }
}
