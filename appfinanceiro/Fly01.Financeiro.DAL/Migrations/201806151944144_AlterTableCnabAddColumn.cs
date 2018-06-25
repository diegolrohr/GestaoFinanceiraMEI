namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableCnabAddColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cnab", "NossoNumeroFormatado", c => c.String(nullable: false, maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cnab", "NossoNumeroFormatado");
        }
    }
}
