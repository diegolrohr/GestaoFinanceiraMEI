namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateParaEmissaoNFe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotaFiscalEntrada",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrdemCompraOrigemId = c.Guid(nullable: false),
                        TipoNotaFiscal = c.Int(nullable: false),
                        TipoVenda = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        FornecedorId = c.Guid(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.CondicaoParcelamento", t => t.CondicaoParcelamentoId)
                .ForeignKey("dbo.Estado", t => t.EstadoPlacaVeiculoId)
                .ForeignKey("dbo.FormaPagamento", t => t.FormaPagamentoId)
                .ForeignKey("dbo.Pessoa", t => t.FornecedorId)
                .ForeignKey("dbo.OrdemCompra", t => t.OrdemCompraOrigemId)
                .ForeignKey("dbo.SerieNotaFiscal", t => t.SerieNotaFiscalId)
                .ForeignKey("dbo.Pessoa", t => t.TransportadoraId)
                .Index(t => t.OrdemCompraOrigemId)
                .Index(t => t.FornecedorId)
                .Index(t => t.TransportadoraId)
                .Index(t => t.EstadoPlacaVeiculoId)
                .Index(t => t.FormaPagamentoId)
                .Index(t => t.CondicaoParcelamentoId)
                .Index(t => t.CategoriaId)
                .Index(t => t.SerieNotaFiscalId);
            
            CreateTable(
                "dbo.NotaFiscalItemEntrada",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotaFiscalEntradaId = c.Guid(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoTributario", t => t.GrupoTributarioId)
                .ForeignKey("dbo.NotaFiscalEntrada", t => t.NotaFiscalEntradaId)
                .Index(t => t.NotaFiscalEntradaId)
                .Index(t => t.GrupoTributarioId);
            
            CreateTable(
                "dbo.NotaFiscalCartaCorrecaoEntrada",
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
                .ForeignKey("dbo.NotaFiscalEntrada", t => t.NotaFiscalId)
                .Index(t => t.NotaFiscalId);
            
            CreateTable(
                "dbo.NotaFiscalItemTributacaoEntrada",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotaFiscalItemEntradaId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.NotaFiscalItemEntrada", t => t.NotaFiscalItemEntradaId)
                .Index(t => t.NotaFiscalItemEntradaId);
            
            CreateTable(
                "dbo.NFeEntrada",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TotalImpostosProdutos = c.Double(nullable: false),
                        TotalImpostosProdutosNaoAgrega = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalEntrada", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.NFeProdutoEntrada",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProdutoId = c.Guid(nullable: false),
                        ValorCreditoICMS = c.Double(nullable: false),
                        ValorICMSSTRetido = c.Double(nullable: false),
                        ValorBCSTRetido = c.Double(nullable: false),
                        ValorFCPSTRetidoAnterior = c.Double(nullable: false),
                        ValorBCFCPSTRetidoAnterior = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalItemEntrada", t => t.Id)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .Index(t => t.Id)
                .Index(t => t.ProdutoId);
            
            AddColumn("dbo.OrdemCompra", "ChaveNFeReferenciada", c => c.String(maxLength: 44, unicode: false));
            AddColumn("dbo.OrdemCompra", "TipoVenda", c => c.Int(nullable: false));
            AddColumn("dbo.OrdemCompra", "GrupoTributarioPadraoId", c => c.Guid(nullable: true));
            AddColumn("dbo.OrdemCompra", "PlacaVeiculo", c => c.String(maxLength: 7, unicode: false));
            AddColumn("dbo.OrdemCompra", "EstadoPlacaVeiculoId", c => c.Guid());
            AddColumn("dbo.OrdemCompra", "AjusteEstoqueAutomatico", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdemCompra", "GeraNotaFiscal", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdemCompra", "TotalImpostosServicos", c => c.Double());
            AddColumn("dbo.OrdemCompra", "TotalImpostosProdutos", c => c.Double());
            AddColumn("dbo.OrdemCompra", "TotalImpostosProdutosNaoAgrega", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompra", "MensagemPadraoNota", c => c.String(maxLength: 5000, unicode: false));
            AddColumn("dbo.OrdemCompra", "NaturezaOperacao", c => c.String(maxLength: 60, unicode: false));
            CreateIndex("dbo.OrdemCompra", "GrupoTributarioPadraoId");
            CreateIndex("dbo.OrdemCompra", "EstadoPlacaVeiculoId");
            AddForeignKey("dbo.OrdemCompra", "EstadoPlacaVeiculoId", "dbo.Estado", "Id");
            AddForeignKey("dbo.OrdemCompra", "GrupoTributarioPadraoId", "dbo.GrupoTributario", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NFeProdutoEntrada", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.NFeProdutoEntrada", "Id", "dbo.NotaFiscalItemEntrada");
            DropForeignKey("dbo.NFeEntrada", "Id", "dbo.NotaFiscalEntrada");
            DropForeignKey("dbo.NotaFiscalItemTributacaoEntrada", "NotaFiscalItemEntradaId", "dbo.NotaFiscalItemEntrada");
            DropForeignKey("dbo.NotaFiscalItemEntrada", "NotaFiscalEntradaId", "dbo.NotaFiscalEntrada");
            DropForeignKey("dbo.NotaFiscalItemEntrada", "GrupoTributarioId", "dbo.GrupoTributario");
            DropForeignKey("dbo.NotaFiscalCartaCorrecaoEntrada", "NotaFiscalId", "dbo.NotaFiscalEntrada");
            DropForeignKey("dbo.NotaFiscalEntrada", "TransportadoraId", "dbo.Pessoa");
            DropForeignKey("dbo.NotaFiscalEntrada", "SerieNotaFiscalId", "dbo.SerieNotaFiscal");
            DropForeignKey("dbo.NotaFiscalEntrada", "OrdemCompraOrigemId", "dbo.OrdemCompra");
            DropForeignKey("dbo.NotaFiscalEntrada", "FornecedorId", "dbo.Pessoa");
            DropForeignKey("dbo.NotaFiscalEntrada", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.NotaFiscalEntrada", "EstadoPlacaVeiculoId", "dbo.Estado");
            DropForeignKey("dbo.NotaFiscalEntrada", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.NotaFiscalEntrada", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.OrdemCompra", "GrupoTributarioPadraoId", "dbo.GrupoTributario");
            DropForeignKey("dbo.OrdemCompra", "EstadoPlacaVeiculoId", "dbo.Estado");
            DropIndex("dbo.NFeProdutoEntrada", new[] { "ProdutoId" });
            DropIndex("dbo.NFeProdutoEntrada", new[] { "Id" });
            DropIndex("dbo.NFeEntrada", new[] { "Id" });
            DropIndex("dbo.NotaFiscalItemTributacaoEntrada", new[] { "NotaFiscalItemEntradaId" });
            DropIndex("dbo.NotaFiscalCartaCorrecaoEntrada", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscalItemEntrada", new[] { "GrupoTributarioId" });
            DropIndex("dbo.NotaFiscalItemEntrada", new[] { "NotaFiscalEntradaId" });
            DropIndex("dbo.OrdemCompra", new[] { "EstadoPlacaVeiculoId" });
            DropIndex("dbo.OrdemCompra", new[] { "GrupoTributarioPadraoId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "SerieNotaFiscalId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "CategoriaId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "FormaPagamentoId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "EstadoPlacaVeiculoId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "TransportadoraId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "FornecedorId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "OrdemCompraOrigemId" });
            DropColumn("dbo.OrdemCompra", "NaturezaOperacao");
            DropColumn("dbo.OrdemCompra", "MensagemPadraoNota");
            DropColumn("dbo.OrdemCompra", "TotalImpostosProdutosNaoAgrega");
            DropColumn("dbo.OrdemCompra", "TotalImpostosProdutos");
            DropColumn("dbo.OrdemCompra", "TotalImpostosServicos");
            DropColumn("dbo.OrdemCompra", "GeraNotaFiscal");
            DropColumn("dbo.OrdemCompra", "AjusteEstoqueAutomatico");
            DropColumn("dbo.OrdemCompra", "EstadoPlacaVeiculoId");
            DropColumn("dbo.OrdemCompra", "PlacaVeiculo");
            DropColumn("dbo.OrdemCompra", "GrupoTributarioPadraoId");
            DropColumn("dbo.OrdemCompra", "TipoVenda");
            DropColumn("dbo.OrdemCompra", "ChaveNFeReferenciada");
            DropTable("dbo.NFeProdutoEntrada");
            DropTable("dbo.NFeEntrada");
            DropTable("dbo.NotaFiscalItemTributacaoEntrada");
            DropTable("dbo.NotaFiscalCartaCorrecaoEntrada");
            DropTable("dbo.NotaFiscalItemEntrada");
            DropTable("dbo.NotaFiscalEntrada");
        }
    }
}
