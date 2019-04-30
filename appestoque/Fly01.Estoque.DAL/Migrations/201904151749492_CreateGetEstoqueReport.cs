namespace Fly01.Estoque.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateGetEstoqueReport : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION[dbo].GetEstoqueReport(
@DATAINI VARCHAR(30),
@DATAFIM VARCHAR(30),
@PLATAFORMA VARCHAR(8000)
) RETURNS TABLE AS RETURN(
SELECT * FROM
(
	SELECT PLATAFORMAID AS PLATAFORMAURL
        , 'INVENTARIOS' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        INVENTARIO
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
        , 'MOVIMENTACOES'
        , COUNT(*) AS TOTAL
    FROM
        MOVIMENTO
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
        PLATAFORMAID AS PLATAFORMAURL
        , 'GRUPOS PRODUTO'
        , COUNT(*) AS TOTAL
    FROM
        GRUPOPRODUTO
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

	SELECT
        PLATAFORMAID AS PLATAFORMAURL
        , 'TIPOS MOVIMENTOS'
        , COUNT(*) AS TOTAL
    FROM
        TIPOMOVIMENTO
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
	
	) AS T
);");
        }
        
        public override void Down()
        {
            Sql("DROP FUNCTION[dbo].[GetEstoqueReport]");
        }
    }
}
