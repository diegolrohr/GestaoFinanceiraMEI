namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateMovimento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movimento",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QuantidadeMovimento = c.Double(),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        SaldoAntesMovimento = c.Double(),
                        TipoMovimentoId = c.Guid(nullable: false),
                        ProdutoId = c.Guid(nullable: false),
                        InventarioId = c.Guid(),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventario", t => t.InventarioId)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .ForeignKey("dbo.TipoMovimento", t => t.TipoMovimentoId)
                .Index(t => t.TipoMovimentoId)
                .Index(t => t.ProdutoId)
                .Index(t => t.InventarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movimento", "TipoMovimentoId", "dbo.TipoMovimento");
            DropForeignKey("dbo.Movimento", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.Movimento", "InventarioId", "dbo.Inventario");
            DropIndex("dbo.Movimento", new[] { "InventarioId" });
            DropIndex("dbo.Movimento", new[] { "ProdutoId" });
            DropIndex("dbo.Movimento", new[] { "TipoMovimentoId" });
            DropTable("dbo.Movimento");
        }
    }
}
