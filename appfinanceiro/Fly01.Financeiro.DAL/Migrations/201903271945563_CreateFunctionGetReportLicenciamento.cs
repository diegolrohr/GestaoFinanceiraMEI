namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFunctionGetReportLicenciamento : DbMigration
    {
        public override void Up()
        {
            Sql(@"
CREATE FUNCTION[dbo].GetLicenceReport(
@DATAINI VARCHAR(30),
@DATAFIM VARCHAR(30),
@PLATAFORMA VARCHAR(250)
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
        AND((@PLATAFORMA = '') OR (PLATAFORMAID = @PLATAFORMA))
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
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

    UNION ALL

    SELECT
        PLATAFORMAID
        , 'VENDEDORES'
        , COUNT(*) AS TOTAL
    FROM
        PESSOA
    WHERE
        VENDEDOR = 1
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

    UNION ALL

    SELECT
        PLATAFORMAID
        , 'TRANSPORTADORAS'
        , COUNT(*) AS TOTAL
    FROM
        PESSOA
    WHERE
        TRANSPORTADORA = 1
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

    UNION ALL


    SELECT
        PLATAFORMAID
        , 'FORMAPAGAMENTO'
        , COUNT(*) AS TOTAL
    FROM
        FORMAPAGAMENTO
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

    UNION ALL


    SELECT
        PLATAFORMAID
        , 'CONDICAOPARCELAMENTO'
        , COUNT(*) AS TOTAL
    FROM
        CONDICAOPARCELAMENTO
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

    UNION ALL


    SELECT
        PLATAFORMAID
        , 'CATEGORIA'
        , COUNT(*) AS TOTAL
    FROM
        CATEGORIA
    WHERE
        ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

    UNION ALL


    SELECT
        PLATAFORMAID
        , 'CONTASAPAGAR'
        , COUNT(*) AS TOTAL
    FROM
        ContaFinanceira
    WHERE
        TipoContaFinanceira = 1
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID

    UNION ALL


    SELECT
        PLATAFORMAID
        , 'CONTARECEBER'
        , COUNT(*) AS TOTAL
    FROM
        ContaFinanceira
    WHERE
        TipoContaFinanceira = 2
        AND ((@DATAINI = '') OR (DATAINCLUSAO >= @DATAINI))
		AND ((@DATAFIM = '' ) OR (DATAINCLUSAO <= @DATAFIM))
        AND((@PLATAFORMA = '') OR(PLATAFORMAID = @PLATAFORMA))
    GROUP BY PLATAFORMAID
    )AS T
);");
        }
        
        public override void Down()
        {
            Sql("DROP FUNCTION [dbo].[GetLicenceReport]");
        }
    }
}
