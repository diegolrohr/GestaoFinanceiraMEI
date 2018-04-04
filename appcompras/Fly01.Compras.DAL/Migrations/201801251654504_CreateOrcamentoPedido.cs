namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateOrcamentoPedido : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdemCompra",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        FormaPagamentoId = c.Guid(),
                        CondicaoParcelamentoId = c.Guid(),
                        DataVencimento = c.DateTime(storeType: "date"),
                        TipoOrdemCompra = c.Int(nullable: false),
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
                .ForeignKey("dbo.CondicaoParcelamento", t => t.CondicaoParcelamentoId)
                .ForeignKey("dbo.FormaPagamento", t => t.FormaPagamentoId)
                .Index(t => t.FormaPagamentoId)
                .Index(t => t.CondicaoParcelamentoId);
            
            CreateTable(
                "dbo.OrdemCompraItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProdutoId = c.Guid(nullable: false),
                        Quantidade = c.Int(nullable: false),
                        Valor = c.Double(nullable: false),
                        Desconto = c.Double(nullable: false),
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
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .Index(t => t.ProdutoId);
            
            CreateTable(
                "dbo.Orcamento",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdemCompra", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Pedido",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FornecedorId = c.Guid(nullable: false),
                        TransportadoraId = c.Guid(),
                        ValorFrete = c.Double(),
                        PesoBruto = c.Double(),
                        PesoLiquido = c.Double(),
                        QuantidadeVolumes = c.Int(),
                        MovimentaEstoque = c.Boolean(nullable: false),
                        GeraFinanceiro = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdemCompra", t => t.Id)
                .ForeignKey("dbo.Pessoa", t => t.FornecedorId)
                .ForeignKey("dbo.Pessoa", t => t.TransportadoraId)
                .Index(t => t.Id)
                .Index(t => t.FornecedorId)
                .Index(t => t.TransportadoraId);
            
            CreateTable(
                "dbo.OrcamentoItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrcamentoId = c.Guid(nullable: false),
                        FornecedorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdemCompraItem", t => t.Id)
                .ForeignKey("dbo.Orcamento", t => t.OrcamentoId)
                .ForeignKey("dbo.Pessoa", t => t.FornecedorId)
                .Index(t => t.Id)
                .Index(t => t.OrcamentoId)
                .Index(t => t.FornecedorId);
            
            CreateTable(
                "dbo.PedidoItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PedidoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdemCompraItem", t => t.Id)
                .ForeignKey("dbo.Pedido", t => t.PedidoId)
                .Index(t => t.Id)
                .Index(t => t.PedidoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PedidoItem", "PedidoId", "dbo.Pedido");
            DropForeignKey("dbo.PedidoItem", "Id", "dbo.OrdemCompraItem");
            DropForeignKey("dbo.OrcamentoItem", "FornecedorId", "dbo.Pessoa");
            DropForeignKey("dbo.OrcamentoItem", "OrcamentoId", "dbo.Orcamento");
            DropForeignKey("dbo.OrcamentoItem", "Id", "dbo.OrdemCompraItem");
            DropForeignKey("dbo.Pedido", "TransportadoraId", "dbo.Pessoa");
            DropForeignKey("dbo.Pedido", "FornecedorId", "dbo.Pessoa");
            DropForeignKey("dbo.Pedido", "Id", "dbo.OrdemCompra");
            DropForeignKey("dbo.Orcamento", "Id", "dbo.OrdemCompra");
            DropForeignKey("dbo.OrdemCompraItem", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.OrdemCompra", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.OrdemCompra", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropIndex("dbo.PedidoItem", new[] { "PedidoId" });
            DropIndex("dbo.PedidoItem", new[] { "Id" });
            DropIndex("dbo.OrcamentoItem", new[] { "FornecedorId" });
            DropIndex("dbo.OrcamentoItem", new[] { "OrcamentoId" });
            DropIndex("dbo.OrcamentoItem", new[] { "Id" });
            DropIndex("dbo.Pedido", new[] { "TransportadoraId" });
            DropIndex("dbo.Pedido", new[] { "FornecedorId" });
            DropIndex("dbo.Pedido", new[] { "Id" });
            DropIndex("dbo.Orcamento", new[] { "Id" });
            DropIndex("dbo.OrdemCompraItem", new[] { "ProdutoId" });
            DropIndex("dbo.OrdemCompra", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.OrdemCompra", new[] { "FormaPagamentoId" });
            DropTable("dbo.PedidoItem");
            DropTable("dbo.OrcamentoItem");
            DropTable("dbo.Pedido");
            DropTable("dbo.Orcamento");
            DropTable("dbo.OrdemCompraItem");
            DropTable("dbo.OrdemCompra");
        }
    }
}
