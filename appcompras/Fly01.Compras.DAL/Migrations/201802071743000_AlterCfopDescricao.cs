namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterCfopDescricao : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cfop", "Descricao", c => c.String(nullable: false, maxLength: 400, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cfop", "Descricao", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
    }
}
