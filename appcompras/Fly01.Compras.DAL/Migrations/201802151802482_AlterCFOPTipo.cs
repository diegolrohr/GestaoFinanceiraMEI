namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterCFOPTipo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cfop", "Tipo", c => c.Int(nullable: false));
            AlterColumn("dbo.Cfop", "Codigo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cfop", "Codigo", c => c.String(nullable: false, maxLength: 200, unicode: false));
            DropColumn("dbo.Cfop", "Tipo");
        }
    }
}
