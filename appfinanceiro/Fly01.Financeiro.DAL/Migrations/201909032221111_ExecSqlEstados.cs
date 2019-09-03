namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExecSqlEstados : DbMigration
    {
        public override void Up()
        {
            string diretorio = (AppDomain.CurrentDomain.BaseDirectory).Replace("bin", "").Replace("Debug", "");

            SqlFile(diretorio + @"Migrations\SQLScripts\InsertEstados.sql");
        }
        
        public override void Down()
        {
            Sql("delete from dbo.estado");
        }
    }
}
