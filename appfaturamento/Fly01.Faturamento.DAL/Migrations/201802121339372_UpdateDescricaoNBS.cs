namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateDescricaoNBS : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NBS", "Descricao", c => c.String(nullable: false, maxLength: 600, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NBS", "Descricao", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
    }
}
