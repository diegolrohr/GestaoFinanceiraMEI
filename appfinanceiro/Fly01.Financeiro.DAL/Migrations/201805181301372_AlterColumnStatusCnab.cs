namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumnStatusCnab : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cnab", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cnab", "Status", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
    }
}
