namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterContaBancariaCamposRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContaBancaria", "Agencia", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.ContaBancaria", "DigitoAgencia", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.ContaBancaria", "Conta", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.ContaBancaria", "DigitoConta", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContaBancaria", "DigitoConta", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.ContaBancaria", "Conta", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.ContaBancaria", "DigitoAgencia", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.ContaBancaria", "Agencia", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
    }
}
