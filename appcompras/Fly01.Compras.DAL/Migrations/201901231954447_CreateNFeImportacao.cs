namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNFeImportacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NFeImportacaoProduto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NFeImportadaId = c.Guid(nullable: false),
                        ProdutoId = c.Guid(),
                        Codigo = c.String(maxLength: 200, unicode: false),
                        CodigoBarras = c.String(maxLength: 200, unicode: false),
                        Descricao = c.String(maxLength: 200, unicode: false),
                        Quantidade = c.String(maxLength: 200, unicode: false),
                        Valor = c.Double(nullable: false),
                        UnidadeMedidaId = c.Guid(nullable: false),
                        FatorConversao = c.Double(nullable: false),
                        TipoFatorConversao = c.Int(nullable: false),
                        MovimentaEstoque = c.Boolean(nullable: false),
                        AtualizaDadosProduto = c.Boolean(nullable: false),
                        AtualizaValorCompra = c.Boolean(nullable: false),
                        AtualizaValorVenda = c.Boolean(nullable: false),
                        ValorVenda = c.Double(nullable: false),
                        TipoValorVenda = c.Int(nullable: false),
                        PlataformaId = c.String(nullable: false, maxLength: 200, unicode: false),
                        RegistroFixo = c.Boolean(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                        NFeImportacao_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NFeImportacao", t => t.NFeImportacao_Id)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .ForeignKey("dbo.UnidadeMedida", t => t.UnidadeMedidaId)
                .Index(t => t.ProdutoId)
                .Index(t => t.UnidadeMedidaId)
                .Index(t => t.NFeImportacao_Id);
            
            CreateTable(
                "dbo.NFeImportacao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Xml = c.String(unicode: false),
                        Json = c.String(unicode: false),
                        XmlMd5 = c.String(nullable: false, maxLength: 32, unicode: false),
                        Tipo = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        AtualizaDadosFornecedor = c.Boolean(nullable: false),
                        NovoFornecedor = c.Boolean(nullable: false),
                        FornecedorId = c.Guid(),
                        TipoFrete = c.Int(nullable: false),
                        AtualizaDadosTransportadora = c.Boolean(nullable: false),
                        NovaTransportadora = c.Boolean(nullable: false),
                        TransportadoraId = c.Guid(),
                        GeraFinanceiro = c.Boolean(nullable: false),
                        CondicaoParcelamentoId = c.Guid(),
                        FormaPagamentoId = c.Guid(),
                        CategoriaId = c.Guid(),
                        DataVencimento = c.DateTime(nullable: false, storeType: "date"),
                        ValorTotal = c.Double(nullable: false),
                        ValorFrete = c.Double(nullable: false),
                        SomatorioICMSST = c.Double(nullable: false),
                        SomatorioIPI = c.Double(nullable: false),
                        SomatorioFCPST = c.Double(nullable: false),
                        SomatorioDesconto = c.Double(nullable: false),
                        ContaFinanceiraPaiId = c.Guid(),
                        NovoPedido = c.Boolean(nullable: false),
                        PedidoId = c.Guid(),
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
                .ForeignKey("dbo.FormaPagamento", t => t.FormaPagamentoId)
                .ForeignKey("dbo.Pessoa", t => t.FornecedorId)
                .ForeignKey("dbo.Pedido", t => t.PedidoId)
                .ForeignKey("dbo.Pessoa", t => t.TransportadoraId)
                .Index(t => t.FornecedorId)
                .Index(t => t.TransportadoraId)
                .Index(t => t.CondicaoParcelamentoId)
                .Index(t => t.FormaPagamentoId)
                .Index(t => t.CategoriaId)
                .Index(t => t.PedidoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NFeImportacaoProduto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.NFeImportacaoProduto", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.NFeImportacaoProduto", "NFeImportacao_Id", "dbo.NFeImportacao");
            DropForeignKey("dbo.NFeImportacao", "TransportadoraId", "dbo.Pessoa");
            DropForeignKey("dbo.NFeImportacao", "PedidoId", "dbo.Pedido");
            DropForeignKey("dbo.NFeImportacao", "FornecedorId", "dbo.Pessoa");
            DropForeignKey("dbo.NFeImportacao", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.NFeImportacao", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.NFeImportacao", "CategoriaId", "dbo.Categoria");
            DropIndex("dbo.NFeImportacao", new[] { "PedidoId" });
            DropIndex("dbo.NFeImportacao", new[] { "CategoriaId" });
            DropIndex("dbo.NFeImportacao", new[] { "FormaPagamentoId" });
            DropIndex("dbo.NFeImportacao", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.NFeImportacao", new[] { "TransportadoraId" });
            DropIndex("dbo.NFeImportacao", new[] { "FornecedorId" });
            DropIndex("dbo.NFeImportacaoProduto", new[] { "NFeImportacao_Id" });
            DropIndex("dbo.NFeImportacaoProduto", new[] { "UnidadeMedidaId" });
            DropIndex("dbo.NFeImportacaoProduto", new[] { "ProdutoId" });
            DropTable("dbo.NFeImportacao");
            DropTable("dbo.NFeImportacaoProduto");
        }
    }
}
