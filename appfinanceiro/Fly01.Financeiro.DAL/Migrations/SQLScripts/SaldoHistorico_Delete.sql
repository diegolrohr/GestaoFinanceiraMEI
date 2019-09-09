USE [GestaoFinanceiraMEI]
GO
--DROP PROCEDURE [dbo].[SaldoHistorico_Delete]
/****** Object:  StoredProcedure [dbo].[SaldoHistorico_Delete]    Script Date: 09/09/2019 19:33:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaldoHistorico_Delete]
    @Id [uniqueidentifier]
AS
BEGIN
    DELETE [dbo].[SaldoHistorico]
    WHERE ([Id] = @Id)
END
GO


