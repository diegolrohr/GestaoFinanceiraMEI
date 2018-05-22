namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPlataformaBaseRegistroFixo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arquivo", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categoria", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConciliacaoBancariaItemContaFinanceira", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContaFinanceira", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.CondicaoParcelamento", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.FormaPagamento", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pessoa", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContaFinanceiraBaixa", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContaBancaria", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConciliacaoBancariaItem", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConciliacaoBancaria", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConfiguracaoNotificacao", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContaFinanceiraRenegociacao", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Feriado", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movimentacao", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.RenegociacaoContaFinanceira", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaldoHistorico", "RegistroFixo", c => c.Boolean(nullable: false));

            AlterStoredProcedure(
                "dbo.SaldoHistorico_Insert",
                p => new
                    {
                        Id = p.Guid(),
                        Data = p.DateTime(storeType: "date"),
                        ContaBancariaId = p.Guid(),
                        SaldoDia = p.Double(),
                        SaldoConsolidado = p.Double(),
                        TotalRecebimentos = p.Double(),
                        TotalPagamentos = p.Double(),
                        PlataformaId = p.String(maxLength: 200, unicode: false),
                        RegistroFixo = p.Boolean(),
                        DataInclusao = p.DateTime(),
                        DataAlteracao = p.DateTime(),
                        DataExclusao = p.DateTime(),
                        UsuarioInclusao = p.String(maxLength: 200, unicode: false),
                        UsuarioAlteracao = p.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = p.String(maxLength: 200, unicode: false),
                        Ativo = p.Boolean(),
                    },
                body:
                    @"
                      INSERT [dbo].[SaldoHistorico]([Id], [Data], [ContaBancariaId], [SaldoDia], [SaldoConsolidado], [TotalRecebimentos], [TotalPagamentos], [PlataformaId], [RegistroFixo], [DataInclusao], [DataAlteracao], [DataExclusao], [UsuarioInclusao], [UsuarioAlteracao], [UsuarioExclusao], [Ativo])
                      VALUES (@Id, @Data, @ContaBancariaId, @SaldoDia, @SaldoConsolidado, @TotalRecebimentos, @TotalPagamentos, @PlataformaId, @RegistroFixo, @DataInclusao, @DataAlteracao, @DataExclusao, @UsuarioInclusao, @UsuarioAlteracao, @UsuarioExclusao, @Ativo)
                      
                      -- Recalculo de Saldos
                      UPDATE [dbo].[SaldoHistorico] SET [SaldoConsolidado] = [SaldoConsolidado] + @SaldoDia
                      WHERE 
                      	[ContaBancariaId] = @ContaBancariaId AND 
                      	[PlataformaId] = @PlataformaId AND 
                      	[Ativo] = 1 AND
                      	[Data] > @Data"
            );
            
            AlterStoredProcedure(
                "dbo.SaldoHistorico_Update",
                p => new
                    {
                        Id = p.Guid(),
                        Data = p.DateTime(storeType: "date"),
                        ContaBancariaId = p.Guid(),
                        SaldoDia = p.Double(),
                        SaldoConsolidado = p.Double(),
                        TotalRecebimentos = p.Double(),
                        TotalPagamentos = p.Double(),
                        PlataformaId = p.String(maxLength: 200, unicode: false),
                        RegistroFixo = p.Boolean(),
                        DataInclusao = p.DateTime(),
                        DataAlteracao = p.DateTime(),
                        DataExclusao = p.DateTime(),
                        UsuarioInclusao = p.String(maxLength: 200, unicode: false),
                        UsuarioAlteracao = p.String(maxLength: 200, unicode: false),
                        UsuarioExclusao = p.String(maxLength: 200, unicode: false),
                        Ativo = p.Boolean(),
                    },
                body:
                    @"
                      DECLARE @DiferencaSaldo float
                      SELECT @DiferencaSaldo = @SaldoDia - S.[SaldoDia]
                      FROM [dbo].[SaldoHistorico] S
                      WHERE ([Id] = @Id)

                      UPDATE [dbo].[SaldoHistorico]
                      SET [Data] = @Data, [ContaBancariaId] = @ContaBancariaId, [SaldoDia] = @SaldoDia, [SaldoConsolidado] = @SaldoConsolidado, [TotalRecebimentos] = @TotalRecebimentos, [TotalPagamentos] = @TotalPagamentos, [PlataformaId] = @PlataformaId, [RegistroFixo] = @RegistroFixo, [DataInclusao] = @DataInclusao, [DataAlteracao] = @DataAlteracao, [DataExclusao] = @DataExclusao, [UsuarioInclusao] = @UsuarioInclusao, [UsuarioAlteracao] = @UsuarioAlteracao, [UsuarioExclusao] = @UsuarioExclusao, [Ativo] = @Ativo
                      WHERE ([Id] = @Id)

                      -- Recalculo de Saldos
                      UPDATE [dbo].[SaldoHistorico] SET [SaldoConsolidado] = [SaldoConsolidado] + @DiferencaSaldo
                      WHERE 
                      	[ContaBancariaId] = @ContaBancariaId AND 
                      	[PlataformaId] = @PlataformaId AND 
                      	[Ativo] = 1 AND
                      	[Data] > @Data"
            );

            AlterStoredProcedure(
                "dbo.SaldoHistorico_Delete",
                p => new
                    {
                        Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[SaldoHistorico]
                      WHERE ([Id] = @Id)"
            );
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaldoHistorico", "RegistroFixo");
            DropColumn("dbo.RenegociacaoContaFinanceira", "RegistroFixo");
            DropColumn("dbo.Movimentacao", "RegistroFixo");
            DropColumn("dbo.Feriado", "RegistroFixo");
            DropColumn("dbo.ContaFinanceiraRenegociacao", "RegistroFixo");
            DropColumn("dbo.ConfiguracaoNotificacao", "RegistroFixo");
            DropColumn("dbo.ConciliacaoBancaria", "RegistroFixo");
            DropColumn("dbo.ConciliacaoBancariaItem", "RegistroFixo");
            DropColumn("dbo.ContaBancaria", "RegistroFixo");
            DropColumn("dbo.ContaFinanceiraBaixa", "RegistroFixo");
            DropColumn("dbo.Pessoa", "RegistroFixo");
            DropColumn("dbo.FormaPagamento", "RegistroFixo");
            DropColumn("dbo.CondicaoParcelamento", "RegistroFixo");
            DropColumn("dbo.ContaFinanceira", "RegistroFixo");
            DropColumn("dbo.ConciliacaoBancariaItemContaFinanceira", "RegistroFixo");
            DropColumn("dbo.Categoria", "RegistroFixo");
            DropColumn("dbo.Arquivo", "RegistroFixo");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
