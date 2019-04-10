namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFunctionGetFaturamento : DbMigration
    {
        public override void Up()
        {
            Sql(@"USE [Fly01Faturamento]
GO
/****** Object:  UserDefinedFunction [dbo].[GetFaturamentoReport]    Script Date: 10/04/2019 09:46:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION[dbo].[GetFaturamentoReport](
@DATAINI VARCHAR(30),
@DATAFIM VARCHAR(30),
@PLATAFORMA VARCHAR(250)
) RETURNS TABLE AS RETURN(
SELECT * FROM
(SELECT PLATAFORMAID AS PLATAFORMAURL
        , 'VENDAS' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        ORDEMVENDA
    WHERE
        TIPOORDEMVENDA = 2
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND ((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID

	
    UNION ALL

    SELECT
        PLATAFORMAID
        , 'ORCAMENTOS'
        , COUNT(*) AS TOTAL
    FROM
        ORDEMVENDA
    WHERE
        TIPOORDEMVENDA = 1
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND ((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID

    UNION ALL

    SELECT
        PLATAFORMAID
        , 'NFE'
        , COUNT(*) AS TOTAL
    FROM
        NOTAFISCAL
    WHERE
        TIPONOTAFISCAL = 1
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND ((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID

    UNION ALL

    SELECT
        PLATAFORMAID
        , 'NFSE'
        , COUNT(*) AS TOTAL
    FROM
        NOTAFISCAL
    WHERE
        TIPONOTAFISCAL = 2
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID

    UNION ALL
	  

    SELECT
        PLATAFORMAID
        , 'CLIENTES'
        , COUNT(*) AS TOTAL
    FROM
        PESSOA
    WHERE
		CLIENTE = 1
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID

    UNION ALL

    SELECT
        PLATAFORMAID
        , 'FORNECEDORES'
        , COUNT(*) AS TOTAL
    FROM
        PESSOA
    WHERE
        FORNECEDOR = 1
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID

    UNION ALL


    SELECT
        PLATAFORMAID
        , 'GRUPOTRIBUTARIO'
        , COUNT(*) AS TOTAL
    FROM
        GRUPOTRIBUTARIO
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID

    UNION ALL


    SELECT PLATAFORMAID AS PLATAFORMAURL
        , 'PRODUTOS'
        , COUNT(*) AS TOTAL
    FROM
        PRODUTO
    WHERE
		(TIPOPRODUTO = 2 OR TIPOPRODUTO = 1 OR TIPOPRODUTO = 4)
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID
    UNION ALL


    SELECT
        PLATAFORMAID
        , 'SERVICOS'
        , COUNT(*) AS TOTAL
    FROM
        PRODUTO
    WHERE
        TIPOPRODUTO = 3
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID
	UNION ALL

	SELECT
        PLATAFORMAID
        , 'KITS'
        , COUNT(*) AS TOTAL
    FROM
        KIT
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID
	UNION ALL

	SELECT PLATAFORMAID AS PLATAFORMAURL
        , 'CENTROCUSTO' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        CENTROCUSTO
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
    GROUP BY PLATAFORMAID
	UNION ALL

	
    SELECT PLATAFORMAID AS PLATAFORMAURL
        , 'CERTIFICADODIGITAL' AS TIPO
		, COUNT(*) AS TOTAL
    FROM CERTIFICADODIGITAL
    WHERE CERTIFICADOVALIDONFS = 1 
	AND((@PLATAFORMA = '') OR (EXISTS (SELECT [Value] 
											FROM 
												(SELECT [Value] = LTRIM(RTRIM(SUBSTRING(@PLATAFORMA, [Number], CHARINDEX(',', @PLATAFORMA + ',', [Number]) - [Number])))
												FROM 
													(SELECT Number = ROW_NUMBER() OVER (ORDER BY name)
													FROM sys.all_columns) AS x 
													WHERE Number <= LEN(@PLATAFORMA)
													AND SUBSTRING(',' + @PLATAFORMA, [Number], LEN(',')) = ',') as [Value] WHERE [Value] = PlataformaId
											)))
	AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
	AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
	GROUP BY PLATAFORMAID

    )AS T
);");
        }
        
        public override void Down()
        {
            Sql("DROP FUNCTION [dbo].[GetFaturamentoReport]");
        }
    }
}
