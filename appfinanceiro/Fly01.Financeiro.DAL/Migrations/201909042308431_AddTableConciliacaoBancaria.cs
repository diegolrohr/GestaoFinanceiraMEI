namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableConciliacaoBancaria : DbMigration
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
                        EmpresaId = c.Guid(nullable: false),
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
                        OfxLancamentoMD5 = c.String(nullable: false, maxLength: 32, unicode: false),
                        StatusConciliado = c.Int(nullable: false),
                        EmpresaId = c.Guid(nullable: false),
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
            
            CreateTable(
                "dbo.ConciliacaoBancaria",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ContaBancariaId = c.Guid(nullable: false),
                        EmpresaId = c.Guid(nullable: false),
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
            
        }
        
        public override void Down()
        {
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
            DropTable("dbo.ConciliacaoBancaria");
            DropTable("dbo.ConciliacaoBancariaItem");
            DropTable("dbo.ConciliacaoBancariaItemContaFinanceira");
        }
    }
}
