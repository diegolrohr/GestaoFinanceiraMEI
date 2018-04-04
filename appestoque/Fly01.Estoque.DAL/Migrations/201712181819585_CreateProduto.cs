namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateProduto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Produto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        GrupoProdutoId = c.Guid(),
                        UnidadeMedidaId = c.Guid(),
                        TipoProdutoId = c.Int(nullable: false),
                        SaldoProduto = c.Double(),
                        CodigoProduto = c.String(maxLength: 200, unicode: false),
                        CodigoBarras = c.String(maxLength: 15, unicode: false),
                        ValorVenda = c.Double(nullable: false),
                        ValorCusto = c.Double(nullable: false),
                        SaldoMinimo = c.Double(nullable: false),
                        NcmId = c.Guid(),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        AliquotaIpi = c.Double(nullable: false),
                        TipoProduto = c.Int(nullable: false),
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
                .ForeignKey("dbo.GrupoProduto", t => t.GrupoProdutoId)
                .ForeignKey("dbo.NCM", t => t.NcmId)
                .ForeignKey("dbo.UnidadeMedida", t => t.UnidadeMedidaId)
                .Index(t => t.GrupoProdutoId)
                .Index(t => t.UnidadeMedidaId)
                .Index(t => t.NcmId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.Produto", "NcmId", "dbo.NCM");
            DropForeignKey("dbo.Produto", "GrupoProdutoId", "dbo.GrupoProduto");
            DropIndex("dbo.Produto", new[] { "NcmId" });
            DropIndex("dbo.Produto", new[] { "UnidadeMedidaId" });
            DropIndex("dbo.Produto", new[] { "GrupoProdutoId" });
            DropTable("dbo.Produto");
        }
    }
}
