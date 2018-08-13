namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UfCidadeOpcionais : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Pessoa", new[] { "CidadeId" });
            DropIndex("dbo.Pessoa", new[] { "EstadoId" });
            AlterColumn("dbo.Pessoa", "CidadeId", c => c.Guid());
            AlterColumn("dbo.Pessoa", "EstadoId", c => c.Guid());
            CreateIndex("dbo.Pessoa", "CidadeId");
            CreateIndex("dbo.Pessoa", "EstadoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pessoa", new[] { "EstadoId" });
            DropIndex("dbo.Pessoa", new[] { "CidadeId" });
            AlterColumn("dbo.Pessoa", "EstadoId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Pessoa", "CidadeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Pessoa", "EstadoId");
            CreateIndex("dbo.Pessoa", "CidadeId");
        }
    }
}
