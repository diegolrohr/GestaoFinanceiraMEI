namespace Fly01.Financeiro.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SaldoHistoricoCreateSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
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
                      INSERT [dbo].[SaldoHistorico]([Id], [Data], [ContaBancariaId], [SaldoDia], [SaldoConsolidado], [TotalRecebimentos], [TotalPagamentos], [PlataformaId], [DataInclusao], [DataAlteracao], [DataExclusao], [UsuarioInclusao], [UsuarioAlteracao], [UsuarioExclusao], [Ativo])
                      VALUES (@Id, @Data, @ContaBancariaId, @SaldoDia, @SaldoConsolidado, @TotalRecebimentos, @TotalPagamentos, @PlataformaId, @DataInclusao, @DataAlteracao, @DataExclusao, @UsuarioInclusao, @UsuarioAlteracao, @UsuarioExclusao, @Ativo)
                      
                      -- Recalculo de Saldos
                      UPDATE [dbo].[SaldoHistorico] SET [SaldoConsolidado] = [SaldoConsolidado] + @SaldoDia
                      WHERE 
                      	[ContaBancariaId] = @ContaBancariaId AND 
                      	[PlataformaId] = @PlataformaId AND 
                      	[Ativo] = 1 AND
                      	[Data] > @Data"
            );
            
            CreateStoredProcedure(
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
                      SET [Data] = @Data, [ContaBancariaId] = @ContaBancariaId, [SaldoDia] = @SaldoDia, [SaldoConsolidado] = @SaldoConsolidado, [TotalRecebimentos] = @TotalRecebimentos, [TotalPagamentos] = @TotalPagamentos, [PlataformaId] = @PlataformaId, [DataInclusao] = @DataInclusao, [DataAlteracao] = @DataAlteracao, [DataExclusao] = @DataExclusao, [UsuarioInclusao] = @UsuarioInclusao, [UsuarioAlteracao] = @UsuarioAlteracao, [UsuarioExclusao] = @UsuarioExclusao, [Ativo] = @Ativo
                      WHERE ([Id] = @Id)

                      -- Recalculo de Saldos
                      UPDATE [dbo].[SaldoHistorico] SET [SaldoConsolidado] = [SaldoConsolidado] + @DiferencaSaldo
                      WHERE 
                      	[ContaBancariaId] = @ContaBancariaId AND 
                      	[PlataformaId] = @PlataformaId AND 
                      	[Ativo] = 1 AND
                      	[Data] > @Data"
            );
            
            CreateStoredProcedure(
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
            DropStoredProcedure("dbo.SaldoHistorico_Delete");
            DropStoredProcedure("dbo.SaldoHistorico_Update");
            DropStoredProcedure("dbo.SaldoHistorico_Insert");
        }
    }
}
