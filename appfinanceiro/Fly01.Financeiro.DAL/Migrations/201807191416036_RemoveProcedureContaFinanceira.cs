namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProcedureContaFinanceira : DbMigration
    {
        public override void Up()
        {
            DropStoredProcedure("dbo.ContaReceber_Insert");
            DropStoredProcedure("dbo.ContaReceber_Update");
            DropStoredProcedure("dbo.ContaReceber_Delete");
            DropStoredProcedure("dbo.ContaPagar_Insert");
            DropStoredProcedure("dbo.ContaPagar_Update");
            DropStoredProcedure("dbo.ContaPagar_Delete");
        }
        
        public override void Down()
        {
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
