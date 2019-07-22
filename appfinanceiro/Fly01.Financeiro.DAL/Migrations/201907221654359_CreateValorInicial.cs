namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateValorInicial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaBancaria", "ValorInicial", c => c.Double(defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaBancaria", "ValorInicial");
        }
    }
}
