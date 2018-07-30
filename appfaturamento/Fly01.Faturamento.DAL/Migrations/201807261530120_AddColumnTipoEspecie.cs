namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnTipoEspecie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVenda", "TipoEspecie", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "TipoEspecie");
        }
    }
}
