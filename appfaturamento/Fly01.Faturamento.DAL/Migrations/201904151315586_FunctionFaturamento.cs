namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FunctionFaturamento : DbMigration
    {
        public override void Up()
        {
            Sql(@"
ALTER FUNCTION[dbo].[GetFaturamentoReport](
@DATAINI VARCHAR(30),
@DATAFIM VARCHAR(30),
@PLATAFORMA NVARCHAR(MAX)
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'ORCAMENTOS' AS TIPO
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'NFE' AS TIPO
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'NFSE' AS TIPO
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'CLIENTES' AS TIPO
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'FORNECEDORES' AS TIPO 
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'GRUPOTRIBUTARIO' AS TIPO
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
        , 'PRODUTOS' AS TIPO
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'SERVICOS' AS TIPO
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'KITS' AS TIPO
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
        }
    }
}
