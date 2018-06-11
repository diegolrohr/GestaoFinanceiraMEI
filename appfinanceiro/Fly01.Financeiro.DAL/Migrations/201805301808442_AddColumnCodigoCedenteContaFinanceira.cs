namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnCodigoCedenteContaFinanceira : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaBancaria", "CodigoCedente", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaBancaria", "CodigoCedente");
        }
    }
}
