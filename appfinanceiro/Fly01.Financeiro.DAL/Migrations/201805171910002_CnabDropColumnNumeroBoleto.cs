namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CnabDropColumnNumeroBoleto : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cnab", "NumeroBoleto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cnab", "NumeroBoleto", c => c.Int(nullable: false));
        }
    }
}
