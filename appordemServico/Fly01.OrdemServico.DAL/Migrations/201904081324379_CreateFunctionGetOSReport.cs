namespace Fly01.OrdemServico.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFunctionGetOSReport : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION[dbo].GetOSReport(
@DATAINI VARCHAR(30),
@DATAFIM VARCHAR(30),
@PLATAFORMA VARCHAR(250)
) RETURNS TABLE AS RETURN(
SELECT * FROM
(
	SELECT PLATAFORMAID AS PLATAFORMAURL
        , 'ORDEMSERVICO' AS TIPO
        , COUNT(*) AS TOTAL
    FROM
        ORDEMSERVICO
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR (PLATAFORMAID = @PLATAFORMA))
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
        AND ((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

    UNION ALL

    SELECT
        PLATAFORMAID AS PLATAFORMAURL
        , 'SERVICOS'
        , COUNT(*) AS TOTAL
    FROM
        PRODUTO
    WHERE
        TIPOPRODUTO = 3
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID
	UNION ALL

	SELECT
        PLATAFORMAID AS PLATAFORMAURL
        , 'CLIENTES'
        , COUNT(*) AS TOTAL
    FROM
        PESSOA
    WHERE
		CLIENTE = 1
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

	UNION ALL

	SELECT
        PLATAFORMAID AS PLATAFORMAURL
        , 'KITS'
        , COUNT(*) AS TOTAL
    FROM
        KIT
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID
	
	) AS T
);");
        }
        
        public override void Down()
        {
            Sql("DROP FUNCTION [dbo].[GetOSReport]");
        }
    }
}