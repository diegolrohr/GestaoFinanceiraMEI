namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTipoNfeComplementar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVenda", "TipoNfeComplementar", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "TipoNfeComplementar");
        }
    }
}
