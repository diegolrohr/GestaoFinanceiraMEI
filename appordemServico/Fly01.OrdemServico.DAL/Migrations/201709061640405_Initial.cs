namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banco",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 200, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoriaFinanceira",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        Classe = c.Int(nullable: false),
                        CategoriaPaiId = c.Guid(),
                        TipoCarteira = c.Int(nullable: false),
                        Codigo = c.String(maxLength: 200, unicode: false),
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
                .ForeignKey("dbo.CategoriaFinanceira", t => t.CategoriaPaiId)
                .Index(t => t.CategoriaPaiId);
            
            CreateTable(
                "dbo.Cidade",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 35, unicode: false),
                        CodigoIBGE = c.String(nullable: false, maxLength: 7, unicode: false),
                        UFId = c.Guid(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UF", t => t.UFId)
                .Index(t => t.UFId);
            
            CreateTable(
                "dbo.UF",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sigla = c.String(nullable: false, maxLength: 2, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        UtcId = c.String(nullable: false, maxLength: 35, unicode: false),
                        CodigoIBGE = c.String(nullable: false, maxLength: 2, unicode: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConciliacaoBancaria",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BancoId = c.String(maxLength: 200, unicode: false),
                        Codigo = c.String(maxLength: 200, unicode: false),
                        Agencia = c.String(maxLength: 200, unicode: false),
                        Conta = c.String(maxLength: 200, unicode: false),
                        BancoNome = c.String(maxLength: 200, unicode: false),
                        Status = c.String(maxLength: 200, unicode: false),
                        DataImportacao = c.DateTime(storeType: "date"),
                        DataImportacaoRest = c.String(maxLength: 200, unicode: false),
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
            
            CreateTable(
                "dbo.CondicaoParcelamento",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        QtdParcelas = c.Int(),
                        CondicoesParcelamento = c.String(maxLength: 200, unicode: false),
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
            
            CreateTable(
                "dbo.ContaBancaria",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 150, unicode: false),
                        Agencia = c.String(nullable: false, maxLength: 200, unicode: false),
                        Conta = c.String(nullable: false, maxLength: 200, unicode: false),
                        BancoId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.Banco", t => t.BancoId)
                .Index(t => t.BancoId);
            
            CreateTable(
                "dbo.ContaFinanceira",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ContaFinanceiraRepeticaoPaiId = c.Guid(),
                        ValorPrevisto = c.Double(nullable: false),
                        ValorPago = c.Double(),
                        CategoriaFinanceiraId = c.Guid(nullable: false),
                        CondicaoParcelamentoId = c.Guid(nullable: false),
                        PessoaId = c.Guid(nullable: false),
                        DataEmissao = c.DateTime(nullable: false, storeType: "date"),
                        DataBaixa = c.DateTime(storeType: "date"),
                        DataVencimento = c.DateTime(nullable: false, storeType: "date"),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        FormaPagamento = c.Int(nullable: false),
                        TipoContaFinanceira = c.Int(nullable: false),
                        StatusContaBancaria = c.Int(nullable: false),
                        Repetir = c.Boolean(nullable: false),
                        TipoPeriodicidade = c.Int(nullable: false),
                        NumeroRepeticoes = c.Int(),
                        DescricaoParcela = c.String(maxLength: 200, unicode: false),
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
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraRepeticaoPaiId)
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .Index(t => t.ContaFinanceiraRepeticaoPaiId)
                .Index(t => t.CategoriaFinanceiraId)
                .Index(t => t.CondicaoParcelamentoId)
                .Index(t => t.PessoaId);
            
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        TipoDocumento = c.String(nullable: false, maxLength: 1, unicode: false),
                        CPFCNPJ = c.String(nullable: false, maxLength: 18, unicode: false),
                        CEP = c.String(maxLength: 8, unicode: false),
                        Endereco = c.String(maxLength: 50, unicode: false),
                        Bairro = c.String(maxLength: 30, unicode: false),
                        Cidade = c.String(maxLength: 35, unicode: false),
                        Estado = c.String(maxLength: 2, unicode: false),
                        Telefone = c.String(maxLength: 15, unicode: false),
                        Celular = c.String(maxLength: 15, unicode: false),
                        Contato = c.String(maxLength: 45, unicode: false),
                        Observacao = c.String(maxLength: 100, unicode: false),
                        Email = c.String(maxLength: 70, unicode: false),
                        NomeComercial = c.String(nullable: false, maxLength: 100, unicode: false),
                        Transportadora = c.Boolean(nullable: false),
                        Cliente = c.Boolean(nullable: false),
                        Fornecedor = c.Boolean(nullable: false),
                        Vendedor = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.ContaFinanceiraBaixa",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.DateTime(nullable: false),
                        ContaFinanceiraId = c.Guid(nullable: false),
                        ContaBancariaId = c.Guid(nullable: false),
                        Valor = c.Double(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
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
                .ForeignKey("dbo.ContaBancaria", t => t.ContaBancariaId)
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraId)
                .Index(t => t.ContaFinanceiraId)
                .Index(t => t.ContaBancariaId);
            
            CreateTable(
                "dbo.Feriado",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        Dia = c.Int(nullable: false),
                        Mes = c.Int(nullable: false),
                        Ano = c.Int(nullable: false),
                        Recorrente = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.SaldoHistorico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        ContaBancariaId = c.Guid(nullable: false),
                        SaldoDia = c.Double(nullable: false),
                        SaldoConsolidado = c.Double(nullable: false),
                        TotalRecebimentos = c.Double(nullable: false),
                        TotalPagamentos = c.Double(nullable: false),
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
                .ForeignKey("dbo.ContaBancaria", t => t.ContaBancariaId)
                .Index(t => t.ContaBancariaId);
            
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
            DropForeignKey("dbo.SaldoHistorico", "ContaBancariaId", "dbo.ContaBancaria");
            DropForeignKey("dbo.ContaFinanceiraBaixa", "ContaFinanceiraId", "dbo.ContaFinanceira");
            DropForeignKey("dbo.ContaFinanceiraBaixa", "ContaBancariaId", "dbo.ContaBancaria");
            DropForeignKey("dbo.ContaFinanceira", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.ContaFinanceira", "ContaFinanceiraRepeticaoPaiId", "dbo.ContaFinanceira");
            DropForeignKey("dbo.ContaFinanceira", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.ContaFinanceira", "CategoriaFinanceiraId", "dbo.CategoriaFinanceira");
            DropForeignKey("dbo.ContaBancaria", "BancoId", "dbo.Banco");
            DropForeignKey("dbo.Cidade", "UFId", "dbo.UF");
            DropForeignKey("dbo.CategoriaFinanceira", "CategoriaPaiId", "dbo.CategoriaFinanceira");
            DropIndex("dbo.ContaReceber", new[] { "Id" });
            DropIndex("dbo.ContaPagar", new[] { "Id" });
            DropIndex("dbo.SaldoHistorico", new[] { "ContaBancariaId" });
            DropIndex("dbo.ContaFinanceiraBaixa", new[] { "ContaBancariaId" });
            DropIndex("dbo.ContaFinanceiraBaixa", new[] { "ContaFinanceiraId" });
            DropIndex("dbo.ContaFinanceira", new[] { "PessoaId" });
            DropIndex("dbo.ContaFinanceira", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.ContaFinanceira", new[] { "CategoriaFinanceiraId" });
            DropIndex("dbo.ContaFinanceira", new[] { "ContaFinanceiraRepeticaoPaiId" });
            DropIndex("dbo.ContaBancaria", new[] { "BancoId" });
            DropIndex("dbo.Cidade", new[] { "UFId" });
            DropIndex("dbo.CategoriaFinanceira", new[] { "CategoriaPaiId" });
            DropTable("dbo.ContaReceber");
            DropTable("dbo.ContaPagar");
            DropTable("dbo.SaldoHistorico");
            DropTable("dbo.Feriado");
            DropTable("dbo.ContaFinanceiraBaixa");
            DropTable("dbo.Pessoa");
            DropTable("dbo.ContaFinanceira");
            DropTable("dbo.ContaBancaria");
            DropTable("dbo.CondicaoParcelamento");
            DropTable("dbo.ConciliacaoBancaria");
            DropTable("dbo.UF");
            DropTable("dbo.Cidade");
            DropTable("dbo.CategoriaFinanceira");
            DropTable("dbo.Banco");
        }
    }
}
