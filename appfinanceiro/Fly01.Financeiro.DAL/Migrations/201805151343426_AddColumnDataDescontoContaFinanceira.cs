namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnDataDescontoContaFinanceira : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cnab", "ValorBoleto", c => c.Double(nullable: false));
            AddColumn("dbo.ContaFinanceira", "DataDesconto", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.ContaFinanceira", "ValorDesconto", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaFinanceira", "ValorDesconto");
            DropColumn("dbo.ContaFinanceira", "DataDesconto");
            DropColumn("dbo.Cnab", "ValorBoleto");
        }
    }
}
