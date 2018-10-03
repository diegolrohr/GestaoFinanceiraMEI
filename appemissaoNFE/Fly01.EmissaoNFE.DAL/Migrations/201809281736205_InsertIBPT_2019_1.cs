namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InsertIBPT_2019_1 : DbMigration
    {
        public override void Up()
        {
            string diretorio = (AppDomain.CurrentDomain.BaseDirectory).Replace("Debug", "").Replace("bin", "").Replace("API", "DAL");

            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_AC.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_AL.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_AM.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_AP.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_BA.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_CE.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_DF.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_ES.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_GO.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_MA.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_MG.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_MS.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_MT.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_PA.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_PB.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_PE.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_PI.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_PR.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_RJ.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_RN.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_RO.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_RR.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_RS.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_SE.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_SC.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_SP.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\IBPT_TO.sql");
        }

        public override void Down()
        {
            Sql("DELETE FROM IbptNcm WHERE UF = 'AC'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'AL'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'AM'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'AP'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'BA'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'CE'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'DF'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'ES'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'GO'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'MA'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'MG'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'MS'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'MT'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'PA'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'PB'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'PE'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'PI'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'PR'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'RJ'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'RN'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'RO'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'RR'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'RS'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'SE'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'SC'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'SP'");
            Sql("DELETE FROM IbptNcm WHERE UF = 'TO'");
        }
    }
}

