namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContaPagarReceber : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContaFinanceira",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ContaFinanceiraRepeticaoPaiId = c.Guid(),
                        ContaFinanceiraParcelaPaiId = c.Guid(),
                        ValorPrevisto = c.Double(nullable: false),
                        ValorPago = c.Double(),
                        CategoriaId = c.Guid(nullable: false),
                        CondicaoParcelamentoId = c.Guid(nullable: false),
                        PessoaId = c.Guid(nullable: false),
                        DataEmissao = c.DateTime(nullable: false, storeType: "date"),
                        DataVencimento = c.DateTime(nullable: false, storeType: "date"),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        FormaPagamentoId = c.Guid(nullable: false),
                        TipoContaFinanceira = c.Int(nullable: false),
                        StatusContaBancaria = c.Int(nullable: false),
                        Repetir = c.Boolean(nullable: false),
                        TipoPeriodicidade = c.Int(nullable: false),
                        NumeroRepeticoes = c.Int(),
                        DescricaoParcela = c.String(maxLength: 200, unicode: false),
                        Numero = c.Int(nullable: false),
                        EmpresaId = c.Guid(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.CondicaoParcelamento", t => t.CondicaoParcelamentoId)
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraParcelaPaiId)
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraRepeticaoPaiId)
                .ForeignKey("dbo.FormaPagamento", t => t.FormaPagamentoId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .Index(t => t.ContaFinanceiraRepeticaoPaiId)
                .Index(t => t.ContaFinanceiraParcelaPaiId)
                .Index(t => t.CategoriaId)
                .Index(t => t.CondicaoParcelamentoId)
                .Index(t => t.PessoaId)
                .Index(t => t.FormaPagamentoId);
            
            CreateTable(
                "dbo.ContaPagar",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContaFinanceira", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ContaReceber",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContaFinanceira", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContaReceber", "Id", "dbo.ContaFinanceira");
            DropForeignKey("dbo.ContaPagar", "Id", "dbo.ContaFinanceira");
            DropForeignKey("dbo.ContaFinanceira", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.ContaFinanceira", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.ContaFinanceira", "ContaFinanceiraRepeticaoPaiId", "dbo.ContaFinanceira");
            DropForeignKey("dbo.ContaFinanceira", "ContaFinanceiraParcelaPaiId", "dbo.ContaFinanceira");
            DropForeignKey("dbo.ContaFinanceira", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.ContaFinanceira", "CategoriaId", "dbo.Categoria");
            DropIndex("dbo.ContaReceber", new[] { "Id" });
            DropIndex("dbo.ContaPagar", new[] { "Id" });
            DropIndex("dbo.ContaFinanceira", new[] { "FormaPagamentoId" });
            DropIndex("dbo.ContaFinanceira", new[] { "PessoaId" });
            DropIndex("dbo.ContaFinanceira", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.ContaFinanceira", new[] { "CategoriaId" });
            DropIndex("dbo.ContaFinanceira", new[] { "ContaFinanceiraParcelaPaiId" });
            DropIndex("dbo.ContaFinanceira", new[] { "ContaFinanceiraRepeticaoPaiId" });
            DropTable("dbo.ContaReceber");
            DropTable("dbo.ContaPagar");
            DropTable("dbo.ContaFinanceira");
        }
    }
}
