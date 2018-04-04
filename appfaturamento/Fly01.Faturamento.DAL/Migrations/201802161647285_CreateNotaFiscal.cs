namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateNotaFiscal : DbMigration
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
                .ForeignKey("dbo.GrupoTributario", t => t.GrupoTributarioId)
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId)
                .Index(t => t.NotaFiscalId)
                .Index(t => t.GrupoTributarioId);
            
            CreateTable(
                "dbo.NotaFiscal",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Numero = c.Int(nullable: false, identity: true),
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
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Pessoa", t => t.ClienteId)
                .ForeignKey("dbo.CondicaoParcelamento", t => t.CondicaoParcelamentoId)
                .ForeignKey("dbo.Estado", t => t.EstadoPlacaVeiculoId)
                .ForeignKey("dbo.FormaPagamento", t => t.FormaPagamentoId)
                .ForeignKey("dbo.OrdemVenda", t => t.OrdemVendaOrigemId)
                .ForeignKey("dbo.Pessoa", t => t.TransportadoraId)
                .Index(t => t.OrdemVendaOrigemId)
                .Index(t => t.ClienteId)
                .Index(t => t.TransportadoraId)
                .Index(t => t.EstadoPlacaVeiculoId)
                .Index(t => t.FormaPagamentoId)
                .Index(t => t.CondicaoParcelamentoId)
                .Index(t => t.CategoriaId);
            
            CreateTable(
                "dbo.NFeProduto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProdutoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalItem", t => t.Id)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .Index(t => t.Id)
                .Index(t => t.ProdutoId);
            
            CreateTable(
                "dbo.NFe",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscal", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.NFSe",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscal", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.NFSeServico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServicoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalItem", t => t.Id)
                .ForeignKey("dbo.Servico", t => t.ServicoId)
                .Index(t => t.Id)
                .Index(t => t.ServicoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NFSeServico", "ServicoId", "dbo.Servico");
            DropForeignKey("dbo.NFSeServico", "Id", "dbo.NotaFiscalItem");
            DropForeignKey("dbo.NFSe", "Id", "dbo.NotaFiscal");
            DropForeignKey("dbo.NFe", "Id", "dbo.NotaFiscal");
            DropForeignKey("dbo.NFeProduto", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.NFeProduto", "Id", "dbo.NotaFiscalItem");
            DropForeignKey("dbo.NotaFiscalItem", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalItem", "GrupoTributarioId", "dbo.GrupoTributario");
            DropForeignKey("dbo.NotaFiscal", "TransportadoraId", "dbo.Pessoa");
            DropForeignKey("dbo.NotaFiscal", "OrdemVendaOrigemId", "dbo.OrdemVenda");
            DropForeignKey("dbo.NotaFiscal", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.NotaFiscal", "EstadoPlacaVeiculoId", "dbo.Estado");
            DropForeignKey("dbo.NotaFiscal", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.NotaFiscal", "ClienteId", "dbo.Pessoa");
            DropForeignKey("dbo.NotaFiscal", "CategoriaId", "dbo.Categoria");
            DropIndex("dbo.NFSeServico", new[] { "ServicoId" });
            DropIndex("dbo.NFSeServico", new[] { "Id" });
            DropIndex("dbo.NFSe", new[] { "Id" });
            DropIndex("dbo.NFe", new[] { "Id" });
            DropIndex("dbo.NFeProduto", new[] { "ProdutoId" });
            DropIndex("dbo.NFeProduto", new[] { "Id" });
            DropIndex("dbo.NotaFiscal", new[] { "CategoriaId" });
            DropIndex("dbo.NotaFiscal", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.NotaFiscal", new[] { "FormaPagamentoId" });
            DropIndex("dbo.NotaFiscal", new[] { "EstadoPlacaVeiculoId" });
            DropIndex("dbo.NotaFiscal", new[] { "TransportadoraId" });
            DropIndex("dbo.NotaFiscal", new[] { "ClienteId" });
            DropIndex("dbo.NotaFiscal", new[] { "OrdemVendaOrigemId" });
            DropIndex("dbo.NotaFiscalItem", new[] { "GrupoTributarioId" });
            DropIndex("dbo.NotaFiscalItem", new[] { "NotaFiscalId" });
            DropTable("dbo.NFSeServico");
            DropTable("dbo.NFSe");
            DropTable("dbo.NFe");
            DropTable("dbo.NFeProduto");
            DropTable("dbo.NotaFiscal");
            DropTable("dbo.NotaFiscalItem");
        }
    }
}
