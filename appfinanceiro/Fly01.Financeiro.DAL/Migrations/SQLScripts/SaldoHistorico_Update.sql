USE [GestaoFinanceiraMEI]
GO

--DROP PROCEDURE [dbo].[SaldoHistorico_Update]
/****** Object:  StoredProcedure [dbo].[SaldoHistorico_Update]    Script Date: 09/09/2019 19:30:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaldoHistorico_Update]
    @Id [uniqueidentifier],
    @Data [date],
    @ContaBancariaId [uniqueidentifier],
    @SaldoDia [float],
    @SaldoConsolidado [float],
    @TotalRecebimentos [float],
    @TotalPagamentos [float],
    @EmpresaId [varchar](200),
    @DataInclusao [datetime],
    @DataAlteracao [datetime],
    @DataExclusao [datetime],
    @UsuarioInclusao [varchar](200),
    @UsuarioAlteracao [varchar](200),
    @UsuarioExclusao [varchar](200),
    @Ativo [bit]
AS
BEGIN
    
    DECLARE @DiferencaSaldo float
    SELECT @DiferencaSaldo = @SaldoDia - S.[SaldoDia]
    FROM [dbo].[SaldoHistorico] S
    WHERE ([Id] = @Id)
    
    UPDATE [dbo].[SaldoHistorico]
    SET [Data] = @Data, [ContaBancariaId] = @ContaBancariaId, [SaldoDia] = @SaldoDia, [SaldoConsolidado] = @SaldoConsolidado, [TotalRecebimentos] = @TotalRecebimentos, [TotalPagamentos] = @TotalPagamentos, [EmpresaId] = @EmpresaId, [DataInclusao] = @DataInclusao, [DataAlteracao] = @DataAlteracao, [DataExclusao] = @DataExclusao, [UsuarioInclusao] = @UsuarioInclusao, [UsuarioAlteracao] = @UsuarioAlteracao, [UsuarioExclusao] = @UsuarioExclusao, [Ativo] = @Ativo
    WHERE ([Id] = @Id)
    
    -- Recalculo de Saldos
    UPDATE [dbo].[SaldoHistorico] SET [SaldoConsolidado] = [SaldoConsolidado] + @DiferencaSaldo
    WHERE 
    	[ContaBancariaId] = @ContaBancariaId AND 
    	[EmpresaId] = @EmpresaId AND 
    	[Ativo] = 1 AND
    	[Data] > @Data
END
GO


