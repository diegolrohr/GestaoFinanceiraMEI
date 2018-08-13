namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddContaFinanceiraRenegociacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContaFinanceiraRenegociacao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PessoaId = c.Guid(nullable: false),
                        TipoContaFinanceira = c.Int(nullable: false),
                        ValorAcumulado = c.Double(nullable: false),
                        TipoRenegociacaoValorDiferenca = c.Int(nullable: false),
                        TipoRenegociacaoCalculo = c.Int(nullable: false),
                        ValorDiferenca = c.Double(nullable: false),
                        ValorFinal = c.Double(nullable: false),
                        CategoriaFinanceiraId = c.Guid(nullable: false),
                        FormaPagamentoId = c.Guid(nullable: false),
                        CondicaoParcelamentoId = c.Guid(nullable: false),
                        DataEmissao = c.DateTime(nullable: false, storeType: "date"),
                        DataVencimento = c.DateTime(nullable: false, storeType: "date"),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        Motivo = c.String(maxLength: 200, unicode: false),
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
                .ForeignKey("dbo.CategoriaFinanceira", t => t.CategoriaFinanceiraId)
                .ForeignKey("dbo.CondicaoParcelamento", t => t.CondicaoParcelamentoId)
                .ForeignKey("dbo.FormaPagamento", t => t.FormaPagamentoId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .Index(t => t.PessoaId)
                .Index(t => t.CategoriaFinanceiraId)
                .Index(t => t.FormaPagamentoId)
                .Index(t => t.CondicaoParcelamentoId);
            
            CreateTable(
                "dbo.RenegociacaoContaFinanceira",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ContaFinanceiraRenegociacaoId = c.Guid(nullable: false),
                        ContaFinanceiraId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraId)
                .ForeignKey("dbo.ContaFinanceiraRenegociacao", t => t.ContaFinanceiraRenegociacaoId)
                .Index(t => t.ContaFinanceiraRenegociacaoId)
                .Index(t => t.ContaFinanceiraId);
            
            CreateTable(
                "dbo.RenegociacaoContaFinanceiraOrigem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RenegociacaoContaFinanceira", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.RenegociacaoContaFinanceiraRenegociada",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RenegociacaoContaFinanceira", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RenegociacaoContaFinanceiraRenegociada", "Id", "dbo.RenegociacaoContaFinanceira");
            DropForeignKey("dbo.RenegociacaoContaFinanceiraOrigem", "Id", "dbo.RenegociacaoContaFinanceira");
            DropForeignKey("dbo.RenegociacaoContaFinanceira", "ContaFinanceiraRenegociacaoId", "dbo.ContaFinanceiraRenegociacao");
            DropForeignKey("dbo.RenegociacaoContaFinanceira", "ContaFinanceiraId", "dbo.ContaFinanceira");
            DropForeignKey("dbo.ContaFinanceiraRenegociacao", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.ContaFinanceiraRenegociacao", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.ContaFinanceiraRenegociacao", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.ContaFinanceiraRenegociacao", "CategoriaFinanceiraId", "dbo.CategoriaFinanceira");
            DropIndex("dbo.RenegociacaoContaFinanceiraRenegociada", new[] { "Id" });
            DropIndex("dbo.RenegociacaoContaFinanceiraOrigem", new[] { "Id" });
            DropIndex("dbo.RenegociacaoContaFinanceira", new[] { "ContaFinanceiraId" });
            DropIndex("dbo.RenegociacaoContaFinanceira", new[] { "ContaFinanceiraRenegociacaoId" });
            DropIndex("dbo.ContaFinanceiraRenegociacao", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.ContaFinanceiraRenegociacao", new[] { "FormaPagamentoId" });
            DropIndex("dbo.ContaFinanceiraRenegociacao", new[] { "CategoriaFinanceiraId" });
            DropIndex("dbo.ContaFinanceiraRenegociacao", new[] { "PessoaId" });
            DropTable("dbo.RenegociacaoContaFinanceiraRenegociada");
            DropTable("dbo.RenegociacaoContaFinanceiraOrigem");
            DropTable("dbo.RenegociacaoContaFinanceira");
            DropTable("dbo.ContaFinanceiraRenegociacao");
        }
    }
}
