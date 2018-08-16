namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInicialNFeEntrada : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotaFiscalItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotaFiscalId = c.Guid(nullable: false),
                        GrupoTributarioId = c.Guid(nullable: false),
                        Quantidade = c.Double(nullable: false),
                        Valor = c.Double(nullable: false),
                        Desconto = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                        ProdutoId = c.Guid(),
                        ValorCreditoICMS = c.Double(),
                        ValorICMSSTRetido = c.Double(),
                        ValorBCSTRetido = c.Double(),
                        ValorFCPSTRetidoAnterior = c.Double(),
                        ValorBCFCPSTRetidoAnterior = c.Double(),
                        ServicoId = c.Guid(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .ForeignKey("dbo.GrupoTributario", t => t.GrupoTributarioId)
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId)
                .ForeignKey("dbo.Servico", t => t.ServicoId)
                .Index(t => t.NotaFiscalId)
                .Index(t => t.GrupoTributarioId)
                .Index(t => t.ProdutoId)
                .Index(t => t.ServicoId);
            
            CreateTable(
                "dbo.NotaFiscal",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrdemVendaOrigemId = c.Guid(nullable: false),
                        TipoNotaFiscal = c.Int(nullable: false),
                        TipoVenda = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        ClienteId = c.Guid(nullable: false),
                        TransportadoraId = c.Guid(),
                        TipoFrete = c.Int(nullable: false),
                        PlacaVeiculo = c.String(maxLength: 7, unicode: false),
                        EstadoPlacaVeiculoId = c.Guid(),
                        ValorFrete = c.Double(),
                        PesoBruto = c.Double(),
                        PesoLiquido = c.Double(),
                        QuantidadeVolumes = c.Int(),
                        FormaPagamentoId = c.Guid(),
                        CondicaoParcelamentoId = c.Guid(),
                        CategoriaId = c.Guid(),
                        DataVencimento = c.DateTime(storeType: "date"),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        SerieNotaFiscalId = c.Guid(),
                        NumNotaFiscal = c.Int(),
                        XML = c.String(unicode: false),
                        PDF = c.String(unicode: false),
                        Mensagem = c.String(unicode: false),
                        Recomendacao = c.String(unicode: false),
                        NaturezaOperacao = c.String(maxLength: 60, unicode: false),
                        SefazId = c.String(maxLength: 44, unicode: false),
                        ChaveNFeReferenciada = c.String(maxLength: 44, unicode: false),
                        MensagemPadraoNota = c.String(maxLength: 5000, unicode: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                        TotalImpostosServicos = c.Double(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Pessoa", t => t.ClienteId)
                .ForeignKey("dbo.CondicaoParcelamento", t => t.CondicaoParcelamentoId)
                .ForeignKey("dbo.Estado", t => t.EstadoPlacaVeiculoId)
                .ForeignKey("dbo.FormaPagamento", t => t.FormaPagamentoId)
                .ForeignKey("dbo.OrdemVenda", t => t.OrdemVendaOrigemId)
                .ForeignKey("dbo.SerieNotaFiscal", t => t.SerieNotaFiscalId)
                .ForeignKey("dbo.Pessoa", t => t.TransportadoraId)
                .Index(t => t.OrdemVendaOrigemId)
                .Index(t => t.ClienteId)
                .Index(t => t.TransportadoraId)
                .Index(t => t.EstadoPlacaVeiculoId)
                .Index(t => t.FormaPagamentoId)
                .Index(t => t.CondicaoParcelamentoId)
                .Index(t => t.CategoriaId)
                .Index(t => t.SerieNotaFiscalId);
            
            CreateTable(
                "dbo.OrdemVenda",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Numero = c.Int(nullable: false),
                        ChaveNFeReferenciada = c.String(maxLength: 44, unicode: false),
                        TipoOrdemVenda = c.Int(nullable: false),
                        TipoVenda = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        ClienteId = c.Guid(nullable: false),
                        GrupoTributarioPadraoId = c.Guid(),
                        TransportadoraId = c.Guid(),
                        TipoFrete = c.Int(nullable: false),
                        PlacaVeiculo = c.String(maxLength: 7, unicode: false),
                        EstadoPlacaVeiculoId = c.Guid(),
                        ValorFrete = c.Double(),
                        PesoBruto = c.Double(),
                        PesoLiquido = c.Double(),
                        QuantidadeVolumes = c.Int(),
                        FormaPagamentoId = c.Guid(),
                        CondicaoParcelamentoId = c.Guid(),
                        CategoriaId = c.Guid(),
                        DataVencimento = c.DateTime(storeType: "date"),
                        MovimentaEstoque = c.Boolean(nullable: false),
                        AjusteEstoqueAutomatico = c.Boolean(nullable: false),
                        GeraFinanceiro = c.Boolean(nullable: false),
                        GeraNotaFiscal = c.Boolean(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        TotalImpostosServicos = c.Double(),
                        TotalImpostosProdutos = c.Double(),
                        TotalImpostosProdutosNaoAgrega = c.Double(nullable: false),
                        MensagemPadraoNota = c.String(maxLength: 5000, unicode: false),
                        NaturezaOperacao = c.String(maxLength: 60, unicode: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.Pessoa", t => t.ClienteId)
                .ForeignKey("dbo.CondicaoParcelamento", t => t.CondicaoParcelamentoId)
                .ForeignKey("dbo.Estado", t => t.EstadoPlacaVeiculoId)
                .ForeignKey("dbo.FormaPagamento", t => t.FormaPagamentoId)
                .ForeignKey("dbo.GrupoTributario", t => t.GrupoTributarioPadraoId)
                .ForeignKey("dbo.Pessoa", t => t.TransportadoraId)
                .Index(t => t.ClienteId)
                .Index(t => t.GrupoTributarioPadraoId)
                .Index(t => t.TransportadoraId)
                .Index(t => t.EstadoPlacaVeiculoId)
                .Index(t => t.FormaPagamentoId)
                .Index(t => t.CondicaoParcelamentoId)
                .Index(t => t.CategoriaId);
            
            CreateTable(
                "dbo.NotaFiscalCartaCorrecao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotaFiscalId = c.Guid(nullable: false),
                        IdRetorno = c.String(maxLength: 200, unicode: false),
                        Status = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        Numero = c.Int(nullable: false),
                        MensagemCorrecao = c.String(nullable: false, maxLength: 1000, unicode: false),
                        Mensagem = c.String(unicode: false),
                        XML = c.String(unicode: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId)
                .Index(t => t.NotaFiscalId);
            
            CreateTable(
                "dbo.NotaFiscalItemTributacao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotaFiscalItemId = c.Guid(nullable: false),
                        FreteValorFracionado = c.Double(nullable: false),
                        CalculaICMS = c.Boolean(nullable: false),
                        ICMSBase = c.Double(nullable: false),
                        ICMSAliquota = c.Double(nullable: false),
                        ICMSValor = c.Double(nullable: false),
                        CalculaIPI = c.Boolean(nullable: false),
                        IPIBase = c.Double(nullable: false),
                        IPIAliquota = c.Double(nullable: false),
                        IPIValor = c.Double(nullable: false),
                        CalculaST = c.Boolean(nullable: false),
                        STBase = c.Double(nullable: false),
                        STAliquota = c.Double(nullable: false),
                        STValor = c.Double(nullable: false),
                        CalculaCOFINS = c.Boolean(nullable: false),
                        COFINSBase = c.Double(nullable: false),
                        COFINSAliquota = c.Double(nullable: false),
                        COFINSValor = c.Double(nullable: false),
                        CalculaPIS = c.Boolean(nullable: false),
                        PISBase = c.Double(nullable: false),
                        PISAliquota = c.Double(nullable: false),
                        PISValor = c.Double(nullable: false),
                        FCPBase = c.Double(nullable: false),
                        FCPAliquota = c.Double(nullable: false),
                        FCPValor = c.Double(nullable: false),
                        FCPSTBase = c.Double(nullable: false),
                        FCPSTAliquota = c.Double(nullable: false),
                        FCPSTValor = c.Double(nullable: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalItem", t => t.NotaFiscalItemId)
                .Index(t => t.NotaFiscalItemId);
            
            CreateTable(
                "dbo.Servico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CodigoServico = c.String(maxLength: 200, unicode: false),
                        Descricao = c.String(maxLength: 200, unicode: false),
                        NbsId = c.Guid(),
                        ValorServico = c.Double(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        TipoTributacaoISS = c.Int(),
                        TipoPagamentoImpostoISS = c.Int(),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nbs", t => t.NbsId)
                .Index(t => t.NbsId);
            
            CreateTable(
                "dbo.Nbs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 600, unicode: false),
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
                "dbo.NFe",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TotalImpostosProdutos = c.Double(nullable: false),
                        TotalImpostosProdutosNaoAgrega = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscal", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NFe", "Id", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalItemTributacao", "NotaFiscalItemId", "dbo.NotaFiscalItem");
            DropForeignKey("dbo.NotaFiscalItem", "ServicoId", "dbo.Servico");
            DropForeignKey("dbo.Servico", "NbsId", "dbo.Nbs");
            DropForeignKey("dbo.NotaFiscalItem", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalItem", "GrupoTributarioId", "dbo.GrupoTributario");
            DropForeignKey("dbo.NotaFiscalCartaCorrecao", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalItem", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.NotaFiscal", "TransportadoraId", "dbo.Pessoa");
            DropForeignKey("dbo.NotaFiscal", "SerieNotaFiscalId", "dbo.SerieNotaFiscal");
            DropForeignKey("dbo.NotaFiscal", "OrdemVendaOrigemId", "dbo.OrdemVenda");
            DropForeignKey("dbo.OrdemVenda", "TransportadoraId", "dbo.Pessoa");
            DropForeignKey("dbo.OrdemVenda", "GrupoTributarioPadraoId", "dbo.GrupoTributario");
            DropForeignKey("dbo.OrdemVenda", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.OrdemVenda", "EstadoPlacaVeiculoId", "dbo.Estado");
            DropForeignKey("dbo.OrdemVenda", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.OrdemVenda", "ClienteId", "dbo.Pessoa");
            DropForeignKey("dbo.OrdemVenda", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.NotaFiscal", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.NotaFiscal", "EstadoPlacaVeiculoId", "dbo.Estado");
            DropForeignKey("dbo.NotaFiscal", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.NotaFiscal", "ClienteId", "dbo.Pessoa");
            DropForeignKey("dbo.NotaFiscal", "CategoriaId", "dbo.Categoria");
            DropIndex("dbo.NFe", new[] { "Id" });
            DropIndex("dbo.Servico", new[] { "NbsId" });
            DropIndex("dbo.NotaFiscalItemTributacao", new[] { "NotaFiscalItemId" });
            DropIndex("dbo.NotaFiscalCartaCorrecao", new[] { "NotaFiscalId" });
            DropIndex("dbo.OrdemVenda", new[] { "CategoriaId" });
            DropIndex("dbo.OrdemVenda", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.OrdemVenda", new[] { "FormaPagamentoId" });
            DropIndex("dbo.OrdemVenda", new[] { "EstadoPlacaVeiculoId" });
            DropIndex("dbo.OrdemVenda", new[] { "TransportadoraId" });
            DropIndex("dbo.OrdemVenda", new[] { "GrupoTributarioPadraoId" });
            DropIndex("dbo.OrdemVenda", new[] { "ClienteId" });
            DropIndex("dbo.NotaFiscal", new[] { "SerieNotaFiscalId" });
            DropIndex("dbo.NotaFiscal", new[] { "CategoriaId" });
            DropIndex("dbo.NotaFiscal", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.NotaFiscal", new[] { "FormaPagamentoId" });
            DropIndex("dbo.NotaFiscal", new[] { "EstadoPlacaVeiculoId" });
            DropIndex("dbo.NotaFiscal", new[] { "TransportadoraId" });
            DropIndex("dbo.NotaFiscal", new[] { "ClienteId" });
            DropIndex("dbo.NotaFiscal", new[] { "OrdemVendaOrigemId" });
            DropIndex("dbo.NotaFiscalItem", new[] { "ServicoId" });
            DropIndex("dbo.NotaFiscalItem", new[] { "ProdutoId" });
            DropIndex("dbo.NotaFiscalItem", new[] { "GrupoTributarioId" });
            DropIndex("dbo.NotaFiscalItem", new[] { "NotaFiscalId" });
            DropTable("dbo.NFe");
            DropTable("dbo.Nbs");
            DropTable("dbo.Servico");
            DropTable("dbo.NotaFiscalItemTributacao");
            DropTable("dbo.NotaFiscalCartaCorrecao");
            DropTable("dbo.OrdemVenda");
            DropTable("dbo.NotaFiscal");
            DropTable("dbo.NotaFiscalItem");
        }
    }
}
