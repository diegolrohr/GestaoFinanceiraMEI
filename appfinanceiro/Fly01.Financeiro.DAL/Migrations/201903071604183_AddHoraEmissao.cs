namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHoraEmissao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaFinanceira", "HoraEmissao", c => c.Time(nullable: true, precision: 7, defaultValue: new TimeSpan(0,0,0)));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaFinanceira", "HoraEmissao");
        }
    }
}
