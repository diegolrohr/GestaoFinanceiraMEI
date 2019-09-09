namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saldoHistoricoProcedures : DbMigration
    {
        public override void Up()
        {
            string diretorio = (AppDomain.CurrentDomain.BaseDirectory).Replace("bin", "").Replace("Debug", "");

            SqlFile(diretorio + @"Migrations\SQLScripts\SaldoHistorico_Insert.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\SaldoHistorico_Update.sql");
            SqlFile(diretorio + @"Migrations\SQLScripts\SaldoHistorico_Delete.sql");
        }
        
        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[SaldoHistorico_Insert]");
            Sql("DROP PROCEDURE [dbo].[SaldoHistorico_Update]");
            Sql("DROP PROCEDURE [dbo].[SaldoHistorico_Delete]");

        }
    }
}
