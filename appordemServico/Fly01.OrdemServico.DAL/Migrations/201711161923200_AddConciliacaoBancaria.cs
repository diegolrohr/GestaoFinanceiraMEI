namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddConciliacaoBancaria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConciliacaoBancariaItemContaFinanceira",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConciliacaoBancariaItemId = c.Guid(nullable: false),
                        ContaFinanceiraId = c.Guid(nullable: false),
                        ContaFinanceiraBaixaId = c.Guid(),
                        ValorConciliado = c.Double(nullable: false),
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
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraId)
                .ForeignKey("dbo.ContaFinanceiraBaixa", t => t.ContaFinanceiraBaixaId)
                .ForeignKey("dbo.ConciliacaoBancariaItem", t => t.ConciliacaoBancariaItemId)
                .Index(t => t.ConciliacaoBancariaItemId)
                .Index(t => t.ContaFinanceiraId)
                .Index(t => t.ContaFinanceiraBaixaId);
            
            CreateTable(
                "dbo.ConciliacaoBancariaItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConciliacaoBancariaId = c.Guid(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 200, unicode: false),
                        Valor = c.Double(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
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
                .ForeignKey("dbo.ConciliacaoBancaria", t => t.ConciliacaoBancariaId)
                .Index(t => t.ConciliacaoBancariaId);
            
            AddColumn("dbo.ConciliacaoBancaria", "ContaBancariaId", c => c.Guid(nullable: false));
            AddColumn("dbo.ConciliacaoBancaria", "ArquivoMD5", c => c.String(nullable: false, maxLength: 32, unicode: false));
            AlterColumn("dbo.ConciliacaoBancaria", "Status", c => c.Int(nullable: false));
            CreateIndex("dbo.ConciliacaoBancaria", "ContaBancariaId");
            AddForeignKey("dbo.ConciliacaoBancaria", "ContaBancariaId", "dbo.ContaBancaria", "Id");
            DropColumn("dbo.ConciliacaoBancaria", "BancoId");
            DropColumn("dbo.ConciliacaoBancaria", "Codigo");
            DropColumn("dbo.ConciliacaoBancaria", "Agencia");
            DropColumn("dbo.ConciliacaoBancaria", "Conta");
            DropColumn("dbo.ConciliacaoBancaria", "BancoNome");
            DropColumn("dbo.ConciliacaoBancaria", "DataImportacao");
            DropColumn("dbo.ConciliacaoBancaria", "DataImportacaoRest");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConciliacaoBancaria", "DataImportacaoRest", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ConciliacaoBancaria", "DataImportacao", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.ConciliacaoBancaria", "BancoNome", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ConciliacaoBancaria", "Conta", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ConciliacaoBancaria", "Agencia", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ConciliacaoBancaria", "Codigo", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ConciliacaoBancaria", "BancoId", c => c.String(maxLength: 200, unicode: false));
            DropForeignKey("dbo.ConciliacaoBancaria", "ContaBancariaId", "dbo.ContaBancaria");
            DropForeignKey("dbo.ConciliacaoBancariaItem", "ConciliacaoBancariaId", "dbo.ConciliacaoBancaria");
            DropForeignKey("dbo.ConciliacaoBancariaItemContaFinanceira", "ConciliacaoBancariaItemId", "dbo.ConciliacaoBancariaItem");
            DropForeignKey("dbo.ConciliacaoBancariaItemContaFinanceira", "ContaFinanceiraBaixaId", "dbo.ContaFinanceiraBaixa");
            DropForeignKey("dbo.ConciliacaoBancariaItemContaFinanceira", "ContaFinanceiraId", "dbo.ContaFinanceira");
            DropIndex("dbo.ConciliacaoBancaria", new[] { "ContaBancariaId" });
            DropIndex("dbo.ConciliacaoBancariaItem", new[] { "ConciliacaoBancariaId" });
            DropIndex("dbo.ConciliacaoBancariaItemContaFinanceira", new[] { "ContaFinanceiraBaixaId" });
            DropIndex("dbo.ConciliacaoBancariaItemContaFinanceira", new[] { "ContaFinanceiraId" });
            DropIndex("dbo.ConciliacaoBancariaItemContaFinanceira", new[] { "ConciliacaoBancariaItemId" });
            AlterColumn("dbo.ConciliacaoBancaria", "Status", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.ConciliacaoBancaria", "ArquivoMD5");
            DropColumn("dbo.ConciliacaoBancaria", "ContaBancariaId");
            DropTable("dbo.ConciliacaoBancariaItem");
            DropTable("dbo.ConciliacaoBancariaItemContaFinanceira");
        }
    }
}
