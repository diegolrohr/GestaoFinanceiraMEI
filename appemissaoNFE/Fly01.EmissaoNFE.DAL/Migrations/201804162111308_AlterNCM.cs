namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterNCM : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ncm", "Codigo", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.Ncm", "Descricao", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ncm", "Descricao", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Ncm", "Codigo", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
