namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExecSqlBancos : DbMigration
    {
        public override void Up()
        {
            string diretorio = (AppDomain.CurrentDomain.BaseDirectory).Replace("bin", "").Replace("Debug", "");
                ;//Diretório no Azure??

            SqlFile(diretorio + @"Migrations\SQLScripts\InsertBancos.sql");
        }

        public override void Down()
        {
        }
    }
}
