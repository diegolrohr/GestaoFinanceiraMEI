namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.Ncm", t => t.NcmId)
                .ForeignKey("dbo.UnidadeMedida", t => t.UnidadeMedidaId)
                .Index(t => t.UnidadeMedidaId)
                .Index(t => t.NcmId);
            
            CreateTable(
                "dbo.Ncm",
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
                "dbo.OrdemServicoItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrdemServicoId = c.Guid(nullable: false),
                        Quantidade = c.Double(nullable: false),
                        Valor = c.Double(nullable: false),
                        Desconto = c.Double(nullable: false),
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
                .ForeignKey("dbo.OrdemServico", t => t.OrdemServicoId)
                .Index(t => t.OrdemServicoId);
            
            CreateTable(
                "dbo.OrdemServico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        PessoaId = c.Guid(nullable: false),
                        Numero = c.Int(nullable: false),
                        DataEmissao = c.DateTime(nullable: false, storeType: "date"),
                        DataEntrega = c.DateTime(nullable: false, storeType: "date"),
                        ResponsavelId = c.Guid(nullable: false),
                        Aprovado = c.Boolean(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
                        ValorTotal = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.Pessoa", t => t.PessoaId)
                .ForeignKey("dbo.Pessoa", t => t.ResponsavelId)
                .Index(t => t.PessoaId)
                .Index(t => t.ResponsavelId);
            
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        TipoDocumento = c.String(maxLength: 1, unicode: false),
                        CPFCNPJ = c.String(maxLength: 18, unicode: false),
                        CEP = c.String(maxLength: 8, unicode: false),
                        Endereco = c.String(maxLength: 80, unicode: false),
                        Numero = c.String(maxLength: 60, unicode: false),
                        Complemento = c.String(maxLength: 60, unicode: false),
                        Bairro = c.String(maxLength: 60, unicode: false),
                        CidadeId = c.Guid(),
                        EstadoId = c.Guid(),
                        Telefone = c.String(maxLength: 15, unicode: false),
                        Celular = c.String(maxLength: 15, unicode: false),
                        Contato = c.String(maxLength: 45, unicode: false),
                        Observacao = c.String(maxLength: 500, unicode: false),
                        Email = c.String(maxLength: 70, unicode: false),
                        NomeComercial = c.String(maxLength: 100, unicode: false),
                        InscricaoEstadual = c.String(maxLength: 18, unicode: false),
                        InscricaoMunicipal = c.String(maxLength: 18, unicode: false),
                        TipoIndicacaoInscricaoEstadual = c.Int(nullable: false),
                        ConsumidorFinal = c.Boolean(nullable: false),
                        Transportadora = c.Boolean(nullable: false),
                        Cliente = c.Boolean(nullable: false),
                        Fornecedor = c.Boolean(nullable: false),
                        Vendedor = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId);
            
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
                "dbo.Produto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        GrupoProdutoId = c.Guid(),
                        UnidadeMedidaId = c.Guid(),
                        NcmId = c.Guid(),
                        CestId = c.Guid(),
                        EnquadramentoLegalIPIId = c.Guid(),
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
                .ForeignKey("dbo.Cest", t => t.CestId)
                .ForeignKey("dbo.EnquadramentoLegalIPI", t => t.EnquadramentoLegalIPIId)
                .ForeignKey("dbo.GrupoProduto", t => t.GrupoProdutoId)
                .ForeignKey("dbo.Ncm", t => t.NcmId)
                .ForeignKey("dbo.UnidadeMedida", t => t.UnidadeMedidaId)
                .Index(t => t.GrupoProdutoId)
                .Index(t => t.UnidadeMedidaId)
                .Index(t => t.NcmId)
                .Index(t => t.CestId)
                .Index(t => t.EnquadramentoLegalIPIId);
            
            CreateTable(
                "dbo.Cest",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(nullable: false, maxLength: 200, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 650, unicode: false),
                        Segmento = c.String(maxLength: 200, unicode: false),
                        Item = c.String(maxLength: 200, unicode: false),
                        Anexo = c.String(maxLength: 200, unicode: false),
                        NcmId = c.Guid(),
                        DataInclusao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataExclusao = c.DateTime(),
                        UsuarioInclusao = c.String(nullable: false, maxLength: 200, unicode: false),
                        UsuarioAlteracao = c.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = c.String(maxLength: 200, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ncm", t => t.NcmId)
                .Index(t => t.NcmId);
            
            CreateTable(
                "dbo.EnquadramentoLegalIPI",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Codigo = c.String(maxLength: 200, unicode: false),
                        GrupoCST = c.String(maxLength: 200, unicode: false),
                        Descricao = c.String(maxLength: 600, unicode: false),
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
                "dbo.ParametroOrdemServico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DiasPadraoEntrega = c.Int(nullable: false),
                        ResponsavelPadraoId = c.Guid(),
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
                .ForeignKey("dbo.Pessoa", t => t.ResponsavelPadraoId)
                .Index(t => t.ResponsavelPadraoId);
            
            CreateTable(
                "dbo.OrdemServicoItemProduto",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProdutoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdemServicoItem", t => t.Id)
                .ForeignKey("dbo.Produto", t => t.ProdutoId)
                .Index(t => t.Id)
                .Index(t => t.ProdutoId);
            
            CreateTable(
                "dbo.OrdemServicoItemServico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServicoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrdemServicoItem", t => t.Id)
                .ForeignKey("dbo.Servico", t => t.ServicoId)
                .Index(t => t.Id)
                .Index(t => t.ServicoId);
            
            CreateTable(
                "dbo.ProdutoOrdemServico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Produto_Id = c.Guid(),
                        ObjetoDeManutencao = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Produto", t => t.Id)
                .ForeignKey("dbo.Produto", t => t.Produto_Id)
                .Index(t => t.Id)
                .Index(t => t.Produto_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProdutoOrdemServico", "Produto_Id", "dbo.Produto");
            DropForeignKey("dbo.ProdutoOrdemServico", "Id", "dbo.Produto");
            DropForeignKey("dbo.OrdemServicoItemServico", "ServicoId", "dbo.Servico");
            DropForeignKey("dbo.OrdemServicoItemServico", "Id", "dbo.OrdemServicoItem");
            DropForeignKey("dbo.OrdemServicoItemProduto", "ProdutoId", "dbo.Produto");
            DropForeignKey("dbo.OrdemServicoItemProduto", "Id", "dbo.OrdemServicoItem");
            DropForeignKey("dbo.ParametroOrdemServico", "ResponsavelPadraoId", "dbo.Pessoa");
            DropForeignKey("dbo.Servico", "NbsId", "dbo.Nbs");
            DropForeignKey("dbo.Produto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.Produto", "NcmId", "dbo.Ncm");
            DropForeignKey("dbo.Produto", "GrupoProdutoId", "dbo.GrupoProduto");
            DropForeignKey("dbo.Produto", "EnquadramentoLegalIPIId", "dbo.EnquadramentoLegalIPI");
            DropForeignKey("dbo.Produto", "CestId", "dbo.Cest");
            DropForeignKey("dbo.Cest", "NcmId", "dbo.Ncm");
            DropForeignKey("dbo.OrdemServicoItem", "OrdemServicoId", "dbo.OrdemServico");
            DropForeignKey("dbo.OrdemServico", "ResponsavelId", "dbo.Pessoa");
            DropForeignKey("dbo.OrdemServico", "PessoaId", "dbo.Pessoa");
            DropForeignKey("dbo.Pessoa", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Pessoa", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.Cidade", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.GrupoProduto", "UnidadeMedidaId", "dbo.UnidadeMedida");
            DropForeignKey("dbo.GrupoProduto", "NcmId", "dbo.Ncm");
            DropIndex("dbo.ProdutoOrdemServico", new[] { "Produto_Id" });
            DropIndex("dbo.ProdutoOrdemServico", new[] { "Id" });
            DropIndex("dbo.OrdemServicoItemServico", new[] { "ServicoId" });
            DropIndex("dbo.OrdemServicoItemServico", new[] { "Id" });
            DropIndex("dbo.OrdemServicoItemProduto", new[] { "ProdutoId" });
            DropIndex("dbo.OrdemServicoItemProduto", new[] { "Id" });
            DropIndex("dbo.ParametroOrdemServico", new[] { "ResponsavelPadraoId" });
            DropIndex("dbo.Servico", new[] { "NbsId" });
            DropIndex("dbo.Cest", new[] { "NcmId" });
            DropIndex("dbo.Produto", new[] { "EnquadramentoLegalIPIId" });
            DropIndex("dbo.Produto", new[] { "CestId" });
            DropIndex("dbo.Produto", new[] { "NcmId" });
            DropIndex("dbo.Produto", new[] { "UnidadeMedidaId" });
            DropIndex("dbo.Produto", new[] { "GrupoProdutoId" });
            DropIndex("dbo.Cidade", new[] { "EstadoId" });
            DropIndex("dbo.Pessoa", new[] { "EstadoId" });
            DropIndex("dbo.Pessoa", new[] { "CidadeId" });
            DropIndex("dbo.OrdemServico", new[] { "ResponsavelId" });
            DropIndex("dbo.OrdemServico", new[] { "PessoaId" });
            DropIndex("dbo.OrdemServicoItem", new[] { "OrdemServicoId" });
            DropIndex("dbo.GrupoProduto", new[] { "NcmId" });
            DropIndex("dbo.GrupoProduto", new[] { "UnidadeMedidaId" });
            DropTable("dbo.ProdutoOrdemServico");
            DropTable("dbo.OrdemServicoItemServico");
            DropTable("dbo.OrdemServicoItemProduto");
            DropTable("dbo.ParametroOrdemServico");
            DropTable("dbo.Nbs");
            DropTable("dbo.Servico");
            DropTable("dbo.EnquadramentoLegalIPI");
            DropTable("dbo.Cest");
            DropTable("dbo.Produto");
            DropTable("dbo.Estado");
            DropTable("dbo.Cidade");
            DropTable("dbo.Pessoa");
            DropTable("dbo.OrdemServico");
            DropTable("dbo.OrdemServicoItem");
            DropTable("dbo.UnidadeMedida");
            DropTable("dbo.Ncm");
            DropTable("dbo.GrupoProduto");
        }
    }
}
