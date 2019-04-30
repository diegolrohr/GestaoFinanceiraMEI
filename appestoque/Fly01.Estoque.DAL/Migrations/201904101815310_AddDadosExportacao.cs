namespace Fly01.Estoque.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDadosExportacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "EXTIPI", c => c.String(maxLength: 3, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produto", "EXTIPI");
        }
    }
}
