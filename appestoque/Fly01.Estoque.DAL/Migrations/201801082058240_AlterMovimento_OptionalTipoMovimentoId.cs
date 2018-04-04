namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterMovimento_OptionalTipoMovimentoId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Movimento", new[] { "TipoMovimentoId" });
            AlterColumn("dbo.Movimento", "TipoMovimentoId", c => c.Guid());
            CreateIndex("dbo.Movimento", "TipoMovimentoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Movimento", new[] { "TipoMovimentoId" });
            AlterColumn("dbo.Movimento", "TipoMovimentoId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Movimento", "TipoMovimentoId");
        }
    }
}
