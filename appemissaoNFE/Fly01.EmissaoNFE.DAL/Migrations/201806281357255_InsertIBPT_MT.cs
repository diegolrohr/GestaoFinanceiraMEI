namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertIBPT_MT : DbMigration
    {
        public override void Up()
        {
            string diretorio = (AppDomain.CurrentDomain.BaseDirectory).Replace("Debug", "").Replace("bin", "").Replace("API", "DAL");

            SqlFile(diretorio + @"Migrations\SQLScripts\Insert_IBPT_MatoGrosso.sql");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM IbptNcm WHERE UF = 'MT'");
        }
    }
}