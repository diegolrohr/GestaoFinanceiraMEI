namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FunctionFinanceiro : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER FUNCTION[dbo].[GetLicenceReport] (
	@DATAINI VARCHAR(30),
	@DATAFIM VARCHAR(30),
	@PLATAFORMA VARCHAR(8000)
) RETURNS TABLE AS RETURN(

SELECT
    *
FROM
(
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
        , 'FORNECEDORES' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        PESSOA
    WHERE
        FORNECEDOR = 1
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
        , 'VENDEDORES' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        PESSOA
    WHERE
        VENDEDOR = 1
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
        , 'TRANSPORTADORAS' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        PESSOA
    WHERE
        TRANSPORTADORA = 1
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
        , 'FORMAPAGAMENTO' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        FORMAPAGAMENTO
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
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
        , 'CONDICAOPARCELAMENTO' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        CONDICAOPARCELAMENTO
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
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
        , 'CATEGORIA' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        CATEGORIA
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
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
        , 'CONTASAPAGAR' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        ContaFinanceira
    WHERE
        TipoContaFinanceira = 1
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
        , 'CONTARECEBER' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        ContaFinanceira
    WHERE
        TipoContaFinanceira = 2
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
        , 'BOLETOS' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        CNAB
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
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
    )AS T
);");
        }
        
        public override void Down()
        {
        }
    }
}
