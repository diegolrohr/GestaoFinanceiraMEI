namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBaixaSaldoHistoricoMovimentacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContaFinanceiraBaixa",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        ContaFinanceiraId = c.Guid(nullable: false),
                        ContaBancariaId = c.Guid(nullable: false),
                        Valor = c.Double(nullable: false),
                        Observacao = c.String(maxLength: 200, unicode: false),
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
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraId)
                .Index(t => t.ContaFinanceiraId)
                .Index(t => t.ContaBancariaId);
            
            CreateTable(
                "dbo.Movimentacao",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        Valor = c.Double(nullable: false),
                        ContaBancariaOrigemId = c.Guid(),
                        ContaBancariaDestinoId = c.Guid(),
                        ContaFinanceiraId = c.Guid(),
                        CategoriaId = c.Guid(),
                        Descricao = c.String(maxLength: 200, unicode: false),
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
                .ForeignKey("dbo.ContaBancaria", t => t.ContaBancariaDestinoId)
                .ForeignKey("dbo.ContaBancaria", t => t.ContaBancariaOrigemId)
                .ForeignKey("dbo.ContaFinanceira", t => t.ContaFinanceiraId)
                .Index(t => t.ContaBancariaOrigemId)
                .Index(t => t.ContaBancariaDestinoId)
                .Index(t => t.ContaFinanceiraId);
            
            CreateTable(
                "dbo.SaldoHistorico",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Data = c.DateTime(nullable: false, storeType: "date"),
                        ContaBancariaId = c.Guid(nullable: false),
                        SaldoDia = c.Double(nullable: false),
                        SaldoConsolidado = c.Double(nullable: false),
                        TotalRecebimentos = c.Double(nullable: false),
                        TotalPagamentos = c.Double(nullable: false),
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
            DropForeignKey("dbo.SaldoHistorico", "ContaBancariaId", "dbo.ContaBancaria");
            DropForeignKey("dbo.Movimentacao", "ContaFinanceiraId", "dbo.ContaFinanceira");
            DropForeignKey("dbo.Movimentacao", "ContaBancariaOrigemId", "dbo.ContaBancaria");
            DropForeignKey("dbo.Movimentacao", "ContaBancariaDestinoId", "dbo.ContaBancaria");
            DropForeignKey("dbo.ContaFinanceiraBaixa", "ContaFinanceiraId", "dbo.ContaFinanceira");
            DropForeignKey("dbo.ContaFinanceiraBaixa", "ContaBancariaId", "dbo.ContaBancaria");
            DropIndex("dbo.SaldoHistorico", new[] { "ContaBancariaId" });
            DropIndex("dbo.Movimentacao", new[] { "ContaFinanceiraId" });
            DropIndex("dbo.Movimentacao", new[] { "ContaBancariaDestinoId" });
            DropIndex("dbo.Movimentacao", new[] { "ContaBancariaOrigemId" });
            DropIndex("dbo.ContaFinanceiraBaixa", new[] { "ContaBancariaId" });
            DropIndex("dbo.ContaFinanceiraBaixa", new[] { "ContaFinanceiraId" });
            DropTable("dbo.SaldoHistorico");
            DropTable("dbo.Movimentacao");
            DropTable("dbo.ContaFinanceiraBaixa");
        }
    }
}
