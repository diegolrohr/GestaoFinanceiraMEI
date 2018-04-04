namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cfop",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
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
                "dbo.Cidade",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 35, unicode: false),
                        CodigoIbge = c.String(nullable: false, maxLength: 7, unicode: false),
                        EstadoId = c.Guid(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .Index(t => t.EstadoId);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sigla = c.String(nullable: false, maxLength: 2, unicode: false),
                        Nome = c.String(nullable: false, maxLength: 20, unicode: false),
                        UtcId = c.String(nullable: false, maxLength: 35, unicode: false),
                        CodigoIbge = c.String(maxLength: 2, unicode: false),
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
                "dbo.GrupoProduto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        AliquotaIpi = c.Double(nullable: false),
                        TipoProduto = c.Int(nullable: false),
                        UnidadeMedidaId = c.Guid(),
                        NcmId = c.Guid(),
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
                .ForeignKey("dbo.NCM", t => t.NcmId)
                .ForeignKey("dbo.UnidadeMedida", t => t.UnidadeMedidaId)
                .Index(t => t.UnidadeMedidaId)
                .Index(t => t.NcmId);
            
            CreateTable(
                "dbo.NCM",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        AliquotaIPI = c.Double(nullable: false),
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
                "dbo.UnidadeMedida",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Abreviacao = c.String(nullable: false, maxLength: 2, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 20, unicode: false),
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
                "dbo.GrupoTributario",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        CfopId = c.Guid(nullable: false),
                        CalculaIcms = c.Boolean(nullable: false),
                        TipoTributacaoICMS = c.Int(nullable: false),
                        CalculaIcmsDifal = c.Boolean(nullable: false),
                        AplicaIpiBaseIcms = c.Boolean(nullable: false),
                        AplicaFreteBaseIcms = c.Boolean(nullable: false),
                        AplicaDespesaBaseIcms = c.Boolean(nullable: false),
                        CalculaIpi = c.Boolean(nullable: false),
                        TipoTributacaoIPI = c.Int(nullable: false),
                        AplicaFreteBaseIpi = c.Boolean(nullable: false),
                        AplicaDespesaBaseIpi = c.Boolean(nullable: false),
                        CalculaPis = c.Boolean(nullable: false),
                        TipoTributacaoPIS = c.Int(nullable: false),
                        AplicaFreteBasePis = c.Boolean(nullable: false),
                        AplicaDespesaBasePis = c.Boolean(nullable: false),
                        CalculaCofins = c.Boolean(nullable: false),
                        TipoTributacaoCOFINS = c.Int(nullable: false),
                        AplicaFreteBaseCofins = c.Boolean(nullable: false),
                        AplicaDespesaBaseCofins = c.Boolean(nullable: false),
                        CalculaIss = c.Boolean(nullable: false),
                        TipoTributacaoISS = c.Int(nullable: false),
                        TipoPagamentoImpostoISS = c.Int(nullable: false),
                        TipoCFPS = c.Int(nullable: false),
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
                .ForeignKey("dbo.Cfop", t => t.CfopId)
                .Index(t => t.CfopId);
            
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
                        Numero = c.String(maxLength: 20, unicode: false),
                        Complemento = c.String(maxLength: 20, unicode: false),
                        Bairro = c.String(maxLength: 30, unicode: false),
                        CidadeId = c.Guid(),
                        EstadoId = c.Guid(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId);
            
            CreateTable(
                "dbo.Produto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        GrupoProdutoId = c.Guid(),
                        UnidadeMedidaId = c.Guid(),
                        NcmId = c.Guid(),
                        TipoProduto = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoProduto", t => t.GrupoProdutoId)
                .ForeignKey("dbo.NCM", t => t.NcmId)
                .ForeignKey("dbo.UnidadeMedida", t => t.UnidadeMedidaId)
                .Index(t => t.GrupoProdutoId)
                .Index(t => t.UnidadeMedidaId)
                .Index(t => t.NcmId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.Produto", "NcmId", "dbo.NCM");
            DropForeignKey("dbo.Produto", "GrupoProdutoId", "dbo.GrupoProduto");
            DropForeignKey("dbo.Pessoa", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Pessoa", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.GrupoTributario", "CfopId", "dbo.Cfop");
            DropForeignKey("dbo.GrupoProduto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.GrupoProduto", "NcmId", "dbo.NCM");
            DropForeignKey("dbo.Cidade", "EstadoId", "dbo.Estado");
            DropIndex("dbo.Produto", new[] { "NcmId" });
            DropIndex("dbo.Produto", new[] { "UnidadeMedidaId" });
            DropIndex("dbo.Produto", new[] { "GrupoProdutoId" });
            DropIndex("dbo.Pessoa", new[] { "EstadoId" });
            DropIndex("dbo.Pessoa", new[] { "CidadeId" });
            DropIndex("dbo.GrupoTributario", new[] { "CfopId" });
            DropIndex("dbo.GrupoProduto", new[] { "NcmId" });
            DropIndex("dbo.GrupoProduto", new[] { "UnidadeMedidaId" });
            DropIndex("dbo.Cidade", new[] { "EstadoId" });
            DropTable("dbo.Produto");
            DropTable("dbo.Pessoa");
            DropTable("dbo.GrupoTributario");
            DropTable("dbo.UnidadeMedida");
            DropTable("dbo.NCM");
            DropTable("dbo.GrupoProduto");
            DropTable("dbo.Estado");
            DropTable("dbo.Cidade");
            DropTable("dbo.Cfop");
        }
    }
}
