USE [GestaoFinanceiraMEI]
GO
--DROP PROCEDURE [dbo].[SaldoHistorico_Insert]
/****** Object:  StoredProcedure [dbo].[SaldoHistorico_Insert]    Script Date: 09/09/2019 19:22:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaldoHistorico_Insert]
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
    
    INSERT [dbo].[SaldoHistorico]([Id], [Data], [ContaBancariaId], [SaldoDia], [SaldoConsolidado], [TotalRecebimentos], [TotalPagamentos], [EmpresaId], [DataInclusao], [DataAlteracao], [DataExclusao], [UsuarioInclusao], [UsuarioAlteracao], [UsuarioExclusao], [Ativo])
    VALUES (@Id, @Data, @ContaBancariaId, @SaldoDia, @SaldoConsolidado, @TotalRecebimentos, @TotalPagamentos, @EmpresaId, @DataInclusao, @DataAlteracao, @DataExclusao, @UsuarioInclusao, @UsuarioAlteracao, @UsuarioExclusao, @Ativo)
    
    -- Recalculo de Saldos
    UPDATE [dbo].[SaldoHistorico] SET [SaldoConsolidado] = [SaldoConsolidado] + @SaldoDia
    WHERE 
    	[ContaBancariaId] = @ContaBancariaId AND 
    	[EmpresaId] = @EmpresaId AND 
    	[Ativo] = 1 AND
    	[Data] > @Data
END
GO


