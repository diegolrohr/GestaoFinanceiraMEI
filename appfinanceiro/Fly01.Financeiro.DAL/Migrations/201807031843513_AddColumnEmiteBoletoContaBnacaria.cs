namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnEmiteBoletoContaBnacaria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaBancaria", "ContaEmiteBoleto", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaBancaria", "ContaEmiteBoleto");
        }
    }
}
