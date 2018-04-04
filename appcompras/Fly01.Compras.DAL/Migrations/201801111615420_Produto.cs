namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Produto : DbMigration
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
                        NcmId = c.Guid(),
                        SaldoProduto = c.Double(nullable: false),
                        CodigoProduto = c.String(maxLength: 200, unicode: false),
                        CodigoBarras = c.String(maxLength: 15, unicode: false),
                        ValorVenda = c.Double(nullable: false),
                        ValorCusto = c.Double(nullable: false),
                        SaldoMinimo = c.Double(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        AliquotaIpi = c.Double(nullable: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Produto");
        }
    }
}
