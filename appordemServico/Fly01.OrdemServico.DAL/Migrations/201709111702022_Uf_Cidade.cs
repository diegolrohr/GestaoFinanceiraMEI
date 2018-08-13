namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Uf_Cidade : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UF", newName: "Estado");
            RenameColumn(table: "dbo.Cidade", name: "UFId", newName: "EstadoId");
            RenameIndex(table: "dbo.Cidade", name: "IX_UFId", newName: "IX_EstadoId");
            AddColumn("dbo.Pessoa", "CidadeId", c => c.Guid(nullable: false));
            AddColumn("dbo.Pessoa", "EstadoId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Estado", "CodigoIbge", c => c.String(maxLength: 2, unicode: false));
            CreateIndex("dbo.Pessoa", "CidadeId");
            CreateIndex("dbo.Pessoa", "EstadoId");
            AddForeignKey("dbo.Pessoa", "CidadeId", "dbo.Cidade", "Id");
            AddForeignKey("dbo.Pessoa", "EstadoId", "dbo.Estado", "Id");
            DropColumn("dbo.Pessoa", "Cidade");
            DropColumn("dbo.Pessoa", "Estado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pessoa", "Estado", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Pessoa", "Cidade", c => c.String(maxLength: 35, unicode: false));
            DropForeignKey("dbo.Pessoa", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Pessoa", "CidadeId", "dbo.Cidade");
            DropIndex("dbo.Pessoa", new[] { "EstadoId" });
            DropIndex("dbo.Pessoa", new[] { "CidadeId" });
            AlterColumn("dbo.Estado", "CodigoIbge", c => c.String(nullable: false, maxLength: 2, unicode: false));
            DropColumn("dbo.Pessoa", "EstadoId");
            DropColumn("dbo.Pessoa", "CidadeId");
            RenameIndex(table: "dbo.Cidade", name: "IX_EstadoId", newName: "IX_UFId");
            RenameColumn(table: "dbo.Cidade", name: "EstadoId", newName: "UFId");
            RenameTable(name: "dbo.Estado", newName: "UF");
        }
    }
}
