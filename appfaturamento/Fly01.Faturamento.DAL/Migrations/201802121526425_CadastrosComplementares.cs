namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CadastrosComplementares : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdemVendaItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrdemVendaId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.OrdemVenda", t => t.OrdemVendaId)
                .Index(t => t.OrdemVendaId)
                .Index(t => t.GrupoTributarioId);
            
            CreateTable(
                "dbo.OrdemVenda",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Numero = c.Int(nullable: false, identity: true),
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
                        GeraFinanceiro = c.Boolean(nullable: false),
                        GeraNotaFiscal = c.Boolean(nullable: false),
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
                "dbo.Categoria",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        CategoriaPaiId = c.Guid(),
                        TipoCarteira = c.Int(nullable: false),
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
                .ForeignKey("dbo.Categoria", t => t.CategoriaPaiId)
                .Index(t => t.CategoriaPaiId);
            
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
                "dbo.FormaPagamento",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        TipoFormaPagamento = c.Int(nullable: false),
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
                "dbo.OrdemVendaProduto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProdutoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdemVendaItem", t => t.Id)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .Index(t => t.Id)
                .Index(t => t.ProdutoId);
            
            CreateTable(
                "dbo.OrdemVendaServico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServicoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdemVendaItem", t => t.Id)
                .ForeignKey("dbo.Servico", t => t.ServicoId)
                .Index(t => t.Id)
                .Index(t => t.ServicoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdemVendaServico", "ServicoId", "dbo.Servico");
            DropForeignKey("dbo.OrdemVendaServico", "Id", "dbo.OrdemVendaItem");
            DropForeignKey("dbo.OrdemVendaProduto", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.OrdemVendaProduto", "Id", "dbo.OrdemVendaItem");
            DropForeignKey("dbo.OrdemVendaItem", "OrdemVendaId", "dbo.OrdemVenda");
            DropForeignKey("dbo.OrdemVenda", "TransportadoraId", "dbo.Pessoa");
            DropForeignKey("dbo.OrdemVenda", "GrupoTributarioPadraoId", "dbo.GrupoTributario");
            DropForeignKey("dbo.OrdemVenda", "FormaPagamentoId", "dbo.FormaPagamento");
            DropForeignKey("dbo.OrdemVenda", "EstadoPlacaVeiculoId", "dbo.Estado");
            DropForeignKey("dbo.OrdemVenda", "CondicaoParcelamentoId", "dbo.CondicaoParcelamento");
            DropForeignKey("dbo.OrdemVenda", "ClienteId", "dbo.Pessoa");
            DropForeignKey("dbo.OrdemVenda", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.Categoria", "CategoriaPaiId", "dbo.Categoria");
            DropForeignKey("dbo.OrdemVendaItem", "GrupoTributarioId", "dbo.GrupoTributario");
            DropIndex("dbo.OrdemVendaServico", new[] { "ServicoId" });
            DropIndex("dbo.OrdemVendaServico", new[] { "Id" });
            DropIndex("dbo.OrdemVendaProduto", new[] { "ProdutoId" });
            DropIndex("dbo.OrdemVendaProduto", new[] { "Id" });
            DropIndex("dbo.Categoria", new[] { "CategoriaPaiId" });
            DropIndex("dbo.OrdemVenda", new[] { "CategoriaId" });
            DropIndex("dbo.OrdemVenda", new[] { "CondicaoParcelamentoId" });
            DropIndex("dbo.OrdemVenda", new[] { "FormaPagamentoId" });
            DropIndex("dbo.OrdemVenda", new[] { "EstadoPlacaVeiculoId" });
            DropIndex("dbo.OrdemVenda", new[] { "TransportadoraId" });
            DropIndex("dbo.OrdemVenda", new[] { "GrupoTributarioPadraoId" });
            DropIndex("dbo.OrdemVenda", new[] { "ClienteId" });
            DropIndex("dbo.OrdemVendaItem", new[] { "GrupoTributarioId" });
            DropIndex("dbo.OrdemVendaItem", new[] { "OrdemVendaId" });
            DropTable("dbo.OrdemVendaServico");
            DropTable("dbo.OrdemVendaProduto");
            DropTable("dbo.FormaPagamento");
            DropTable("dbo.CondicaoParcelamento");
            DropTable("dbo.Categoria");
            DropTable("dbo.OrdemVenda");
            DropTable("dbo.OrdemVendaItem");
        }
    }
}
