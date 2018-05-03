namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertColumnBancoEmiteBoleto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Banco", "EmiteBoleto", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Banco", "EmiteBoleto");
        }
    }
}
