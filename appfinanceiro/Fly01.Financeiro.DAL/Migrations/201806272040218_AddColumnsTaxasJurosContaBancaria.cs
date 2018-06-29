namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsTaxasJurosContaBancaria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaBancaria", "TaxaJuros", c => c.Double());
            AddColumn("dbo.ContaBancaria", "PercentualMulta", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaBancaria", "PercentualMulta");
            DropColumn("dbo.ContaBancaria", "TaxaJuros");
        }
    }
}
